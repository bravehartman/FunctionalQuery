using System.Data;

namespace FunctionalQuery.Mocking {
	class MockConnection : IDbConnection {
		private readonly MockResults _results;
		public MockConnection(MockResults results) {
			_results = results;
		}
		public void Dispose() {}
		public IDbTransaction BeginTransaction() {
			throw new System.NotImplementedException();
		}
		public IDbTransaction BeginTransaction(IsolationLevel il) {
			throw new System.NotImplementedException();
		}
		public void Close() {}
		public void ChangeDatabase(string databaseName) {}
		public IDbCommand CreateCommand() {
			return new MockCommand(GetNextResult){Connection = this};
		}
		public void Open() {}
		public string ConnectionString { get; set; }
		public int ConnectionTimeout { get; private set; }
		public string Database { get; private set; }
		public ConnectionState State { get; private set; }

		private MockDataSet GetNextResult(string connectionString, string sql) {
			if (!_results.ContainsKey(connectionString)) {
				throw new DatabaseException(string.Format("Database with connection string {0} was not set up enough times in ReturnRow/ReturnTable/ReturnDataSet", connectionString));
			}
			var connData = _results[connectionString];
			if (!connData.ContainsKey(sql) || connData[sql].Count == 0) {
				throw new DatabaseException(string.Format("SQL {0} was not set up enough times in ReturnRow/ReturnTable/ReturnDataSet", sql));
			}
			var sqlData = connData[sql];
			var result = sqlData[0];
			sqlData.RemoveAt(0);
			return result;
		}
	}
}
