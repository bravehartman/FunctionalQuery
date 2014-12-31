using System;
using System.Collections.Generic;

namespace FunctionalQuery.Mocking {
	class MockConnectionResults : Dictionary<string, MockSqlResults> {
		public MockConnectionResults() : base(StringComparer.OrdinalIgnoreCase) { }
	}
}
