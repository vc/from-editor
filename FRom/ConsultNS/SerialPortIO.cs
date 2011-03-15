using System.IO.Ports;
using System.Threading;
using FRom.Logger;
using System;

namespace FRom.ConsultNS
{
	/// <summary>
	/// Базовая функциональность последовательного интерфейса
	/// </summary>
	public class SerialPortIO
	{
		public SerialPortIO() { }

		public SerialPortIO(string port)
		{
			Port.PortName = port;
		}

		private object _lockPort = new object();
		protected Log _log = Log.Instance;

		private SerialPort _port;

		protected SerialPort Port
		{
			get
			{
				if (_port == null)
					_port = new SerialPort();

				return _port;
			}
			set { _port = value; }
		}

		/// <summary>
		/// Имя порта
		/// </summary>
		public virtual string COMPort
		{
			get { return Port.PortName; }
			set { Port.PortName = value; }
		}

		/// <summary>
		/// Количество милисекунд, в промежутке между записью и чтением из порта
		/// </summary>
		protected int _cTimeWaitBeforeRead = 60;
		private string port;

		/// <summary>
		/// Запрос в порт
		/// </summary>
		/// <param name="send">Массив запроса</param>
		/// <param name="len">Длина ожидаемого ответа. null - читать все</param>
		/// <returns>Массив ответа</returns>
		protected byte[] Request(byte[] send, int? len)
		{
			byte[] recv;

			lock (_lockPort)
			{
				//Очищаем входной буффер
				_port.ReadExisting();
				Transmit(send);

				Thread.Sleep(_cTimeWaitBeforeRead);
				if (len == null)
					recv = Receive();
				else
					recv = Receive((int)len);
			}

			return recv;
		}

		/// <summary>
		/// Прочитать блок данных из порта
		/// </summary>
		/// <param name="n">Количествой байт для считывания</param>
		/// <returns>Массив байт</returns>
		protected byte[] Receive(int n)
		{
			byte[] arr = new byte[n];
			try
			{
				for (int i = 0; i < n; i++)
					arr[i] = (byte)_port.ReadByte();
			}
			catch (TimeoutException)
			{

			}
			_log.WriteEntry(this, arr, false);

			return arr;
		}

		/// <summary>
		/// Прочитать все что есть в буффере для чтения
		/// </summary>
		/// <returns>Массив байт</returns>
		protected byte[] Receive()
		{
			int n = _port.BytesToRead;
			byte[] arr = new byte[n];

			for (int i = 0; i < n; i++)
				arr[i] = (byte)_port.ReadByte();

			_log.WriteEntry(this, arr, false);

			return arr;
		}

		/// <summary>
		/// Послать массив в порт
		/// </summary>
		/// <param name="arr">Массив для передачи</param>
		protected void Transmit(byte[] arr)
		{
			_log.WriteEntry(this, arr, true);

			_port.Write(arr, 0, arr.Length);
		}

		/// <summary>
		/// Послать байт в порт 
		/// </summary>
		/// <param name="b">Байт для передачи</param>
		protected void Transmit(byte b)
		{
			Transmit(new byte[] { b });
		}

		/// <summary>
		/// Освободить ресурсы
		/// </summary>
		internal void Dispose(bool disposing)
		{
			if (disposing)
			{
				Disconnect();
				if (_port != null)
				{
					_port.Dispose();
					_port = null;
				}
			}
		}

		/// <summary>
		/// закрыть порт
		/// </summary>
		public virtual void Disconnect()
		{
			if (_port != null && _port.IsOpen)
				_port.Close();
		}
	}
}
