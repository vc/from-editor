using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.ConsultNS.ActiveTests
{
	public class ScallableEventArgs : EventArgs
	{
		public float Value { get; set; }
		public ScallableEventArgs() { }
		public ScallableEventArgs(float val)
		{
			Value = val;
		}
	}
}
