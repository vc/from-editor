using System;

namespace FRom.Consult.ActiveTests
{
	public class OnOffEventArgs : EventArgs
	{
		public bool Value { get; set; }
		public OnOffEventArgs() { }
		public OnOffEventArgs(bool val)
		{
			Value = val;
		}
	}
}
