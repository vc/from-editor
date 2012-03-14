using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.Consult.Data
{
	public delegate SensorValue SensorConvertFunc(int data);


	public class ConversionFunctions
	{
		const double _cCelsius2fahrenheitMultiplyer = 33.2;
		const double _cKphPerHour2mphPerHourMultiplyer = 0.6213712;
		public static double SpeedCorrectCoefficient = 1;

		//Engine
		public static ScaleDescription _speedMph = new ScaleDescription("mph", 0, (int)(300 * _cKphPerHour2mphPerHourMultiplyer));
		public static ScaleDescription _speedKph = new ScaleDescription("kph", 0, 300);
		public static ScaleDescription _tempCelsius = new ScaleDescription("deg C", -50, 100);
		public static ScaleDescription _tempFahrenheit = new ScaleDescription("deg F", (int)(-50 * _cCelsius2fahrenheitMultiplyer), (int)(100 * _cCelsius2fahrenheitMultiplyer));
		public static ScaleDescription _voltageV = new ScaleDescription("V", 0, 15);
		public static ScaleDescription _voltageMV = new ScaleDescription("mV", 0, 15000);
		public static ScaleDescription _RPM = new ScaleDescription("RPM", 0, 10000);
		public static ScaleDescription _ignitionBTDC = new ScaleDescription("Deg BTDC", -30, 30);
		public static ScaleDescription _timeMs = new ScaleDescription("mS", 0, 20);
		public static ScaleDescription _percents = new ScaleDescription("%", 0, 100);
		public static ScaleDescription _onOff = new ScaleDescription("ON/OFF", 0, 1);
		public static ScaleDescription _step = new ScaleDescription("Step", 0, 255);
		public static ScaleDescription _airFlow = new ScaleDescription("gm/S", 0, 255);
		public static ScaleDescription _emptyByte = new ScaleDescription("", Byte.MinValue, Byte.MaxValue);

		//Aircon
		public static ScaleDescription _airCon_ambient = new ScaleDescription("", 0, 30);
		public static ScaleDescription _airCon_tempCelsius = new ScaleDescription("°C", 0, 30);
		public static ScaleDescription _airCon_tempFahrenheit = new ScaleDescription("°F", 0, 50);
		public static ScaleDescription _airCon_sunload = new ScaleDescription("kCAL", 0, 2);
		public static ScaleDescription _degrees = new ScaleDescription("DEG", 0, 180);
		public static ScaleDescription _airConAmbient = new ScaleDescription(String.Empty, 0, 30);

		//AT
		public static ScaleDescription _ATgear = new ScaleDescription("Gear", 0, 5);

		//HICAS
		public static ScaleDescription _HICASAmpere = new ScaleDescription("Ampere", 0, 255 / 200f);
		public static bool _isMetric = true;

		public static SensorConvertFunc[] BitConverters = new SensorConvertFunc[] { 
			convertBit0,
			convertBit1,
			convertBit2,
			convertBit3,
			convertBit4,
			convertBit5,
			convertBit6,
			convertBit7
		};

		public static SensorValue convertBit0(int v) { return new SensorValue((v & 0x01) == 1, _onOff); }
		public static SensorValue convertBit1(int v) { return new SensorValue((v & 0x02) == 1, _onOff); }
		public static SensorValue convertBit2(int v) { return new SensorValue((v & 0x04) == 1, _onOff); }
		public static SensorValue convertBit3(int v) { return new SensorValue((v & 0x08) == 1, _onOff); }
		public static SensorValue convertBit4(int v) { return new SensorValue((v & 0x10) == 1, _onOff); }
		public static SensorValue convertBit5(int v) { return new SensorValue((v & 0x20) == 1, _onOff); }
		public static SensorValue convertBit6(int v) { return new SensorValue((v & 0x40) == 1, _onOff); }
		public static SensorValue convertBit7(int v) { return new SensorValue((v & 0x80) == 1, _onOff); }

		#region Engine
		// Converts coolant/fuel temp value from ECU into human readable form
		public static SensorValue convertCoolantTemp(int v)
		{
			v = v - 50;
			return _isMetric
				? new SensorValue(v, _tempCelsius)
				: new SensorValue(v * _cCelsius2fahrenheitMultiplyer, _tempFahrenheit);
		}

		// Convert vehicle speed value from ECU into human readable form
		public static SensorValue convertVehicleSpeed(int v)
		{
			v = v * 2;
			return _isMetric
				? new SensorValue(v * SpeedCorrectCoefficient, _speedMph)
				: new SensorValue(v * _cKphPerHour2mphPerHourMultiplyer * SpeedCorrectCoefficient, _speedMph);
		}

		// Convert Battery Voltage from ECU into human readable form
		public static SensorValue convertBatteryVoltage(int v)
		{
			// Convert to mV
			return new SensorValue(v * 80, _voltageMV);
		}

		// Convert Ignition Timing value from ECU to human readable form
		public static SensorValue convertIgnitionTiming(int v)
		{
			return new SensorValue(110 - v, _ignitionBTDC);
		}

		// Convert tach from ECU to human readable form
		public static SensorValue convertTachometer(int v)
		{
			return new SensorValue(v * 12.5, _RPM);
		}

		public static SensorValue convertTachometerReference(int v)
		{
			return new SensorValue(v * 8, _RPM);
		}

		// Convert Maf voltage from ECU to human readable form
		public static SensorValue convertMafVoltage(int v)
		{
			// return value is in mV
			return new SensorValue(v * 5, _voltageV);
		}

		// Convert O2 voltage from ECU to human readable form
		public static SensorValue convertO2Voltage(int v)
		{
			// Return value is in mV
			return new SensorValue(v * 10, _voltageMV);
		}

		// Convert Throttle position/exhaust gas temp from ECU to human readable form
		public static SensorValue convertThrottlePosition(int v)
		{
			// Return value is in mV
			return new SensorValue(v * 20, _voltageMV);
		}

		// Injection Time
		public static SensorValue convertInjectionTime(int v)
		{
			//Return value is in mS
			return new SensorValue(v / 100, _timeMs);
		}

		// AAC Valve
		public static SensorValue convertAACValve(int v)
		{
			//Return value is in percent
			return new SensorValue(v / 2, _percents);
		}

		//Percent
		public static SensorValue convertPercents(int v)
		{
			return new SensorValue(v, _percents);
		}

		//On/Off
		public static SensorValue convertOnOff(int v) { return new SensorValue(v == 1, _onOff); }

		#endregion

		#region AT
		public static SensorValue convertATSpeed(int v)
		{
			return _isMetric
				? new SensorValue(v * SpeedCorrectCoefficient, _speedKph)
				: new SensorValue(v * _cKphPerHour2mphPerHourMultiplyer * SpeedCorrectCoefficient, _speedMph);
		}
		public static SensorValue convertATVolt(int v) { return new SensorValue(v / 50f, _voltageV); }
		public static SensorValue convertATBatteryVoltage(int v) { return new SensorValue(v * 2 / 25f, _voltageV); }
		public static SensorValue convertATEngineRPM(int v) { return new SensorValue(v * 32, _RPM); }
		public static SensorValue convertATGear(int v) { return new SensorValue(v, _ATgear); }
		public static SensorValue convertATThrottlePosPercent(int v) { return new SensorValue(v / 2.55f, _percents); }
		public static SensorValue convertATLinePressDuty(int v) { return new SensorValue(v * 16 / 25f, _percents); }
		#endregion

		#region HICAS
		public static SensorValue convertHICASSteeringAngle(int v) { return new SensorValue(v / 2f, _degrees); }
		public static SensorValue convertHICASSolenoid(int v) { return new SensorValue(v / 200f, _HICASAmpere); }
		#endregion

		#region AirCon
		public static SensorValue convertAirConTemp(int v)
		{
			double d = v / 16f;
			if (_isMetric)
				return new SensorValue(d, _airCon_tempCelsius);
			else
				return new SensorValue(d * _cCelsius2fahrenheitMultiplyer, _airCon_tempFahrenheit);
		}
		public static SensorValue convertAirConBlowerMotorVoltage(int v) { return new SensorValue(v / 16f, _voltageV); }
		public static SensorValue convertAirConSunload(int v) { return new SensorValue(v / 160f, _airCon_sunload); }
		public static SensorValue convertAirConAmbient(int v) { return new SensorValue(v / 16f, _airCon_ambient); }
		#endregion

		#region AsIs Converters
		public static SensorValue convertAsIs(int v) { return new SensorValue(v, _emptyByte); }

		public static SensorValue convertAsIsPercent(int v) { return new SensorValue(v, _percents); }
		public static SensorValue convertAsIsVolt(int v) { return new SensorValue(v, _voltageV); }
		public static SensorValue convertAsIsStep(int v) { return new SensorValue(v, _step); }
		public static SensorValue convertAsIsTemperature(int v)
		{
			if (_isMetric)
				return new SensorValue(v, _tempCelsius);
			else
				return new SensorValue(v * _cCelsius2fahrenheitMultiplyer, _tempFahrenheit);
		}
		public static SensorValue convertAsIsSpeed(int v)
		{
			if (_isMetric)
				return new SensorValue(v, _speedKph);
			else
				return new SensorValue(v * _cKphPerHour2mphPerHourMultiplyer, _speedMph);
		}
		public static SensorValue convertAsIsTimeMS(int v) { return new SensorValue(v, _timeMs); }
		public static SensorValue convertAsIsAirFlow(int v) { return new SensorValue(v, _airFlow); }
		public static SensorValue convertAsIsDegrees(int v) { return new SensorValue(v, _degrees); }
		#endregion
	}

	/// <summary>
	/// Description (Scale name, min value, max value) of Scale
	/// </summary>
	public class ScaleDescription
	{
		public string Scale { get; private set; }
		public float Min { get; private set; }
		public float Max { get; private set; }
		public ScaleDescription(string scale, float min, float max)
		{
			this.Scale = scale;
			this.Min = min;
			this.Max = max;
		}
	}
}
