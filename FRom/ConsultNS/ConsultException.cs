using System;

namespace FRom.ConsultNS
{

	/// <summary>
	/// Исключение протокола работы с эмулятором
	/// </summary>
	[Serializable]
	public class ConsultException : Exception
	{

		/// <summary>
		/// Данные для трассировки исключения
		/// </summary>
		internal byte[] _data;		

		public ConsultException(string msg, byte[] data)
			: base(msg)
		{
			_data = data;
		}
		public ConsultException(string msg, Exception innerException)
			: base(msg, innerException) { }

		public ConsultException(string msg) :
			base(msg) { }
	}

}
