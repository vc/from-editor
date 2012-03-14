using System;
using System.Collections.Generic;
using System.Threading;

namespace FRom.Consult.Helper.Logger
{
	public class Log : IDisposable
	{
		// Fields
		private FileLogger _fileLogger;
		private static Log _instance;
		private bool catchEx;
		private bool enableLogFile;
		private static object lockObj;
		private Thread workThread;
		private AutoResetEvent flag;
		private Queue<LogEvent> eventQueue;
		private EventEntryType _logLevel;

		// Events
		public event EventHandler<NewMessageEventArgs> NewMessage;

		// Methods
		private Log ()
		{
			_fileLogger = new FileLogger ();
			enableLogFile = false;
			catchEx = true;


			workThread = new Thread (new ThreadStart (this.WorkFunction));
			workThread.Name = "Logger Thread";
			workThread.Priority = ThreadPriority.BelowNormal;
			workThread.Start ();

			flag = new AutoResetEvent (false);
			eventQueue = new Queue<LogEvent> ();


		}

		static Log ()
		{
			lockObj = new object ();
		}

		public void Close ()
		{
			((IDisposable)this).Dispose ();
		}

		private void Dispose (bool disposing)
		{
			if (disposing) {
				lock (eventQueue) {
					if (this.workThread != null) {
						this.workThread.Abort ();
						this.workThread.Join ();
						this.workThread = null;
					}
					if (this.flag != null) {
						this.flag.Close ();
						this.flag = null;
					}
					if (_fileLogger != null) {
						_fileLogger.Dispose (true);
						_fileLogger = null;
					}
				}
			}
		}

		~Log ()
		{
			Dispose (false);
		}

		void IDisposable.Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		
		public void WriteEntry (object sender, string message)
		{
			this.WriteEntry (sender, message, EventEntryType.Event);
		}
		
		public void WriteEntry (object sender, string message, EventEntryType type)
		{

			if (type < _logLevel)
				return;

			LogEvent item = new LogEvent (sender, message, type);

			lock (this.eventQueue) {
				this.eventQueue.Enqueue (item);
				this.flag.Set ();
			}
			if (this.NewMessage != null) {
				NewMessage.DynamicInvoke (new object[] { this, new NewMessageEventArgs (item) });
			}
		}

		public void WriteEntry (object sender, EventEntryType type, string format, params object[] args)
		{
			string message = string.Format (format, args);
			WriteEntry (sender, message, type);
		}

		public static string GetExceptionInfo(Exception ex)
		{
			string message = String.Format("{4}Exception: {1} ({2}){0}{3}{0}",
				Environment.NewLine,
				ex.GetType().ToString(),
				ex.Message,
				ex.StackTrace,
				ex.InnerException == null ? "" : GetExceptionInfo(ex.InnerException) + "  --\nInner"
				);

			return message;
		}

		public void WriteEntry (object sender, Exception ex)
		{
			string message = GetExceptionInfo (ex);
			WriteEntry (this, message.ToString (), EventEntryType.Error);
		}
		
		public void WriteEntry (object sender, byte[] arr, bool direction)
		{
			this.WriteEntry (sender, arr, direction);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="type"></param>
		/// <param name="arr"></param>
		/// <param name="direction">Направление передачи данных. True=Send, False=Receive</param>
		public void WriteEntry (object sender, byte[] arr, bool direction, EventEntryType type)
		{
			string msg = (direction ? "[Send -->] " : "[Recv <--] ") + "<" + BitConverter.ToString (arr) + ">";
			WriteEntry (sender, type, msg);
		}
		
		public void WriteEntry (object sender, byte[] arr)
		{
			this.WriteEntry (sender, arr, null);
		}
		
		public void WriteEntry (object sender, byte[] arr, string description)
		{
			this.WriteEntry (sender, arr, description, EventEntryType.Debug);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="arr"></param>
		/// <param name="description"></param>
		/// <param name="type"></param>
		public void WriteEntry (object sender, byte[] arr, string description, EventEntryType type)
		{

			string msg = description == null
				? "<" + BitConverter.ToString (arr) + ">"
				: "[" + description + "] " + "<" + BitConverter.ToString (arr) + ">";
			WriteEntry (sender, type, msg);
		}

		/// <summary>
		/// Рабочая потоковая функция
		/// </summary>
		private void WorkFunction ()
		{
			while (true) {
				this.flag.WaitOne ();
				LogEvent message = null;
				while ((message = this.Dequeue()) != null) {
					if (this.enableLogFile) {
						this._fileLogger.Append (message);
					}
				}
				this._fileLogger.Flush ();
			}
		}

		private LogEvent Dequeue ()
		{
			lock (this.eventQueue) {
				if (this.eventQueue.Count > 0) {
					return this.eventQueue.Dequeue ();
				}
				return null;
			}
		}

		// Properties
		public EventEntryType LogLevel {
			get { return _logLevel; }
			set {
				_logLevel = value;
				if (_logLevel == EventEntryType.Debug)
					_fileLogger.LogDateTimeRecordFormat = "yyyy.MM.dd HH:mm:ss.fff";
				else
					_fileLogger.LogDateTimeRecordFormat = "yyyy.MM.dd HH:mm:ss";
			}
		}

		public bool CatchExceptions {
			get
			{ return catchEx; }
			set
			{ catchEx = value; }
		}

		public string LogNote (LogEvent evnt)
		{
			return _fileLogger.LogNote (evnt);
		}

		public static Log Instance {
			get {
				if (_instance == null) {
					lock (lockObj) {
						if (_instance == null) {
							_instance = new Log ();
						}
					}
				}
				return _instance;
			}
		}

		public string LogDirectory {
			get
			{ return _fileLogger.LogDirectory; }
			set
			{ _fileLogger.LogDirectory = value; }
		}

		/// <summary>
		/// Полный путь к лог файлу
		/// </summary>
		public string LogFilePath {
			get
			{ return _fileLogger.LogFileNamePath; }
		}

		public bool LogFileEnabled {
			get
			{ return enableLogFile; }
			set
			{ enableLogFile = value; }
		}

		public string Postfix {
			get
			{ return _fileLogger.Postfix; }
			set
			{ _fileLogger.Postfix = value; }
		}
	}
}
