using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using FRom.Logger;
using System.Threading;

namespace FRom.Emulator
{
	public class RomulatorBase : IRomulator
	{
		//Константы используемые в протоколе
		const byte _cRomVersion = (byte)'V';
		const byte _cRomReadBlock = (byte)'R';
		const byte _cRomReadByte = (byte)'r';
		const byte _cRomWriteBlock = (byte)'X';
		const byte _cRomHiddenWrite = (byte)'w';
		const byte _cRomHiddenWriteOk = (byte)'O';
		const byte _cRomStatus = (byte)'S';
		const byte _cRomStatusReady = (byte)'O';
		const byte _cRomStatusBusy = (byte)'B';
		const byte _cRomUnknownCommand = (byte)'?';
		const byte _cRomScanFunctionOn = (byte)'P';
		const byte _cRomScanFunctionOff = (byte)'p';
		const byte _cRomWriteOk = (byte)'O';

		const int _cTimeReadPortInit = 800;
		const int _cTimeWaitBeforeRead = 20;

		//счетчик сбоев работы с устройством
		static int _errorsCount = 0;

		//Объект критической секции посылки и приема данных
		Object _lockPort = new Object();

		/// <summary>
		/// класс для приема/передачи информации с RS-232
		/// </summary>
		private SerialPort _port;

		#region Properties
		/// <summary>
		/// Количество повторов при ошибке чтения (не совпадении контрольных байтов)
		/// </summary>
		public int _RetryReadCount
		{
			get;
			set;
		}

		/// <summary>
		/// Количество повторов при ошибке записи (не совпадении контрольных байтов)
		/// </summary>
		public int _RetryWriteCount
		{
			get;
			set;
		}

		/// <summary>
		/// Количество повторов при ошибке скрытой записи (не совпадении контрольных байтов)
		/// </summary>
		public int _RetryHiddenWriteCount
		{
			get;
			set;
		}

		/// <summary>
		/// Количество попыток получения статуса 'Ready' от устройства
		/// </summary>
		public int _RetryGetStatusCount
		{
			get;
			set;
		}

		/// <summary>
		/// Количество ошибок при работе с устройством
		/// </summary>
		public int ErorsCount
		{
			get { return _errorsCount; }
		}

		/// <summary>
		/// Имя COM порта
		/// </summary>
		public string Port
		{
			get { return _port.PortName; }
		}

		#endregion

		public RomulatorBase(string port)
		{
			Init(port);
		}

		public RomulatorBase() { }

		~RomulatorBase()
		{
			Dispose();
		}

		internal void Dispose()
		{
			if (_port != null)
			{
				_port.Close();
				_port.Dispose();
			}
		}

		//версия протокола Интерфейса
		public static RomulatorVersion _SWver = new RomulatorVersion(new byte[] { 1, 0, 0 });

		//версия протокола эмулятор
		private RomulatorVersion _HWver;

		//логгер
		protected Log _log = Log.Instance;

		/// <summary>
		/// Общая инициализация класса
		/// </summary>
		public void Init(string port)
		{
			_port = new SerialPort(port, 115200, Parity.None, 8, StopBits.One);

			_RetryReadCount = 10;			//Количество повторов при ошибках чтения
			_RetryWriteCount = 10;			//Количество повторов при ошибках записи
			_RetryHiddenWriteCount = 10;	//Количество повторов при ошибках скрытой записи
			_RetryGetStatusCount = 100;		//Количество повторов запросов в ожидании статуса 'Ready'

			_port.Handshake = Handshake.None;
			_port.Parity = Parity.None;
			_port.StopBits = StopBits.Two;
			_port.DataBits = 8;

			_port.DtrEnable = true;
			_port.RtsEnable = true;
			_port.ReadBufferSize = 0x4000;
			_port.WriteBufferSize = 0x4000;


			_port.ReadTimeout = 500;		//Таймаут чтения данных из порта
			_port.WriteTimeout = 5000;

			_port.Encoding = Encoding.ASCII;
			//количество принятых байт, для срабатывания события DataReceived
			//_port.ReceivedBytesThreshold = 1;

			//_port.ParityReplace = 0xfe;
			try
			{
				_port.Open();
				_port.BreakState = false;
				Thread.Sleep(_cTimeReadPortInit);
			}
			catch (UnauthorizedAccessException ex)
			{
				throw new RomulatorException("Ошибка при открытии порта", ex);
			}


			RomulatorVersion ver = GetVersion();
			if (ver >= _SWver)		//версия протокола эмулятора больше или равна версии ПО
				_HWver = ver;
			else
				throw new RomulatorException("Версия протокола эмулятора выше чем версия протокола ПО, пожалуйста обновитесь.");
			_log.WriteEntry(this, "Инициализация Romulator прошла успешно. Версия аппаратной части: " + _HWver);

		}

		/// <summary>
		/// Подсчитать контрольный байт к блоку данных, без учета переноса разряда
		/// </summary>
		/// <param name="arr">Массив байт для подсчета контрольного байта</param>
		/// <returns>Байт контрольной суммы</returns>		
		private byte[] CalcChecksumByte(byte[] arr)
		{
			byte checksum = 0;
			unchecked
			{
				for (int i = 0; i < arr.Length; i++)
					checksum += arr[i];
			}
			return new byte[] { checksum };
		}

		/// <summary>
		/// Добавляет к массиву байт контрольной суммы
		/// </summary>
		/// <param name="arr">исходный массив</param>
		/// <returns>массив с контрольной суммой</returns>
		private byte[] AddChecksumByte(byte[] arr)
		{
			int len = arr.Length;
			byte[] rtn = new byte[len + sizeof(byte)];
			Buffer.BlockCopy(arr, 0, rtn, 0, len);
			rtn[len] = CalcChecksumByte(arr)[0];
			return rtn;
		}

		/// <summary>
		/// Проверка контрольной суммы
		/// </summary>
		/// <param name="arr">Массив</param>
		/// <returns>Контрольная сумма в порядке (true)</returns>
		private byte[] VerifyChecksumByte(byte[] arr)
		{
			int arrLen = arr.Length;

			//выделяется исходный массив без контрольной суммы
			byte[] arrWithoutChecksum = new byte[arr.Length - 1];
			Buffer.BlockCopy(arr, 0, arrWithoutChecksum, 0, arrLen - 1);

			byte actualChecksum = CalcChecksumByte(arrWithoutChecksum)[0];

			if (actualChecksum == arr[arrLen - 1])
				return arrWithoutChecksum;
			else
				return null;
		}

		/// <summary>
		/// Подсчитать два контрольных байта к блоку данных, без учета переноса разряда
		/// </summary>
		/// <param name="arr">Массив данных для подсчета контрольного байта</param>
		/// <returns>Два байта контрольной суммы</returns>		
		private byte[] CalcChecksumShort(byte[] arr)
		{
			short checksum = 0;
			unchecked
			{
				for (int i = 0; i < arr.Length; i++)
					checksum += arr[i];
			}
			byte[] rtn = BitConverter.GetBytes(checksum);
			return new byte[] { rtn[0], rtn[1] };
		}

		/// <summary>
		/// Добавляет к массиву байт контрольной суммы
		/// </summary>
		/// <param name="arr">исходный массив</param>
		/// <returns>массив с контрольной суммой</returns>
		private byte[] AddChecksumShort(byte[] arr)
		{
			int lenArr = arr.Length;
			const int lenCheckSum = sizeof(ushort);

			byte[] arrWithChecksum = new byte[lenArr + lenCheckSum];
			Buffer.BlockCopy(arr, 0, arrWithChecksum, 0, lenArr);

			byte[] checkSum = CalcChecksumShort(arr);
			Buffer.BlockCopy(checkSum, 0, arrWithChecksum, lenArr, lenCheckSum);

			return arrWithChecksum;
		}

		/// <summary>
		/// Прочитать блок данных из порта
		/// </summary>
		/// <param name="n">Количествой байт для считывания</param>
		/// <returns>Массив байт</returns>
		private byte[] Receive(int n)
		{
			byte[] arr = new byte[n];

			for (int i = 0; i < n; i++)
				arr[i] = (byte)_port.ReadByte();

			return arr;
		}

		/// <summary>
		/// Прочитать все что есть в буффере для чтения
		/// </summary>
		/// <returns>Массив байт</returns>
		private byte[] Receive()
		{
			int n = _port.BytesToRead;
			byte[] arr = new byte[n];

			for (int i = 0; i < n; i++)
				arr[i] = (byte)_port.ReadByte();

			return arr;

			//string line = _port.ReadExisting();
			//return Encoding.ASCII.GetBytes(line);
			//BitConverter.GetBytes(line); //Read(_port.BytesToRead);
		}

		/// <summary>
		/// Послать массив в порт
		/// </summary>
		/// <param name="arr">Массив для передачи</param>
		private void Transmit(byte[] arr)
		{
			_port.Write(arr, 0, arr.Length);
		}

		/// <summary>
		/// Запрос в порт
		/// </summary>
		/// <param name="send">Массив запроса</param>
		/// <returns>Массив ответа</returns>
		private byte[] Request(byte[] send)
		{
			return Request(send, null);
		}

		/// <summary>
		/// Запрос в порт
		/// </summary>
		/// <param name="send">Массив запроса</param>
		/// <param name="len">Длина ожидаемого ответа</param>
		/// <returns>Массив ответа</returns>
		private byte[] Request(byte[] send, int? len)
		{
#if DEBUG
			_log.WriteEntry(this, send, true);
#endif
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

#if DEBUG
			_log.WriteEntry(this, recv, false);
#endif
			return recv;
		}

		/// <summary>
		/// Сравнить два массива. True - если равны.
		/// </summary>
		/// <param name="arr1">Первый массив</param>
		/// <param name="arr2">Второй массив</param>
		/// <returns>Истина - Массивы равны, Лож - массивы различны</returns>
		private bool Compare(byte[] arr1, byte[] arr2)
		{
			if (arr1.Length != arr2.Length)
				return false;
			for (int i = 0; i < arr1.Length; i++)
				if (arr1[i] != arr2[i]) return false;
			return true;
		}


		/// <summary>
		/// Прочитать блок произвольной длинны
		/// </summary>
		/// <param name="startAddr">Адрес начала чтения</param>
		/// <param name="size">Количество байт для чтения</param>
		/// <returns></returns>
		public byte[] ReadBlockL(ushort startAddr, ushort size)
		{
			//ushort size = 0x8000;
			byte[] arr = new byte[size];
			ushort step = 0x100;
			try
			{
				for (ushort addr = startAddr; addr < startAddr + size; addr += step)
				{
					if ((size + startAddr - addr) < step)
						step = (ushort)(size + startAddr - addr);

					byte[] tmp = ReadBlock((byte)step, addr);
					Buffer.BlockCopy(tmp, 0, arr, addr - startAddr, tmp.Length);
				}
			}
			catch (RomulatorException ex)
			{
				_log.WriteEntry(this, ex);
				return null;
			}
			return arr;
		}

		public void WriteBlockL(ushort startAddr, byte[] data)
		{
			ushort step = 0x100;
			ushort size = (ushort)data.Length;
			try
			{
				for (ushort i = 0; i < size; i += step)
				{
					if ((size - i) < step)
						step = (ushort)(size - i);

					byte[] arr = new byte[step];

					Buffer.BlockCopy(data, i, arr, 0, step);
					WriteBlock(i, arr);
				}
			}
			catch (RomulatorException ex)
			{
				_log.WriteEntry(this, ex);
			}
		}

		public void HiddenWriteL(ushort address, byte[] data)
		{
			ushort step = 0x100;
			ushort size = (ushort)data.Length;
			try
			{
				for (ushort i = 0; i < size; i += step)
				{
					if ((size - i) < step)
						step = (ushort)(size - i);

					byte[] arr = new byte[step];

					Buffer.BlockCopy(data, i, arr, 0, step);
					HiddenWrite(i, arr);
				}
			}
			catch (RomulatorException ex)
			{
				_log.WriteEntry(this, ex);
			}
		}


		#region IFRomulator Members

		/// <summary>
		/// Получить версию ПО эмулятора
		/// </summary>
		/// <returns>Версия в формате #(XXh).(XXh),ID(XXh)</returns>
		public RomulatorVersion GetVersion()
		{
			RomulatorVersion v = null;

			GetStatus(true);

			byte[] vArr;
			int i = 0;
			do
			{
				vArr = Request(new byte[] { _cRomVersion, _cRomVersion });
			} while (vArr.Length != 3 && i-- < _RetryGetStatusCount);

			//инициализации класса данными прочитанными из порта, размером 3 байта			
			v = new RomulatorVersion(vArr);
			return v;
		}

		/// <summary>
		/// Прочитать массив байт от 1 до 256 байт
		/// </summary>
		/// <param name="size">Размер массива (для 256 используй значение 0)</param>
		/// <param name="address">Адрес первого байта</param>
		/// <returns>Запрошенный массив байт</returns>
		public byte[] ReadBlock(byte size, ushort address)
		{
			//разбиваю ushort на 2 байта
			byte[] addr = BitConverter.GetBytes(address);

			//формирование заголовка запроса
			byte[] send = new byte[4] { _cRomReadBlock, size, addr[1], addr[0] };
			send = AddChecksumByte(send);

			//размер массива для чтения
			int len = size == 0 ? 256 : size;
			len += 1;

			//объявление переменной для входного массива
			byte[] recv = null;

			//счетчик количества повторов запроса
			for (int i = 0; i < _RetryReadCount; i++)
			{
				//жду статуса Ready
				GetStatus(true);

				recv = Request(send, len);

				//проверка размера массива и количества считанных байт
				if (len != recv.Length) continue;

				byte[] recvWithoutChecksum = VerifyChecksumByte(recv);
				if (recvWithoutChecksum != null)
				{
					return recvWithoutChecksum;
				}
			}
			throw new RomulatorException("Контрольная сумма не совпадает. О");
		}

		/// <summary>
		/// Прочитать байт
		/// </summary>
		/// <param name="address">Адрес байта</param>
		/// <returns>Запрошенный байт</returns>
		public byte ReadByte(ushort address)
		{
			//разбиваю ushort на 2 байта для передачи
			byte[] addr = BitConverter.GetBytes(address);
			//формирование массива запроса
			byte[] arr_send = new byte[3] { _cRomReadByte, addr[1], addr[0] };
			byte[] recv = null;

			GetStatus();

			recv = Request(arr_send);

			if (recv != null && recv.Length > 0)
				return recv[0];
			else if (recv != null)
				throw new RomulatorException("Неверный ответ эмулятора",
					new ArgumentException("Ожидается байт, пришло: " + BitConverter.ToString(recv))
					);
			else
				throw new RomulatorException("Неверный ответ эмулятора",
					new ArgumentException("Вернулся пустой ответ")
					);
		}

		/// <summary>
		/// Записать массив байт
		/// </summary>
		/// <param name="address">Адрес первого байта</param>
		/// <param name="data">Массив для записи</param>
		public virtual void WriteBlock(ushort address, byte[] data)
		{
			//проверка длины массива
			if (data.Length > 256)
				throw new RomulatorException("Неверный вызов функции",
					new ArgumentException("Длина массива не может быть больше 256 байт")
				);

			//разбиваю ushort на 2 байта
			byte[] addr = BitConverter.GetBytes(address);

			//формирование массива запроса
			byte[] sendHead = new byte[] { _cRomWriteBlock, (byte)data.Length, addr[1], addr[0] };
			byte[] send = new byte[data.Length + sendHead.Length];
			Buffer.BlockCopy(sendHead, 0, send, 0, sendHead.Length);
			Buffer.BlockCopy(data, 0, send, sendHead.Length, data.Length);

			send = AddChecksumByte(send);
			byte[] checksumData = CalcChecksumShort(data);

			//пытаемся записать данные
			for (int i = 0; i < _RetryWriteCount; i++)
			{
				//жду статуса Ready
				GetStatus(true);

				byte[] recv = Request(send);

				if (recv != null && recv.Length == 2 && Compare(recv, checksumData))
					return;

				_errorsCount++;
			}
			throw new RomulatorException("Ошибка записи");
		}

		/// <summary>
		/// "Прозрачная" для конечного устройства запись данных
		/// </summary>
		/// <param name="address">Адрес первого байта</param>
		/// <param name="data">Массив байт</param>
		public virtual void HiddenWrite(ushort address, byte[] data)
		{
			//проверка длины массива
			if (data.Length > 256)
				throw new RomulatorException("Неверный вызов функции",
					new ArgumentException("Длина массива не может быть больше 256 байт")
				);

			//разбиваю ushort на 2 байта
			byte[] addr = BitConverter.GetBytes(address);

			//формирование массива запроса
			byte[] sendHead = new byte[4] { _cRomHiddenWrite, (byte)data.Length, addr[1], addr[0] };
			byte[] send = new byte[data.Length + sendHead.Length];
			Buffer.BlockCopy(sendHead, 0, send, 0, sendHead.Length);
			Buffer.BlockCopy(data, 0, send, sendHead.Length, data.Length);

			send = AddChecksumByte(send);
			byte[] checksumData = CalcChecksumShort(data);

			//пытаемся записать данные
			for (int i = 0; i < _RetryWriteCount; i++)
			{
				//жду статуса Ready
				GetStatus(true);

				byte[] recv = Request(send);

				if (recv != null && recv.Length > 0 && _cRomHiddenWriteOk == recv[0])
					return;

				_errorsCount++;
			}
			throw new RomulatorException("Ошибка записи");
		}

		/// <summary>
		/// Проверка состояния устройства
		/// </summary>
		/// <param name="waitReady">Признак ожидания статуса 'Ready'</param>
		/// <returns></returns>
		public bool GetStatus(bool waitReady)
		{
			_log.WriteEntry(this, "Проверка статуса: ");
			byte[] send = new byte[] { _cRomStatus, _cRomStatus };

			for (int i = 0; waitReady && i < _RetryGetStatusCount; i++)
			{
				byte[] resp = Request(send);

				if (resp == null || resp.Length == 0 || resp[0] != _cRomStatusReady)
				{
					_errorsCount++;
					//Thread.Sleep(5);
					continue;
				}
				else if (resp[0] == _cRomStatusReady) return true;
			}

			if (waitReady)
				throw new RomulatorException("Ошибка при работе с эмулятором",
					new TimeoutException("Время ожидания статуса 'Ready' истекло"));
			else
				return false;
		}

		public bool GetStatus()
		{
			return GetStatus(false);
		}

		#endregion

		public void Scan(ushort startAddress, ushort endAddress)
		{
			//разбиваю ushort на 2 байта для передачи
			byte[] stAddr = BitConverter.GetBytes(startAddress);
			byte[] endAddr = BitConverter.GetBytes(endAddress);

			//формирование массива запроса
			byte[] send = new byte[] { 
				_cRomScanFunctionOn, 
				stAddr[0], stAddr[1], 
				endAddr[0], endAddr[1]
			};


			GetStatus(true);

			Request(AddChecksumByte(send));

			string message = "Response HEX string: ";

			for (int i = 0; i < 1000; i++)
				message += "." + BitConverter.ToString(Receive());

			_log.WriteEntry(this, message);
		}

	}
}
