using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace FRom.Logger
{
	internal class FileLogger : IDisposable
	{
		// Fields
		private string _currentFileName;
		private Encoding _encoding;
		private string _logDirectory;
		private string _postfix;
		private bool _disposed = false;
		private DateTime _lastClearTime;
		private Dictionary<string, TimeStreamWriter> _openFiles;
#if DEBUG
		internal string _dateTimeFormat = "yyyy.MM.dd HH:mm:ss.fff";
#else
		internal string _dateTimeFormat = "yyyy.MM.dd HH:mm:ss";
#endif

		// Methods
		public FileLogger()
		{
			_encoding = Encoding.UTF8;
			_logDirectory = string.Empty;
			_postfix = string.Empty;
			_currentFileName = string.Empty;
			_openFiles = new Dictionary<string, TimeStreamWriter>(1);
			_lastClearTime = DateTime.Now;
			Init();
		}

		public FileLogger(Encoding enc)
		{
			_encoding = Encoding.UTF8;
			_logDirectory = string.Empty;
			_postfix = string.Empty;
			_currentFileName = string.Empty;
			_openFiles = new Dictionary<string, TimeStreamWriter>(1);
			_lastClearTime = DateTime.Now;
			_encoding = enc;
			Init();
		}

		public void Append(Event message)
		{
			DateTime now = DateTime.Now;
			TimeSpan span = (TimeSpan)(now - _lastClearTime);
			if (span.TotalSeconds > 60.0)
			{
				this._lastClearTime = now;
				this.CloseUnusedFiles(this._lastClearTime);
			}
			string key = this.FileName();
			try
			{
				if (!this._openFiles.ContainsKey(key))
				{
					this._openFiles[key] = new TimeStreamWriter(key, this._encoding);
				}
				this._openFiles[key].StreamWriter.WriteLine(this.LogNote(message));
			}
			catch (NotImplementedException)
			{
				throw;
			}
			catch
			{
			}

		}

		/// <summary>
		/// Закрыть файлы которые не используется больше заданного времени 
		/// </summary>
		/// <param name="maxActiveTime">maxActiveTime</param>
		private void CloseUnusedFiles(DateTime maxActiveTime)
		{
			DateTime now = DateTime.Now;
			List<TimeStreamWriter> list = new List<TimeStreamWriter>(1);
			foreach (KeyValuePair<string, TimeStreamWriter> pair in this._openFiles)
			{
				if (pair.Value.LastAccsessTime < maxActiveTime)
				{
					list.Add(pair.Value);
				}
			}
			foreach (TimeStreamWriter writer in list)
			{
				writer.Close();
				this._openFiles.Remove(writer.FileName);
			}
		}


		~FileLogger()
		{
			Dispose(false);
		}

		void IDisposable.Dispose()
		{
			Dispose(true);
		}

		internal void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (TimeStreamWriter writer in this._openFiles.Values)
				{
					writer.Dispose();
				}
				this._openFiles = null;
			}
		}

		private string FileName()
		{
			return Path.Combine(LogDirectory, "FromEditor.log");
			//return Path.Combine(LogDirectory, string.Format("{0}.log", time.ToString(DateTimeFormat)));

		}

		public void Flush()
		{
			try
			{
				foreach (KeyValuePair<string, TimeStreamWriter> pair in this._openFiles)
				{
					pair.Value.Flush();
				}
			}
			catch
			{
			}
		}

		private void Init()
		{
			//string str = Path.Combine(Path.GetDirectoryName(base.GetType().Assembly.Location), "logs");
			//LogDirectory = str;
			LogDirectory = Path.GetDirectoryName(base.GetType().Assembly.Location);

		}

		private string LogNote(Event message)
		{
			Object o = message.Sender;
			return string.Format("[{0} : {1} : {2}] {3}",
				message.Time.ToString(_dateTimeFormat),
				message.Type.ToString(),
				o.GetType().ToString(),
				message.Message);
		}

		public string LogDirectory
		{
			get
			{
				return _logDirectory;
			}
			set
			{
				if (!Directory.Exists(value))
				{
					Directory.CreateDirectory(value);
				}
				_logDirectory = value;
			}
		}

		public string LogFilePath
		{
			get
			{
				return _currentFileName;
			}
		}

		public string Postfix
		{
			get
			{
				return _postfix;
			}
			set
			{
				_postfix = (value == null) ? string.Empty : value;
			}
		}
	}
}

