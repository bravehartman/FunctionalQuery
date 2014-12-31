using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace FunctionalQuery.Helpers {
	static class DataSetConverter {
		public static DataSet GetDataSetFromRow(object row) {
			return GetDataSetFromTable(new[] {row});
		}

		public static DataSet GetDataSetFromTable(IEnumerable<object> table) {
			return GetDataSet(new [] { table });
		}
		
		public static DataSet GetDataSet(IEnumerable<IEnumerable<object>> dataSet) {
			var ds = new DataSet();
			foreach (var table in dataSet) {
				ds.Tables.Add(GetDataTable(table));
			}
			return ds;
		}

		public static DataTable GetDataTable(IEnumerable<object> tableRows) {
			if (tableRows == null) {
				return new DataTable();
			}
			var table = tableRows.ToArray();
			if (table.Length == 0 || (table.Length == 1 && table[0] == null)) {
				return new DataTable();
			}
			var type = table[0].GetType();
			var properties = type.GetProperties();
			var dt = CreateDataTable(properties);
			if (table.Length != 0) {
				foreach (var o in table) {
					AddRow(properties, dt, o);
				}
			}
			return dt;
		}

		private static DataTable CreateDataTable(IEnumerable<PropertyInfo> properties) {
			var dt = new DataTable();
			foreach (var pi in properties) {
				var propertyType = pi.PropertyType;
				if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) {
					propertyType = propertyType.GetGenericArguments()[0];
				}
				var dc = new DataColumn {
					ColumnName = pi.Name,
					DataType = propertyType
				};
				dt.Columns.Add(dc);
			}
			return dt;
		}

		private static void AddRow(IEnumerable<PropertyInfo> properties, DataTable dt, Object o) {
			var dr = dt.NewRow();
			foreach (var pi in properties) {
				dr[pi.Name] = pi.GetValue(o, null) ?? DBNull.Value;
			}
			dt.Rows.Add(dr);
		}
	}
}