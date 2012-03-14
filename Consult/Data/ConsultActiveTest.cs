
using System.Collections.Generic;
namespace FRom.Consult.Data
{
	/// <summary>
	/// Типы активных тестов
	/// </summary>
	public enum ConsultTypeOfActiveTest
	{
		/// <summary>
		/// Регистр воздействия без параметров
		/// </summary>
		Alone,
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
	public class ConsultActiveTest
	{
		/// <summary>
		/// Имя активного теста
		/// </summary>
		public readonly string _name;

		/// <summary>
		/// Адрес регистра
		/// </summary>
		public readonly byte _reg;

		/// <summary>
		/// Еденица измерения
		/// </summary>
		public readonly string _scale;

		/// <summary>
		/// Смещение регистра при нулевом значении шкалы
		/// </summary>
		public readonly int _offset;

		/// <summary>
		/// Минимальное значение шкалы
		/// </summary>
		public readonly float _minScale;

		/// <summary>
		/// Масимальное значение шкалы
		/// </summary>
		public readonly float _maxScale;

		public readonly byte _scallableDefault;

		/// <summary>
		/// Множитель регистра. reg * multiply = realScale
		/// </summary>
		public readonly float _multiply;

		/// <summary>
		/// Указывает на то битовый ли регистр.
		/// </summary>
		public readonly ConsultTypeOfActiveTest _type;

		/// <summary>
		/// Если регистр битовый, описывает что обозначает каждый бит в регистре
		/// </summary>
		public readonly string[] _bitMap;

		/// <summary>
		/// Значение по умолчанию для Bit
		/// </summary>
		public readonly byte _bitDefaultValue;

		/// <summary>
		/// Словарь дискретного состояния регистра <регистр, значение>
		/// </summary>
		public readonly Dictionary<byte, float> _descMap;

		/// <summary>
		/// Словарь состояний регистра в положениях On/Off
		/// </summary>
		public readonly Dictionary<bool, byte> _onOffMap;

		/// <summary>
		/// Значение по умолчанию On/Off
		/// </summary>
		public readonly bool _onOffDefaultValue;

		/// <summary>
		/// Значение регистра воздействия типа Alone
		/// </summary>
		public readonly byte _value = 0x00;

		#region ctor
		/// <summary>
		/// Bit registers
		/// </summary>
		/// <param name="pName">Reg.Name</param>
		/// <param name="pReg">Reg.Byte</param>
		/// <param name="pBitMap">Bit.Map</param>
		public ConsultActiveTest(string pName, byte pReg, string[] pBitMap, byte pDefaultValue)
		{
			_type = ConsultTypeOfActiveTest.Bit;

			_name = pName;
			_reg = pReg;
			_bitMap = pBitMap;
			_bitDefaultValue = pDefaultValue;
		}

		
		public ConsultActiveTest(string pName, byte pReg, string pScale, float pMinScale, float pMaxScale)
			:this(pName, pReg,pScale, pMinScale, pMaxScale,0x00)
		{}
		public ConsultActiveTest(string pName, byte pReg, string pScale, float pMinScale, float pMaxScale, byte pDefault)
			:this(pName, pReg,pScale, pMinScale, pMaxScale,pDefault,0)
		{}
		public ConsultActiveTest(string pName, byte pReg, string pScale, float pMinScale, float pMaxScale, byte pDefault, int pRegOffset):this(pName, pReg,pScale, pMinScale, pMaxScale,pDefault,pRegOffset, 1)
		{}
		/// <summary>
		/// Scallable Type
		/// </summary>
		/// <param name="pName">Имя активного теста</param>
		/// <param name="pReg">Адрес регистра для активации теста</param>
		/// <param name="pRegOffset">Смещение значения регистра при нулевом значении шкалы</param>
		/// <param name="pScaleMultiply">Множитель регистра для получения шкалы</param>
		/// <param name="pMinScale">Минимальное значение шкалы</param>
		/// <param name="pMaxScale">Максимальное значение шкалы</param>
		public ConsultActiveTest(string pName, byte pReg, string pScale, float pMinScale, float pMaxScale, byte pDefault, int pRegOffset, float pScaleMultiply)
		{
			_type = ConsultTypeOfActiveTest.Scallable;

			_name = pName;
			_reg = pReg;
			_scale = pScale;
			_offset = pRegOffset;
			_multiply = pScaleMultiply;
			_minScale = pMinScale;
			_maxScale = pMaxScale;
			_scallableDefault = pDefault;
		}

		public ConsultActiveTest(string pName, byte pReg, byte pOff, byte pOn):this(pName,pReg,pOff,pOn,false)
		{}
			
		/// <summary>
		/// OnOff Type
		/// </summary>
		/// <param name="pName">Имя активного теста</param>
		/// <param name="pReg">Адрес регистра для активации теста</param>
		/// <param name="pOff">Значение регистра для выключения</param>
		/// <param name="pOn">Значение регистра для включения</param>
		public ConsultActiveTest(string pName, byte pReg, byte pOff, byte pOn, bool pDefaultValue)
		{
			_type = ConsultTypeOfActiveTest.OnOff;

			_name = pName;
			_reg = pReg;
			_onOffMap = new Dictionary<bool, byte>() { { false, pOff }, { true, pOn } };
		}

		/// <summary>
		/// Alone Type
		/// </summary>
		/// <param name="pName">Имя активного теста</param>
		/// <param name="pReg">Адрес регистра для активации теста</param>
		/// <param name="pValue">Значение регистра для активации</param>
		public ConsultActiveTest(string pName, byte pReg, byte pValue)
		{
			_type = ConsultTypeOfActiveTest.Alone;

			_name = pName;
			_reg = pReg;
			_value = pValue;
		}
		#endregion

		/// <summary>
		/// Для выборки экземпляра класса из списка путем индексирования по строке.
		/// </summary>
		/// <returns>Имя экземпляра</returns>
		public override string ToString()
		{
			return _name;
		}
		#region Calc region
		/// <summary>
		/// Преобразовать цифровые данные в реальные, согласно заданным в классе настройкам
		/// </summary>
		/// <param name="val">Цифровое значение</param>
		/// <returns>Реальное значение</returns>
		public float GetValue(byte val)
		{
			float valRet = 0;
			switch (_type)
			{
				case ConsultTypeOfActiveTest.OnOff:
					valRet = val;
					break;
				case ConsultTypeOfActiveTest.Scallable:
					valRet = val * _multiply + _offset;
					break;
				case ConsultTypeOfActiveTest.Bit:

					break;
				case ConsultTypeOfActiveTest.Descrette:
					break;
				default:
					break;
			}

			return valRet;
		}
		#endregion
	}

}
