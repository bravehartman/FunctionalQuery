using System.Collections.Generic;
using System.Data;

namespace FunctionalQuery.Mocking {
	class MockDataSet {
		public DataSet DataSet { get; set; }
		public Dictionary<string, DatabaseParameter> Parameters { get; set; }
	}
}
