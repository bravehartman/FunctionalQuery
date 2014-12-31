using System;
using System.Collections.Generic;
using System.Data;
using FunctionalQuery.Helpers;

namespace FunctionalQuery.Mocking {
	class MockResults : Dictionary<string, MockConnectionResults> {
		public MockResults() : base(StringComparer.OrdinalIgnoreCase){}

		public void ReturnRow(string connectionKey, string sql, object row, object parameters = null) {
			ReturnDataSet(connectionKey, sql, DataSetConverter.GetDataSetFromRow(row), parameters);
		}

		public void ReturnTable(string connectionKey, string sql, IEnumerable<object> table, object parameters = null) {
			ReturnDataSet(connectionKey, sql, DataSetConverter.GetDataSetFromTable(table), parameters);
		}

		public void ReturnTable(string connectionKey, string sql, DataTable table, object parameters = null) {
			var ds = new DataSet();
			ds.Tables.Add(table);
			ReturnDataSet(connectionKey, sql, ds, parameters);
		}

		public void ReturnDataSet(string connectionKey, string sql, IEnumerable<IEnumerable<object>> dataSet, object parameters = null) {
			ReturnDataSet(connectionKey, sql, DataSetConverter.GetDataSet(dataSet), parameters);
		}

		public void ReturnDataSet(string connectionKey, string sql, DataSet dataSet, object parameters = null) {
			var connKey = connectionKey.Trim();
			if (!ContainsKey(connKey)) {
				Add(connKey, new MockConnectionResults());
			}
			var connData = this[connKey];
			var sqlKey = sql.Trim();
			if (!connData.ContainsKey(sqlKey)) {
				connData.Add(sqlKey, new MockSqlResults());
			}
			connData[sql].Add( new MockDataSet{DataSet = dataSet, Parameters = ParameterConverter.CreateParameters(new MockProvider(), parameters)});
		}

	}
}
