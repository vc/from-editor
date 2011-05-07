using System;
using System.Collections.Generic;
using System.Text;

namespace InfoLibrary
{

	public class InjectorInfo : Info
	{
		public enum InjectorType
		{
			TopFeed,
			SideFeed,
			NotSpecified
		}
		string _frame;
		EngineInfo _engine;
		InjectorType _injType;
		int _injCC;
		int _fuelPump;

		const int _cCountOfParams = 6;
		const string _cCountOfParamsErrorString = "Invalid lenth of array. Expected - ";

		public InjectorInfo(object[] values)
		{
			if (values.Length != _cCountOfParams)
				throw new ArgumentException(_cCountOfParamsErrorString + _cCountOfParams);
			string[] arr = new string[values.Length];
			for (int i = 0; i < values.Length; i++)
				arr[i] = values[i].ToString();
			Init(arr);
		}

		public InjectorInfo(string[] val)
		{
			Init(val);
		}

		private void Init(string[] val)
		{
			if (val.Length != _cCountOfParams)
				throw new ArgumentException(_cCountOfParamsErrorString + _cCountOfParams);
			try
			{
				_frame = val[0];
				_engine = base.EngineCollection[val[1]];
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
