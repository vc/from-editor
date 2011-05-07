using System;
using System.Collections.Generic;
using System.Text;
using Helper;
using System.Data;

namespace InfoLibrary
{

	public class EngineInfoCollection:Info
	{
		ListIndexString<EngineInfo> _engines;

		public EngineInfoCollection()
		{
			_engines = new ListIndexString<EngineInfo>();

			foreach (DataRow dr in DataSource.Rows)
				_engines.Add(new EngineInfo(dr.ItemArray));
		}

		public EngineInfo this[string eng]
		{
			get
			{
				EngineInfo e;
				try
				{ e = _engines[eng]; }
				catch (KeyNotFoundException)
				{
					e = new EngineInfo(eng);
					_engines.Add(e);
				}
				return e;
			}
		}

		internal DataTable DataSource
		{
			get { return base.Library.GetDataSource(Library.Tables.Engine); }
		}
	}
}
