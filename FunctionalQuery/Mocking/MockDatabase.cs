using System.Collections.Generic;
using System.Data;

namespace FunctionalQuery.Mocking {
	public class MockDatabase : Database {

		public MockDatabase(string defaultConnectionString = null) : base(new MockProvider(), defaultConnectionString, null, null) { }

		public MockDatabase(IEnumerable<KeyValuePair<string, string>> connectionStrings):base(new MockProvider(), connectionStrings, null, null) {}

		public MockDatabase(IEnumerable<KeyValuePair<string, string>> connectionStrings, string defaultConnectionString): base(new MockProvider(), connectionStrings, defaultConnectionString, null, null) {}

		#region Results Pass-through functions

		private MockResults Results { get { return ((MockProvider) DbProvider).Results; } }

		public void ReturnRow(string sql, object row, object parameters = null) {
			ReturnRow(null, sql, row, parameters);
		}

		public void ReturnRow(string connectionKey, string sql, object row, object parameters = null) {
			Results.ReturnRow(GetConnectionString(connectionKey), sql, row, parameters);
		}

		public void ReturnTable(string sql, IEnumerable<object> table, object parameters = null) {
			ReturnTable(null, sql, table, parameters);
		}

		public void ReturnTable(string connectionKey, string sql, IEnumerable<object> table, object parameters = null) {
			Results.ReturnTable(GetConnectionString(connectionKey), sql, table, parameters);
		}

		public void ReturnTable(string sql, DataTable table, object parameters = null) {
			ReturnTable(null, sql, table, parameters);
		}

		public void ReturnTable(string connectionKey, string sql, DataTable table, object parameters = null) {
			Results.ReturnTable(GetConnectionString(connectionKey), sql, table, parameters);
		}

		public void ReturnDataSet(string sql, IEnumerable<IEnumerable<object>> dataSet, object parameters = null) {
			ReturnDataSet(null, sql, dataSet, parameters);
		}

		public void ReturnDataSet(string connectionKey, string sql, IEnumerable<IEnumerable<object>> dataSet, object parameters = null) {
			Results.ReturnDataSet(GetConnectionString(connectionKey), sql, dataSet, parameters);
		}

		public void ReturnDataSet(string sql, DataSet dataSet, object parameters = null) {
			ReturnDataSet(null, sql, dataSet, parameters);
		}

		public void ReturnDataSet(string connectionKey, string sql, DataSet dataSet, object parameters = null) {
			Results.ReturnDataSet(GetConnectionString(connectionKey), sql, dataSet, parameters);
		}
		#endregion
	}
}