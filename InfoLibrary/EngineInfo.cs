using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace InfoLibrary
{
	public class EngineInfo
	{
		const int _cCountOfParams = 8;
		const string _cCountOfParamsErrorString = "Invalid lenth of array. Expected - ";

		string _name;
		string _frame;
		int _displacement;
		BoreStroke _boreStroke;
		float _compression;
		ChamberVol _chamberVol;
		float _pistonCrown;
		int _cylindersCount;

		public EngineInfo(object[] values)
		{
			if (values.Length != 8)
				throw new ArgumentException(_cCountOfParamsErrorString + _cCountOfParams);
			string[] arr = new string[values.Length];
			for (int i = 0; i < values.Length; i++)
				arr[i] = values[i].ToString();
			Init(arr);
		}

		public EngineInfo(string eng)
		{
			_name = eng;
			_cylindersCount = CheckStroke(eng);
		}

		/// <summary>
		/// Определение количества горшков по марке двигателя Nissan
		/// </summary>
		/// <param name="eng">марка двига Пр: "ca18det"</param>
		/// <returns>Количество горшков</returns>
		private static int CheckStroke(string eng)
		{
			eng = eng.ToUpper();
			int len = eng.Length;
			if (len >= 1 && eng.Substring(0, 1) == "A")
				return 4;
			else if (len >= 1 && eng.Substring(0, 1) == "L")
			{
				switch (eng)
				{
					case "L13":
					case "L14":
					case "L16":
					case "L16S":
					case "L16T":
					case "L16P":
					case "L18":
					case "L18S":
					case "L18T":
					case "L18P":
					case "L20B":
					case "L22":
					case "LZ":
					case "LD20":
					case "LD20T":
						return 4;
					case "L20":
					case "L20A":
					case "L20ET":
					case "L20P":
					case "L23":
					case "L24":
					case "L24E":
					case "L26":
					case "L28":
					case "L28E":
					case "L28ET":
					case "LD28":
					case "LD28T":
						return 6;
					default:
						return 0;
				}
			}
			else if (len >= 2)
			{
				string s2 = eng.Substring(0, 2);
				switch (s2)
				{
					case "FJ":
					case "SR":
					case "CA":
					case "CG":
						return 4;
					case "RB":
					case "VQ":
					case "VG":
						return 6;
					default:
						return 0;
				}
			}
			return 0;
		}

		private void Init(string[] values)
		{
			_name = values[0];
			_frame = values[1];
			if (!Int32.TryParse(values[2],
				System.Globalization.NumberStyles.Any,
				CultureInfo.InvariantCulture, out _displacement))
				_displacement = 0;
			_boreStroke = new BoreStroke(values[3]);
			if (!float.TryParse(values[4],
				System.Globalization.NumberStyles.Any,
				CultureInfo.InvariantCulture, out _compression))
				_compression = 0;
			_chamberVol = new ChamberVol(values[5]);
			if (!float.TryParse(values[6],
				System.Globalization.NumberStyles.Any,
				CultureInfo.InvariantCulture, out _pistonCrown))
				_pistonCrown = 0;
			if (!Int32.TryParse(values[7],
				System.Globalization.NumberStyles.Any,
				CultureInfo.InvariantCulture, out _cylindersCount))
				_cylindersCount = CheckStroke(_name);
		}

		private void Init(string name, string frame, int displacement, BoreStroke bs, float compression, ChamberVol cv, float pistoncrown, int cyl)
		{
			_name = name;
			_frame = frame;
			_displacement = displacement;
			_boreStroke = bs;
			_compression = compression;
			_chamberVol = cv;
			_pistonCrown = pistoncrown;
			_cylindersCount = cyl;
		}

		public override bool Equals(object obj)
		{
			EngineInfo e = obj as EngineInfo;
			if (e != null && e._name == this._name)
				return true;

			return false;
		}

		public string Name { get { return _name; } }
		public string Frame { get { return _frame; } }
		public int Displacement { get { return _displacement; } }
		public BoreStroke BoreStroke { get { return _boreStroke; } }
		public float Compression { get { return _compression; } }
		public ChamberVol ChamberVol { get { return _chamberVol; } }
		public float PistonCrown { get { return _pistonCrown; } }
		public int CylindersCount { get { return _cylindersCount; } }
	}
}
