using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using FunctionalQuery.Providers;

namespace FunctionalQuery.Helpers {
	internal static class ParameterConverter {
		public static Dictionary<string, DatabaseParameter> CreateParameters(IDatabaseProvider dbProvider, object parameters) {
			if (parameters == null) {
				return new Dictionary<string, DatabaseParameter>();
			}

			var type = parameters.GetType();
			var propertyInfos = type.GetProperties();
			var paramDictionary = new Dictionary<string, DatabaseParameter>(StringComparer.OrdinalIgnoreCase);
			foreach (var param in propertyInfos.Select(pi => CreateParameter(dbProvider, parameters, pi))) {
				paramDictionary.Add(param.ParameterName, param);
			}
			return paramDictionary;
		}

		private static DatabaseParameter CreateParameter(IDatabaseProvider dbProvider, object obj, PropertyInfo propertyInfo) {
			var parameterName = "@" + propertyInfo.Name;
			var value = propertyInfo.GetValue(obj, null);
			if (value is DatabaseParameter) {
				var param = (DatabaseParameter) value;
				param.ParameterName = parameterName;
				var valueType = GetPropertyType(param.Value == null ? typeof (object) : param.Value.GetType());
				return AddRealParameter(dbProvider, param, valueType);
			}
			return AddRealParameter(dbProvider, new DatabaseParameter {ParameterName = parameterName, Value = value, Direction = ParameterDirection.Input}, GetPropertyType(propertyInfo));
		}

		private static DatabaseParameter AddRealParameter(IDatabaseProvider dbProvider, DatabaseParameter param, Type valueType) {
			var isList = IsList(valueType);
			var isTable = isList || param.Value is DataTable;
			var realParameter = isTable ? dbProvider.CreateTableParameter() : dbProvider.CreateParameter();

			realParameter.ParameterName = param.ParameterName;
			realParameter.Value = isList ? DataSetConverter.GetDataTable((IEnumerable<object>)param.Value) : param.Value ?? DBNull.Value;
			realParameter.Direction = param.Direction ?? ParameterDirection.Input;
			if (!isTable) {
				realParameter.DbType = param.DbType ?? TypeMap.GetDbType(valueType);
				var defaultOptions = TypeMap.GetOptions(realParameter.DbType);
				realParameter.Size = param.Size ?? defaultOptions.Size ?? 0;
				realParameter.Precision = param.Precision ?? defaultOptions.Precision ?? 0;
				realParameter.Scale = param.Scale ?? defaultOptions.Scale ?? 0;
			}
			param.RealParameter = realParameter;
			return param;
		}

		private static bool IsList(this Type type) {
			if (type == null || type == typeof (string)) {
				return false;
			}
			return typeof(IEnumerable).IsAssignableFrom(type);
		}

		private static Type GetPropertyType(PropertyInfo propertyInfo) {
			return GetPropertyType(propertyInfo.PropertyType);
		}

		private static Type GetPropertyType(Type type) {
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>)) {
				return type.GetGenericArguments()[0];
			}
			return type;
		}
	}
}