using System;
using System.Collections.Generic;
using System.Text;

namespace InfoLibrary
{
	public class ECUInfo : Info
	{
		const int _cCountOfParams = 10;
		const string _cCountOfParamsErrorString = "Invalid lenth of array. Expected - ";
		string _model;
		string _frame;
		string _transmission;
		string _romId;
		string _boxNumber;
		string _date;
		EngineInfo _engine;
		int _engineValCC = 0;
		int _injCC = 0;
		string _comment;

		public ECUInfo(string[] val)
		{
			Init(val);
		}

		public ECUInfo(object[] values)
		{
			if (values.Length != _cCountOfParams)
				throw new ArgumentException(_cCountOfParamsErrorString + _cCountOfParams);
			string[] arr = new string[values.Length];
			for (int i = 0; i < values.Length; i++)
				arr[i] = values[i].ToString();
			Init(arr);
		}

		private void Init(string[] val)
		{
			if (val.Length != _cCountOfParams)
				throw new ArgumentException(_cCountOfParamsErrorString + _cCountOfParams);
			_model = val[0];
			_frame = val[1];
			_transmission = val[2];
			_romId = val[3];
			_boxNumber = val[4];
			_date = val[5];
			_engine = base.EngineCollection[val[6]];
			Int32.TryParse(val[7], out _engineValCC);
			Int32.TryParse(val[8], out _injCC);
			_comment = val[9];
		}
	}
}
