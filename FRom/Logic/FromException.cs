using System;

namespace FRom.Logic
{
	public class FromException : Exception
	{
		FromExceptionErrorNumber _error;
		public FromException(string message, FromExceptionErrorNumber err)
			: base(message)
		{
			_error = err;
		}

		public FromException(string msg)
			: base(msg) { }

		public FromException(string message, Exception ex)
			: base(message, ex) { }

		public override string Message
		{
			get
			{
				return String.Format("{0} Тип ошибки: {1}", base.Message, _error.ToString());
			}
		}

		public FromExceptionErrorNumber GetErrorNumber()
		{
			return _error;
		}

	}
}
