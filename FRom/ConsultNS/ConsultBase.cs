﻿using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using FRom.ConsultNS.Data;

namespace FRom.ConsultNS
{
	/// <summary>
	/// Базовая функциональность протокола Consult
	/// </summary>
	public class ConsultBase : SerialPortIO
	{
		#region Constatns
		/// <summary>
		/// Количество попыток подключения к ECU
		/// </summary>
		protected int _cCountRetryInitECU = 5;

		/// <summary>
		/// Задержка между попытками инициализации ECU
		/// </summary>
		protected int _cTimeRetryInitDelay = 500;

		/// <summary>
		/// Задержка в милисекундах, в промежутке между инициализацией порта и первым чтением/записью в него
		/// </summary>
		protected int _cTimeReadPortInit = 800;

		/// <summary>
		/// Количество повторов запросов к ECU, если получили TimeoutException на чтение ответа
		/// </summary>
		protected int _cCountRetryReadByte = 5;

		/// <summary>
		/// Количество повторов запросов ECU, в случае ошибки "неожиданнйы ответ от устройства"
		/// </summary>
		protected int _cCountRetry = 5;
		#endregion

		/// <summary>
		/// интерфейс, через который будем получать команды консульта
		/// </summary>
		internal IConsultData _consultData;

		public ConsultBase() { }
		public ConsultBase(IConsultData data)
		{
			_consultData = data;
		}
		public ConsultBase(string port, IConsultData data)
		{
			DataSource = data;
			COMPort = port;
			Initialise();
		}

		/// <summary>
		/// Общая инициализация класса
		/// </summary>
		public virtual void Initialise()
		{
			if (Port.IsOpen)
				Port.Close();

			//Port = new SerialPort();

			Port.BaudRate = 9600;
			Port.Parity = Parity.None;
			Port.DataBits = 8;
			Port.StopBits = StopBits.Two;

			Port.Handshake = Handshake.None;

			Port.DtrEnable = true;
			Port.RtsEnable = true;
			Port.ReadBufferSize = 0x4000;
			Port.WriteBufferSize = 0x4000;

			Port.ReadTimeout = 1000;		//Таймаут чтения данных из порта
			Port.WriteTimeout = 5000;

			//_port.Encoding = Encoding.ASCII;
			//количество принятых байт, для срабатывания события DataReceived
			//_port.ReceivedBytesThreshold = 1;
			//_port.ParityReplace = 0xfe;

			try
			{
				//Open&Close port. Except eny errors with not properly closed port.
				Port.Open(); Thread.Sleep(100); Port.Close();
				//wait any time
				Thread.Sleep(50);
				Port.Open();
				Port.BreakState = false;
				Thread.Sleep(_cTimeReadPortInit);
			}
			catch (UnauthorizedAccessException ex)
			{
				throw new ConsultException("Ошибка при открытии порта. Ex:" + ex.Message, ex);
			}

			ConsultInit();
		}

		/// <summary>
		/// Инициализация ECU
		/// </summary>
		internal void ConsultInit()
		{
			for (int i = 0; i < _cCountRetryInitECU; i++)
			{
				try
				{
					SendCommand(_consultData.InitBytes, null);
					return;
				}
				catch (TimeoutException)
				{
					throw new ConsultException("Время ожидания ответа от устройста истекло");
				}
				catch (ConsultException)
				{
					Thread.Sleep(_cTimeRetryInitDelay);
					continue;
				}
			}

			throw new ConsultException("Количество попыток подключения истекло");
		}

		public override string COMPort
		{
			get { return base.COMPort; }
			set
			{
				if (Port.IsOpen)
				{
					Port.Close();
					base.COMPort = value;
					Initialise();
				}
				else
					Port.PortName = value;
			}
		}

		/// <summary>
		/// Источник комманд consult
		/// </summary>
		public virtual IConsultData DataSource
		{
			get { return _consultData; }
			set
			{
				if (value == _consultData || value == null)
					return;
				if (_consultData == null)
					_consultData = value;
				else if (Port.IsOpen)
				{
					Disconnect();
					_consultData = value;
					Initialise();
				}
				else
				{
					_consultData = value;
				}
			}
		}

		/// <summary>
		/// Отправить массив в порт и проверить ответ на инверсность входным данным
		/// </summary>
		/// <param name="send">Массив для отсылки</param>
		/// <param name="cmd">Тип комманды для выборочной проверки инверсии</param>
		internal void SendCommand(byte[] send, ConsultECUConst? cmd)
		{
			byte[] receive = base.Request(send, null);
			const string errMsg = "Неожиданный ответ от устройства";
			//Проверка ответа различная, в зависимости от типа переданной команды.
			switch (cmd)
			{
				case ConsultECUConst.ECU_ROM_READ_BYTE_CMD:
					if (receive.Length != send.Length)
						throw new ConsultException(errMsg);

					//проверяем на инверию каждый третий байт, начиная с нулевого
					//send: c9 80 00 c9 80 01 c9 80 02
					//recv: 36 80 00 36 80 01 36 80 02
					//       0  1  2  3  4  5  6  7  8
					//       1  0  0  1  0  0  1  0  0
					for (int i = 0; i < receive.Length; i += 3)
					{
						if (CheckInverseBytes(send[i], receive[i]))
							throw new ConsultException(errMsg);
					}
					break;
				case ConsultECUConst.ECU_REG_READ_CMD:
					if (receive.Length != send.Length)
						throw new ConsultException(errMsg);
					// Send:
					//5A 00 5A 01 5A 04 5A 05 5A 08 5A 09 5A 0B 5A 0C 5A 0D 5A 10 
					//5A 11 5A 13 5A 14 5A 15 5A 16 5A 1A 5A 1C 5A 1E 5A 1F 5A 21
					//
					// Recv:
					//A5 00 A5 01 A5 04 A5 05 A5 08 A5 09 A5 0B A5 0C A5 0D A5 10
					//A5 11 A5 13 A5 14 A5 15 A5 16 A5 1A A5 1C A5 1E A5 1F A5 21
					for (int i = 0; i < receive.Length; i++)
					{
						if (CheckInverseBytes(send[i], receive[i]))
							throw new ConsultException(errMsg);
						i++;
						if (receive[i] == (byte)ConsultECUConst.ECU_REG_NOT_SUPPORTED)
							throw new NotSupportedException();
					}
					break;
				case null:
				default:
					if (send.Length != receive.Length || CheckInverseBytes(receive, send))
						throw new ConsultException(errMsg);
					break;
			}
		}

		/// <summary>
		/// Запросить ответ от устройства с завершающим символом приема данных (ECUConst.ECU_END_RX)
		/// </summary>
		/// <param name="cmd">Массив для передачи</param>
		/// <param name="cmd_arr">Тип команды</param>
		/// <returns>ответ</returns>
		internal byte[] RequestECUData(byte[] cmd_arr, byte cmd = 0x00)
		{
			//Отправка запроса и проверка корреткности ответа ECU
			SendCommand(cmd_arr, (ConsultECUConst)cmd);

			//Подаем сигнал о том, что ждем фрейм от ECU
			ECUFrameStart();

			//принимаем фрейм
			byte[] frame = ECUFrameGet();

			//Останавливаем передачу данных
			ECUFrameStop();

			return frame;
		}
		internal byte[] RequestECUData(byte cmd_byte, byte cmd = 0x00)
		{
			return RequestECUData(new byte[] { cmd_byte }, cmd);
		}
		internal byte[] RequestECUData(ConsultCommand cons_cmd, byte cmd = 0x00)
		{
			return RequestECUData(new byte[] { cons_cmd.Command }, cmd);
		}

		/// <summary>
		/// Останов приема фрейма от ECU
		/// </summary>
		internal void ECUFrameStop()
		{
			for (int i = 0; i < _cCountRetryReadByte; i++)
			{
				try
				{
					//Transmit((byte)ECUConst.ECU_END_RX);			
					Transmit((byte)ConsultECUConst.ECU_FRAME_END_CMD);
					//TODO: Зациклить получение байта и его проверку на инверсность стоповому. ввести счетчик
					byte recv = Receive(1)[0];
					CheckInverseBytes((byte)ConsultECUConst.ECU_FRAME_END_CMD, recv);
					return;
				}
				catch (TimeoutException)
				{
					continue;
				}
			}
		}

		/// <summary>
		/// Посыл сигнала ECU об ожидании фрейма
		/// </summary>
		internal void ECUFrameStart()
		{
			//Сигнал о начале передачи данных от ECU
			base.Transmit((byte)ConsultECUConst.ECU_FRAME_BEGIN_CMD);

			//Ждем появления стартового байта
			//TODO: добавить счетчик принятых байт до появления ожидаемого
			while (Receive(1)[0] != (byte)ConsultECUConst.ECU_FRAME_START_BYTE) ;
		}

		/// <summary>
		/// Прочитать фрейм от ECU
		/// </summary>
		/// <returns>Принятые байты</returns>
		private byte[] ECUFrameGet()
		{
			//Читаем количетсво байт во фрейме
			int frameLen = Receive(1)[0];

			//Принимаем фрейм
			byte[] frame = Receive(frameLen);
			return frame;
		}

		/// <summary>
		/// Проверка двух массивов байт на инверсию 
		/// </summary>
		/// <param name="arr1">Первый массив</param>
		/// <param name="arr2">Второй массив</param>
		/// <returns>Если не являются инверсными - true. Иначе - false</returns>
		private bool CheckInverseBytes(byte[] arr1, byte[] arr2)
		{
			if (arr1.Length != arr2.Length)
				return true;
			for (int i = 0; i < arr1.Length; i++)
				if ((byte)~arr1[i] != arr2[i])
					return true;
			return false;
		}

		/// <summary>
		/// Проверка двух байт на инверсию 
		/// </summary>
		/// <param name="arr1">Первый байт</param>
		/// <param name="arr2">Второй байт</param>
		/// <returns>Если не являются инверсными - true. Иначе - false</returns>
		private bool CheckInverseBytes(byte b1, byte b2)
		{
			if ((byte)~b1 != b2)
				return true;
			return false;
		}
	}
}
