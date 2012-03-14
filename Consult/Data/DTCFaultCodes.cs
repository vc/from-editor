using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.Consult.Data
{
	public class ConsultDTCFaultCodes
	{
		/// <summary>
		/// Описание кодов самодиагностики
		/// </summary>
		static Dictionary<byte, string> _codesDescription = new Dictionary<byte, string>()
		{
			{ 0x11, "Crank Angle Sensor/Camshaft Position Sensor." },
			{ 0x12, "Air Flow Meter/Mass Air Flow Sensor." },
			{ 0x13, "Engine Coolant Temperature Sensor." },
			{ 0x14, "Vehicle Speed Sensor." },
			{ 0x21, "Ignition Signal." },
			{ 0x22, "Fuel Pump." },
			{ 0x23, "Idle Switch." },
			{ 0x24, "Throttle Valve Switch." },
			{ 0x25, "Idle Speed Control Valve." },
			{ 0x28, "Cooling Fan Circuit." },
			{ 0x31, "ECM." },
			{ 0x32, "EGR Function." },
			{ 0x33, "Heated Oxygen Sensor." },
			{ 0x34, "Knock Sensor." },
			{ 0x35, "Exhaust Gas Temperature Sensor." },
			{ 0x36, "EGR Control-Back Pressure Transducer." },
			{ 0x37, "Knock Sensor." },
			{ 0x38, "Right hand bank Closed Loop (B2)." },
			{ 0x41, "Intake Air Temperature Sensor." },
			{ 0x42, "Fuel Temperature Sensor." },
			{ 0x43, "Throttle Position Sensor." },
			{ 0x44, "ECCS Normal Operation." },
			{ 0x45, "Injector Leak." },
			{ 0x47, "Crankshaft Position Sensor." },
			{ 0x51, "Injector Circuit." },
			{ 0x53, "Oxygen Sensor." },
			{ 0x54, "A/T Control." },
			{ 0x55, "No Malfunction." },
			{ 0x63, "No. 6 Cylinder Misfire." },
			{ 0x64, "No. 5 Cylinder Misfire." },
			{ 0x65, "No. 4 Cylinder Misfire." },
			{ 0x66, "No. 3 Cylinder Misfire." },
			{ 0x67, "No. 2 Cylinder Misfire." },
			{ 0x68, "No. 1 Cylinder Misfire." },
			{ 0x71, "Random Misfire." },
			{ 0x72, "TWC Function right hand bank." },
			{ 0x73, "TWC Function right hand bank." },
			{ 0x76, "Fuel Injection System Function right hand bank." },
			{ 0x77, "Rear Heated Oxygen Sensor Circuit." },
			{ 0x82, "Crankshaft Position Sensor." },
			{ 0x84, "A/T Diagnosis Communication Line." },
			{ 0x85, "VTC Solenoid Valve Circuit." },
			{ 0x86, "Fuel Injection System Function right hand bank." },
			{ 0x87, "Canister Control Solenoid Valve Circuit." },
			{ 0x91, "Front Heated Oxygen Sensor Heater Circuit right hand bank." },
			{ 0x94, "TCC Solenoid Valve." },
			{ 0x95, "Crankshaft Position Sensor." },
			{ 0x98, "Engine Coolant Temperature Sensor." },
			{ 0xa1, "Front Heated Oxygen Sensor Heater Circuit right hand bank." },
			{ 0xa3, "Park/Neutral Position Switch Circuit." },
			{ 0xa5, "EGR and EGR Canister Control Solenoid Valve Circuit." },
			{ 0xa8, "Canister Purge Control Valve Circuit" },
		};

		/// <summary>
		/// первое значение - code number. 
		/// второе - number of ECU (engine starts) since the code last ocured
		/// </summary>
		List<KeyValuePair<byte, byte>> _codes;

		/// <summary>
		/// Строка формаирования описания ошибки
		/// {0} - code, {1} - repeat, {2} - description, {3} - new line
		/// </summary>
		string _formatString = "Code:[0x{0:x}] Repeat {1} times since last ocured.  Description:\"{2}\"{3}";

		public ConsultDTCFaultCodes(byte[] arr)
		{
			if (arr.Length % 2 != 0 && arr.Length >= 2)
				throw new ConsultException("Длина массива должна быть не нулевой и кратной двум !");

			_codes = new List<KeyValuePair<byte, byte>>(arr.Length / 2);

			for (int i = 0; i < arr.Length; i = i + 2)
			{
				_codes.Add(new KeyValuePair<byte, byte>( arr[i], arr[i + 1]));
			}
		}

		/// <summary>
		/// ToString со строкой форматирования по умолчанию:
		/// "Code:{0} Repeat {1} times since last ocured.  Description:{2}{3}"
		/// </summary>		
		public override string ToString()
		{
			return ToString(_formatString);
		}

		/// <summary>
		/// ToString с пользовательской строкой форматирования строки
		/// </summary>
		/// <param name="format">Строка форматирования описания ошибки
		/// {0} - code, {1} - repeat, {2} - description, {3} - new line</param>		
		public string ToString(string format)
		{
			string str = "";

			foreach (KeyValuePair<byte, byte> i in _codes)
			{
				string desc;

				try
				{ desc = _codesDescription[i.Key]; }
				catch
				{ desc = "No description"; }

				str += String.Format(format,
					i.Key,
					i.Value,
					desc,
					Environment.NewLine);
			}

			return str;
		}
	}
}
