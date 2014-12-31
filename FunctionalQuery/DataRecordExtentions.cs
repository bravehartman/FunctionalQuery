using System;
using System.Data;
using System.Globalization;

namespace FunctionalQuery {
	public static class DataRecordExtentions {

		public static T? Get<T>(this IDataRecord row, string columnName) where T: struct {
			return GetStructOrNull<T>(row, columnName, null);
		}

		public static T? Get<T>(this IDataRecord row, int ordinal) where T : struct {
			return GetStructOrNull<T>(row, null, ordinal);
		}

		// c# compiler won't let me overload Get<T> for classes
		public static string Get(this IDataRecord row, string columnName) {
			return GetObjectOrNull<string>(row, columnName, null);
		}

		// c# compiler won't let me overload Get<T> for classes
		public static string Get(this IDataRecord row, int ordinal) {
			return GetObjectOrNull<string>(row, null, ordinal);
		}

		private static T GetObjectOrNull<T>(IDataRecord row, string columnName, int? ordinal) where T : class {
			Func<IDataRecord, int, T> action = (r, idx) => row.IsDBNull(idx) ? (T)null : (T)row.GetValue(idx);
			return GetDataOrNull(row, columnName, ordinal, action);
		}

		private static T? GetStructOrNull<T>(IDataRecord row, string columnName, int? ordinal) where T : struct {
			Func<IDataRecord, int, T?> action = (r, idx) => row.IsDBNull(idx) ? null : (T?)row.GetValue(idx);
			return GetDataOrNull(row, columnName, ordinal, action);
		}

		private static T GetDataOrNull<T>(IDataRecord row, string columnName, int? ordinal, Func<IDataRecord, int, T> converter) {
			var idx = GetOrdinal(row, columnName, ordinal);
			try {
				return converter(row, idx);
			} catch (Exception ex) {
				throw new DatabaseException(columnName ?? ordinal.Value.ToString(), typeof(T), ex);
			}
		}

		private static int GetOrdinal(IDataRecord row, string columnName, int? ordinal) {
			if (ordinal == null) {
				try {
					ordinal = row.GetOrdinal(columnName);
				} catch (Exception ex) {
					throw new DatabaseException(string.Format("Failed reading column: {0}. Column not found", columnName ?? ordinal.Value.ToString()), ex);
				}
			}
			if (ordinal < 0 || ordinal >= row.FieldCount) {
				throw new DatabaseException(string.Format("Failed reading column: {0}. Column not found", columnName ?? ordinal.Value.ToString()));
			}
			return ordinal.Value;
		}

		/*
		public static bool? GetBooleanOrNull(this IDataRecord record, string columnName) {
			return GetDataOrNull<bool>(record, columnName, r => r.GetBoolean);
		}

		public static bool? GetBooleanOrNull(this IDataRecord record, int columnIndex) {
			return GetDataOrNull<bool>(record, columnIndex, r => r.GetBoolean);
		}

		public static byte? GetByteOrNull(this IDataRecord record, string columnName) {
			return GetDataOrNull<byte>(record, columnName, r => r.GetByte);
		}

		public static byte? GetByteOrNull(this IDataRecord record, int columnIndex) {
			return GetDataOrNull<byte>(record, columnIndex, r => r.GetByte);
		}

		public static char? GetCharOrNull(this IDataRecord record, string columnName) {
			return GetDataOrNull<char>(record, columnName, r => r.GetChar);
		}

		public static char? GetCharOrNull(this IDataRecord record, int columnIndex) {
			return GetDataOrNull<char>(record, columnIndex, r => r.GetChar);
		}

		public static DateTime? GetDateTimeOrNull(this IDataRecord record, string columnName) {
			return GetDataOrNull<DateTime>(record, columnName, r => r.GetDateTime);
		}

		public static DateTime? GetDateTimeOrNull(this IDataRecord record, int columnIndex) {
			return GetDataOrNull<DateTime>(record, columnIndex, r => r.GetDateTime);
		}

		public static decimal? GetDecimalOrNull(this IDataRecord record, string columnName) {
			return GetDataOrNull<decimal>(record, columnName, r => r.GetDecimal);
		}

		public static decimal? GetDecimalOrNull(this IDataRecord record, int columnIndex) {
			return GetDataOrNull<decimal>(record, columnIndex, r => r.GetDecimal);
		}

		public static double? GetDoubleOrNull(this IDataRecord record, string columnName) {
			return GetDataOrNull<double>(record, columnName, r => r.GetDouble);
		}

		public static double? GetDoubleOrNull(this IDataRecord record, int columnIndex) {
			return GetDataOrNull<double>(record, columnIndex, r => r.GetDouble);
		}

		public static Guid? GetGuidOrNull(this IDataRecord record, string columnName) {
			return GetDataOrNull<Guid>(record, columnName, r => r.GetGuid);
		}

		public static Guid? GetGuidOrNull(this IDataRecord record, int columnIndex) {
			return GetDataOrNull<Guid>(record, columnIndex, r => r.GetGuid);
		}

		public static float? GetFloatOrNull(this IDataRecord record, string columnName) {
			return GetDataOrNull<float>(record, columnName, r => r.GetFloat);
		}

		public static float? GetFloatOrNull(this IDataRecord record, int columnIndex) {
			return GetDataOrNull<float>(record, columnIndex, r => r.GetFloat);
		}

		public static short? GetInt16OrNull(this IDataRecord record, string columnName) {
			return GetDataOrNull<short>(record, columnName, r => r.GetInt16);
		}

		public static short? GetInt16OrNull(this IDataRecord record, int columnIndex) {
			return GetDataOrNull<short>(record, columnIndex, r => r.GetInt16);
		}

		public static int? GetInt32OrNull(this IDataRecord record, string columnName) {
			return GetDataOrNull<int>(record, columnName, r => r.GetInt32);
		}

		public static int? GetInt32OrNull(this IDataRecord record, int columnIndex) {
			return GetDataOrNull<int>(record, columnIndex, r => r.GetInt32);
		}

		public static long? GetInt64OrNull(this IDataRecord record, string columnName) {
			return GetDataOrNull<long>(record, columnName, r => r.GetInt64);
		}

		public static long? GetInt64OrNull(this IDataRecord record, int columnIndex) {
			return GetDataOrNull<long>(record, columnIndex, r => r.GetInt64);
		}

		public static string GetStringOrNull(this IDataRecord record, string columnName) {
			return GetObjectOrNull<string>(record, columnName, r => r.GetString);
		}

		public static string GetStringOrNull(this IDataRecord record, int columnIndex) {
			return GetObjectOrNull<string>(record, columnIndex, r => r.GetString);
		}

		public static object GetValueOrNull(this IDataRecord record, string columnName) {
			return GetObjectOrNull<object>(record, columnName, r => r.GetValue);
		}

		public static object GetValueOrNull(this IDataRecord record, int columnIndex) {
			return GetObjectOrNull<object>(record, columnIndex, r => r.GetValue);
		}

		private static T? GetDataOrNull<T>(IDataRecord record, int columnIndex, Func<IDataRecord, Func<int, T>> converter) where T : struct {
			return GetDataOrNull(record, columnIndex, columnIndex.ToString(), converter);
		}

		private static T? GetDataOrNull<T>(IDataRecord record, string columnName, Func<IDataRecord, Func<int, T>> converter) where T : struct {
			var idx = record.GetOrdinal(columnName);
			return GetDataOrNull(record, idx, columnName, converter);
		}

		private static T? GetDataOrNull<T>(IDataRecord record, int ordinal, string columnLogName, Func<IDataRecord, Func<int, T>> converter) where T : struct {
			Func<IDataRecord, int, T?> action = (r, idx) => record.IsDBNull(ordinal) ? null : (T?)converter(record)(ordinal);
			return GetDataOrNull(record, ordinal, columnLogName, action);
		}
		
		private static T GetObjectOrNull<T>(IDataRecord record, string columnName, Func<IDataRecord, Func<int, T>> converter) {
			var idx = record.GetOrdinal(columnName);
			return GetObjectOrNull(record, idx, columnName, converter);
		}

		private static T GetObjectOrNull<T>(IDataRecord record, int columnIndex, Func<IDataRecord, Func<int, T>> converter) {
			return GetObjectOrNull(record, columnIndex, null, converter);
		}
		
		private static T GetObjectOrNull<T>(IDataRecord record, int columnIndex, string columnLogName, Func<IDataRecord, Func<int, T>> converter) {
			Func<IDataRecord, int, T> action = (r, idx) => record.IsDBNull(columnIndex) ? default(T) : converter(record)(columnIndex);
			return GetDataOrNull(record, columnIndex, columnLogName, action);
		}
		*/


		public static string GetFormatedValue(this IDataRecord row, string columnName, string format = null, IFormatProvider provider = null) {
			return GetFormatedValue(row, columnName, null, format);
		}

		public static string GetFormatedValue(this IDataRecord row, int ordinal, string format = null, IFormatProvider provider = null) {
			return GetFormatedValue(row, null, ordinal, format);
		}

		private static string GetFormatedValue(this IDataRecord row, string columnName, int? ordinal, string format = null, IFormatProvider provider = null) {
			var idx = GetOrdinal(row, columnName, ordinal);
			if (row.IsDBNull(idx)) {
				return "";
			}
			var type = row.GetDataTypeName(idx);
			try {
				switch (type) {
					case "char":
					case "varchar":
						return row.GetString(idx);
					case "tinyint":
						return row.GetByte(idx).ToString(format ?? "0",  provider ?? CultureInfo.InvariantCulture);
					case "int":
						return row.GetInt32(idx).ToString(format ?? "0", provider ?? CultureInfo.InvariantCulture);
					case "money":
						return row.GetDecimal(idx).ToString(format ?? "#.00", provider ?? CultureInfo.InvariantCulture);
					case "date":
					case "datetime":
						return row.GetDateTime(idx).ToString(format ?? "d", provider ?? CultureInfo.InvariantCulture);
					case "decimal":
						return row.GetDecimal(idx).ToString(format ?? "#.0000", provider ?? CultureInfo.InvariantCulture);
				}
			} catch (Exception ex) {
				throw new DatabaseException(string.Format("Failed formatting column: {0}, type: {1}", columnName, type), ex);
			}
			throw new DatabaseException(string.Format("Failed formatting column: {0}, type: {1}. Type not supported", columnName, type));
		}

	}
}