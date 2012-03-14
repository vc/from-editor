using System;
using System.Collections.Generic;
using System.ComponentModel;
using FRom.Consult.Data;
using FRom.Consult.Helper;
using FRom.Consult.Helper.Logger;

namespace FRom.Consult
{
	public class ConsultProvider : ConsultBase, IComponent
	{
		public ConsultProvider(string port, IConsultData data)
			: base(port, data) { }

		public ConsultProvider(IConsultData data)
			: base(data) { }


		public override void Initialise()
		{
			this.Initialise(false);
		}

		public override void Initialise(bool fast)
		{
			if (DataSource == null || String.IsNullOrEmpty(base.Port.PortName))
				throw new ConsultException("Перед инициализацией необходимо задать порт и источник данных");

			SetClassState(ConsultClassState.ECU_CONNECTING);
			try
			{
				base.Initialise(fast);
				SetClassState(ConsultClassState.ECU_IDLE);

				//Диагностика ECU
				SelfTest(_consultData);
			}
			catch (Exception ex)
			{
				Disconnect();
				throw new ConsultException("Ошибка при подключении", ex);
			}
		}

		public void Initialise(string port)
		{
			this.Initialise(false);
		}

		public void Initialise(string port, bool fast)
		{
			if (String.IsNullOrEmpty(port))
				throw new ConsultException("Некорректно задан порт");
			else
			{
				if (IsOnline)
					this.Disconnect();

				//Порт занят нашим классом
				if (!COMPortName.IsPortFree(port))
					throw new ConsultException("Порт недоступен");
			}

			base.COMPort = port;
			Initialise(fast);
		}

		public override string COMPort
		{
			get { return base.COMPort; }
		}

		/// <summary>
		/// Объект для блокировки одновременного использования процедуры CheckState
		/// </summary>
		object _lockChangeClassState = new object();

		/// <summary>
		/// Class state
		/// </summary>
		protected ConsultClassState _classState = ConsultClassState.ECU_OFFLINE;

		/// <summary>
		/// Проверка состояния ECU
		/// </summary>
		/// <param name="newState">Состояние ECU, которое надо выставить если ECU_IDLE.
		/// Если ECU_IDLE - освободить</param>
		internal void SetClassState(ConsultClassState newState)
		{
			lock (_lockChangeClassState)
			{
				ConsultClassState oldState = _classState;
				switch (newState)
				{
					//Если ECU_IDLE то выставляем - свободен и выходим
					case ConsultClassState.ECU_IDLE:
						_classState = ConsultClassState.ECU_IDLE;
						break;
					case ConsultClassState.ECU_CONNECTING:
						if (_classState != ConsultClassState.ECU_OFFLINE)
							throw new ConsultException("Перед подключением необходимо отключиться!");
						break;
					case ConsultClassState.ECU_OFFLINE:
						_classState = newState;
						break;
					case ConsultClassState.ECU_STREAMING_MONITORS:
					default:
						if (_classState != ConsultClassState.ECU_IDLE)
						{
							string errMess = String.Format("Невозможно выставить новое состояние [{0}] при текущем [{2}]. Возможно только если [{1}]",
								newState.ToString(),
								ConsultClassState.ECU_IDLE,
								State);
							throw new ConsultException(errMess);
						}

						_classState = newState;
						break;
				}

				//raise event if not null
				if (newState != oldState && ClassStateChanged != null)
					ClassStateChanged.DynamicInvoke(_classState);
			}
		}

		/// <summary>
		/// Источник комманд consult
		/// </summary>
		public override IConsultData DataSource
		{
			get { return base._consultData; }
			set
			{
				if (value == _consultData)
					return;
				switch (_classState)
				{
					case ConsultClassState.ECU_OFFLINE:
						base._consultData = value;
						break;
					default:
						Disconnect();
						base._consultData = value;
						Initialise();
						break;
				}
			}
		}

		/// <summary>
		/// Class state
		/// </summary>
		public ConsultClassState State
		{
			get { return _classState; }
		}

		/// <summary>
		/// Прототип функции вызова при срабатывании события ClassStateChanged
		/// </summary>
		/// <param name="state"></param>
		public delegate void HandleConsultClassStateChange(ConsultClassState state);

		/// <summary>
		/// Событие, срабатываемое при изменении статуса класса
		/// </summary>
		public event HandleConsultClassStateChange ClassStateChanged;

		/// <summary>
		/// Read ECU Part Number
		/// </summary>
		/// <returns></returns>
		public ConsultECUPartNumber GetECUInfo()
		{
			ConsultCommand cmd = _consultData.GetCommand(ConsultTypeOfCommand.ECU_INFO);

			for (int i = 0; i < _cCountRetry; i++)
			{
				try
				{
					ConsultECUPartNumber ecuInfo = new ConsultECUPartNumber(base.RequestECUData(cmd));
					return ecuInfo;
				}
				catch (ConsultException ex)
				{
					_log.WriteEntry(this, ex);
					continue;
				}
			}

			throw new ConsultException("Количество попыток запросов к устройству истекло");
		}

		/// <summary>
		/// Read DTC (fault codes)
		/// </summary>
		/// <returns>Коды ошибок</returns>
		public ConsultDTCFaultCodes DTCFaultCodesRead()
		{
			ConsultCommand cmd = _consultData.GetCommand(ConsultTypeOfCommand.ECU_SELF_DIAGNOSTIC);
			for (int i = 0; i < _cCountRetry; i++)
			{
				try
				{
					ConsultDTCFaultCodes codes = new ConsultDTCFaultCodes(base.RequestECUData(cmd));
					return codes;
				}
				catch (ConsultException ex)
				{
					_log.WriteEntry(this, ex);
					continue;
				}
			}

			throw new ConsultException("Количество попыток запросов к устройству истекло");
		}

		/// <summary>
		/// Clear DTC (fault codes)		
		/// </summary>
		public ConsultDTCFaultCodes DTCFaultCodesClear()
		{
			ConsultCommand cmd = _consultData.GetCommand(ConsultTypeOfCommand.ECU_ERASE_ERROR_CODES);
			for (int i = 0; i < _cCountRetry; i++)
			{
				try
				{
					ConsultDTCFaultCodes codes = new ConsultDTCFaultCodes(base.RequestECUData(cmd));
					return codes;
				}
				catch (ConsultException ex)
				{
					_log.WriteEntry(this, ex);
					continue;
				}
			}

			throw new ConsultException("Количество попыток запросов к устройству истекло");
		}

		/// <summary>
		/// Read any ROM BYTE (Internal ROM, program code)
		/// </summary>
		/// <param name="addresses"></param>
		/// <returns></returns>
		public byte[] ReadAnyRomBytes(UInt16[] addresses)
		{
			//Прочитанные байты
			byte[] resp = new byte[addresses.Length];
			int resp_index = 0;

			int counter = 0;
			List<byte> request = new List<byte>(20);
			for (int i = 0; i < addresses.Length; i++)
			{
				//Добавляем код запроса байта
				request.Add((byte)ECUConst.ECU_ROM_READ_BYTE_CMD);

				//Делим uint16 на два байта и добавляем к коллекции
				byte[] tmp_addr = BitConverter.GetBytes(addresses[i]);
				request.Add(tmp_addr[1]);
				request.Add(tmp_addr[0]);

				counter++;
				if (counter == (int)ECUConst.ECU_ROM_MAX_BYTES)
				{
					//Выделяется место под байтовый массив запроса и заполняется из списка
					byte[] tmp = request.ToArray();
					//Получаем ответ от ECU
					byte[] tmp_resp = base.RequestECUData(tmp, ECUConst.ECU_ROM_READ_BYTE_CMD);
					//Копируем в общий массив запрошенных байт
					Buffer.BlockCopy(tmp_resp, 0, resp, resp_index, tmp_resp.Length);

					resp_index += tmp_resp.Length;
					request = new List<byte>(20);
					counter = 0;
				}
			}
			//досылаем остатки, если они остались
			if (counter != 0)
			{
				//Выделяется место под байтовый массив запроса и заполняется из списка
				byte[] tmp = request.ToArray();
				//Получаем ответ от ECU
				byte[] tmp_resp = base.RequestECUData(tmp, ECUConst.ECU_ROM_READ_BYTE_CMD);
				//Копируем в общий массив запрошенных байт
				Buffer.BlockCopy(tmp_resp, 0, resp, resp_index, tmp_resp.Length);
			}
			return resp;
		}

		/// <summary>
		/// Прочитать фрейм из потока
		/// </summary>
		/// <returns>Фрейм</returns>
		internal byte[] GetFrame()
		{
			byte recv;

			//Ждем появления стартового байта
			do
			{
				recv = base.Receive(1)[0];
			} while (recv != (byte)ECUConst.ECU_FRAME_START_BYTE);

			//Длина фрейма
			int lenFrame = base.Receive(1)[0];

			byte[] frame = Receive(lenFrame);

			return frame;
		}

		public override void Disconnect()
		{
			if (State != ConsultClassState.ECU_OFFLINE)
				ECUFrameStop();
			base.Disconnect();
			SetClassState(ConsultClassState.ECU_OFFLINE);
		}

		void IDisposable.Dispose()
		{
			Dispose(true);
		}

		private new void Dispose(bool disposing)
		{
			Disconnect();
			base.Dispose(true);
		}

		public bool IsOnline
		{
			get
			{
				return _classState != ConsultClassState.ECU_OFFLINE;
			}
		}

		/// <summary>
		/// коллекция сенсоров, которые читаются в режиме StreamingSensors
		/// </summary>
		SensorMonitoringParams _sensors;

		/// <summary>
		/// Сенсоры для мониторинга
		/// </summary>
		public SensorMonitoringParams MonitoringSensors
		{
			get
			{
				if (_sensors == null)
					_sensors = new SensorMonitoringParams(this);
				return _sensors;
			}
			set
			{
				_sensors = value;
			}
		}

		internal void SelfTest(IConsultData data)
		{
			// Sensors
			_log.WriteEntry(this, "Begin selftest SENSORS", EventEntryType.Debug);
			SensorMonitoringParams sensMon;
			foreach (ConsultSensor i in data.AllSensors)
			{
				sensMon = new SensorMonitoringParams(this);
				sensMon.Add(i);
				byte[] cmd = sensMon.GetCommandToECU().ToArray();
				try
				{
					SendCommand(cmd, ECUConst.ECU_REG_READ_CMD);
					data.ValidateSensor(i);
					string msg = String.Format("Sensor '{0}' [{1}] valid",
						i._name,
						BitConverter.ToString(i._registers));
					_log.WriteEntry(this, msg, EventEntryType.Debug);
				}
				catch (NotSupportedException)
				{
					string msg = String.Format("Sensor '{0}' [{1}] is NOT supported",
						i._name,
						BitConverter.ToString(i._registers));
					_log.WriteEntry(this, msg, EventEntryType.Debug);
				}
				catch (Exception ex)
				{
					_log.WriteEntry(this, ex);
				}
			}
			ECUFrameStop();
		}

		#region Active Tests

		public void ActiveTest(ConsultActiveTest activeTest, byte data)
		{
			byte[] reqest = new byte[]{
				(byte)ECUConst.ECU_ACTIVE_TEST_CMD,
				activeTest._reg,
				data
			};
			SendCommand(reqest, ECUConst.ECU_ACTIVE_TEST_CMD);
		}

		public void ActiveTestReset()
		{
			byte[] reqest = new byte[]{
				(byte)ECUConst.ECU_ACTIVE_TEST_STOP
			};
			SendCommand(reqest);
		}

		#endregion


		#region IComponent Members

		public event EventHandler Disposed;

		public ISite Site
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		#endregion
	}
}
