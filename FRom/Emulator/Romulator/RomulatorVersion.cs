using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.Emulator
{

	/// <summary>
	/// Класс одержащий информацию о версии и ID Устройства
	/// </summary>
	public class RomulatorVersion
	{
		private byte _first;		//первый байт версии
		private byte _second;	//второй байт версии
		private byte _id;		//id устройства

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="init_arr">Массив байт. Размер = 4</param>
		public RomulatorVersion(byte[] init_arr)
		{
			if (init_arr.Length != 3)
				throw new RomulatorException("Ошибка инициализации версии.",
					new ArgumentException("Ожидается Length=3, пришло: Length=" + init_arr.Length)
					);
			_first = init_arr[0];
			_second = init_arr[1];
			_id = init_arr[2];
		}

		public static bool operator >=(RomulatorVersion v1, RomulatorVersion v2)
		{
			return v1._first >= v2._first && v1._second >= v2._second;
		}
		public static bool operator <=(RomulatorVersion v1, RomulatorVersion v2)
		{
			return v1._first <= v2._first && v1._second <= v2._second;
		}

		/// <summary>
		/// Версия в строковом представлении
		/// </summary>
		/// <returns>Версия в формате (XXh).(XXh), ID (XXh)</returns>
		public override string ToString()
		{
			return _first.ToString() + '.' + _second.ToString() + ", ID" + _id.ToString();
		}
	}
}
