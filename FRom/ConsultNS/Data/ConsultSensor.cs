
using System.Collections.Generic;
using System;
namespace FRom.ConsultNS.Data
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
		/// Еденица измерения
		/// </summary>
		public readonly string _scaling;
		/// <summary>
		/// Смещение значения
		/// </summary>
		public readonly float _offset = 0;
		/// <summary>
		/// Множитель значения
		/// </summary>
		public readonly float _multiply = 1;
		/// <summary>
		/// Индикатор положительного или отрицательного значения
		/// </summary>
		public readonly bool _positive = true;
		/// <summary>
		/// Описание истинного значения
		/// </summary>
		public readonly string _deskTrue = "";
		/// <summary>
		/// Описание ложного значения
		/// </summary>
		public readonly string _deskFalse = "";
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

		#region Constructors
		/// <summary>
		/// Регистр[, со смещенным значением [,множителем [и знаком минуса]]]
		/// </summary>
		/// <param name="name">Имя</param>
		/// <param name="reg">Регистр</param>
		/// <param name="scale">Еденица измерения</param>
		/// <param name="mul">Множитель значения [default=]</param>
		/// <param name="offset">Смещение относительно нуля [default=]</param>
		/// <param name="positive">Индикатор value (pos=true/neg=false)  [default=true]</param>
		public ConsultSensor(string name, byte[] reg, string scale, float mul = 1, float offset = 0, bool positive = true)
		{
			_type = ConsultTypeOfSensors.Scallable;
			switch (scale.ToLower())
			{
				case "rpm":
					MinimumScallable = 0; MaximumScallable = 10000;
					break;
				case "mv":
					MinimumScallable = 0; MaximumScallable = 15000;
					break;
				case "deg c":
					MinimumScallable = -50; MaximumScallable = 100;
					break;
				case "kph":
					MinimumScallable = 0; MaximumScallable = 300;
					break;
				case "ms":
					MinimumScallable = 0; MaximumScallable = 20;
					break;
				case "deg btdc":
					MinimumScallable = -30; MaximumScallable = 30;
					break;
				case "%":
					MinimumScallable = 0; MaximumScallable = 100;
					break;
				case "v":
					MinimumScallable = 0; MaximumScallable = 15;
					break;
				default:
					break;
			}

			_name = name;
			_registers = reg;
			_scaling = scale;
			_multiply = mul;
			_offset = offset;
			_positive = positive;
		}

		/// <summary>
		/// Регистр True/False
		/// </summary>
		/// <param name="name">Имя</param>
		/// <param name="reg">Регистр</param>
		/// <param name="mask">Маска регистра</param>
		/// <param name="T">Описание истинного значения (1)</param>
		/// <param name="F">Описание ложного значения (0)</param>
		public ConsultSensor(string name, byte[] reg, string T, string F, byte mask = 0x01)
		{
			_type = ConsultTypeOfSensors.OnOff;

			_name = name;
			_registers = reg;
			_scaling = "";
			_mask = mask;
			_deskTrue = T;
			_deskFalse = F;
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
				_bitSensors[i] = new ConsultSensor(name + "_" + bitMap[i], reg, "ON", "OFF", GetMask(i));
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
		/// <summary>
		/// Основная функция преобразования данных из массива в конечное значение датчика.
		/// Учитывает типа сенсора _typeSensor
		/// </summary>
		/// <param name="data">Байтовый массив входных данных</param>
		/// <returns>Конечное значение датчика</returns>
		public float GetValue(byte[] data)
		{
			//Переворачиваем массив, т.к. первый байт всегда MSB, второй LSB
			int len = data.Length;
			byte[] dataConv = new byte[len];
			for (int i = 0; i < data.Length; i++)
				dataConv[i] = data[len - i - 1];

			int val = 0;
			switch (len)
			{
				case 1:
					val = dataConv[0];
					break;
				case 2:
					val = BitConverter.ToInt16(dataConv, 0);
					break;
				case 4:
					val = BitConverter.ToInt32(dataConv, 0);
					break;
				default:
					return 0;
			}
			return ConvertData(val);
		}

		/// <summary>
		/// Преобразовать цифровые данные в реальные, согласно заданным в классе настройкам
		/// </summary>
		/// <param name="val">Цифровое значение</param>
		/// <returns>Реальное значение</returns>
		private float ConvertData(int val)
		{
			float valRet = 0;
			switch (_type)
			{
				case ConsultTypeOfSensors.OnOff:
					valRet = SetMask((byte)val, _mask);
					break;
				case ConsultTypeOfSensors.Scallable:
					valRet = _positive
						? val * _multiply + _offset
						: -val * _multiply + _offset;
					break;
				case ConsultTypeOfSensors.Bit:

					break;
				case ConsultTypeOfSensors.Descrette:
					break;
				default:
					break;
			}

			SetMinMax(valRet);

			return valRet;
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

		/// <summary>
		/// Возвращает значение датчика в процентах от 0 до 1.
		/// При расчете значения, функция опирается на значения _min, _max
		/// </summary>
		/// <param name="data">Входные даннык</param>
		/// <returns>Процентное значение от 0 до 1</returns>
		public float GetPercantageFloatValue(byte[] data)
		{
			float fData = GetValue(data);
			float min = MinimumScallable;
			float max = MaximumScallable;

			float ret = (fData - min) / (max - min);
			return ret;
		}

		/// <summary>
		/// Возвращает значение датчика в процентах от 0 до 100.
		/// При расчете значения, функция опирается на значения _min, _max
		/// </summary>
		/// <param name="data">Входные данные</param>
		/// <returns>Процентное значение от 0 до 100</returns>
		public int GetPercantageIntValue(byte[] data)
		{
			float fData = GetValue(data);
			float min = MinimumScallable;
			float max = MaximumScallable;

			int ret = (int)((100 * (fData - min)) / (max - min));
			return ret;
		}


		#endregion
		
		/// <summary>
		/// Строковое описание датчика
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public string ToString(byte[] data)
		{
			return GetValue(data).ToString() + " " + _scaling;
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
		public delegate void SensorNewDataByteEvent(byte[] data);

		/// <summary>
		/// Прототип функции, вызываемой при появлении новых данных сенсоров
		/// </summary>
		/// <param name="data">данные по датчику (real)</param>
		public delegate void SensorNewDataFloatEven(float data);

		/// <summary>
		/// Событие появления новых данных датчика (byte)
		/// </summary>
		public event SensorNewDataByteEvent NewDataByte;

		/// <summary>
		/// Событие появления новых данных датчика (real)
		/// </summary>
		public event SensorNewDataFloatEven NewDataFloat;

		/// <summary>
		/// Функция вызова события NewData
		/// </summary>
		/// <param name="data"></param>
		public void EnvokeNewDataEvent(byte[] data)
		{
			if (NewDataByte != null)
				NewDataByte(data);

			if (NewDataFloat != null)
			{
				float tmp = GetValue(data);
				NewDataFloat(tmp);
			}
		}

		#region Minimum / Maximum area
		/// <summary>
		/// Минимальное конечное значение датчика
		/// </summary>
		float _minScallable;
		/// <summary>
		/// Максимальное конечное значение датчика
		/// </summary>
		float _maxScallable;

		/// <summary>
		/// Подсчитать минимальное и максимальное значение
		/// </summary>
		/// <param name="val">Реальное значение датчика</param>
		void SetMinMax(float val)
		{
			if (val < MinimumScallable) MinimumScallable = val;
			if (val > MaximumScallable) MaximumScallable = val;
		}

		/// <summary>
		/// Делегат функции, вызываемой при новом значении манимального или максимальног означения
		/// </summary>
		/// <param name="newValue"></param>
		public delegate void SensorMaxMinChangedEventHandler(float newValue);

		/// <summary>
		/// Событие нового минимального значения
		/// </summary>
		public event SensorMaxMinChangedEventHandler MinScaleValueChanged;

		/// <summary>
		/// Событие нового максимального значения датчика
		/// </summary>
		public event SensorMaxMinChangedEventHandler MaxScaleValueChanged;

		/// <summary>
		/// Максимальное значение датчика (реальное)
		/// </summary>
		public float MaximumScallable
		{
			get
			{
				return _maxScallable;
			}
			set
			{
				if (MaxScaleValueChanged != null)
					MaxScaleValueChanged(value);
				_maxScallable = value;
			}
		}

		/// <summary>
		/// Минимальное значение датчика (реальное)
		/// </summary>
		public float MinimumScallable
		{
			get
			{
				return _minScallable;
			}
			set
			{
				if (MinScaleValueChanged != null)
					MinScaleValueChanged(value);
				_minScallable = value;
			}
		}
		#endregion
	}
}
