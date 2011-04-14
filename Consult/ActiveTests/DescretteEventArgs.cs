using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.ConsultNS.ActiveTests
{
	public class DescretteEventArgs : EventArgs
	{
		public byte Value { get; set; }

		public DescretteEventArgs() { }

		public DescretteEventArgs(byte selectedItems)
		{
			Value = selectedItems;
		}
	}
}
