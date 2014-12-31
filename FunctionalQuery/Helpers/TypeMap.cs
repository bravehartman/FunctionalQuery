using System;
using System.Collections.Generic;
using System.Data;

namespace FunctionalQuery.Helpers {
	internal class TypeMap {

		private static readonly Dictionary<Type, DbType> DbTypeMap = new Dictionary<Type, DbType> {
			{typeof(bool), DbType.Boolean},
			{typeof(bool?), DbType.Boolean},
			{typeof(byte), DbType.Byte},
			{typeof(byte?), DbType.Byte},
			{typeof(char), DbType.String},  // DbType.StringFixedLength ?
			{typeof(char?), DbType.String}, // DbType.StringFixedLength ?
			{typeof(sbyte), DbType.Int16}, // SByte?
			{typeof(sbyte?), DbType.Int16}, // SByte?
			{typeof(short), DbType.Int16},
			{typeof(short?), DbType.Int16},
			{typeof(ushort), DbType.Int32}, // UInt16 ?
			{typeof(ushort?), DbType.Int32}, // UInt16 ?
			{typeof(int), DbType.Int32},
			{typeof(int?), DbType.Int32},
			{typeof(uint), DbType.Int64}, // UInt32 ?
			{typeof(uint?), DbType.Int64}, // UInt32 ?
			{typeof(long), DbType.Int64},
			{typeof(long?), DbType.Int64},
			{typeof(ulong), DbType.Decimal}, // UInt64 ?
			{typeof(ulong?), DbType.Decimal}, // UInt64 ?
			{typeof(float), DbType.Single},
			{typeof(float?), DbType.Single},
			{typeof(double), DbType.Double},
			{typeof(double?), DbType.Double},
			{typeof(decimal), DbType.Decimal},
			{typeof(decimal?), DbType.Decimal},
			{typeof(string), DbType.String},
			{typeof(Guid), DbType.Guid},
			{typeof(Guid?), DbType.Guid},
			{typeof(DateTime), DbType.DateTime},
			{typeof(DateTime?), DbType.DateTime},
			{typeof(DateTimeOffset), DbType.DateTimeOffset},
			{typeof(DateTimeOffset?), DbType.DateTimeOffset},
			{typeof(TimeSpan), DbType.Time},
			{typeof(TimeSpan?), DbType.Time},
			{typeof(byte[]), DbType.Binary},
			{typeof(object), DbType.Object}
		};
		private static readonly Dictionary<DbType, Option> OptionMap = new Dictionary<DbType, Option> {
			{DbType.AnsiString, new Option(null, null, null)},
			{DbType.AnsiStringFixedLength, new Option(null, null, null)},
			{DbType.Binary, new Option(null, null, null)},
			{DbType.Boolean, new Option(null, null, null)},
			{DbType.Byte, new Option(null, null, null)},
			{DbType.Currency, new Option(null, null, null)},
			{DbType.Date, new Option(null, null, null)},
			{DbType.DateTime, new Option(null, null, null)},
			{DbType.DateTime2, new Option(null, null, null)},
			{DbType.DateTimeOffset, new Option(null, null, null)},
			{DbType.Decimal, new Option(null, 19, 5)},
			{DbType.Double, new Option(null, null, null)},
			{DbType.Guid, new Option(null, null, null)},
			{DbType.Int16, new Option(null, null, null)},
			{DbType.Int32, new Option(null, null, null)},
			{DbType.Int64, new Option(null, null, null)},
			{DbType.Object, new Option(null, null, null)},
			{DbType.SByte, new Option(null, null, null)},
			{DbType.Single, new Option(null, null, null)},
			{DbType.String, new Option(null, null, null)},
			{DbType.StringFixedLength, new Option(null, null, null)},
			{DbType.Time, new Option(null, null, null)},
			{DbType.UInt16, new Option(null, null, null)},
			{DbType.UInt32, new Option(null, null, null)},
			{DbType.UInt64, new Option(null, null, null)},
			{DbType.VarNumeric, new Option(null, null, null)},
			{DbType.Xml, new Option(null, null, null)}
		};

		public class Option {
			public int? Size { get; set; }
			public byte? Precision { get; set; }
			public byte? Scale { get; set; }
			public Option(int? size, byte? precision, byte? scale) {
				Size = size;
				Precision = precision;
				Scale = scale;
			}
		}

		public static DbType GetDbType(Type type) {
			return DbTypeMap[type];
		}

		public static Option GetOptions(DbType type) {
			return OptionMap[type];
		}
	}
}
