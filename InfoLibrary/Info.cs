using System;
using System.Collections.Generic;
using System.Text;

namespace InfoLibrary
{
	public class Info
	{
		static Library _sDataSource;
		
		public static EngineInfoCollection EngineCollection
		{
			get { return _sDataSource.GetEngineCollection(); }
		}
	}
}
