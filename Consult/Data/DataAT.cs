
using System.Collections.Generic;
using FRom.Consult.Helper;
namespace FRom.Consult.Data
{
	/// <summary>
	/// A/T ECU Sensor Register Table
	/// </summary>
	public class DataAT : IConsultDataSource
	{
		protected const string _name = "AT ECU";

		#region Static members (Sensors, Commands, ActveTests)
		/// <summary>
		/// Control Unit Initialization
		/// </summary>
		static byte[] _initBytes = { 0xFF, 0xFF, 0xEA };

		/// <summary>
		/// Known Control Unit Commands
		/// </summary>
		static internal ListIndexString<ConsultCommand> _lstCommands
			= new ListIndexString<ConsultCommand>(new ConsultCommand[] {
				new ConsultCommand(ConsultTypeOfCommand.ECU_STREAM_ALL_AVAILABLE_SENSORS, 0x9f),
				new ConsultCommand(ConsultTypeOfCommand.ECU_SELF_DIAGNOSTIC, 0xd1),
				new ConsultCommand(ConsultTypeOfCommand.ECU_ERASE_ERROR_CODES, 0xc1),
				new ConsultCommand(ConsultTypeOfCommand.ECU_INFO, 0xf0),
			                                      });

		/// <summary>
		/// Known Sensors
		/// </summary>
		static internal ListIndexString<ConsultSensor> _lstSensors
			= new ListIndexString<ConsultSensor>(new ConsultSensor[] {
				new ConsultSensor("VHCL/S SE-A/T", new byte[] { 0x00 }, ConversionFunctions.convertATSpeed),
				new ConsultSensor("VHCL/S SE-MTR", new byte[] { 0x01 }, ConversionFunctions.convertATSpeed),
				new ConsultSensor("THRTL POS SEN", new byte[] { 0x02 }, ConversionFunctions.convertATVolt),
				new ConsultSensor("FLUID REMP SEN", new byte[] { 0x03 },ConversionFunctions.convertATVolt),
				new ConsultSensor("BATTERY VOLTAGE", new byte[] { 0x04 }, ConversionFunctions.convertBatteryVoltage),
				new ConsultSensor("ENGINE SPEED", new byte[] { 0x06 }, ConversionFunctions.convertATEngineRPM),
				new ConsultSensor("MULTI 1", new byte[] { 0x08 }, new string[] { "OD-SW", "R-POS SW", "ASCD-CRUISE", "ASCD-OD CUT", "W/O THRTL/P-SW", "POWER SHIFT SW","CLODED THL/SW","KICKDOWN SW"} ),
				new ConsultSensor("MULTI 2", new byte[] { 0x09 }, new string[] { "SHIFT S/V A", "SHIFT S/V B", "OVRRUN/C S/V", "HOLD SW", "", "", "", "" }),
				new ConsultSensor("MULTI 3", new byte[] { 0x0a }, new string[] { "","","","","1-POS SW", "2-POS SW", "D-POS SQ", "P/N POS SW" }),
				new ConsultSensor("GEAR", new byte[] { 0x0c }, ConversionFunctions.convertATGear),
				new ConsultSensor("SELECT LVR POS", new byte[] { 0x0d }, new Dictionary<int,string>{{0x80,"D"}, {0x81,"2"}, {0x82,"1"},{0x83,"N-P"}, {0x87, "R"}}),
				new ConsultSensor("VEHICLE SPEED", new byte[] { 0x0e }, ConversionFunctions.convertATSpeed),
				new ConsultSensor("THROTTLE POS", new byte[] { 0x0f }, ConversionFunctions.convertATThrottlePosPercent),
				new ConsultSensor("LINE PRESS DUTY", new byte[] { 0x10 }, ConversionFunctions.convertATLinePressDuty),
				new ConsultSensor("TCC S/V DUTY", new byte[] { 0x11 }, ConversionFunctions.convertAsIsPercent),
			                                      });

		/// <summary>
		/// Known Active Tests
		/// </summary>
		static internal ListIndexString<ConsultActiveTest> _lstActiveTests
			= new ListIndexString<ConsultActiveTest>(new ConsultActiveTest[] { });
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
