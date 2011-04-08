using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace InfoLibrary
{
	public class BoreStroke
	{
		public float Bore;
		public float Stroke;
		public BoreStroke(float bore, float stroke)
		{
			Init(bore, stroke);
		}
		public BoreStroke(object o)
		{
			string line = o as string;
			if (line == null)
				throw new ArgumentException("Null value not accepted");
			Init(line);
		}
		public BoreStroke(string line)
		{
			Init(line);
		}

		private void Init(string line)
		{
			string[] split = line.ToLower().Split(new char[] { 'x' }, 2, StringSplitOptions.RemoveEmptyEntries);
			if (split.Length < 2
				|| !float.TryParse(split[0], System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out Bore)
				|| !float.TryParse(split[1], System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out Stroke)
				)
			{
				Bore = 0;
				Stroke = 0;
			}

		}

		private void Init(float bore, float stroke)
		{
			Bore = bore;
			Stroke = stroke;
		}


	}
}
