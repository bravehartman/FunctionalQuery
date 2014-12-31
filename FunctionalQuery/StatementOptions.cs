using System.Data;

namespace FunctionalQuery {
	public class StatementOptions {
		public int? TimeoutSeconds { get; set; }
		public CommandType? CommandType { get; set; }
		public bool? HasXmlResults { get; set; }
	}
}
