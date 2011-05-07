using System;
using System.Collections.Generic;
using System.Text;
using Helper;
using System.Data;

namespace InfoLibrary
{
	public class InjectorInfoCollection : Info
	{
		ListIndexString<InjectorInfo> _inj;

		public InjectorInfoCollection()
		{

			_inj = new ListIndexString<InjectorInfo>();

			foreach (DataRow dr in DataSource.Rows)
				_inj.Add(new InjectorInfo(dr.ItemArray));
		}

		public InjectorInfo this[string inj]
		{
			get
			{
				InjectorInfo e;
				try
				{ e = _inj[inj]; }
				catch (KeyNotFoundException)
				{ e = null; }
				return e;
			}
		}

		internal DataTable DataSource
		{
			get { return base.Library.GetDataSource(Library.Tables.Injector); }
		}
	}
}
