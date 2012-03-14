using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using FRom.Consult.Data;
using FRom.Consult.Helper;
using FRom.Consult.Helper.Logger;

namespace FRom.Consult
{
	public class SensorMonitoringParams : IEnumerable, IComponent
	{
		static Log _log = Log.Instance;

		/// <summary>
		/// список сенсоров для мониторинга
		/// </summary>
		ListIndexString<ConsultSensor> _sensors;

		ConsultProvider _consult;

		/// <summary>
		/// флаг работы live приема данных от ECU по сенсорам.
		/// </summary>
		private bool _flagIsScanning = false;

		public SensorMonitoringParams(ConsultProvider consult)
		{
			_consult = consult;
			_sensors = new ListIndexString<ConsultSensor>((int)ECUConst.ECU_REG_MAX_READS);
		}

		/// <summary>
		/// Количство байт в запросе/ответе к ECU
		/// </summary>
		public int CountOfSensors
		{
			get { return _sensors.Count; }
		}

		/// <summary>
		/// Добавить сенсор для мониторинга
		/// </summary>
		/// <param name="sens">Сенсор</param>
		/// <param name="evnt">Функция callback, вызываемая при появлении данных сенсора</param>
		public void Add(ConsultSensor sens)
		{
			//смотрим существует ли уже сенсор в списке
			ConsultSensor existSensor = this[sens._name];

			//Сенсор уже есть в списке
			if (existSensor != null)
				return;

			_sensors.Add(sens);
		}

		public void Remove(ConsultSensor sens)
		{
			if (_sensors.Contains(sens))
			{
				_sensors.Remove(sens);
			}
		}

		/// <summary>
		/// Команда запроса состояние регисров в ECU 
		/// </summary>
		/// <returns></returns>
		public List<byte> GetCommandToECU()
		{
			//Формирую комманду запроса показаний сенсоров
			//Список байт (сенсоров) которые будут запрашиваться
			List<byte> regToScan = new List<byte>((int)ECUConst.ECU_REG_MAX_READS);
			foreach (ConsultSensor i in _sensors)
				foreach (byte j in i._registers)
					regToScan.Add(j);

			/// непосредственно команда запроса состояний сенсоров
			List<byte> cmd = new List<byte>((int)ECUConst.ECU_REG_MAX_READS);
			foreach (byte i in regToScan)
			{
				cmd.Add((byte)ECUConst.ECU_REG_READ_CMD);
				cmd.Add(i);
			}

			return cmd;
		}

		/// <summary>
		/// Отправка сообщений функциям по callback при появлении новых данных
		/// </summary>
		/// <param name="frame">фрейм данных по сенорам</param>
		public void DataUpdate(object o)
		{
			byte[] frame = (byte[])o;
			//По сенсорам
			for (int s = 0, f = 0; s < _sensors.Count; s++)
			{
				byte[] dataToCallback = new byte[_sensors[s]._registers.Length];

				//По байтам в сенсоре - заполняем массив для отправки.				
				try
				{
					for (int i = 0; i < dataToCallback.Length; i++)
						dataToCallback[i] = frame[f++];

					_log.WriteEntry(this, dataToCallback, _sensors[s]._name);

					//отправляем данные подписанным получателям
					_sensors[s].RaiseNewDataEvent(dataToCallback);
				}
				catch (Exception ex)
				{
					_log.WriteEntry(this, ex);
					throw ex;
				}
			}
		}

		/// <summary>
		/// Добавить сенсор для сканирования
		/// </summary>
		public void SensorAdd(ConsultSensor sens)
		{
			_log.WriteEntry(this,
				String.Format("Adding live sensor: [{0}]", sens.ToString()),
				EventEntryType.Debug);

			if (CountOfSensors + 1 > (int)ECUConst.ECU_REG_MAX_READS)
				throw new ConsultException("Достигнуто максимальное количество сенсоров - " + (int)ECUConst.ECU_REG_MAX_READS);
			if (_flagIsScanning)
			{
				SensorStopLive();
				lock (this)
				{
					_sensors.Add(sens);
				}
				this.SensorStartLive();
			}
			else
			{
				_sensors.Add(sens);
			}
		}
		public void SensorAdd(List<ConsultSensor> sens)
		{
			if (_flagIsScanning)
			{
				SensorStopLive();
				lock (this)
				{
					foreach (ConsultSensor i in sens)
						this.SensorAdd(i);
				}
				this.SensorStartLive();
			}
			else
			{
				foreach (ConsultSensor i in sens)
					this.SensorAdd(i);
			}
		}

		public void SensorRemove(ConsultSensor sens)
		{
			_log.WriteEntry(this,
				String.Format("Removing live sensor: [{0}]", sens.ToString()),
				EventEntryType.Debug);

			//Если сейчас идет прием данных
			if (_flagIsScanning)
			{
				this.SensorStopLive();

				//Дождемся завершения приема данных
				lock (this)
				{
					//удалим информацию о сенсоре
					_sensors.Remove(sens);
				}

				//стартанем скан сенсоров заново
				this.SensorStartLive();
			}
			else
			{
				_sensors.Remove(sens);
			}
		}

		/// <summary>
		/// Идет ли сканирование в текущий момент
		/// </summary>
		public bool IsScanning
		{
			get { return _flagIsScanning; }
		}

		/// <summary>
		/// остановить прием данных по сенсорам
		/// </summary>
		public void SensorStopLive()
		{
			_flagIsScanning = false;
			//ждем завершения сканирования
			//while (_consult.State != ConsultClassState.ECU_IDLE)
			lock (this) { }
		}

		/// <summary>
		/// Старт захвата параметров
		/// </summary>
		public void SensorStartLive()
		{
			//Если сейчас уже скан идет - остановим
			if (_flagIsScanning)
				_flagIsScanning = false;

			lock (this)
			{
				if (CountOfSensors == 0)
					throw new ConsultException("Нет доступных сенсоров, Сначала добавьте сенсор");

				byte[] cmd = GetCommandToECU().ToArray();

				try
				{
					_consult.SetClassState(ConsultClassState.ECU_STREAMING_MONITORS);

					//Отправляю команду
					_consult.SendCommand(cmd, ECUConst.ECU_REG_READ_CMD);

					//старт приема данных
					_consult.ECUFrameStart();

					//Выставляем флаг начала сканирования
					_flagIsScanning = true;

					// Запускаем процедуру приема данных
					// чтобы из рабочего потока можно было "присоединиться" к текущему потоку.
					AsyncOperation ao = AsyncOperationManager.CreateOperation(null);
					// запустить рабочий поток
					new AsyncOperationInvoker(SensorLiveScanProcedure).BeginInvoke(ao, null, null);
				}
				catch (Exception ex)
				{
					_consult.ECUFrameStop();
					_consult.SetClassState(ConsultClassState.ECU_IDLE);
					throw ex;
				}
			}
		}

		private delegate void AsyncOperationInvoker(AsyncOperation operation);

		/// <summary>
		/// Потоковая функция приема данных сенсоров
		/// </summary>
		/// <param name="operation"></param>
		private void SensorLiveScanProcedure(AsyncOperation operation)
		{
			lock (this)
			{
				try
				{
					while (_flagIsScanning)
					{
						byte[] frame = _consult.GetFrame();
						operation.Post(DataUpdate, frame);
					}
				}
				finally
				{
					try
					{
						_consult.ECUFrameStop();
						_consult.SetClassState(ConsultClassState.ECU_IDLE);
					}
					catch (Exception) { }
				}
			}
		}


		private ConsultSensor this[string sensorName]
		{
			get
			{
				try { return _sensors[sensorName]; }
				catch (KeyNotFoundException) { return null; }
			}
		}

		#region IEnumerable Members
		public IEnumerator GetEnumerator()
		{
			return _sensors.GetEnumerator();
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

		#region IDisposable Members

		public void Dispose()
		{
			//Остановим сканирование
			SensorStopLive();
		}

		#endregion
	}
}
