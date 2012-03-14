using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.Consult.Data
{
	public class BooleanFormatProvider : IFormatProvider, ICustomFormatter
	{
		/// <summary>
		/// Gets a formatter based on the requested type.
		/// </summary>
		/// <param name="formatType">The requested type.</param>
		/// <returns>The formatter if one is found.</returns>
		public object GetFormat(Type formatType)
		{
			if (formatType == typeof(ICustomFormatter))
				return this;
			else
				return null;
		}

		/// <summary>
		/// Formats a <see cref="Boolean"/> based on the supplied format strings.
		/// </summary>
		/// <param name="format">The format in which to format.</param>
		/// <param name="arg">The value to format.</param>
		/// <param name="formatProvider">The formatter to use.</param>
		/// <returns>A formatted string.</returns>
		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			if (!(arg is bool))
				throw new ArgumentException("Must be a boolean type", "arg");

			bool value = (bool)arg;

			if (string.IsNullOrEmpty(format))
				return value ? "ON" : "OFF";

			format = format.Trim();

			if (string.Equals(format, "yn", StringComparison.OrdinalIgnoreCase))
				return value ? "Yes" : "No";

			string[] parts = format.Split('|');
			if ((parts.Length == 1 && !string.IsNullOrEmpty(format)) || parts.Length > 2)
				throw new FormatException("Format string is invalid>");

			if (parts.Length == 2)
				return value ? parts[0] : parts[1];
			else
				return arg.ToString();
		}
	}
}
