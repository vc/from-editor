using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.Logger
{
	public class NewMessageEventArgs : EventArgs
	{
		LogEvent _evnt;

		// Methods
		internal NewMessageEventArgs(LogEvent ev)
		{
			_evnt = ev;
		}

		public LogEvent EventInstance
		{
			get { return _evnt; }
		}

		public string Message
		{
			get { return _evnt.Message; }
		}

		public EventEntryType MessageType
		{
			get { return _evnt.Type; }
		}

		public DateTime Time
		{
			get { return _evnt.Time; }
		}
	}
}
