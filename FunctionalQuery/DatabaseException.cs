using System;
using System.Data;

namespace FunctionalQuery {
	public class DatabaseException : Exception {

		public DatabaseException(string message) : base(message) { }

		public DatabaseException(string message, Exception inner) : base(message, inner) { }

		public DatabaseException(string columnName, Type type, Exception inner) : base(string.Format("Failed reading column {0}, type {1}", columnName, type.Name), inner) { }

		public DatabaseException(string connectionKey, string sql, int timeoutSeconds, CommandType commandType, Exception inner)
			: base(string.Format("Failed calling statement '{0}' as {1} in database '{2}' with timeout {3} seconds. {4}", sql, commandType, connectionKey, timeoutSeconds, inner.Message), inner) { }
	}
}
