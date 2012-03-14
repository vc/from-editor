using System;

namespace FRom.Consult.ActiveTests
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
