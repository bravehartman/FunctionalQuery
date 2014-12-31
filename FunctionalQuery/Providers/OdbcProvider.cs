using System.Data;
using System.Data.Odbc;

namespace FunctionalQuery.Providers {
	public class OdbcProvider : IDatabaseProvider {
		public IDbDataParameter CreateParameter() {
			return new OdbcParameter();
		}

		public IDbDataParameter CreateTableParameter() {
			throw new System.NotImplementedException();
		}

		public IDbConnection CreateConnection() {
			return new OdbcConnection();
		}

		public IDbDataAdapter CreateDataAdapter() {
			return new OdbcDataAdapter();
		}
	}
}
