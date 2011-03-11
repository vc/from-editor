
using System.Runtime.Serialization;
using System;
namespace FRom.ConsultNS.Data
{
	class DataEngine : IConsultDataSource
	{
		/// <summary>
		/// Имя ECU для которого предназначены команды
		/// </summary>
		const string _name = "Engine ECU";

		#region Static members (Sensors, Commands, ActveTests)
		/// <summary>
		/// Control Unit Initialization
		/// </summary>
		static internal byte[] InitBytes = { 0xFF, 0xFF, 0xEF };

		/// <summary>
		/// Known Control Unit Commands
		/// </summary>
		static internal ListIndexString<ConsultCommand> _lstCommands
			= new ListIndexString<ConsultCommand> { 
			new ConsultCommand(ConsultTypeOfCommand.ECU_SELF_DIAGNOSTIC, 0xd1),
			new ConsultCommand(ConsultTypeOfCommand.ECU_ERASE_ERROR_CODES, 0xc1),
			new ConsultCommand(ConsultTypeOfCommand.ECU_INFO, 0xd0),
			new ConsultCommand(ConsultTypeOfCommand.ECU_ACTIVE_TEST, 0x0a),
		};

		/// <summary>
		/// Known Sensors
		/// </summary>
		static internal ListIndexString<ConsultSensor> _lstSensors
			= new ListIndexString<ConsultSensor> {
			new ConsultSensor("CAS Position", new byte[] { 0x00, 0x01 }, "RPM", 12.5f),
			new ConsultSensor("CAS Reference", new byte[] { 0x02, 0x03 }, "RPM", 8f),
			new ConsultSensor("MAF Voltage", new byte[] { 0x04, 0x05 }, "mV", 5f),
			new ConsultSensor("RH MAF voltage", new byte[] { 0x06, 0x07 }, "mV",5f),
			new ConsultSensor("Coolant temp", new byte[] { 0x08 }, "deg C", 1f, -50f),
			new ConsultSensor("LH O2 Sensor Voltage", new byte[] { 0x09 }, "mV", 10f),
			new ConsultSensor("RH O2 Sensor Voltage", new byte[] { 0x0a }, "mV", 10f),
			new ConsultSensor("Vehicle speed", new byte[] { 0x0b }, "kph", 2f),
			new ConsultSensor("Battery Voltage", new byte[] { 0x0c }, "mV", 80f),
			new ConsultSensor("Throttle Position Sensor", new byte[] { 0x0d }, "mV", 20f),
			new ConsultSensor("Fuel temp", new byte[] { 0x0f }, "deg C", 1f, -50f),
			new ConsultSensor("Intake Air Temp", new byte[] { 0x11 }, "deg C", 1f, -50f),
			new ConsultSensor("Exhaust Gas Temp", new byte[] { 0x12 }, "mV", 20f),
			new ConsultSensor("Digital Bit Register 0x13", new byte[] {0x13}, 
				new string[] {
					"CLSD/THL POS", 
					"Start Signal(Cranking)", 
					"Park/Neutral Switch",
					"Power Steering Sw",
					"A/C On Switch", 
					"",
					"",
					"" }),
			new ConsultSensor("Injection Time (LH)", new byte[] { 0x14, 0x15 }, "mS", 1 / 100f),
			new ConsultSensor("Ignition Timing", new byte[] { 0x16 }, "Deg BTDC", 1f, 110f, false),
			new ConsultSensor("AAC Valve (Idle Air Valve)", new byte[] { 0x17 }, "%", 1 / 2f),
			new ConsultSensor("A/F ALPHA-LH", new byte[] { 0x1a }, "%"),
			new ConsultSensor("A/F ALPHA-RH", new byte[] { 0x1b }, "%"),
			new ConsultSensor("A/F ALPHA-LH (SELFLEARN)", new byte[] { 0x1c }, "%"),
			new ConsultSensor("A/F ALPHA-RH (SELFLEARN)", new byte[] { 0x1d }, "%"),
			new ConsultSensor("Data Control Register 0x1e", new byte[] { 0x1e }, 
				new string[] { 
					"Coolant Fan Lo", 
					"Coolant Fan Hi", 
					"", 
					"", 
					"", 
					"VTC Sol", 
					"Fuel pump Relay", 
					"Aitcon Relay" }),
			new ConsultSensor("Data Control Register 0x1f", new byte[] { 0x1f }, 
				new string[] { 
					"EGR Sol", 
					"", 
					"", 
					"IACV FICD Sol", 
					"", 
					"Wastegate Sol", 
					"P/Reg control valve",
					"" }),
			new ConsultSensor("M/R F/C MNT", new byte[] { 0x21 }, 
				new string[] { "", "", "", "", "", "", "RH-BANK 1=Lean", "LH-BANK 1=Lean" }),
			new ConsultSensor("Injector time (RH)", new byte[] { 0x22, 0x23 }, "mS", 1 / 100f),
			new ConsultSensor("Waste Gate Solenoid", new byte[] { 0x28 }, "%"),
			new ConsultSensor("Turbo Boost Sensor", new byte[] { 0x29 }, "V"),
			new ConsultSensor("Engine Mount", new byte[] { 0x2a }, "On/Off"),
			new ConsultSensor("Position Counter", new byte[] { 0x2e }, ""),
			new ConsultSensor("Purg. Vol. Control Valve", new byte[] { 0x25 }, "Step"),
			new ConsultSensor("Tank Fuel Temperature", new byte[] { 0x26 }, "deg C"),
			new ConsultSensor("FPCM DR", new byte[] { 0x27 }, "V"),
			new ConsultSensor("Fuel Gauge", new byte[] { 0x2f }, "V"),
			new ConsultSensor("FR O2 Heater-B1", new byte[] { 0x30, 0x031 }, "V"),
			new ConsultSensor("Ignition Switch", new byte[] { 0x32 }, ""),
			new ConsultSensor("CAL/LD Value", new byte[] { 0x33 }, "%"),
			new ConsultSensor("B/Fuel Schedule", new byte[] { 0x34 }, "mS"),
			new ConsultSensor("RR O2 Sensor", new byte[] { 0x35, 0x36 }, "V"),
			new ConsultSensor("Absolute Throttle Position", new byte[] { 0x37 }, "V"),
			new ConsultSensor("MAF", new byte[] { 0x38 }, "gm/S"),
			new ConsultSensor("Evap System Pressure", new byte[] { 0x39 }, "V"),
			new ConsultSensor("Absolute Pressure Sensor", new byte[] { 0x3a, 0x4a }, "V"),
			new ConsultSensor("FPCM F/P", new byte[] { 0x52, 0x53 }, "V"),
		};

		/// <summary>
		/// Known Active Tests
		/// </summary>
		static internal ListIndexString<ConsultActiveTest> _lstActiveTests
			= new ListIndexString<ConsultActiveTest> {
			new ConsultActiveTest("COOLANT TEMP TEST", 0x80, "°C", -50, 30, 0x32),
			new ConsultActiveTest("FUEL INJECTION MAIN SIGNALS", 0x81, "%", -10, 10, 0x64),
			new ConsultActiveTest("IGN TIMING TEST", 0x82, "°BTDC", -15, 15),
			new ConsultActiveTest("IACV-AAC/V OPENING", 0x84, "%", 0, 100),
			new ConsultActiveTest("IACV OPENING", 0x85, "%", 0, 100),
			new ConsultActiveTest("IACV/FICD-SOLENOID", 0x87, 0x00, 0x01),
			new ConsultActiveTest("POWER BALANCE TEST", 0x88, 
				new string[] { "CYL1", "CYL2", "CYL3", "CYL4", "CYL5", "CYL6", "CYL7" }),
			new ConsultActiveTest("FUEL PUMP RLY TEST", 0x89, 0x00, 0x01),
			new ConsultActiveTest("P/REG CONTROL SOLENOID", 0x8A, 0x00, 0x01),
			new ConsultActiveTest("SELF LEARN CONTROL", 0x8B, 0x00),
			new ConsultActiveTest("IACV-AAC/V CONTROL", 0x8C, 0x00, 0x01),
			new ConsultActiveTest("VALVE TIMING SOLENOID", 0x8F, 0x00, 0x01),
			new ConsultActiveTest("MAP SW CONTROL SOLENOID", 0x90, 0x00, 0x01),
			new ConsultActiveTest("COOLANT FAN LOW SPEED TEST", 0x93, 0x00, 0x01),
			new ConsultActiveTest("COOLANT FAN HIGH SPEED TEST", 0x94, 0x00, 0x01),
		};
		#endregion

		#region IConsultDataSource Members

		public string ToString()
		{
			return _name;
		}

		byte[] IConsultDataSource.InitBytes
		{
			get { return InitBytes; }
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
