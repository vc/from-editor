using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace InfoLibrary
{
	public class ChamberVol
	{
		public float Vol;
		public float Tolerance;
		public ChamberVol(float volume, float tolerance)
		{
			Vol = volume;
			Tolerance = tolerance;
		}
		/// <param name="line">Строка типа "Approx.46.1+-0.5"</param>
		public ChamberVol(string line)
		{
			line = Init(line);
		}
		public ChamberVol(object o)
		{
			string line = o as string;
			if (line == null)
				throw new ArgumentException("Null value not accepted");
			Init(line);
		}
		private string Init(string line)
		{
			StringBuilder cleanLine = new StringBuilder();
			//replace all non digin characters on spaces
			foreach (char c in line)
			{
				if (!Char.IsDigit(c))
					cleanLine.Append(' ');
				else
					cleanLine.Append(c);
			}
			line = cleanLine.ToString();
			string[] split = line.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
			if (split.Length < 2
					|| !float.TryParse(split[0],
					System.Globalization.NumberStyles.Any,
					CultureInfo.InvariantCulture, out Vol)
					|| !float.TryParse(split[1],
					System.Globalization.NumberStyles.Any,
					CultureInfo.InvariantCulture, out Tolerance))
			{
				Vol = 0;
				Tolerance = 0;
			}
			return line;
		}
	}
}
