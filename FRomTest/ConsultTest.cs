using FRom.ConsultNS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FRom.Logger;
using FRom.ConsultNS.Data;
using System.Threading;

namespace FRomTest
{
	/// <summary>
	///This is a test class for ConsultTest and is intended
	///to contain all ConsultTest Unit Tests
	///</summary>
	[TestClass()]
	public class ConsultTest
	{
		static Consult _target;
		IConsultData _data;
		string _port;

		private Log _log = Log.Instance;
		
		/// <summary>
		/// Перехватчик событий NewMessage
		/// </summary>
		void Instance_NewMessage(object sender, NewMessageEventArgs e)
		{
			Console.WriteLine(e.Message);
		}

		/// <summary>
		/// конструктор
		/// </summary>
		public ConsultTest()
		{
			Initialize();
		}

		private void Initialize()
		{
			_port = "COM11";
			_data = new ConsultData(new DataEngine());
			_target = new Consult(_data);

			Log.Instance.NewMessage += new EventHandler<NewMessageEventArgs>(Instance_NewMessage);
		}

		/// <summary>
		///A test for Consult Constructor
		///</summary>
		[TestMethod()]
		public void ConsultConstructorTest()
		{
			lock (_target)
			{
				_target.Initialise(_port);
				Assert.IsTrue(_target.State == ConsultClassState.ECU_IDLE);
			}
		}

		/// <summary>
		///A test for Initialise
		///</summary>
		[TestMethod()]
		public void InitialiseTest()
		{
			lock (_target)
			{
				try
				{
					_target.Initialise(_port);
				}
				catch (Exception ex)
				{
					Assert.Fail(ex.Message + ex.StackTrace);
				}
			}
		}

		/// <summary>
		///A test for GetECUInfo
		///</summary>
		[TestMethod()]
		public void GetECUInfoTest()
		{
			lock (_target)
			{
				_target.Initialise(_port);
				ConsultECUPartNumber actual;
				try
				{
					actual = _target.GetECUInfo();
					_log.WriteEntry(this, actual.ToString());

					System.Windows.Forms.MessageBox.Show(actual.ToString(), "Actual");

					Assert.IsTrue(true);
				}
				catch (Exception ex)
				{
					_log.WriteEntry(this, ex);
					Assert.Fail(ex.Message + ex.StackTrace);
				}
				//2104-23710-49U10
				//2104-23710-39.55.31.30.
				//2104-23710-9U10
			}
		}

		/// <summary>
		///A test for ReadDTCFaultCodes
		///</summary>
		[TestMethod()]
		public void ReadDTCFaultCodesTest()
		{
			lock (_target)
			{
				_target.Initialise(_port);
				string expected = new ConsultDTCFaultCodes(new byte[] { 0x41, 0x04, 0x14, 0x00, 0x15, 0x0a }).ToString();
				string actual;

				actual = _target.DTCFaultCodesRead().ToString();

				_log.WriteEntry(this, actual);

				System.Windows.Forms.MessageBox.Show(actual, "Actual");

				Assert.AreEqual(expected, actual);
			}
		}

		/// <summary>
		///A test for DTCFaultCodesClear
		///</summary>
		[TestMethod()]
		public void DTCFaultCodesClearTest()
		{
			lock (_target)
			{
				_target.Initialise(_port);
				string expected = new ConsultDTCFaultCodes(new byte[] { 0x55, 0x00 }).ToString();
				string actual;
				actual = _target.DTCFaultCodesClear().ToString();

				_log.WriteEntry(this, actual);

				System.Windows.Forms.MessageBox.Show(actual, "Actual");

				Assert.AreEqual(expected, actual);
			}
		}

		/// <summary>
		///A test for ReadAnyRomBytes
		///</summary>
		[TestMethod()]
		public void ReadAnyRomBytesTest()
		{
			lock (_target)
			{
				_target.Initialise(_port);
				const ushort addr_base = 0x8000;

				int len = 80;

				if (len > 0x3fff)
					Assert.Fail("Len не может быть больше " + 0x3fff);

				ushort[] addresses = new ushort[len];
				byte[] expected = new byte[len];

				for (int i = 0, c = 0; i < len; i++, c = (c == 7) ? 0 : c + 1)
				{
					addresses[i] = (ushort)(addr_base + i);
					expected[i] = (byte)c;
					//byte[] expected = new byte[] { 0x0f, 0xdd };
				}

				byte[] actual;

				actual = _target.ReadAnyRomBytes(addresses);

				_log.WriteEntry(this, "ACTUAL: " + BitConverter.ToString(actual));
				_log.WriteEntry(this, "EXPECT: " + BitConverter.ToString(expected));

				for (int i = 0; i < actual.Length; i++)
					Assert.AreEqual(expected[i], actual[i]);
			}
		}

		/// <summary>
		///A test for AddSensor
		///</summary>
		[TestMethod()]
		public void AddSensorTest()
		{			
			lock (_target)
			{
				try
				{
					_target.Initialise(_port);
					ConsultSensor sens = new ConsultData(new DataEngine()).GetSensor("Injection Time (LH)");
					sens.NewDataByte += new ConsultSensor.SensorNewDataByteEvent(PrintByte);
					_target.SensorAdd(sens);
					_target.SensorStartLive();
					Thread.Sleep(10000);
					_target.SensorStopLive();
				}
				catch (Exception ex)
				{
					Assert.Fail(ex.Message + ex.StackTrace);
				}
			}
		}
		private void PrintByte(byte[] b)
		{
			_log.WriteEntry(this, BitConverter.ToString(b));
		}
	}
}
