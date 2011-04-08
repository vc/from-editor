using System;
using System.Collections.Generic;
using System.Text;

namespace InfoLibrary
{
	public class AFMInfo
	{
		string _engine;
		string _frame;
		string _partNumber;
		string _power;
		string _color;
		string _pin;
		string _size;

		public AFMInfo(string[] values)
		{
			Init(values);
		}

		private void Init(string[] values)
		{
			if (values.Length != 7)
				throw new ArgumentException("Invalid count of params");
			_engine = values[0];
			_frame = values[1];
			_partNumber = values[2];
			_power = values[3];
			_color = values[4];
			_pin = values[5];
			_size = values[6];
		}

		public string Engine { get { return _engine; } }
		public string Frame { get { return _frame; } }
		public string PartNumber { get { return _partNumber; } }
		public string Power { get { return _power; } }
		public string Color { get { return _color; } }
		public string Pin { get { return _pin; } }
		public string Size { get { return _size; } }

		public override string ToString()
		{
			return base.ToString();
		}

	}
}
