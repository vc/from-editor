
namespace FRom.ConsultNS.Data
{
	/// <summary>
	/// AIRCON ECU Register Table
	/// </summary>
	public class DataAirCon : IConsultDataSource
	{
		protected const string _name = "AirCon ECU";

		#region Static members (Sensors, Commands, ActveTests)
		/// <summary>
		/// Control Unit Initialization
		/// </summary>
		static internal byte[] _initBytes = { 0xFF, 0xFF, 0xDF };

		/// <summary>
		/// Known Control Unit Commands
		/// </summary>
		static internal ListIndexString<ConsultCommand> _lstCommands
			= new ListIndexString<ConsultCommand> { 			
				new ConsultCommand(ConsultTypeOfCommand.ECU_SELF_DIAGNOSTIC, 0xd1),
				new ConsultCommand(ConsultTypeOfCommand.ECU_ERASE_ERROR_CODES, 0xc1),				
				new ConsultCommand(ConsultTypeOfCommand.ECU_ACTIVE_TEST, 0x0a),
			};

		/// <summary>
		/// Known Sensors
		/// </summary>
		static internal ListIndexString<ConsultSensor> _lstSensors
			= new ListIndexString<ConsultSensor> {
				new ConsultSensor("AMBIENT TEMP/S", new byte[] { 0x20 }, "", 1/16f),
				new ConsultSensor("IN CAR SEN HD", new byte[] { 0x21 }, "°CELCIUS", 1/16f),
				new ConsultSensor("IN CAR SEN FT", new byte[] { 0x22 }, "°CELCIUS", 1/16f),
				new ConsultSensor("DEF DUCT SEN", new byte[] { 0x23 }, "°CELCIUS", 1/16f),
				new ConsultSensor("VENT DUCT SEN", new byte[] { 0x24 }, "°CELCIUS", 1/16f),
				new ConsultSensor("FLOOR DUCT SEN", new byte[] { 0x25 }, "°CELCIUS", 1/16f),
				new ConsultSensor("SUNLOAD", new byte[] { 0x26 }, "KCAL", 1/160f),
				new ConsultSensor("COOLANT TEMP/S", new byte[] { 0x27 }, "°CELCIUS", 1/16f),
				new ConsultSensor("MODE DOOR PBR", new byte[] { 0x28 }, "VOLT"),
				new ConsultSensor("OBJ TEMP HEAD", new byte[] { 0x29 }, "°CELCIUS", 1/16f),
				new ConsultSensor("OBJ TEMP FOOT", new byte[] { 0x2a }, "°CELCIUS", 1/16f),
				new ConsultSensor("AIRMIX DOOR 1", new byte[] { 0x2b }, "°CELCIUS", 1/16f),
				new ConsultSensor("AIRMIX DOOR 2", new byte[] { 0x2c }, "°CELCIUS", 1/16f),
				new ConsultSensor("MODE DOOR ANGLE", new byte[] { 0x2d }, "DEGREES"),
				new ConsultSensor("INTAKE DOOR ANGLE", new byte[] { 0x2e }, "DEGREES"),
				new ConsultSensor("BLOWER MOTOR", new byte[] { 0x2f }, "VOLT", 1/16f),
				new ConsultSensor("SET TEMP ADJ", new byte[] { 0x40 }, "°CELCIUS", 1/16f),
				new ConsultSensor("COMPRESSOR", new byte[] { 0x41 }, "ON", "OFF"),
				new ConsultSensor("SET TEMP", new byte[] { 0x42 }, "°CELCIUS", 1/16f),
				new ConsultSensor("MULTI 1", new byte[] { 0x43 }, 
					new string[]{ 
						"", 
						"", 
						"FAN LO MODE", 
						"FAN HI MODE", 
						"REC MODE", 
						"ECON MODE", 
						"DEF MODE", 
						"AUTO MODE" } ),
				new ConsultSensor("MULTI 2", new byte[] { 0x47 }, 
					new string[]{ "DWN SW", "UP SW", "", "", "", "", "", "" }),
			};

		/// <summary>
		/// Known Active Tests
		/// </summary>
		static internal ListIndexString<ConsultActiveTest> _lstActiveTests
			= new ListIndexString<ConsultActiveTest>
			{
				//TODO: FILL TABLE
				/*
new ConsultActiveTest("AIRMIX DOORS OPERATION", 0x2b, ),
new ConsultActiveTest("MODE DOOR OPERATION", 0x2d, ),
new ConsultActiveTest("INTAKE DOOR OPERATION", 0x2e, ),
new ConsultActiveTest("BLOWER MOTOR OPERATION", 0x2f, ),
new ConsultActiveTest("SET DIFF. UPPER/LOWER TARGET TEMP", 0x40, ),
new ConsultActiveTest("CHECK MAGNET CLUTCH OPERATION", 0x41, ),
new ConsultActiveTest("COMPLEX PATTERNS", 0x44, ),
				 */
			};
		#endregion

		#region IConsultDataSource Members

		public string ToString()
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
