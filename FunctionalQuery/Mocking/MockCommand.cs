using System;
using System.Data;
using System.Data.OleDb;

namespace FunctionalQuery.Mocking {
	class MockCommand : IDbCommand {
		private readonly Func<string, string, MockDataSet> _getData;
		public MockCommand(Func<string, string, MockDataSet> getData) {
			_getData = getData;
			Parameters = new MockParameterCollection();
		}

		public DataSet GetData() {
			var data = _getData(Connection.ConnectionString, CommandText);
			foreach (var param in data.Parameters.Values) {
				((IDbDataParameter)Parameters[param.ParameterName]).Value = param.Value;
			}
			return data.DataSet;
		}
		public void Dispose() {}
		public void Prepare() {}
		public void Cancel() {}
		public IDbDataParameter CreateParameter() {return new OleDbParameter();}
		public int ExecuteNonQuery() {
			GetData();
			return 0;
		}
		public IDataReader ExecuteReader() {
			return new MockDataReader(GetData());
		}
		public IDataReader ExecuteReader(CommandBehavior behavior) {
			return new MockDataReader(GetData());
		}
		public object ExecuteScalar() {
			GetData();
			return 0;
		}
		public IDbConnection Connection { get; set; }
		public IDbTransaction Transaction { get; set; }
		public string CommandText { get; set; }
		public int CommandTimeout { get; set; }
		public CommandType CommandType { get; set; }
		public IDataParameterCollection Parameters { get; private set; }
		public UpdateRowSource UpdatedRowSource { get; set; }
	}
}
