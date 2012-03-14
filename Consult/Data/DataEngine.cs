
using System;
using FRom.Consult.Helper;
namespace FRom.Consult.Data
{
	public class DataEngine : IConsultDataSource
	{
		/// <summary>
		/// Имя ECU для которого предназначены команды
		/// </summary>
		const string _name = "Engine ECU";

		#region Static members (Sensors, Commands, ActveTests)
		/// <summary>
		/// Control Unit Initialization
		/// </summary>
		static internal byte[] _initBytes = { 0xFF, 0xFF, 0xEF };

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
			        new ConsultSensor("CAS Position", new byte[] { 0x00, 0x01 },ConversionFunctions.convertTachometer),
			        new ConsultSensor("CAS Reference", new byte[] { 0x02, 0x03 }, ConversionFunctions.convertTachometerReference),
			        new ConsultSensor("MAF Voltage", new byte[] { 0x04, 0x05 },  ConversionFunctions.convertMafVoltage),
			        new ConsultSensor("RH MAF voltage", new byte[] { 0x06, 0x07 }, ConversionFunctions.convertMafVoltage),
			        new ConsultSensor("Coolant temp", new byte[] { 0x08 }, ConversionFunctions.convertCoolantTemp),
			        new ConsultSensor("LH O2 Sensor Voltage", new byte[] { 0x09 }, ConversionFunctions.convertO2Voltage),
			        new ConsultSensor("RH O2 Sensor Voltage", new byte[] { 0x0a }, ConversionFunctions.convertO2Voltage),
			        new ConsultSensor("Vehicle speed", new byte[] { 0x0b }, ConversionFunctions.convertVehicleSpeed),
			        new ConsultSensor("Battery Voltage", new byte[] { 0x0c }, ConversionFunctions.convertBatteryVoltage),
			        new ConsultSensor("Throttle Position Sensor", new byte[] { 0x0d },ConversionFunctions.convertThrottlePosition),
			        new ConsultSensor("Fuel temp", new byte[] { 0x0f }, ConversionFunctions.convertCoolantTemp),
			        new ConsultSensor("Intake Air Temp", new byte[] { 0x11 }, ConversionFunctions.convertCoolantTemp),
			        new ConsultSensor("Exhaust Gas Temp", new byte[] { 0x12 }, ConversionFunctions.convertThrottlePosition),
			        new ConsultSensor("Digital Bit Register 0x13", new byte[] {0x13},
			                            new string[] {
			                            "CLSD/THL POS",
			                            "Start Signal(Cranking)",
			                            "Park/Neutral Switch",
			                            "Power Steering Sw",
			                            "A/C On Switch",
			                            String.Empty,
			                            String.Empty,
			                            String.Empty }),
			        new ConsultSensor("Injection Time (LH)", new byte[] { 0x14, 0x15 },ConversionFunctions.convertInjectionTime),
			        new ConsultSensor("Ignition Timing", new byte[] { 0x16 }, ConversionFunctions.convertIgnitionTiming),
			        new ConsultSensor("AAC Valve (Idle Air Valve)", new byte[] { 0x17 }, ConversionFunctions.convertAACValve),
			        new ConsultSensor("A/F ALPHA-LH", new byte[] { 0x1a },ConversionFunctions.convertPercents),
			        new ConsultSensor("A/F ALPHA-RH", new byte[] { 0x1b }, ConversionFunctions.convertPercents),
			        new ConsultSensor("A/F ALPHA-LH (SELFLEARN)", new byte[] { 0x1c }, ConversionFunctions.convertPercents),
			        new ConsultSensor("A/F ALPHA-RH (SELFLEARN)", new byte[] { 0x1d }, ConversionFunctions.convertPercents),
			        new ConsultSensor("Data Control Register 0x1e", new byte[] { 0x1e },
			                            new string[] {
			                            "Coolant Fan Lo",
			                            "Coolant Fan Hi",
			                            String.Empty,
			                            String.Empty,
			                            String.Empty,
			                            "VTC Sol",
			                            "Fuel pump Relay",
			                            "Aitcon Relay" }),
			        new ConsultSensor("Data Control Register 0x1f", new byte[] { 0x1f },
			                            new string[] {
			                            "EGR Sol",
			                            String.Empty,
			                            String.Empty,
			                            "IACV FICD Sol",
			                            String.Empty,
			                            "Wastegate Sol",
			                            "P/Reg control valve",
			                            String.Empty }),
			        new ConsultSensor("M/R F/C MNT", new byte[] { 0x21 },
			                            new string[] { String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, "RH-BANK 1=Lean", "LH-BANK 1=Lean" }),
			        new ConsultSensor("Injector time (RH)", new byte[] { 0x22, 0x23 }, ConversionFunctions.convertInjectionTime),
			        new ConsultSensor("Waste Gate Solenoid", new byte[] { 0x28 }, ConversionFunctions.convertAsIsPercent),
			        new ConsultSensor("Turbo Boost Sensor", new byte[] { 0x29 }, ConversionFunctions.convertAsIsVolt),
			        new ConsultSensor("Engine Mount", new byte[] { 0x2a }, ConversionFunctions.convertOnOff),
			        new ConsultSensor("Position Counter", new byte[] { 0x2e }),
			        new ConsultSensor("Purg. Vol. Control Valve", new byte[] { 0x25 }, ConversionFunctions.convertAsIsStep),
			        new ConsultSensor("Tank Fuel Temperature", new byte[] { 0x26 }, ConversionFunctions.convertAsIsTemperature),
			        new ConsultSensor("FPCM DR", new byte[] { 0x27 }, ConversionFunctions.convertAsIsVolt),
			        new ConsultSensor("Fuel Gauge", new byte[] { 0x2f }, ConversionFunctions.convertAsIsVolt),
			        new ConsultSensor("FR O2 Heater-B1", new byte[] { 0x30, 0x031 }, ConversionFunctions.convertAsIsVolt),
			        new ConsultSensor("Ignition Switch", new byte[] { 0x32 }),
			        new ConsultSensor("CAL/LD Value", new byte[] { 0x33 }, ConversionFunctions.convertAsIsPercent),
			        new ConsultSensor("B/Fuel Schedule", new byte[] { 0x34 }, ConversionFunctions.convertAsIsTimeMS),
			        new ConsultSensor("RR O2 Sensor", new byte[] { 0x35, 0x36 }, ConversionFunctions.convertAsIsVolt),
			        new ConsultSensor("Absolute Throttle Position", new byte[] { 0x37 }, ConversionFunctions.convertAsIsVolt),
			        new ConsultSensor("MAF", new byte[] { 0x38 }, ConversionFunctions.convertAsIsAirFlow),
			        new ConsultSensor("Evap System Pressure", new byte[] { 0x39 }, ConversionFunctions.convertAsIsVolt),
			        new ConsultSensor("Absolute Pressure Sensor", new byte[] { 0x3a, 0x4a }, ConversionFunctions.convertAsIsVolt),
			        new ConsultSensor("FPCM F/P", new byte[] { 0x52, 0x53 }, ConversionFunctions.convertAsIsVolt),
			        };

		/// <summary>
		/// Known Active Tests
		/// </summary>
		static internal ListIndexString<ConsultActiveTest> _lstActiveTests
			= new ListIndexString<ConsultActiveTest>
			{
			new ConsultActiveTest("COOLANT TEMP TEST", 0x80, "°C", -50, 130, 0x32, -0x32),
			new ConsultActiveTest("FUEL INJECTION MAIN SIGNALS", 0x81, "%", -10, 10, (byte)0x64, (int)-0x64),
			new ConsultActiveTest("IGN TIMING TEST", 0x82, "°BTDC", -15, 15),
			new ConsultActiveTest("IACV-AAC/V OPENING", 0x84, "%", 0, 100),
			new ConsultActiveTest("IACV OPENING", 0x85, "%", 0, 100),
			new ConsultActiveTest("IACV/FICD-SOLENOID", 0x87, 0x00, 0x01),
			new ConsultActiveTest("POWER BALANCE TEST", 0x88,
			                      new string[] { "CYL1", "CYL2", "CYL3", "CYL4", "CYL5", "CYL6", "CYL7", "CYL8" },
			                      0xff),
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

		public new string ToString()
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
