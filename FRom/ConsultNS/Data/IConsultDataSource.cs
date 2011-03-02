using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.ConsultNS.Data
{
	public interface IConsultDataSource
	{
		
		string Tostring();

		byte[] InitBytes { get; }

		ListIndexString<ConsultCommand> Commands { get; }

		ListIndexString<ConsultSensor> Sensors { get; }

		ListIndexString<ConsultActiveTest> ActiveTests { get; }
	}
}
