using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace FRom
{
	/// <summary>
	/// Типизированный класс на базе List, 
	/// позволяющий индексировать по string, сравниваясь с  T.ToString()
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ListIndexString<T> : List<T>
	{
		public T this[string index]
		{
			get
			{
				Enumerator e = GetEnumerator();
				e.MoveNext();
				while (e.Current != null)
				{
					if (index == e.Current.ToString())
						return e.Current;
					e.MoveNext();
				}

				throw new KeyNotFoundException();
			}
		}

		public ListIndexString(int capacity)
			: base(capacity) { }

		public ListIndexString()
			: base() { }
	}
}
