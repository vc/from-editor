using System;

namespace FRom.Emulator
{
	/// <summary>
	/// Исключение протокола работы с эмулятором
	/// </summary>
	public class RomulatorException : Exception
	{

		/// <summary>
		/// Данные для трассировки исключения
		/// </summary>
		internal byte[] _data;

		public RomulatorException(string msg, byte[] data)
			: base(msg)
		{
			_data = data;
		}
		public RomulatorException(string msg, Exception innerException)
			: base(msg, innerException) { }

		public RomulatorException(string msg) :
			base(msg) { }
	}
}
