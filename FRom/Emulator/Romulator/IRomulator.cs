using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.Emulator
{
	/// <summary>
	/// Интерфейс описывающий функциональность протокола обмена данными между ПО и аппаратной частью эмулятора
	/// </summary>
	public interface IRomulator
	{
		/// <summary>
		/// Запросить версию микропрограммы эмулятора
		/// </summary>
		/// <returns>Версия микропрограммы</returns>
		FRom.Emulator.RomulatorVersion GetVersion();

		/// <summary>
		/// Запросить массив байт из памяти эмулятора
		/// </summary>
		/// <param name="size">Размер массива</param>
		/// <param name="address">Адрес смещения, относительно нуля</param>
		/// <returns>Массив запрошенных байт</returns>
		byte[] ReadBlock(byte size, ushort address);

		/// <summary>
		/// Запросить байт
		/// </summary>
		/// <param name="address">Адрес смещения, относительно нуля</param>
		/// <returns>Запрошенный байт</returns>
		byte ReadByte(ushort address);

		/// <summary>
		/// Послать данные на запись.
		/// Бастрая запись с блокировкой доступа к эмулятору.
		/// (использовать только для инициализации памяти)
		/// </summary>
		/// <param name="address">Адрес смещения, относительно 0 байта</param>
		/// <param name="data">Данные для записи</param>
		void WriteBlock(ushort address, byte[] data);

		/// <summary>
		/// Послать данные на запись в эмулятор.
		/// Запись данных "на лету", без блокировки доступа к эмулятору.
		/// </summary>
		/// <param name="address">Адрес смещения, относительно 0 байта</param>
		/// <param name="data">Данные для записи</param>
		void HiddenWrite(ushort address, byte[] data);

		/// <summary>
		/// Запросить состояние эмулятора
		/// </summary>
		/// <returns>True - Готов (READY); 
		/// False - Не готов (BUSY)</returns>
		bool GetStatus();
	}


}
