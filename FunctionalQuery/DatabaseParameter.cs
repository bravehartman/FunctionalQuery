using System;
using System.Data;

namespace FunctionalQuery {
	public class DatabaseParameter {
		private object _value;
		public DbType? DbType { get; set; }
		public ParameterDirection? Direction { get; set; }
		public string ParameterName { get; set; }
		public object Value {
			get { return RealParameter != null ? NoDBNull(RealParameter.Value) : _value; }
			set { _value = NoDBNull(value); }
		}
		public byte? Precision { get; set; }
		public byte? Scale { get; set; }
		public int? Size { get; set; }
		internal IDbDataParameter RealParameter { get; set; }

		public DatabaseParameter() { }

		public DatabaseParameter(string paramenterName, object value) : this(paramenterName, value, null) { }

		public DatabaseParameter(string paramenterName, object value, ParameterDirection? direction, DbType? dbType = null, int? size = null, byte? precision = null, byte? scale = null) {
			DbType = dbType;
			Direction = direction;
			ParameterName = paramenterName;
			Value = value;
			Precision = precision;
			Scale = scale;
			Size = size;
		}
		
		public static DatabaseParameter In(object value, DbType? dbType = null, int? size = null, byte? precision = null, byte? scale = null) {
			return new DatabaseParameter(null, value, ParameterDirection.Input, dbType, size, precision, scale);
		}

		public static DatabaseParameter Out(DbType dbType, int? size = null, byte? precision = null, byte? scale = null) {
			return new DatabaseParameter(null, null, ParameterDirection.Output, dbType, size, precision, scale);
		}

		public static DatabaseParameter InOut(object value, DbType? dbType, int? size = null, byte? precision = null, byte? scale = null) {
			return new DatabaseParameter(null, value, ParameterDirection.InputOutput, dbType, size, precision, scale);
		}

		public static DatabaseParameter Return() {
			return new DatabaseParameter(null, null, ParameterDirection.ReturnValue, System.Data.DbType.Int32);
		}

		private object NoDBNull(object value) {
			return value == DBNull.Value ? null : value;
		}
	}
}