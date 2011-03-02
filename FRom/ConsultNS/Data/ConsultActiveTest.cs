
using System.Collections.Generic;
namespace FRom.ConsultNS.Data
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
		public readonly byte _regOffset;

		/// <summary>
		/// Минимальное значение шкалы
		/// </summary>
		public readonly float _minScale;

		/// <summary>
		/// Масимальное значение шкалы
		/// </summary>
		public readonly float _maxScale;

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
		/// Словарь дискретного состояния регистра <регистр, значение>
		/// </summary>
		public readonly Dictionary<byte, float> _descMap;

		/// <summary>
		/// Словарь состояний регистра в положениях On/Off
		/// </summary>
		public readonly Dictionary<bool, byte> _onOffMap;

		/// <summary>
		/// Значение регистра воздействия типа Alone
		/// </summary>
		public readonly byte _value = 0x00;

		/// <summary>
		/// Bit registers
		/// </summary>
		/// <param name="pName">Reg.Name</param>
		/// <param name="pReg">Reg.Byte</param>
		/// <param name="pBitMap">Bit.Map</param>
		public ConsultActiveTest(string pName, byte pReg, string[] pBitMap)
		{
			_type = ConsultTypeOfActiveTest.Bit;

			_name = pName;
			_reg = pReg;
			_bitMap = pBitMap;
		}

		/// <summary>
		/// Scallable Type
		/// </summary>
		/// <param name="pName">Имя активного теста</param>
		/// <param name="pReg">Адрес регистра для активации теста</param>
		/// <param name="pRegOffset">Смещение значения регистра при нулевом значении шкалы</param>
		/// <param name="pScaleMultiply">Множитель регистра для получения шкалы</param>
		/// <param name="pMinScale">Минимальное значение шкалы</param>
		/// <param name="pMaxScale">Максимальное значение шкалы</param>
		public ConsultActiveTest(string pName, byte pReg, string pScale, float pMinScale, float pMaxScale, byte pRegOffset = 0, float pScaleMultiply = 1)
		{
			_type = ConsultTypeOfActiveTest.Scallable;

			_name = pName;
			_reg = pReg;
			_scale = pScale;
			_regOffset = pRegOffset;
			_multiply = pScaleMultiply;
			_minScale = pMinScale;
			_maxScale = pMaxScale;
		}

		/// <summary>
		/// OnOff Type
		/// </summary>
		/// <param name="pName">Имя активного теста</param>
		/// <param name="pReg">Адрес регистра для активации теста</param>
		/// <param name="pOff">Значение регистра для выключения</param>
		/// <param name="pOn">Значение регистра для включения</param>
		public ConsultActiveTest(string pName, byte pReg, byte pOff, byte pOn)
		{
			_type = ConsultTypeOfActiveTest.OnOff;

			_name = pName;
			_reg = pReg;
			_onOffMap = new Dictionary<bool, byte>();
			_onOffMap.Add(false, pOff);
			_onOffMap.Add(true, pOn);
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

		/// <summary>
		/// Для выборки экземпляра класса из списка путем индексирования по строке.
		/// </summary>
		/// <returns>Имя экземпляра</returns>
		public override string ToString()
		{
			return _name;
		}
	}

}
