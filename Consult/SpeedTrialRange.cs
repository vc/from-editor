using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.ConsultNS
{
	public class SpeedTrialRange : IComparer<SpeedTrialRange>
	{
		int _beginInterval;
		int _endInterval;
		string _name;
		DateTime _timeBeginInterval;
		DateTime _timeEndInterval;
		bool _started;

		public SpeedTrialRange(int BeginInterval, int EndInterval, string name = null)
		{
			_name = name;
			_beginInterval = BeginInterval;
			_endInterval = EndInterval;
		}

		public void Start(DateTime? dtStart = null)
		{
			if (!_started)
			{
				_timeBeginInterval = dtStart == null
					? DateTime.Now
					: (DateTime)dtStart;
				_started = true;
			}
		}

		public void Stop(DateTime? dtStop)
		{
			_timeEndInterval = dtStop == null
				? DateTime.Now
				: (DateTime)dtStop;
		}

		public bool IsStarted
		{
			get { return _started; }
		}

		public int IntervalBegin
		{
			get { return _beginInterval; }
		}

		public int IntervalEnd
		{
			get { return _endInterval; }
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

		/// <summary>
		/// Строка с информацией об интервале и промежутке времени
		/// </summary>
		/// <returns>Строка с информацией об интервале и промежутке времени</returns>
		public string GetDescriptionTimeAccounted()
		{
			TimeSpan time = GetTime();

			return String.Format("{0} = {1}sec.",
				this.ToString(),
				Math.Round(time.TotalSeconds, 3)
			);
		}

		public override string ToString()
		{
			return
			String.Format("{2} [{0} - {1}]",
				this._beginInterval,
				this._endInterval,
				_name == null ? "" : _name
				);
		}

		#region IComparer<SpeedTrialRange> Members

		public int Compare(SpeedTrialRange x, SpeedTrialRange y)
		{
			return (x.IntervalBegin - y.IntervalBegin) + (x.IntervalEnd - y.IntervalEnd);
		}

		#endregion
	}
}
