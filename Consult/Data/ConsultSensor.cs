using System.Collections.Generic;
using System;
namespace FRom.Consult.Data
{

	/// <summary>
	/// Типы сенсоров
	/// </summary>
	public enum ConsultTypeOfSensors
	{
		/// <summary>
		/// Два состояния, ВКЛ/ВЫКЛ
		/// </summary>
		OnOff,
		/// <summary>
		/// Переменное значение, измеряется в еденицах scale
		/// </summary>
		Scallable,
		/// <summary>
		/// Битовое представление. Используется bitMap
		/// </summary>
		Bit,
		/// <summary>
		/// Дискретные состояния. Используется словарь descMap
		/// </summary>
		Descrette,
	}
	/// <summary>
	/// Описывает один сенсор
	/// </summary>
	public class ConsultSensor
	{

		#region Members
		/// <summary>
		/// Наименование данных
		/// </summary>
		public readonly string _name;
		/// <summary>
		/// Регистры
		/// </summary>
		public readonly byte[] _registers;
		/// <summary>
		/// Маска регистров
		/// </summary>
		public readonly byte _mask = 0xff;
		/// <summary>
		/// Карта битовых значений
		/// </summary>
		public readonly string[] _bitMap;
		/// <summary>
		/// Тип сенсора
		/// </summary>
		ConsultTypeOfSensors _type = ConsultTypeOfSensors.Scallable;
		/// <summary>
		/// Если _type == Bit, то в этом массиве будет список сенсоров
		/// </summary>
		ConsultSensor[] _bitSensors;
		#endregion

		#region Properties
		private SensorConvertFunc _convertFunc;
		public SensorConvertFunc ConvFunc
		{
			get { return this._convertFunc; }
			private set
			{
				this._convertFunc = value;
				if (value != null)
					this.ScaleDescription = value(1).Scale;
			}
		}

		public ScaleDescription ScaleDescription { get; private set; }
		#endregion

		#region Constructors
		public ConsultSensor(string name, byte[] reg)
			: this(name, reg, ConversionFunctions.convertAsIs) { }

		public ConsultSensor(string name, byte[] reg, SensorConvertFunc func)
		{
			if (func == null)
				throw new NullReferenceException("func");

			this._name = name;
			this._registers = reg;
			this.ConvFunc = func;
		}

		/// <summary>
		/// Битовый регистр
		/// </summary>
		/// <param name="name">Имя</param>
		/// <param name="reg">Регистр</param>
		/// <param name="bitMap">Карта битовых значений
		/// "" - Если бит не используется</param>
		public ConsultSensor(string name, byte[] reg, string[] bitMap)
		{
			if (bitMap.Length != 8)
				throw new ArgumentException("Длина массива карты битовых значений должна быть = 8");

			_type = ConsultTypeOfSensors.Bit;

			//TODO: Разобрать битовый массив с параметрами
			_bitSensors = new ConsultSensor[8];
			for (int i = 0; i < 8; i++)
			{
				if (bitMap[i] == "")
					continue;
				string nameReg = String.Format("{0}_{1}", name, bitMap[i]);
				_bitSensors[i] = new ConsultSensor(nameReg, reg, ConversionFunctions.BitConverters[i]);
			}

			_name = name;
			_registers = reg;
			_bitMap = bitMap;
		}

		/// <summary>
		/// Дискретные значения сенсора
		/// </summary>
		/// <param name="name">Имя</param>
		/// <param name="reg">Регистр</param>
		/// 
		public ConsultSensor(string name, byte[] reg, Dictionary<int, string> dic)
		{

			_type = ConsultTypeOfSensors.Descrette;

			_name = name;
			_registers = reg;

		}
		#endregion

		#region Calc region
		public int BytesToInt(byte[] data)
		{
			//Reverse array, but 1st MSB, 2nd LSB byte
			//TEST: check this reverse
			Array.Reverse(data);
			return (int)BitConverter.ToUInt32(data, 0);
		}

		/// <summary>
		/// Возвращает значение бита в байте, соответствующего переданной маске
		/// </summary>
		/// <param name="data">байт данных</param>
		/// <param name="mask">маска</param>
		/// <returns></returns>
		int SetMask(byte data, byte mask)
		{
			//Маскирую данные
			data = (byte)(data & mask);

			//Выделяю данные
			for (int i = 0; i < 7; i++)
			{
				//определяю номер регистра, в котором значение
				mask = (byte)(mask >> 1);
				if (mask == 0x01)
					return data >> i;
			}
			throw new ArgumentException("Неверная маска");
		}

		byte GetMask(int index)
		{
			if (!(index >= 0 && index <= 7))
				throw new ArgumentException("параметр должен быть в диапазоне от 0 до 7");

			int ret = 1;
			for (int i = 0; i < index; i++)
				ret = ret << 1;

			return (byte)ret;
		}

		#endregion

		/// <summary>
		/// Строковое описание датчика
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public string ValueOf(byte[] data)
		{
			return this.ConvFunc(this.BytesToInt(data)).ToString();
		}

		/// <summary>
		/// Для выборки экземпляра класса из списка путем индексирования по строке.
		/// </summary>
		/// <returns>Имя сенсора</returns>
		public override string ToString()
		{
			return _name;
		}

		/// <summary>
		/// Прототип функции, вызываемой при появлении новых данных сенсоров
		/// </summary>
		/// <param name="data">данные датчиков (bytes)</param>
		[Obsolete()]
		public delegate void SensorNewDataByteEvent(byte[] data);

		/// <summary>
		/// Прототип функции, вызываемой при появлении новых данных сенсоров
		/// </summary>
		/// <param name="data">данные по датчику (real)</param>		
		[Obsolete()]
		public delegate void SensorNewDataFloatEven(float data);

		public delegate void SensorNewData(SensorValue data);

		/// <summary>
		/// Событие появления новых данных датчика (byte)
		/// </summary>
		[Obsolete("Use 'NewData' event")]
		public event SensorNewDataByteEvent NewDataByte;

		/// <summary>
		/// Событие появления новых данных датчика (real)
		/// </summary>
		[Obsolete("Use 'NewData' event")]
		public event SensorNewDataFloatEven NewDataFloat;

		public event SensorNewData NewData;

		/// <summary>
		/// Отправить новые данные пописанным получателям
		/// </summary>
		/// <param name="data">Данные</param>
		public void RaiseNewDataEvent(byte[] data)
		{
			if (NewDataByte != null)
				this.NewDataByte(data);

			if (NewDataFloat != null)
			{
				float tmp = (float)this.ConvFunc(this.BytesToInt(data)).Value;
				this.NewDataFloat(tmp);
			}

			if (NewData != null)
				this.NewData(this.ConvFunc(this.BytesToInt(data)));
		}
	}
}
