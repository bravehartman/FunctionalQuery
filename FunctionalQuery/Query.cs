using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FunctionalQuery.Providers;

namespace FunctionalQuery {
	public class Query {
		private readonly string _connectionString;
		private readonly string _connectionKey;
		private readonly IDatabaseProvider _dbProvider;
		private readonly StatementOptions _statementOptions;
		private readonly string _sql;
		private readonly IDictionary<string, DatabaseParameter> _parameters;

		internal Query(IDatabaseProvider provider, string connectionString, string connectionKey, string sql, Dictionary<string, DatabaseParameter> parameters, StatementOptions statementOptions) {
			_dbProvider = provider;
			_connectionString = connectionString;
			_connectionKey = connectionKey ?? connectionString;
			_sql = sql;
			_statementOptions = statementOptions;
			_parameters = parameters;
		}

		public IDictionary<string, DatabaseParameter> Parameters { get { return _parameters; } }

		internal IDictionary<string, DatabaseParameter> Execute() {
			CallDb(cmd => cmd.ExecuteNonQuery());
			return _parameters;
		}

		public DataSet GetDataSet() {
			Func<IDbCommand, DataSet> action = cmd => {
				var ds = new DataSet();
				if ((_statementOptions.HasXmlResults ?? false) && cmd is SqlCommand) {
					using (var reader = ((SqlCommand)cmd).ExecuteXmlReader()) {
						ds.ReadXml(reader, XmlReadMode.Fragment);
					}
				} else {
					var adapter = _dbProvider.CreateDataAdapter();
					adapter.SelectCommand = cmd;
					adapter.Fill(ds);
				}
				return ds;
			};
			return CallDb(action);
		}

		public DataTable GetDataTable() {
			return GetDataSet().Tables[0];
		}

		public T GetItem<T>(Func<IDataRecord, T> map) {
			foreach (var record in GetDataRecords()) {
				return map(record);
			}
			return default(T);
		}

		public IEnumerable<T> GetItems<T>(Func<IDataRecord, T> map) {
			return GetDataRecords().Select(map);
		}

		// todo
		// dynamic
		// object
		// Expando
		// Model
		//public IEnumerable<T> GetItems<T>() {
		//	if (typeof(T) is DataTable) {
		//		
		//	}
		//}

		// {Id: "CarID"}
		//public IEnumerable<T> GetItems<T>(object map) {
		//	// read map object
		//}

		public T GetItemsFromReader<T>(Func<IDataReader, T> map) {
			return CallDb(cmd => GetReaderData(cmd, map));
		}

		public DataTable GetSchema() {
			return GetItemsFromReader(reader => reader.GetSchemaTable());
		}

		public void Run(Action<IDataRecord> action) {
			GetDataRecords().ForEach(action);
		}

		public void RunReader(Action<IDataReader> action) {
			CallDb(cmd => RunReader(cmd, action));
		}

		////////////////////
		
		private IEnumerable<IDataRecord> GetDataRecords() {
			try {
				return CallDb();
			} catch (Exception ex) {
				throw new DatabaseException(_connectionKey, _sql, _statementOptions.TimeoutSeconds ?? 30, _statementOptions.CommandType ?? CommandType.StoredProcedure, ex);
			}
		}

		private IDbConnection GetConnection() {
			try {
				var conn = _dbProvider.CreateConnection();
				conn.ConnectionString = _connectionString;
				conn.Open();
				return conn;
			} catch (Exception ex) {
				throw new DatabaseException("Failed opening connection " + _connectionKey, ex);
			}
		}

		private IDbCommand GetCommand(IDbConnection conn) {
			try {
				var cmd = conn.CreateCommand();
				cmd.CommandText = _sql;
				cmd.CommandType = _statementOptions.CommandType ?? CommandType.StoredProcedure;
				cmd.CommandTimeout = _statementOptions.TimeoutSeconds ?? 30;
				foreach (var param in _parameters) {
					cmd.Parameters.Add(param.Value.RealParameter);
				}
				return cmd;
			} catch (Exception ex) {
				throw new DatabaseException("Failed creating command " + _sql, ex);
			}
		}

		private T CallDb<T>(Func<IDbCommand, T> action) {
			using (var conn = GetConnection())
			using (var cmd = GetCommand(conn)) {
				try {
					return action(cmd);
				} catch (Exception ex) {
					throw new DatabaseException(_connectionKey, _sql, cmd.CommandTimeout, cmd.CommandType, ex);
				}
			}
		}

		private IEnumerable<IDataRecord> CallDb() {
			using (var conn = GetConnection())
			using (var cmd = GetCommand(conn))
			using (var reader = cmd.ExecuteReader()) {
				while (reader.Read()) {
					yield return reader;
				}
			}
		}

		private static T GetReaderData<T>(IDbCommand cmd, Func<IDataReader, T> map) {
			using (var reader = cmd.ExecuteReader()) {
				return map(reader);
			}
		}

		private static bool RunReader(IDbCommand cmd, Action<IDataReader> map) {
			using (var reader = cmd.ExecuteReader()) {
				map(reader);
			}
			return true;
		}
	}
}