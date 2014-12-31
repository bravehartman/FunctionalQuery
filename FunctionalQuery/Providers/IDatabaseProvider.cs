using System.Data;

namespace FunctionalQuery.Providers {
	public interface IDatabaseProvider {
		IDbDataParameter CreateParameter();
		IDbDataParameter CreateTableParameter();
		IDbConnection CreateConnection();
		IDbDataAdapter CreateDataAdapter();
	}
}
