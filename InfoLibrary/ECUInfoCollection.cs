using System;
using System.Collections.Generic;
using System.Text;
using Helper;
using System.Data;

namespace InfoLibrary
{
	public class ECUInfoCollection
	{
		ListIndexString<ECUInfo> _ecu;
		Library _datasource;

		public ECUInfoCollection(Library lib)
		{
			_datasource = lib;
			_ecu = new ListIndexString<ECUInfo>();

			foreach (DataRow dr in DataSource.Rows)
				_ecu.Add(new ECUInfo(dr.ItemArray));
		}

		public ECUInfo this[string ecu]
		{
			get
			{
				ECUInfo e;
				try
				{ e = _ecu[ecu]; }
				catch (KeyNotFoundException)
				{ e = null; }
				return e;
			}
		}

		internal DataTable DataSource
		{
			get { return _datasource.GetDataSource(Library.Tables.ECU); }
		}
	}
}
