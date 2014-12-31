using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;

namespace FunctionalQuery.Providers {
	public class OleProvider : IDatabaseProvider {

		public IDbDataParameter CreateParameter() {
			return new OleDbParameter();
		}

		public IDbDataParameter CreateTableParameter() {
			throw new System.NotImplementedException();
		}

		public IDbConnection CreateConnection() {
			return new OleDbConnection();
		}

		public IDbDataAdapter CreateDataAdapter() {
			return new OleDbDataAdapter();
		}
	}
}
