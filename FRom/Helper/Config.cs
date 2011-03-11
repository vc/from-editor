using System;
using System.Collections.Generic;
using System.Text;
using FRom.Properties;

namespace FRom.Helper
{
	public class Config
	{
		static Settings _cfg;

		internal static Settings Instance
		{
			get
			{
				if (_cfg == null)
					_cfg = new Settings();
				return _cfg;
			}
			//set { _cfg = value; }
		}
	}
}
