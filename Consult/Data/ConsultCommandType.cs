using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.Consult.Data
{
	/// <summary>
	/// Тип комманды consult и соответствующий байт команды ( 0xff-0xff-? )
	/// </summary>
	public enum ConsultTypeOfCommand
	{
		ECU_STREAM_ALL_AVAILABLE_SENSORS = 0x9f,
		ECU_SELF_DIAGNOSTIC = 0xd1,
		ECU_ERASE_ERROR_CODES = 0xc1,
		ECU_INFO = 0xd0,
		ECU_ACTIVE_TEST = 0x0a,
	}
}
