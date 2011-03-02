
namespace FRom.ConsultNS.Data
{
	/// <summary>
	/// Константы ECU, протокола consult
	/// </summary>
	internal enum ConsultECUConst
	{
		/// <summary>
		///  Байт начала посылки данных от ECU
		/// </summary>
		ECU_FRAME_BEGIN_CMD = 0xf0,
		/// <summary>
		/// Байт прекращения посылки данных от ECU
		/// </summary>
		ECU_FRAME_END_CMD = 0x30,
		/// <summary>
		/// Frame start leader character
		/// </summary>
		ECU_FRAME_START_BYTE = 0xff,
		
		/// <summary>
		/// Команда чтения байта ROM или RAM ECU
		/// </summary>
		ECU_ROM_READ_BYTE_CMD = 0xc9,
		/// <summary>
		/// Максимальное количество одновременно запрашиваемых байт в одном фрейме
		/// </summary>
		ECU_ROM_MAX_BYTES = 8,

		/// <summary>
		/// Команда чтения содержимого регистра ECU
		/// </summary>		
		ECU_REG_READ_CMD = 0x5a,
		/// <summary>
		/// Ответ от ECU если запрошенный сенсор не поддерживается
		/// </summary>
		ECU_REG_NOT_SUPPORTED = 0xfe,
		/// <summary>
		/// Максимальное количество регистров, которое можно читать одновременно
		/// </summary>
		ECU_REG_MAX_READS = 20,		

		/// <summary>
		/// Команда запуска активного теста ECU
		/// </summary>
		ECU_ACTIVE_TEST_CMD = 0x0a,

	}
}
