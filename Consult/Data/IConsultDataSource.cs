using FRom.Consult.Helper;

namespace FRom.Consult.Data
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
