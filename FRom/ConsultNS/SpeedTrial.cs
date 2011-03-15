using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.ConsultNS
{
	class SpeedTrial
	{
		int _beginInterval;
		int _endInterval;
		string _name;
		DateTime _timeBeginInterval;
		DateTime _timeEndInterval;
		bool _started;

		public SpeedTrial(int BeginInterval, int EndInterval, string name = null)
		{
			_name = String.Format("{2} [{0} - {1}]",
				BeginInterval,
				EndInterval,
				name == null ? "" : name
				);
			_beginInterval = BeginInterval;
			_endInterval = EndInterval;
		}

		public void Start()
		{
			if (!_started)
			{
				_timeBeginInterval = DateTime.Now;
				_started = true;
			}
		}

		public bool IsStarted
		{
			get { return _started; }
		}

		public void Stop()
		{
			_timeEndInterval = DateTime.Now;
		}

		/// <summary>
		/// Get time period from start() to stop()
		/// </summary>
		/// <returns></returns>
		public TimeSpan GetTime()
		{
			if (_timeBeginInterval == null || _timeEndInterval == null)
				return new TimeSpan(0);
			else
				return _timeEndInterval - _timeBeginInterval;
		}

		public string GetDescription()
		{
			TimeSpan time = GetTime();

			return String.Format("{0} = {1}seconds",
				_name,
				Math.Round(time.TotalSeconds, 3)
			);
		}

		public override string ToString()
		{
			return _name;
		}
	}
}
