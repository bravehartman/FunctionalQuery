using System.Data;

namespace FunctionalQuery.Mocking {
	class MockDataAdapter : IDbDataAdapter {
		public DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType) {
			throw new System.NotImplementedException();
		}
		public int Fill(DataSet dataSet) {
			var ds = ((MockCommand)SelectCommand).GetData();
			foreach (DataTable table in ds.Tables) {
				dataSet.Tables.Add(table.Copy());
			}
			return 1;
		}
		public IDataParameter[] GetFillParameters() {
			throw new System.NotImplementedException();
		}

		public int Update(DataSet dataSet) {
			throw new System.NotImplementedException();
		}

		public MissingMappingAction MissingMappingAction { get; set; }
		public MissingSchemaAction MissingSchemaAction { get; set; }
		public ITableMappingCollection TableMappings { get; private set; }
		public IDbCommand SelectCommand { get; set; }
		public IDbCommand InsertCommand { get; set; }
		public IDbCommand UpdateCommand { get; set; }
		public IDbCommand DeleteCommand { get; set; }
	}
}