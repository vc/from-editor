using System;
using System.Collections.Generic;
using System.Text;
using Helper;
using System.Data;

namespace InfoLibrary
{
	public class AFMInfoCollection:Info
	{
		ListIndexString<AFMInfo> _afm;

		public AFMInfoCollection()
		{
			_afm = new ListIndexString<AFMInfo>();

			foreach (DataRow dr in DataSource.Rows)
			_afm.Add(new AFMInfo(dr.ItemArray));
		}

		internal DataTable DataSource
		{
			get { return base.Library.GetDataSource(Library.Tables.AFM); }
		}
	}
}
