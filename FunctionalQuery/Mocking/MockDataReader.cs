using System;
using System.Data;

namespace FunctionalQuery.Mocking {
	class MockDataReader : IDataReader {
		private readonly DataSet _dataSet;
		private DataTable _table;
		private DataRow _row;
		private int _tableIndex = -1;
		private int _rowIndex = -1;

		public MockDataReader(DataSet data) {
			_dataSet = data;
		}
		
		public void Dispose() {
			Close();
		}

		public string GetName(int i) {
			return _table.Columns[i].ColumnName;
		}

		public string GetDataTypeName(int i) {
			return _table.Columns[i].DataType.Name;
		}

		public Type GetFieldType(int i) {
			return _table.Columns[i].DataType;
		}

		public object GetValue(int i) {
			return GetValue<object>(i);
		}

		public int GetValues(object[] values) {
			throw new NotImplementedException();
		}

		public int GetOrdinal(string name) {
			return _table.Columns[name].Ordinal;
		}

		private T GetValue<T>(int i) {
			return (T)_row[i];
		}

		public bool GetBoolean(int i) {
			return GetValue<bool>(i);
		}

		public byte GetByte(int i) {
			return GetValue<byte>(i);
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) {
			throw new NotImplementedException();
		}

		public char GetChar(int i) {
			return GetValue<char>(i);
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) {
			throw new NotImplementedException();
		}

		public Guid GetGuid(int i) {
			return GetValue<Guid>(i);
		}

		public short GetInt16(int i) {
			return GetValue<short>(i);
		}

		public int GetInt32(int i) {
			return GetValue<int>(i);
		}

		public long GetInt64(int i) {
			return GetValue<long>(i);
		}

		public float GetFloat(int i) {
			return GetValue<float>(i);
		}

		public double GetDouble(int i) {
			return GetValue<double>(i);
		}

		public string GetString(int i) {
			return GetValue<string>(i);
		}

		public decimal GetDecimal(int i) {
			return GetValue<decimal>(i);
		}

		public DateTime GetDateTime(int i) {
			return GetValue<DateTime>(i);
		}

		public IDataReader GetData(int i) {
			throw new NotImplementedException();
		}

		public bool IsDBNull(int i) {
			return _row[i] == null || _row[i] == DBNull.Value;
		}

		public int FieldCount { get { return _row.Table.Columns.Count; } }

		object IDataRecord.this[int i] {
			get { return GetValue<object>(i); }
		}

		object IDataRecord.this[string name] {
			get { return GetValue<object>(GetOrdinal(name)); }
		}

		public void Close() {
			IsClosed = true;
		}

		public DataTable GetSchemaTable() {
			var table = new DataTable();
			table.Columns.Add("ColumnName", typeof(string));
			foreach (DataColumn column in _dataSet.Tables[0].Columns) {
				var row = table.NewRow();
				row[0] = column.ColumnName;
				table.Rows.Add(row);
			}
			return table;
		}

		public bool NextResult() {
			_tableIndex++;
			if (_dataSet == null || _dataSet.Tables.Count <= _tableIndex) {
				return false;
			}
			_table = _dataSet.Tables[_tableIndex];
			_rowIndex = -1;
			return true;
		}

		public bool Read() {
			if (_table == null && ! NextResult()) {
				return false;
			}
			_rowIndex++;
			if (_table == null || _table.Rows == null || _table.Rows.Count <= _rowIndex) {
				return false;
			}
			_row = _table.Rows[_rowIndex];
			return _row != null;
		}

		public int Depth { get; private set; }
		public bool IsClosed { get; private set; }
		public int RecordsAffected { get; private set; }
	}
}
