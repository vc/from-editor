using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Helper.Logger;

namespace FRom.Logic
{
	/// <summary>
	/// Базовый класс, описывающий минимально необходимую структуру карты 
	/// один экземпляр класса соответствует одной строке конфигурации в адресном файле (*.adr):
	/// HIGH_FUEL,&H3D00,16,16,256,1,High octane Fuel map,Comment
	/// </summary>
	[Serializable]
	public class AddressInstanceBase
	{
		enum AddressParam
		{
			/// <summary>
			/// Имя константы, однозначно определяющей карту
			/// </summary>
			Variable = 0,
			/// <summary>
			/// Стартовый адрес карты в HEX формате
			/// </summary>
			StartAddress = 1,
			/// <summary>
			/// Размер карты по X
			/// </summary>
			X = 2,
			/// <summary>
			/// Размер карты по Y
			/// </summary>
			Y = 3,
			/// <summary>
			/// Количествоя чеек в карте
			/// </summary>
			MapSize = 4,
			/// <summary>
			/// Количество реальных едениц в одной логической
			/// </summary>
			Value = 5,
			/// <summary>
			/// Описание карты
			/// </summary>
			MapName = 6,
			/// <summary>
			/// Пользовательский комментарий к карте
			/// </summary>
			MapComment = 7
		}

		/// Variable,Start-Address [,X,Y,Map size,value,Map-name,Comment] <= OPTIONAL
		#region Members
		// ********* Данные ********* //
		protected uint _startAddress;		//StartAddress - стартовый адрес карты
		protected int _byteOnCell;			//размер одной ячейки в байтах
		protected int _mapSize;		//количество ячеек в карте

		// ********* Отображение данных ********* //
		protected int _X;					//X - размер карты по X (Columns)
		protected int _Y;					//Y - размер карты по Y (Rows)
		protected float _value;				//Value - количество реальных едениц в одной логической (шкала)

		// ********* Доп.данные ********* //
		protected string _variable;			//Variable - имя константы, однозначно определяющей карту
		protected string _mapName;			//MapName - пользовательское наименование карты
		protected string _comment = "";		// - комментарий к карте

		//Признак коррекции карты
		private bool _corrected = false;

		//лог
		private Log _log = Log.Instance;
		#endregion

		#region Constructors and Initialisation
		public AddressInstanceBase() { }

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="line">Строка описывающая карту</param>
		public AddressInstanceBase(string line)
		{
			DeSerialize(line);
		}

		/// <summary>
		/// Конструктор копирования
		/// </summary>
		/// <param name="aib">Источник копирования</param>
		public AddressInstanceBase(AddressInstanceBase aib)
		{
			_byteOnCell = aib._byteOnCell;
			_X = aib._X;
			_comment = aib._comment;
			_variable = aib._variable;
			_mapSize = aib._mapSize;
			_mapName = aib._mapName;
			_Y = aib._Y;
			_value = aib._value;
			_startAddress = aib._startAddress;
		}
		#endregion

		#region Properties
		//Константы названий разделов, для последующего вынесения 
		//их в ресурсник и возможности перевода на другие языки
		internal const string _cAdresation = "Адресация данных";
		internal const string _cView = "Отображение данных";
		internal const string _cName = "Идентификация карты";

		/// <summary>
		/// Стартовый адрес
		/// </summary>
		[Description("Адрес с которого начинается карта. В 16-ричном формате")]
		[DisplayName("Start Map Adress")]
		[Category(_cAdresation)]
		[TypeConverter(typeof(UInt32HexTypeConverter))]
		public UInt32 StartAdress
		{
			get { return _startAddress; }
			set { _startAddress = value; }
		}

		/// <summary>
		/// Количество Столбцов в карте
		/// </summary>
		[Description("Количество ячеек в карте по X")]
		[DisplayName("Count of X cells")]
		[Category(_cAdresation)]
		public int Columns
		{
			get { return _X; }
			set { _X = value; VerifyVariables(); }
		}

		/// <summary>
		/// Количество Строк в карте
		/// </summary>
		[Description("Количество ячеек в карте по Y")]
		[DisplayName("Count of Y cells")]
		[Category(_cAdresation)]
		public int Rows
		{
			get { return _Y; }
			set { _Y = value; VerifyVariables(); }
		}

		/// <summary>
		/// Количество байт в ячейке
		/// </summary>
		[Description("Длина одной ячейки, в байтах")]
		[DisplayName("Bytes on Cell")]
		[Category(_cAdresation)]
		public int LengthCell
		{
			get { return _byteOnCell; }
			set { _byteOnCell = value; }
		}

		/// <summary>
		/// Количество байт в карте. (Высчитывается)
		/// </summary>
		//[NonSerialized]
		[Description("Длина карты, в байтах")]
		[DisplayName("Length")]
		[Category(_cAdresation)]
		public int Length
		{
			get { return _mapSize * _byteOnCell; }
		}

		/// <summary>
		/// Количество ячеек в карте
		/// </summary>
		[Description("Колчетво ячеек в карте")]
		[DisplayName("Count of cells")]
		[Category(_cAdresation)]
		public int CountOfCells
		{
			get { return _mapSize; }
			set { _mapSize = value; VerifyVariables(); }
		}

		/// <summary>
		/// Количество реальных едениц в одной логической (мосштаб)
		/// </summary>
		[Description("Мосштаб (Количество реальных едениц в одной логичесокй)")]
		[DisplayName("Scale")]
		[Category(_cView)]
		public float ValueOfElement
		{
			get { return _value; }
			set { _value = value; }
		}

		/// <summary>
		/// Константа - Идентификатор карты
		/// </summary>
		[Description("Идентификатор карты")]
		[DisplayName("Map constant name")]
		[Category(_cName)]
		public string ConstName
		{
			get { return _variable; }
			set { _variable = value; }
		}

		/// <summary>
		/// Пользовательское название карты
		/// </summary>
		[Description("Описание карты")]
		[DisplayName("Map name")]
		[Category(_cName)]
		public string MapName
		{
			get { return _mapName; }
			set { _mapName = value; }
		}

		/// <summary>
		/// Пользовательский комментарий к карте
		/// </summary>
		[Description("Пользовательский комментарий к карте")]
		[DisplayName("Comment")]
		[Category(_cName)]
		public string Comment
		{
			get { return _comment; }
			set { _comment = value; }
		}
		#endregion

		#region Serialize & Deserialize
		/// <summary>
		/// Проверяется валидность переменных карты
		/// </summary>
		internal void VerifyVariables()
		{
			if (_X > 0 && _Y > 0 && _mapSize > 0)	//если нет нулевых
			{
				if (_X * _Y != _mapSize)
				{
					string m = String.Format("Ошибка при задании свойств X,Y и MapSize в карте '{3}'. Ожидается: X * Y = MapSize. X={0}; Y={1}; MapSize={2}", _X, _Y, _mapSize, _variable);
					throw new FromException(m, FromExceptionErrorNumber.COUNT_OF_CELLS_MUST_EQUAL_COLUMNS_MULTIPLY_ROWS);
				}
			}
		}

		/// <summary>
		/// ДеСериализация карты из строки, вида:
		/// HIGH_FUEL,&H3D00,16,16,256,1,High octane Fuel map
		/// , где данные расположены следующим образом:
		/// Variable,Start-Address,X,Y,Map size,value,Map-name
		/// Variable,Start-Address [,X,Y,Map size,value,Map-name,Comment] <= OPTIONAL
		/// </summary>
		/// <param name="line">Строка для десеарилизации</param>
		private void DeSerialize(string line)
		{
			/*
			#ENGINE SPECIFIC PARAMETERS
			CYLINDER=6
			ENGINECC=3000
			INJECTORCC=370

			#ECU SPECIFIC PARAMETERS
			NTLOOKUPID=42

			#UNIT NAME
			TYPE_UNIT,Z32(VG30DETT) Z32,*,*,*
			VERSION=3

			#HIGH/LOW octane Fuel map and scalers
			HIGH_FUEL,&H7D00,16,16,256,1,High octane Fuel map
			REG_FUEL,&H7000,16,16,256,1,Low octane Fuel map
			/*/
			if (!line.Contains(","))
			{
				string message = String.Format("Неверный формат строки: '{0}'{1}Ожидается наличие хотя бы одного разделителя ','", line, Environment.NewLine);
				//_log.WriteEntry(message, Logger.EventEntryType.Error);
				throw new FromException(message, FromExceptionErrorNumber.LINE_FORMAT);
			}

			//Разбивается строка и заносится в массив
			string[] arr = line.Split(new char[] { ',' }, 8);

			for (int i = 0; i < arr.Length; i++)
			{
				string errMessage = ParceParam(arr[i], (AddressParam)i);
				if (null != errMessage)
				{
					string m = String.Format("Ошибка в строке: '{1}'{0}{2}", Environment.NewLine, line, errMessage);
					throw new FromException(m, FromExceptionErrorNumber.LINE_FORMAT);
				}
			}

			//# Variable,       Start-Address,  X,  Y,  Map size,   value,  Map-name
			//  VQ_MAP,         &H3E18,         16, 1,  104,        1,      VQ map
			//  REG_FIRE,       &H3A00,         16, 16, 256,        1,      Low octane Ignition time
			//  SPEED1_LIMIT,   &H392C,         2,  1,  1,          2,      Speed Limit 1

			//# Variable,Start-Address [X,Y,Map size,value,Map-name] <= OPTIONAL

			StringBuilder errorMessage = new StringBuilder();

			if (_startAddress == 0 && _X == 00 && _Y == 0)
			{
				string m = String.Format("{0}Нулевые значения StartAddress, X, Y. Карта не несет в себе никакой информации. ", Environment.NewLine);
				throw new FromException(m, FromExceptionErrorNumber.LINE_FORMAT);
			}

			int cells = _X * _Y;
			//Проверим чтобы ячеек небыло больше чем размер карты в байтах
			//т.е. в одной ячейке не может быть меньше одного байта
			if (cells > _mapSize)
			{
				//Если всего одна строка, то размер карты = количество ячеек по X
				if (1 == _Y)
				{
					_X = _mapSize;
					string m = String.Format("{0}Неверное значение параметров карты. X не может быть больше MapSize. Минимально допустимый размер ячейки = один байт. т.к. Y = 1, Корректирую X = MapSize. ", Environment.NewLine);
					errorMessage.Append(m);
					_corrected = true;
				}
				//Иначе - ошибка
				else
				{
					string m = String.Format("{0}Неверное значение параметров карты. X не может быть больше MapSize. Минимально допустимый размер ячейки = один байт. ", Environment.NewLine);
					throw new FromException(m, FromExceptionErrorNumber.X_MORE_THAN_MAPSIZE);
				}
			}
			//Определение количество байт на ячеку из MapSize и общего колчества ячеек
			float byteOnCell = (cells == 0) ? 0 : _mapSize / (cells);

			//Коррекция размера ячейки в зависимости от карты:
			switch (_variable)
			{
				case "VQ_MAP":
					if (byteOnCell != 2 || _X != (int)(_mapSize / byteOnCell) || _Y != 1)
					{
						string m = String.Format("{0}Количество байт на ячейку 'MapSize/(X*Y)' не может отличаться от 2. Текущее значение: '{1}' Откорректировано. ", Environment.NewLine, byteOnCell);
						errorMessage.Append(m);
						_corrected = true;
						byteOnCell = 2;
						_X = (int)(_mapSize / byteOnCell);
						_Y = 1;
					}
					break;
				default:
					if (byteOnCell < 1)
					{
						string m = String.Format("{0}Количество байт на ячейку 'MapSize/(X*Y)' не может быть меньше 1. Текущее значение: '{1}' Откорректировано. ", Environment.NewLine, byteOnCell);
						errorMessage.Append(m);
						_corrected = true;
						byteOnCell = 1;
					}
					else if (byteOnCell > 4)
					{
						string m = String.Format("{0}Количество байт на ячейку 'MapSize/(X*Y)' не может быть больше 4. Текущее значение: '{1}' Откорректировано. ", Environment.NewLine, byteOnCell);
						errorMessage.AppendLine(m);
						_corrected = true;
						byteOnCell = 4;
					}
					break;
			}

			//Инициализирую мемберов
			//_startAddress = Start_Address;
			_byteOnCell = (int)byteOnCell;
			_mapSize = _X * _Y * _byteOnCell;

			if (_corrected)
			{
				string m = String.Format("Ошибка в строке: '{1}'{2}{0}Новая строка: '{3}'", Environment.NewLine, line, errorMessage.ToString(), ToString());
				_log.WriteEntry(this, m, EventEntryType.Warning);
			}
		}


		private string ParceParam(string param, AddressParam i)
		{
			string message = null;
			switch (i)
			{
				case AddressParam.Variable:		//Variable
					_variable = param;
					break;
				case AddressParam.StartAddress:		//Start-Address 
					string addressHex = Regex.Match(param, @"[A-Fa-f0-9]+$").Value;
					//Если адрес не найден то заполняется нулем
					try { _startAddress = uint.Parse(addressHex, NumberStyles.HexNumber, CultureInfo.InvariantCulture); }
					catch (Exception ex)
					{
						message = String.Format("{1}{0}Неверный формат адреса: '{2}'{0}Ожидается '&Hxxxx', где xxxx - адрес в hex формате.", Environment.NewLine, ex.Message, param);
						_startAddress = 0;
					}
					break;
				case AddressParam.X:		//X
					try { _X = int.Parse(param, NumberStyles.Integer, CultureInfo.InvariantCulture); }
					catch (Exception ex)
					{
						message = String.Format("{1}{0}Неверный формат параметра: '{2}'{0}Ожидается 'X', где X - целое, положительное число.", Environment.NewLine, ex.Message, param);
						_X = -1;
					}
					break;
				case AddressParam.Y:		//Y
					try { _Y = int.Parse(param, NumberStyles.Integer, CultureInfo.InvariantCulture); }
					catch (Exception ex)
					{
						message = String.Format("{1}{0}Неверный формат параметра: '{2}'{0}Ожидается 'Y', где Y - целое, положительное число.", Environment.NewLine, ex.Message, param);
						_Y = -1;
					}
					break;
				case AddressParam.MapSize:		//Map size
					try { _mapSize = int.Parse(param, NumberStyles.Integer, CultureInfo.InvariantCulture); }
					catch (Exception ex)
					{
						message = String.Format("{1}{0}Неверный формат параметра: '{2}'{0}Ожидается 'n', где n - целое, положительное число.", Environment.NewLine, ex.Message, param);
						_mapSize = -1;
					}
					break;
				case AddressParam.Value:		//value
					try { _value = float.Parse(param, NumberStyles.Float, CultureInfo.InvariantCulture); }
					catch (Exception ex)
					{
						message = String.Format("{1}{0}Неверный формат параметра: '{2}'{0}Ожидается 'n', где n - положительное число.", Environment.NewLine, ex.Message, param);
						_value = -1;
					}
					break;
				case AddressParam.MapName:		//Map-name
					_mapName = param;
					break;
				case AddressParam.MapComment:		//Comment
					_comment = param;
					break;
				default:
					break;
			}
			return message;
		}

		/// <summary>
		/// Инициализация класса
		/// </summary>
		private void InitMap(string Variable, int Start_Address, int X, int Y, int MapSize, float Value, string Map_Name)
		{

		}


		/// <summary>
		/// Перегруженный оператор ToString()
		/// </summary>
		/// <returns>Строка описывающая одну карту</returns>
		public string MapToString()
		{//Adr_File_struct tmp = new Adr_File_struct("RPM_SCALE_FIRE,&H3BF0,16,1,16,50,RPM scale (Ignition time),123");
			return string.Format("{0},&H{1},{2},{3},{4},{5}{6}{7}",
				_variable,
				_startAddress.ToString("X4"),
				_X,
				_Y,
				_mapSize,
				_value.ToString(CultureInfo.InvariantCulture),
				_mapName == "" ? "" : "," + _mapName,
				_comment == "" ? "" : "," + _comment);
		}

		/// <summary>
		/// Конструктор сериализатора
		/// </summary>
		protected AddressInstanceBase(
			  System.Runtime.Serialization.SerializationInfo info,
			  System.Runtime.Serialization.StreamingContext context) { }
		#endregion
	}
}
