using System;

namespace FRom.Consult.ActiveTests
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
