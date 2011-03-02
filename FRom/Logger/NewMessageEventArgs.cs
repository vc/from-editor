using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.Logger
{
	public class NewMessageEventArgs : EventArgs
	{
		// Fields
		public readonly string Message;
		public readonly EventEntryType MessageType;
		public readonly DateTime Time;

		// Methods
		internal NewMessageEventArgs(Event ev)
		{
			this.Message = ev.Message;
			this.MessageType = ev.Type;
			this.Time = ev.Time;
		}
	}
}
