
namespace FRom.Logic
{
	public enum FromExceptionErrorNumber
	{
		UNKNOWN = 1,			//неизвестная ошибка
		COUNT_OF_PARAM = 1000,	//неверное количетсво параметров в строке
		X_MORE_THAN_MAPSIZE,	//X - больше параметра MapSize
		LINE_FORMAT,				//неверный формат строки
		COUNT_OF_CELLS_MUST_EQUAL_COLUMNS_MULTIPLY_ROWS
	}
}
