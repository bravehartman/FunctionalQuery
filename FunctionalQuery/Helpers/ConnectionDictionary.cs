using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace FunctionalQuery.Helpers {
	internal class ConnectionDictionary {
		private readonly string _defaultConnectionString;

		private readonly Dictionary<string, string> _connectionStrings;

		public ConnectionDictionary() : this(null, null) { }

		public ConnectionDictionary(string defaultConnectionString) : this(null, defaultConnectionString) { }

		public ConnectionDictionary(IEnumerable<KeyValuePair<string, string>> connectionStrings) : this(connectionStrings, null) { }

		public ConnectionDictionary(IEnumerable<KeyValuePair<string, string>> connectionStrings, string defaultConnectionString) {
			_defaultConnectionString = string.IsNullOrWhiteSpace(defaultConnectionString) ? null : defaultConnectionString.Trim();
			if (connectionStrings == null) {
				connectionStrings = ReadFromConfig();
			}
			_connectionStrings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			foreach (var connString in connectionStrings) {
				_connectionStrings.Add(connString.Key.Trim(), connString.Value.Trim());
			}
			if (_defaultConnectionString == null && _connectionStrings.Keys.Count == 1) {
				_defaultConnectionString = _connectionStrings.Values.First();
			}
		}

		public string this[string connectionKey] {
			get {
				var key = GetConnectionKeyOrDefault(connectionKey);
				if (_connectionStrings != null && _connectionStrings.ContainsKey(key)) {
					return _connectionStrings[key].Trim();
				}
				return key;
			}
		}

		private string GetConnectionKeyOrDefault(string connectionKey) {
			if (string.IsNullOrWhiteSpace(connectionKey)) {
				if (_defaultConnectionString == null) {
					throw new DatabaseException("No connection provided.  Add connection string(s) to constructor or pass it into the function");
				}
				return _defaultConnectionString;
			}
			return connectionKey.Trim();
		}

		public static IEnumerable<KeyValuePair<string, string>> ReadFromConfig() {
			var connectionStrings = ConfigurationManager.ConnectionStrings;
			var conns = new Dictionary<string, string>();
			foreach (ConnectionStringSettings connectionString in connectionStrings) {
				conns[connectionString.Name] = connectionString.ConnectionString;
			}
			return conns;
		}
	}
}
