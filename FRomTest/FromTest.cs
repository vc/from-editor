using System;
using System.IO;
using FRom.Logger;
using FRom.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FRomTest
{


	/// <summary>
	///This is a test class for FromTest and is intended
	///to contain all FromTest Unit Tests
	///</summary>
	[TestClass()]
	public class FromTest
	{
		public FromTest()
		{

			Log.Instance.NewMessage += new EventHandler<NewMessageEventArgs>(Instance_NewMessage);
		}

		void Instance_NewMessage(object sender, NewMessageEventArgs e)
		{
			Console.WriteLine(e.Message);
		}

		/// <summary>
		///A test for From Constructor
		///</summary>
		[TestMethod()]
		public void FromConstructorTest()
		{
			string addressFolder = @"d:\MyDocs\200SX\_ChipTuning\NISTune.bins\Address\";
			Log _log = Log.Instance;
			foreach (string file in Directory.EnumerateFiles(addressFolder, "*.adr"))
			{
				//_log.WriteEntry(EventEntryType.Event, " *** Testing file: '{0}'", new string[] { file });
				From target = new From();
				target.OpenAddressFile(file);

			}

			//Assert.Inconclusive("TODO: Implement code to verify target");
		}
	}
}
