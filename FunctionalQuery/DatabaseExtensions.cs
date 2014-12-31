using System.Collections.Generic;

namespace FunctionalQuery {
	public static class DatabaseExtensions {
		public static IDictionary<string, DatabaseParameter> Execute(this IDatabase db, string sql, object parameters = null, StatementOptions options = null) {
			return db.Query(sql, parameters, options).Execute();
		}

		public static IDictionary<string, DatabaseParameter> Execute(this IDatabase db, string connectionKey, string sql, object parameters = null, StatementOptions options = null) {
			return db.Query(connectionKey, sql, parameters, options).Execute();
		}
	}
}
