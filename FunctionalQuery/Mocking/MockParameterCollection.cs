﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace FunctionalQuery.Mocking {
	class MockParameterCollection : IDataParameterCollection {
		private readonly Dictionary<string, IDbDataParameter> _parameters = new Dictionary<string, IDbDataParameter>();

		public bool Contains(object value) {
			throw new NotImplementedException();
		}

		public void Clear() {
			throw new NotImplementedException();
		}

		public int IndexOf(object value) {
			throw new NotImplementedException();
		}

		public void Insert(int index, object value) {
			throw new NotImplementedException();
		}

		public void Remove(object value) {
			throw new NotImplementedException();
		}

		public void RemoveAt(int index) {
			throw new NotImplementedException();
		}

		object IList.this[int index] {
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public bool IsReadOnly { get; private set; }
		public bool IsFixedSize { get; private set; }

		public IEnumerator GetEnumerator() {
			return _parameters.Values.GetEnumerator();
		}

		protected IDbDataParameter GetParameter(string parameterName) {
			return _parameters[parameterName];
		}

		public void CopyTo(Array array, int index) {
			throw new NotImplementedException();
		}

		public int Count { get; private set; }
		public object SyncRoot { get; private set; }
		public bool IsSynchronized { get; private set; }
		int IList.Add(object value) {
			var param = (IDbDataParameter)value;
			_parameters.Add(param.ParameterName, param );
			return 1;
		}

		public bool Contains(string parameterName) {
			throw new NotImplementedException();
		}

		public int IndexOf(string parameterName) {
			throw new NotImplementedException();
		}

		public void RemoveAt(string parameterName) {
			throw new NotImplementedException();
		}

		object IDataParameterCollection.this[string parameterName] {
			get { return _parameters[parameterName]; }
			set { _parameters[parameterName] = (IDbDataParameter)value; }
		}
	}
}
