using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FRom.ConsultNS;
using FRom.ConsultNS.Data;
using Helper;

namespace FRom.ConsultNS
{
	public partial class FormSpeedTrial : Form, IDisposable
	{
		FormMain _parrent;
		bool _started = false;
		ConsultSensor _sensSpeed;

		static KeyValuePair<int, int>[] _speedIntervals = new KeyValuePair<int, int>[] {			
				new KeyValuePair<int,int>(0,60),
				new KeyValuePair<int,int>(0,100),
				new KeyValuePair<int,int>(50,100),
				new KeyValuePair<int,int>(100,200),
			};

		List<KeyValuePair<int, int>> _currentSpeedIntervals;

		Dictionary<KeyValuePair<int, int>, SpeedTrial> _st;

		public FormSpeedTrial(FormMain frm)
		{
			_parrent = frm;

			InitializeConsult();
			InitializeComponent();
		}

		private void InitializeConsult()
		{
			ConsultData data = new ConsultData(new DataEngine());
			if (Consult.DataSource.ToString() != data.ToString())
			{
				Consult.DataSource = data;
				_sensSpeed = data.ValidSensors["Vehicle speed"];
			}
			else
			{
				_sensSpeed = Consult.DataSource.ValidSensors["Vehicle speed"];
			}
			Consult.MonitoringSensors.Add(_sensSpeed);
		}

		Consult Consult
		{
			get { return _parrent._consult; }
		}

		private void btnStartStop_Click(object sender, EventArgs e)
		{
			string startstop = btnStartStop.Tag.ToString();

			//Stopping
			if (_started)
			{
				Stop();
				btnStartStop.Text = startstop.Substring(0, startstop.IndexOf('|'));
			}
			//Starting
			else
			{
				Start();
				btnStartStop.Text = startstop.Substring(startstop.IndexOf('|') + 1);
			}

			_started = !_started;
		}

		/// <summary>
		/// Start Button
		/// </summary>
		private void Start()
		{
			_st = new Dictionary<KeyValuePair<int, int>, SpeedTrial>();
			_currentSpeedIntervals = new List<KeyValuePair<int, int>>(_speedIntervals);

			foreach (KeyValuePair<int, int> i in _speedIntervals)
				_st.Add(i, new SpeedTrial(i.Key, i.Value));

			_sensSpeed.NewDataFloat += new ConsultSensor.SensorNewDataFloatEven(sens_NewDataFloat);

			Consult.MonitoringSensors.SensorStartLive();
		}

		/// <summary>
		/// New Speed data 
		/// </summary>
		/// <param name="data"></param>
		void sens_NewDataFloat(float data)
		{
			DateTime now = DateTime.Now;
			List<KeyValuePair<int, int>> tmpRemoveList = new List<KeyValuePair<int, int>>();

			foreach (KeyValuePair<int, int> i in _currentSpeedIntervals)
			{
				SpeedTrial stTmp = _st[i];
				if (data > i.Key && data <= i.Value && !stTmp.IsStarted)
					stTmp.Start();
				else if (data >= i.Value && stTmp.IsStarted)
				{
					//TODO: Show result
					stTmp.Stop();
					textBox1.Text += stTmp.GetDescription() + Environment.NewLine;
					tmpRemoveList.Add(i);
				}
			}
			foreach (KeyValuePair<int, int> i in tmpRemoveList)
			{
				_st.Remove(i);
				_currentSpeedIntervals.Remove(i);
			}

			//update Speed label
			HelperClass.Invoke(this, delegate
			{
				lblCurrentSpeed.Text = data.ToString();
			});
		}

		/// <summary>
		/// Stop Button
		/// </summary>
		private void Stop()
		{
			_sensSpeed.NewDataFloat -= new ConsultSensor.SensorNewDataFloatEven(sens_NewDataFloat);
			if (Consult.MonitoringSensors.IsScanning)
				Consult.MonitoringSensors.SensorStopLive();
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		protected override void OnClosed(EventArgs e)
		{
			((IDisposable)this).Dispose();
			base.OnClosed(e);
		}

		void IDisposable.Dispose()
		{
			Stop();
			Dispose(true);

		}
	}
}
