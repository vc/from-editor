using System;
using System.IO.Ports;
using FRom.Emulator;
using FRom.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace FRomTest
{


	/// <summary>
	///This is a test class for RomulatorTest and is intended
	///to contain all RomulatorTest Unit Tests
	///</summary>
	[TestClass()]
	public class RomulatorTest
	{
		string _binFile = @"d:\MyDocs\_Source\_FRom\from.svn\FRomTest\Test Requirements\R32_rb26det.bin";

		private Log _log = Log.Instance;

		Romulator _target;

		public RomulatorTest()
		{
			_target = new Romulator("COM14");
			Log.Instance.NewMessage += new EventHandler<NewMessageEventArgs>(Instance_NewMessage);
		}

		void Instance_NewMessage(object sender, NewMessageEventArgs e)
		{
			Console.WriteLine(e.Message);
		}

		/// <summary>
		///A test for HiddenWrite
		///</summary>
		[TestMethod()]
		public void HiddenWriteTest()
		{
			Assert.IsTrue(File.Exists(_binFile));
			byte[] bin = File.ReadAllBytes(_binFile);
			byte[] arr = new byte[4];
			Buffer.BlockCopy(bin, 0, arr, 0, 4);
			ushort address = 0;
			try
			{
				_target.HiddenWrite(address, bin);
			}
			catch (RomulatorException ex)
			{
				_log.WriteEntry(this, ex);
			}
			_log.WriteEntry(this, "Количество сбоев работы с устройством: " + _target.ErorsCount);
		}

		/// <summary>
		///A test for ReadBlock
		///</summary>
		[TestMethod()]
		public void ReadBlockTest()
		{
			byte size = 0x00;
			ushort address = 0x0;
			byte[] actual = null;
			try
			{
				actual = _target.ReadBlock(size, address);
			}
			catch (RomulatorException ex)
			{
				_log.WriteEntry(this, ex);
				Assert.Fail(ex.Message);
			}
		}

		/// <summary>
		///A test for ReadBlock
		///</summary>
		[TestMethod()]
		public void ReadBlockTest1()
		{
			try
			{
				ushort step = 0xff;
				for (ushort i = 0; i <= 0x8000; i += step)
					_target.ReadBlockL(i, step);
			}
			catch (RomulatorException ex)
			{
				_log.WriteEntry(this, ex);
				Assert.Fail(ex.Message);
			}
		}

		/// <summary>
		///A test for ReadByte
		///</summary>
		[TestMethod()]
		public void ReadByteTest()
		{
			ushort address = 0x1;
			byte expected = 0;
			byte actual;
			actual = ReadByte(address);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for ReadByte
		///</summary>
		[TestMethod()]
		public void ReadAllByteTest()
		{
			int size = 0x8000;
			byte[] actual = new byte[size];
			try
			{
				for (ushort addr = 0x00; addr < size; addr++)
					actual[addr] = ReadByte(addr);
			}
			catch (RomulatorException ex)
			{
				_log.WriteEntry(this, ex);
				Assert.Fail(ex.Message);
			}
		}

		byte ReadByte(ushort addr)
		{
			return _target.ReadByte(addr);
		}

		/// <summary>
		///A test for ReadByte
		///</summary>
		[TestMethod()]
		public void ScanTest()
		{
			ushort stAddr = 0x0001;
			ushort endAddr = 0x3FFF;

			_target.Scan(stAddr, endAddr);
			//Assert.AreEqual(expected, actual);
		}

		/// <summary>
		///A test for WriteBlock
		///</summary>
		[TestMethod()]		
		public void WriteBlockTest()
		{
			Assert.IsTrue(File.Exists(_binFile));
			byte[] bin = File.ReadAllBytes(_binFile);
			byte[] arr = new byte[4];
			Buffer.BlockCopy(bin, 0, arr, 0, 4);
			ushort address = 0;
			try
			{
				_target.WriteBlock(address, bin);
			}
			catch (RomulatorException ex)
			{
				_log.WriteEntry(this, ex);
			}
			_log.WriteEntry(this, "Количество сбоев работы с устройством: " + _target.ErorsCount);
		}

		/// <summary>
		///A test for GetStatus
		///</summary>
		[TestMethod()]
		public void GetStatusTest()
		{
			bool actual = false;
			try
			{
				actual = _target.GetStatus(true);
			}
			catch (RomulatorException ex)
			{
				_log.WriteEntry(this, ex);
			}
			_log.WriteEntry(this, "Количество сбоев работы с устройством: " + _target.ErorsCount);
			Assert.IsTrue(actual);
		}

		/// <summary>
		///A test for GetVersion
		///</summary>
		[TestMethod()]
		public void GetVersionTest()
		{
			RomulatorVersion expected = new RomulatorVersion(new byte[] { 1, 0, 0 });
			RomulatorVersion actual = null;

			try
			{
				actual = _target.GetVersion();
			}
			catch (RomulatorException ex)
			{
				_log.WriteEntry(this, ex);
			}

			_log.WriteEntry(this, "Версия Romulator: " + actual.ToString());

			Assert.AreEqual(actual >= expected, true);
		}
	}
}
