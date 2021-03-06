﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Helper
{
	class BaseSortedCollection<T> : Collection<T>, ICollection<T>, IEnumerable<T>,
		System.Collections.ICollection, System.Collections.IEnumerable
		where T : IComparable<T>
	{
		/// <summary>
		///     Adds an item to the Collection<T> at the correct position.
		/// </summary>
		/// <param name="item">The object to add to </param>
		public new void Add (T item)
		{
			int pos = GetInsertPositio (item);
			base.InsertItem (pos, item);
		}

		public new void Remove (T item)
		{
			List<T> lstToRemove = new List<T> ();
			foreach (T i in Items)
				if (i.Equals (item))
					lstToRemove.Add (i);
			foreach (T i in lstToRemove)
				base.Remove (i);
		}

		public IList<T> Itemss {
			get { return base.Items; }
		}

		/// <summary>
		/// Convinience function to add variable number of items in one Functioncall
		/// </summary>
		/// <param name="itemsToBeAdded">The items to be added.</param>
		/// <returns>this to allow fluent interface</returns>
		public BaseSortedCollection<T> AddItems (params T[] itemsToBeAdded)
		{
			foreach (T item in itemsToBeAdded)
				Add (item);
			return this;
		}

		/// <summary>
		/// Get position where item should be inserted.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>Get position where item should be inserted.</returns>
		private int GetInsertPositio (T item)
		{
			if (item == null)
				throw new ArgumentNullException ();

			for (int pos = this.Count - 1; pos >= 0; pos--) {
				if (item.CompareTo (this.Items [pos]) > 0)
					return pos + 1;
			}

			return 0;
		}
	}
}
