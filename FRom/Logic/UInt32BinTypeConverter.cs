// Included...
// [1] Define TypeConverter class for hex.
// [2] use it when defining a member of a data class using attribute TypeConverter.
// [3] Connect the data class to a PropertyGrid.

using System;
using System.ComponentModel;
using System.Globalization;
namespace FRom.Logic
{
	// [1] define UInt32HexTypeConverter is-a TypeConverter
	public class UInt32BinTypeConverter : TypeConverter
	{
		const int _cUseBase = 2;

		static uint _binLenth = 8;

		public static uint BinLength
		{
			get { return _binLenth; }
			set { _binLenth = value; }
		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			else
			{
				return false;// base.CanConvertFrom(context, sourceType);
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
				return false;// base.CanConvertTo(context, destinationType);
			}
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string) && value.GetType() == typeof(UInt16))
			{
				UInt16 input = (UInt16)value;
				string str = Convert.ToString(input, _cUseBase);
				return str.PadLeft((int)_binLenth, '0') + "b";
			}
			else
			{
				return value;// base.ConvertTo(context, culture, value, destinationType);
			}
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value.GetType() == typeof(string))
			{
				string input = (string)value;
				if (input.EndsWith("b", StringComparison.OrdinalIgnoreCase))
				{
					input = input.Substring(0, input.Length - 1);
				}

				try { return Convert.ToUInt16(input, _cUseBase); }
				catch (FormatException ex)
				{
					throw new FormatException("Недопустимые символы в строке.\n\r В двоичном формате используются только '0' и '1'.", ex);
				}
			}
			else
			{
				return value;// base.ConvertFrom(context, culture, value);
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