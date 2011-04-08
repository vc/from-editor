using System;
using System.Collections.Generic;
using System.Text;

namespace InfoLibrary
{

	public class InjectorInfo
	{
		public enum InjectorType
		{
			TopFeed,
			SideFeed,
			NotSpecified
		}
		string _frame;
		string _engine;
		InjectorType _injType;
		int _injCC;
		int _fuelPump;

		public InjectorInfo(string[] val)
		{
			Init(val);
		}

		private void Init(string[] val)
		{
			if (val.Length != 5)
				throw new ArgumentException("Invalid lenth of array");
			try
			{
				_frame = val[0];
				_engine = val[1];
				val[2] = val[2].ToLower();
				if (val[2].Contains("top"))
					_injType = InjectorType.TopFeed;
				else if (val[2].Contains("side"))
					_injType = InjectorType.SideFeed;
				else
					_injType = InjectorType.NotSpecified;
				_injCC = Int32.Parse(val[3]);
				_fuelPump = Int32.Parse(val[4]);
			}
			catch (Exception ex)
			{
				throw new ArgumentException("Invalid prams", ex);
			}
		}
	}
}
