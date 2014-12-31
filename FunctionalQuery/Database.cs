using System.Collections.Generic;
using System.Data;
using FunctionalQuery.Helpers;
using FunctionalQuery.Providers;

namespace FunctionalQuery {
	public class Database : IDatabase {
		private readonly ConnectionDictionary _connections;
		private readonly StatementOptions _defaultStatementOptions;
		protected IDatabaseProvider DbProvider { get; private set; }

		public Database(CommandType? commandType) : this(null, null, null, null, commandType) { }

		public Database(int? timeoutSeconds, CommandType? commandType = null) : this(null, null, null, timeoutSeconds, commandType) { }

		public Database(IDatabaseProvider provider, int? timeoutSeconds = null, CommandType? commandType = null):this(provider, null, null, timeoutSeconds, commandType) {}

		public Database(string defaultConnectionString, int? timeoutSeconds = null, CommandType? commandType = null) : this(null, null, defaultConnectionString, timeoutSeconds, commandType) { }

		public Database(IDatabaseProvider provider, string defaultConnectionString, int? timeoutSeconds = null, CommandType? commandType = null) : this(provider, null, defaultConnectionString, timeoutSeconds, commandType) { }

		public Database(IEnumerable<KeyValuePair<string, string>> connectionStrings, int? timeoutSeconds = null, CommandType? commandType = null) : this(null, connectionStrings, null, timeoutSeconds, commandType) { }

		public Database(IDatabaseProvider provider, IEnumerable<KeyValuePair<string, string>> connectionStrings, int? timeoutSeconds = null, CommandType? commandType = null) : this(provider, connectionStrings, null, timeoutSeconds, commandType) { }

		public Database(IEnumerable<KeyValuePair<string, string>> connectionStrings, string defaultConnectionString, int? timeoutSeconds = null, CommandType? commandType = null): this(null, connectionStrings, defaultConnectionString, timeoutSeconds, commandType) { }

		public Database(IDatabaseProvider provider = null, IEnumerable<KeyValuePair<string, string>> connectionStrings = null, string defaultConnectionString = null, int? timeoutSeconds = null, CommandType? commandType = null) {
			DbProvider = provider ?? new SqlProvider();
			_connections = new ConnectionDictionary(connectionStrings, defaultConnectionString);
			_defaultStatementOptions = new StatementOptions {
				TimeoutSeconds = timeoutSeconds,
				CommandType = commandType
			};
		}

		protected string GetConnectionString(string connectionKey) {
			return _connections[connectionKey];
		}

		private StatementOptions GetOptions(StatementOptions options) {
			var opts = options ?? _defaultStatementOptions;
			return new StatementOptions {
				CommandType = opts.CommandType ?? _defaultStatementOptions.CommandType ?? CommandType.StoredProcedure,
				TimeoutSeconds = opts.TimeoutSeconds ?? _defaultStatementOptions.TimeoutSeconds ?? 30,
				HasXmlResults = opts.HasXmlResults ?? _defaultStatementOptions.HasXmlResults ?? false
			};
		}

		public Query Query(string sql, object parameters = null, StatementOptions options = null) {
			return Query(null, sql, parameters, options);
		}

		public Query Query(string connectionKey, string sql, object parameters = null, StatementOptions options = null) {
			return new Query(DbProvider, _connections[connectionKey], connectionKey, sql, ParameterConverter.CreateParameters(DbProvider, parameters), GetOptions(options));
		}

	}
}