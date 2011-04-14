using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.ConsultNS.ActiveTests
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
