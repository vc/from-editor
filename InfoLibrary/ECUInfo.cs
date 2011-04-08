using System;
using System.Collections.Generic;
using System.Text;

namespace InfoLibrary
{
	public class ECUInfo : Info
	{
		string _model;
		string _frame;
		string _transmission;
		string _romId;
		string _boxNumber;
		string _date;
		EngineInfo _engine;
		int _engineValCC;
		int _injCC;
		string _comment;

		public ECUInfo(string[] val)
		{
			Init(val);
		}

		private void Init(string[] val)
		{
			if (val.Length != 10)
				throw new ArgumentException("Invalid array lenth");
			_model = val[0];
			_frame = val[1];
			_transmission = val[2];
			_romId = val[3];
			_boxNumber = val[4];
			_date = val[5];
			//_engine = _sEngineCollection[val[6]];
		}
	}
}
