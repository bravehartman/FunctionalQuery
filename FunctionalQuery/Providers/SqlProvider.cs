using System.Data;
using System.Data.SqlClient;

namespace FunctionalQuery.Providers {
	public class SqlProvider : IDatabaseProvider {

		public IDbDataParameter CreateParameter() {
			return new SqlParameter();
		}

		public IDbDataParameter CreateTableParameter() {
			return new SqlParameter {SqlDbType = SqlDbType.Structured};
		}

		public IDbConnection CreateConnection() {
			return new SqlConnection();
		}

		public IDbDataAdapter CreateDataAdapter() {
			return new SqlDataAdapter();
		}
	}
}
