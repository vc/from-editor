using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace FRom.Consult.Data
{
	public class SensorValue : IFormattable
	{
		static SensorValue()
		{
			FormatInt = "{0}";
			FormatDouble = "{0:2}";
			FormatBool = "ON|OFF";
			FormatBoolProvider = new BooleanFormatProvider();
		}

		public static string FormatInt { get; set; }
		public static string FormatDouble { get; set; }
		public static string FormatBool { get; set; }
		public static BooleanFormatProvider FormatBoolProvider { get; set; }

		public ScaleDescription Scale { get; private set; }
		public object Value { get; private set; }
		public Type TypeOfValue { get; private set; }

		public SensorValue(int value, ScaleDescription scale)
		{
			this.Value = value;
			this.Scale = scale;
			this.TypeOfValue = typeof(int);
		}

		public SensorValue(double value, ScaleDescription scale)
		{
			this.Value = value;
			this.Scale = scale;
			this.TypeOfValue = typeof(double);
		}

		public SensorValue(bool value, ScaleDescription scale)
		{
			this.Value = value;
			this.Scale = scale;
			this.TypeOfValue = typeof(bool);
		}

		public string ValueString
		{
			get
			{
				string format = "";
				IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;
				if (this.TypeOfValue == typeof(int))
					format = FormatInt;
				else if (this.TypeOfValue == typeof(double))
					format = FormatDouble;
				else if (this.TypeOfValue == typeof(bool))
				{
					format = FormatBool;
					formatProvider = FormatBoolProvider;
				}
				return String.Format(formatProvider, format, this.Value);
			}
		}

		public override string ToString()
		{

			return String.Format("{0}[{1}]", this.ValueString, this.Scale);
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			throw new NotImplementedException();
		}
	}
}
