using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using FRom.ConsultNS.Data;
using System.ComponentModel;

namespace FRom.ConsultNS
{
	public class Consult : ConsultBase, IComponent
	{
		public Consult(string port, IConsultData data)
			: base(port, data) { }

		public Consult(IConsultData data)
			: base(data) { }

		public override void Initialise(string port)
		{
			SetClassState(ConsultClassState.ECU_CONNECTING);
			try
			{
				base.Initialise(port);
				SetClassState(ConsultClassState.ECU_IDLE);

				//Диагностика ECU
				SelfTest(_consultData);
			}
			catch (Exception ex)
			{
				SetClassState(ConsultClassState.ECU_OFFLINE);
				throw new ConsultException("Ошибка при подключении", ex);
			}
		}

		/// <summary>
		/// Объект для блокировки одновременного использования процедуры CheckState
		/// </summary>
		object _lockCheckClassState = new object();

		/// <summary>
		/// Class state
		/// </summary>
		protected ConsultClassState _classState = ConsultClassState.ECU_OFFLINE;

		/// <summary>
		/// Проверка состояния ECU
		/// </summary>
		/// <param name="consultClassState">Состояние ECU, которое надо выставить если ECU_IDLE.
		/// Если ECU_IDLE - освободить</param>
		private void SetClassState(ConsultClassState consultClassState)
		{
			lock (_lockCheckClassState)
			{
				switch (consultClassState)
				{
					//Если ECU_IDLE то выставляем - свободен и выходим
					case ConsultClassState.ECU_IDLE:
						_classState = ConsultClassState.ECU_IDLE;
						return;

					case ConsultClassState.ECU_CONNECTING:
						if (_classState != ConsultClassState.ECU_OFFLINE)
							throw new ConsultException("Перед подключением необходимо отключиться!");
						break;

					case ConsultClassState.ECU_OFFLINE:
						_classState = consultClassState;
						break;

					default:
						if (_classState != ConsultClassState.ECU_IDLE)
							throw new ConsultException("ECU Занят...");

						_classState = consultClassState;
						break;
				}

				//raise event if not null
				if (consultClassState != _classState && ClassStateChanged != null)
					ClassStateChanged(_classState);
			}
		}

		/// <summary>
		/// Источник комманд consult
		/// </summary>
		public IConsultData DataSource
		{
			get { return base._consultData; }
			set
			{
				if (value == _consultData)
					return;
				if (_classState != ConsultClassState.ECU_OFFLINE)
					Disconnect();
				base._consultData = value;
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
			return new ConsultECUPartNumber(base.RequestECUData(cmd));
		}

		/// <summary>
		/// Read DTC (fault codes)
		/// </summary>
		/// <returns>Коды ошибок</returns>
		public ConsultDTCFaultCodes DTCFaultCodesRead()
		{
			ConsultCommand cmd = _consultData.GetCommand(ConsultTypeOfCommand.ECU_SELF_DIAGNOSTIC);
			return new ConsultDTCFaultCodes(base.RequestECUData(cmd));
		}

		/// <summary>
		/// Clear DTC (fault codes)		
		/// </summary>
		public ConsultDTCFaultCodes DTCFaultCodesClear()
		{
			ConsultCommand cmd = _consultData.GetCommand(ConsultTypeOfCommand.ECU_ERASE_ERROR_CODES);
			return new ConsultDTCFaultCodes(base.RequestECUData(cmd));
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
				request.Add((byte)ConsultECUConst.ECU_ROM_READ_BYTE_CMD);

				//Делим uint16 на два байта и добавляем к коллекции
				byte[] tmp_addr = BitConverter.GetBytes(addresses[i]);
				request.Add(tmp_addr[1]);
				request.Add(tmp_addr[0]);

				counter++;
				if (counter == (int)ConsultECUConst.ECU_ROM_MAX_BYTES)
				{
					//Выделяется место под байтовый массив запроса и заполняется из списка
					byte[] tmp = request.ToArray();
					//Получаем ответ от ECU
					byte[] tmp_resp = base.RequestECUData(tmp, (byte)ConsultECUConst.ECU_ROM_READ_BYTE_CMD);
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
				byte[] tmp_resp = base.RequestECUData(tmp, (byte)ConsultECUConst.ECU_ROM_READ_BYTE_CMD);
				//Копируем в общий массив запрошенных байт
				Buffer.BlockCopy(tmp_resp, 0, resp, resp_index, tmp_resp.Length);
			}
			return resp;
		}

		public override void Disconnect()
		{
			//TODO: Послать команду ECU о прекращении передачи всех пакетов.
			base.Disconnect();
			SetClassState(ConsultClassState.ECU_OFFLINE);
		}

		void IDisposable.Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
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
		SensorMonitoringParams _sensors =
			new SensorMonitoringParams((int)ConsultECUConst.ECU_REG_MAX_READS);

		/// <summary>
		/// Read any Register Parameter (Live sensor data stream)
		/// </summary>
		public void SensorAdd(ConsultSensor sens)
		{
			if (_sensors.Length + sens._registers.Length > (int)ConsultECUConst.ECU_REG_MAX_READS)
				throw new ConsultException("Максимальное количество сенсоров - " + (int)ConsultECUConst.ECU_REG_MAX_READS);
			_sensors.Add(sens);
		}

		public void SensorRemove(ConsultSensor sens)
		{
			_sensors.Remove(sens);
		}

		public void SensorAddRange(List<ConsultSensor> sens)
		{
			foreach (ConsultSensor i in sens)
				this.SensorAdd(i);
		}

		/// <summary>
		/// Старт захвата параметров
		/// </summary>
		public void SensorStartLive()
		{
			if (_sensors.Length == 0)
				throw new ConsultException("Нет доступных сенсоров, Сначала добавьте сенсор");

			byte[] cmd = _sensors.GetCommandToECU().ToArray();

			//Отправляю команду
			SendCommand(cmd, ConsultECUConst.ECU_REG_READ_CMD);

			//старт приема данных
			ECUFrameStart();

			_flagSensorsDataReceive = true;

			//Запускаем процедуру приема данных

			// чтобы из рабочего потока можно было "присоединиться" к текущему потоку.
			AsyncOperation ao = AsyncOperationManager.CreateOperation(null);
			// запустить рабочий поток
			new AsyncOperationInvoker(SensorLiveScanProcedure).BeginInvoke(ao, null, null);
		}

		public delegate void AsyncOperationInvoker(AsyncOperation operation);

		/// <summary>
		/// флаг работы live приема данных от ECU по сенсорам.
		/// </summary>
		private bool _flagSensorsDataReceive = false;

		/// <summary>
		/// остановить прием данных по сенсорам
		/// </summary>
		public void SensorStopLive()
		{
			_flagSensorsDataReceive = false;
		}

		/// <summary>
		/// Потоковая функция приема данных сенсоров
		/// </summary>
		/// <param name="operation"></param>
		protected void SensorLiveScanProcedure(AsyncOperation operation)
		{
			byte recv;
			while (_flagSensorsDataReceive)
			{
				//Ждем появления стартового байта
				do
				{
					recv = base.Receive(1)[0];
				} while (recv != (byte)ConsultECUConst.ECU_FRAME_START_BYTE);

				//Длина фрейма
				int lenFrame = base.Receive(1)[0];

				List<byte> frame = new List<byte>(Receive(lenFrame));

				// в теле этого метода можно обращаться к любому контролу в UI.
				SendOrPostCallback callback = delegate(object newFrame)
				{
					//отправляем фрейм получателям
					_sensors.DataUpdate(newFrame as List<byte>);
				};
				// передать данные
				operation.Post(callback, frame);
			}
			ECUFrameStop();
		}

		internal void SelfTest(IConsultData data)
		{
			// Sensors
			_log.WriteEntry(this, "Begin selftest SENSORS", Logger.EventEntryType.Debug);
			SensorMonitoringParams sensMon;
			foreach (ConsultSensor i in data.AllSensors)
			{
				sensMon = new SensorMonitoringParams(1);
				sensMon.Add(i);
				byte[] cmd = sensMon.GetCommandToECU().ToArray();
				try
				{
					SendCommand(cmd, ConsultECUConst.ECU_REG_READ_CMD);
					data.ValidateSensor(i);
					string msg = String.Format("Sensor '{0}' [{1}] valid",
						i._name,
						BitConverter.ToString(i._registers));
					_log.WriteEntry(this, msg, Logger.EventEntryType.Debug);
				}
				catch (NotSupportedException)
				{
					string msg = String.Format("Sensor '{0}' [{1}] is NOT supported",
						i._name,
						BitConverter.ToString(i._registers));
					_log.WriteEntry(this, msg, Logger.EventEntryType.Debug);
				}
				catch (Exception ex)
				{
					_log.WriteEntry(this, ex);
				}
			}
			ECUFrameStop();

		}

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
