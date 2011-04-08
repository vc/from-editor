// Included...
// [1] Define TypeConverter class for hex.
// [2] use it when defining a member of a data class using attribute TypeConverter.
// [3] Connect the data class to a PropertyGrid.

using System;
using System.ComponentModel;

namespace Helper
{
	// [1] define UInt32HexTypeConverter is-a TypeConverter
	public class UInt32HexTypeConverter : TypeConverter
	{
		static uint _hexLenth = 4;
		static string _hexFormatString = "0x{0:X" + _hexLenth + "}";
		public static uint HexLength
		{
			get { return _hexLenth; }
			set
			{
				_hexLenth = value;
				_hexFormatString = "0x{0:X" + _hexLenth + "}";
			}
		}

		public UInt32HexTypeConverter()
			: base()
		{
			HexLength = 4;
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			else
			{
				return base.CanConvertFrom(context, sourceType);
			}
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				return true;
			}
			else
			{
				return base.CanConvertTo(context, destinationType);
			}
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value.GetType() == typeof(UInt32))
			{
				return string.Format(_hexFormatString, value);
			}
			else
			{
				return base.ConvertTo(context, culture, value, destinationType);
			}
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			//string s = (context.Instance as FRom.AddressInstance).ConstName;
			if (value.GetType() == typeof(string))
			{
				string input = (string)value;

				if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
				{
					input = input.Substring(2);
				}
				try
				{
					return UInt32.Parse(input, System.Globalization.NumberStyles.HexNumber, culture);
				}
				catch (FormatException ex)
				{
					throw new FormatException("Недопустимые символы в строке.\n\r В 16-ричном формате используются следующие символы: '0-9, A, B, C, D, E, F'.", ex);
				}
			}
			else
			{
				return base.ConvertFrom(context, culture, value);
			}
		}
	} // UInt32HexTypeConverter used by grid to display hex.


	//// [2] define a class for data to be associated with the PropertyGrid.
	//      [DefaultPropertyAttribute("Name")]
	//      public class Data 
	//.
	//.
	//.
	//         public UInt32 stat;
	//         [CategoryAttribute("Main Scanner"), DescriptionAttribute("Status"), TypeConverter(typeof(UInt32HexTypeConverter))]
	//         public UInt32 Status
	//         {
	//            get { return stat; }
	//         }
	//.
	//.
	//.
	//// [3] reference data for propertyGrid. (here, myData is a Data).
	//         propertyGrid1.SelectedObject = myData ;
}