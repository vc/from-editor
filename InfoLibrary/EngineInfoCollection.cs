using System;
using System.Collections.Generic;
using System.Text;
using Helper;
using System.Data;

namespace InfoLibrary
{

	public class EngineInfoCollection
	{
		ListIndexString<EngineInfo> _engines;
		Library _datasource;

		public EngineInfoCollection(Library lib)
		{
			_datasource = lib;
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
				{ e = new EngineInfo(eng); }
				return e;
			}
		}

		internal DataTable DataSource
		{
			get { return _datasource.GetDataSource(Library.Tables.Engine); }
		}
	}
}
