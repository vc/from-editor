using System;
using System.Collections.Generic;
using System.Text;

namespace InfoLibrary
{
	public class Info
	{
		internal static Library _library;

		public Library Library
		{
			get { return _library; }
		}

		public EngineInfoCollection EngineCollection
		{
			get { return _library.EngineCollection; }
		}
	}
}
