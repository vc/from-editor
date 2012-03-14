using System;
using System.IO;
using System.Text;

namespace FRom.Consult.Helper.Logger
{
	class TimeStreamWriter : IDisposable
	{
		// Fields
		private Encoding _enc;
		private string _filename;
		private DateTime _lastAccsesTime;
		private StreamWriter _writer;

		// Methods
		public TimeStreamWriter(string filename, Encoding enc)
		{
			this._filename = filename;
			this._enc = enc;
			this._lastAccsesTime = DateTime.Now;
			this._writer = this.GetWriter();
		}

		public void Close()
		{
			if (this._writer != null)
			{
				this._writer.Flush();
				this._writer.Close();
				this._writer.Dispose();
				this._writer = null;
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Close();
			}
		}

		public void Flush()
		{
			if (this._writer != null)
			{
				try
				{
					this._writer.Flush();
				}
				catch
				{
				}
			}
		}

		private StreamWriter GetWriter()
		{
			return new StreamWriter(this._filename, true, this._enc);
		}

		// Properties
		public string FileName
		{
			get
			{
				return this._filename;
			}
		}

		public DateTime LastAccsessTime
		{
			get
			{
				return this._lastAccsesTime;
			}
		}

		public StreamWriter StreamWriter
		{
			get
			{
				if (this._writer == null)
				{
					this._writer = this.GetWriter();
				}
				this._lastAccsesTime = DateTime.Now;
				return this._writer;
			}
		}
	}

}