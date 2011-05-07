using System;
using System.Collections.Generic;
using System.Text;
using Helper;
using System.Data;

namespace InfoLibrary
{
	public class ECUInfoCollection:Info
	{
		ListIndexString<ECUInfo> _ecu;		

		public ECUInfoCollection()
		{			
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
			get { return base.Library.GetDataSource(Library.Tables.ECU); }
		}
	}
}
