
namespace FRom.ConsultNS.Data
{
	/// <summary>
	/// S-HICAS ECU Register Table. (hydraulic)
	/// </summary>
	class DataHICAS : IConsultDataSource
	{
		protected const string _name = "HICAS ECU";

		#region Static members (Sensors, Commands, ActveTests)
		/// <summary>
		/// Control Unit Initialization
		/// </summary>
		static byte[] _initBytes = { 0xFF, 0xFF, 0xE4 };

		/// <summary>
		/// Known Control Unit Commands
		/// </summary>
		static internal ListIndexString<ConsultCommand> _lstCommands
			= new ListIndexString<ConsultCommand> { 
				new ConsultCommand(ConsultTypeOfCommand.ECU_STREAM_ALL_AVAILABLE_SENSORS, 0x9f),
				new ConsultCommand(ConsultTypeOfCommand.ECU_SELF_DIAGNOSTIC, 0xd1),
				new ConsultCommand(ConsultTypeOfCommand.ECU_ERASE_ERROR_CODES, 0xc1),
				new ConsultCommand(ConsultTypeOfCommand.ECU_INFO, 0xf0),
				new ConsultCommand(ConsultTypeOfCommand.ECU_ACTIVE_TEST, 0x0a),
			};

		/// <summary>
		/// Known Sensors
		/// </summary>
		static internal ListIndexString<ConsultSensor> _lstSensors
			= new ListIndexString<ConsultSensor> {
				new ConsultSensor("VHCL SPEED SENSOR", new byte[] { 0x01 }, "KM/H"),
				new ConsultSensor("STEERING ANGLE", new byte[] { 0x02, 0x03 }, "Degrees", 1/2f),
				new ConsultSensor("MULTI 1", new byte[] { 0x04 }, new string[] { "Steering Direction (0=L/1=R)","Neutral Signal", "StopLamp SW", "PKB/Clutch SW", "WOT/P-SW", "Engine Speed 0:<1500RPM, 1:>1500RPM","","" }),
				new ConsultSensor("HICAS SOL", new byte[] { 0x05 }, "Ampere", 1/200f),
				new ConsultSensor("POWER STR SOL", new byte[] { 0x06 }, "Ampere", 1/200f),				
			};

		/// <summary>
		/// Known Active Tests
		/// </summary>
		static internal ListIndexString<ConsultActiveTest> _lstActiveTests
			= new ListIndexString<ConsultActiveTest>
			{
				new ConsultActiveTest("SIMULATED DRIVE", 0x01, 0x00,0x01),
			};

		#endregion

		#region IConsultDataSource Members

		public string Tostring()
		{
			return _name;
		}

		byte[] IConsultDataSource.InitBytes
		{
			get { return _initBytes; }
		}

		public ListIndexString<ConsultCommand> Commands
		{
			get { return _lstCommands; }
		}

		public ListIndexString<ConsultSensor> Sensors
		{
			get { return _lstSensors; }
		}

		public ListIndexString<ConsultActiveTest> ActiveTests
		{
			get { return _lstActiveTests; }
		}

		#endregion
	}
}

