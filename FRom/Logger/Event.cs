using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.Logger
{
	[Serializable]
	public enum EventEntryType
	{
		Debug = 0,
		Event = 1,
		Warning = 2,
		Error = 3,
		CriticalError = 4,
	}

	public class LogEvent
	{
		// Fields
		private string message;
		private DateTime time;
		private EventEntryType type;
		private object sender;

		// Methods
		public LogEvent(object sender, string message, EventEntryType type)
		{
			this.sender = sender;
			this.message = message;
			this.type = type;
			this.time = DateTime.Now;
		}

		// Properties
		public object Sender
		{
			get { return this.sender; }
		}

		public string Message
		{
			get
			{
				return this.message;
			}
		}

		public DateTime Time
		{
			get
			{
				return this.time;
			}
		}

		public EventEntryType Type
		{
			get
			{
				return this.type;
			}
		}
	}
}