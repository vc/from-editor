using System;
using System.Collections.Generic;
using System.Text;
using Helper;

namespace FRom.ConsultNS.Data
{
	public interface IConsultDataSource
	{
		
		string ToString();

		byte[] InitBytes { get; }

		ListIndexString<ConsultCommand> Commands { get; }

		ListIndexString<ConsultSensor> Sensors { get; }

		ListIndexString<ConsultActiveTest> ActiveTests { get; }
	}
}
