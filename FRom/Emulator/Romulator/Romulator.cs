using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace FRom.Emulator
{
	public class Romulator : RomulatorBase
	{
		public Romulator(string port) : base(port) { }

		public Romulator() : base() { }
	}
}
