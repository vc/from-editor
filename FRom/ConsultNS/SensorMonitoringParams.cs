using System.Collections;
using System.Collections.Generic;
using FRom.ConsultNS.Data;
using System;
using System.Threading;

namespace FRom.ConsultNS
{
	public class SensorMonitoringParams : IEnumerable
	{
		static Logger.Log _log = Logger.Log.Instance;

		/// <summary>
		/// список сенсоров для мониторинга
		/// </summary>
		ListIndexString<ConsultSensor> _sensors;

		int _lenSensBytes = 0;

		public SensorMonitoringParams(int capacity)
		{
			_sensors = new ListIndexString<ConsultSensor>(capacity);
		}
		public SensorMonitoringParams()
		{
			_sensors = new ListIndexString<ConsultSensor>();
		}

		/// <summary>
		/// Количство байт в запросе/ответе к ECU
		/// </summary>
		public int Length
		{
			get { return _lenSensBytes; }
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

			//Инкримент количества запрошенных регистров
			_lenSensBytes += sens._registers.Length;
		}

		public void Remove(ConsultSensor sens)
		{
			if (_sensors.Contains(sens))
			{
				_sensors.Remove(sens);
				_lenSensBytes -= sens._registers.Length;
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
			List<byte> regToScan = new List<byte>((int)ConsultECUConst.ECU_REG_MAX_READS);
			foreach (ConsultSensor i in _sensors)
				foreach (byte j in i._registers)
					regToScan.Add(j);

			/// непосредственно команда запроса состояний сенсоров
			List<byte> cmd = new List<byte>((int)ConsultECUConst.ECU_REG_MAX_READS);
			foreach (byte i in regToScan)
			{
				cmd.Add((byte)ConsultECUConst.ECU_REG_READ_CMD);
				cmd.Add(i);
			}

			return cmd;
		}

		/// <summary>
		/// Отправка сообщений функциям по callback при появлении новых данных
		/// </summary>
		/// <param name="frame">фрейм данных по сенорам</param>
		public void DataUpdate(List<byte> frame)
		{
			//По сенсорам
			for (int s = 0, f = 0; s < _sensors.Count; s++)
			{
				byte[] dataToCallback = new byte[_sensors[s]._registers.Length];

				//По байтам в сенсоре - заполняем массив для отправки.				
				for (int i = 0; i < dataToCallback.Length; i++)
					dataToCallback[i] = frame[f++];
//at System.ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource)
//at System.ThrowHelper.ThrowArgumentOutOfRangeException()
//at System.Collections.Generic.List`1.get_Item(Int32 index)
//at FRom.ConsultNS.SensorMonitoringParams.DataUpdate(List`1 frame) in d:\MyDocs\_Source\_FRom\from.svn\FRom\ConsultNS\SensorMonitoringParams.cs:line 103
//at FRom.ConsultNS.Consult.<SensorLiveScanProcedure>b__0(Object newFrame) in d:\MyDocs\_Source\_FRom\from.svn\FRom\ConsultNS\Consult.cs:line 319
				_log.WriteEntry(this, dataToCallback, _sensors[s]._name);

				//отправляем данные получателю
				_sensors[s].EnvokeNewDataEvent(dataToCallback);
			}
		}

		public ConsultSensor this[string sensorName]
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


	}
}
