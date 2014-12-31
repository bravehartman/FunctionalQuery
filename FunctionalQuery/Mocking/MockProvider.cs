using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using FunctionalQuery.Providers;

namespace FunctionalQuery.Mocking {
	class MockProvider : IDatabaseProvider {

		public MockResults Results = new MockResults();
		
		public IDbDataParameter CreateParameter() {
			return new OleDbParameter();
		}

		public IDbDataParameter CreateTableParameter() {
			return new SqlParameter{SqlDbType = SqlDbType.Structured};
		}

		public IDbConnection CreateConnection() {
			return new MockConnection(Results);
		}

		public IDbDataAdapter CreateDataAdapter() {
			return new MockDataAdapter();
		}

	}
}
