using System;
using System.Collections;
using Helper;

namespace FRom.Logic
{
	/// <summary>
	/// Отображение данных карты
	/// </summary>
	public enum ViewEnum
	{
		Raw,			// "Как есть" в десятичной форме
		RawHex,			// "как есть" в 16ричной форме.
		Filtered,			// Контроль флага обратной связи
		InjectorMS,		// Длительность импульса форсунок
		AFRatio,		// Air/Fuel Ratio
		InjDutyCycle,	// Injector Duty Cycle
		EstHP,			// Расчетная мощность
		Scale			// В соответствии с полями Scale
	}

	/// <summary>
	/// Описание карты и ее содержимое
	/// </summary>
	public class Map : IEnumerable
	{
		//Описание карты
		AddressInstance _adr;
		//Массив из которого берется содержимое карты по описанию _adr
		byte[] _bin;
		//Ссылка на родителя, для возможности доставать привязанные карты 
		From _from;
		//массив для кэширования данных из массива
		uint?[,] _map;
		object[,] _mapObj;

		public Map(From pFrom, AddressInstance pAddr)
		{
			_adr = pAddr;
			_bin = pFrom.GetBin();
			_from = pFrom;
			_map = new uint?[_adr.Columns, _adr.Rows];
			_mapObj = new object[_adr.Columns, _adr.Rows];

		}

		/// <summary>
		/// Взять описание карты
		/// </summary>
		public AddressInstance Address
		{
			get { return _adr; }
		}

		/// <summary>
		/// Взять родителя
		/// </summary>
		/// <returns></returns>
		public From GetFRom()
		{
			return _from;
		}

		/// <summary>
		/// Ячейка
		/// </summary>
		/// <param name="x">положение по x</param>
		/// <param name="y">положение по y</param>
		/// <returns>значение ячейки</returns>
		public uint this[int x, int y]
		{
			get
			{
				if (_map[x, y] != null) return (uint)_map[x, y];
				int bytes = _adr.LengthCell;
				if (bytes < 1 || bytes > 4)
					throw new ArgumentException("Неверный размер ячейки", _adr.LengthCell.ToString());
				if (x > _adr.Columns || y > _adr.Rows)
					throw new ArgumentOutOfRangeException();
				byte[] dest = new byte[bytes];
				int offset = (y * _adr.Rows + x) * bytes;
				Buffer.BlockCopy(_bin, (int)_adr.StartAdress + offset, dest, 0, bytes);
				uint ret = 0;
				for (int i = bytes - 1; i >= 0; i--)
					ret |= ((uint)dest[i]) << 8 * (bytes - i - 1);
				_map[x, y] = (uint?)ret;
				return ret;
			}
			set
			{
				if (_map[x, y] == (uint?)value) return;
				else _map[x, y] = null;
				int bytes = _adr.LengthCell;
				if (bytes < 1 || bytes > 4)
					throw new ArgumentException("Неверный размер ячейки", _adr.LengthCell.ToString());
				if (x > _adr.Columns || y > _adr.Rows)
					throw new ArgumentOutOfRangeException();
				byte[] dest = new byte[bytes];
				for (int i = 0; i < bytes; i++)
					dest[i] = (byte)(0xFF & (value >>= 8 * i));
				int offset = (int)_adr.StartAdress + (_adr.Columns * y + x) * _adr.LengthCell;
				Buffer.BlockCopy(dest, 0, _bin, offset, bytes);
			}
		}

		/// <summary>
		/// Длина карты в байтах
		/// </summary>
		public int Length
		{
			get { return _adr.Length; }
		}

		/// <summary>
		/// Количество Строк в карте
		/// </summary>
		public int Rows
		{
			get { return _adr.Rows; }
		}

		/// <summary>
		/// Количество Колонок в карте
		/// </summary>
		public int Columns
		{
			get { return _adr.Columns; }
		}

		#region IEnumerable Members
		public IEnumerator GetEnumerator()
		{
			return GetEnumerable(this).GetEnumerator();
		}
		static IEnumerable GetEnumerable(Map map)
		{
			//return new MapEnumerator(this);
			int x, y;
			x = y = 0;
			while (y != map._adr.Rows)
			{
				yield return map[x++, y];
				if (x == map._adr.Columns)
				{
					y++;
					x = 0;
				}
			}
		}
		#endregion

		public override string ToString()
		{
			return _adr.ToString();
		}

		#region DEBUG chapter
#if DEBUG
		static Timer tmr = new Timer(10);
#endif
		public object this[int x, int y, ViewEnum? view]
		{
			get
			{
#if DEBUG
				tmr.Start(0);
#endif
				//view = _adr.MapView;
				//if (_mapObj[x, y] != null) return _mapObj[x, y];
				//если параметр null, то берем текущее отображение
				if (view == null) view = _adr.MapView;
				uint raw = this[x, y];
				object obj = null;// = (float)raw;
				switch (view)
				{
					case ViewEnum.Raw:
						obj = raw;
						break;
					case ViewEnum.RawHex:
						obj = Convert.ToString(raw, 16);
						break;
					case ViewEnum.Filtered:
						if (raw > 128) obj = (float)raw - 192;
						else obj = raw;
						break;
					case ViewEnum.InjectorMS:
						break;
					case ViewEnum.AFRatio:
						break;
					case ViewEnum.InjDutyCycle:
						break;
					case ViewEnum.EstHP:
						break;
					case ViewEnum.Scale:
						obj = raw * _adr.ValueOfElement;
						break;
					default:
						break;
				}
				//_mapObj[x, y] = obj;

#if DEBUG
				tmr.Write(0);
#endif
				return obj;
			}
			set
			{
				if (_mapObj[x, y] == value) return;
				else _mapObj[x, y] = null;
				//если параметр null, то берем текущее отображение
				if (view == null) view = _adr.MapView;
				uint oldVal = this[x, y];	//текущее значение
				uint newVal = (uint)value;
				switch (view)
				{
					case ViewEnum.Raw:
						break;
					case ViewEnum.RawHex:
						newVal = Convert.ToUInt32(value.ToString(), 16);
						break;
					case ViewEnum.Filtered:
						if (oldVal > 128) newVal = oldVal + 192;
						break;
					case ViewEnum.InjectorMS:
						break;
					case ViewEnum.AFRatio:
						break;
					case ViewEnum.InjDutyCycle:
						break;
					case ViewEnum.EstHP:
						break;
					default:
						break;
				}
				this[x, y] = newVal;
			}
		}
		#endregion

		/// <summary>
		/// Возвращает байтовый массив карты
		/// </summary>
		internal float[][] GetBinaryMap()
		{
			float[][] arr = new float[Columns][];
			for (int i = 0; i < Columns; i++)
			{
				arr[i] = new float[Rows];
				for (int j = 0; j < Rows; j++)
				{
					object o = this[i, j, ViewEnum.Filtered];
					//int n = Convert.ToInt32(o);
					arr[i][j] = (float)Convert.ToDouble(o);
				}
			}
			return arr;
		}
	}
}
