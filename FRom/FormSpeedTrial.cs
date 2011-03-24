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
using FRom.Properties;

namespace FRom.ConsultNS
{
	public partial class FormSpeedTrial : Form, IDisposable
	{
		Consult _consult;
		bool _started = false;
		ConsultSensor _sensSpeed;
		Settings cfg;

		static List<SpeedTrialRange> _speedIntervals = new List<SpeedTrialRange> {
				new SpeedTrialRange(0,60),
				new SpeedTrialRange(0,100),
				new SpeedTrialRange(50,100),
				new SpeedTrialRange(100,200),
			};

		List<SpeedTrialRange> _currentSpeedIntervals;

		//Dictionary<KeyValuePair<int, int>, SpeedTrial> _st;

		public FormSpeedTrial(Consult consult)
		{
			_consult = consult;

			cfg = new Settings();

			InitializeConsult();
			InitializeComponent();
			InitializeMenu();
		}


		private void InitializeMenu()
		{
			base.Menu = new MainMenu(new MenuItem[]{
				new MenuItem("Tyre Calc", MenuTyreCalculatorEventHandler),
				new MenuItem("Add range", MenuAddRangeEventHandler)
			});

		}

		private void MenuAddRangeEventHandler(object sender, EventArgs e)
		{
			string val = "0";
			int begin, end;
			while (true)
			{
				if (HelperClass.InputBox("Add speed range", "Enter _BEGIN_ speed interval", ref val)
				== DialogResult.OK)
				{
					if (!Int32.TryParse(val, out begin))
						continue;

					if (HelperClass.InputBox("Add speed range", "Enter _END_ speed interval", ref val)
				== DialogResult.OK)
					{
						if (!Int32.TryParse(val, out end))
							continue;
						if (end <= begin)
						{
							HelperClass.Message(this, "'end' must be > 'begin'", null, MessageBoxIcon.Error);
							continue;
						}
						SpeedTrialRange tmp = new SpeedTrialRange(begin, end, "(Custom)");
						if (_speedIntervals.BinarySearch(tmp, tmp) > 0)
						{
							HelperClass.Message(this, "This range is alrady exist!");
							continue;
						}
						else
						{
							_speedIntervals.Add(tmp);
							HelperClass.Message(this, "Range added: " + tmp.ToString());
						}
					}
					else
						break;
				}
				else
					break;
			}
		}

		private void MenuTyreCalculatorEventHandler(object sender, EventArgs e)
		{
			FormTyreCalc frmTyreCalc = new FormTyreCalc(cfg.cfgTyreOrigin, cfg.cfgTyreCurrent);
			if (frmTyreCalc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				cfg.cfgTyreOrigin = frmTyreCalc._tOrigin;
				cfg.cfgTyreCurrent = frmTyreCalc._tNew;

				cfg.Save();

				ConsultSensor._speedCorrect = TyreParams.CalcK(frmTyreCalc._tOrigin, frmTyreCalc._tNew);
			}
		}

		private void InitializeConsult()
		{
			ConsultData data = new ConsultData(new DataEngine());
			if (_consult.DataSource.ToString() != data.ToString())
			{
				_consult.DataSource = data;
				_sensSpeed = data.ValidSensors["Vehicle speed"];
			}
			else
			{
				try
				{
					_sensSpeed = _consult.DataSource.ValidSensors["Vehicle speed"];
				}
				catch (KeyNotFoundException)
				{
					throw new ConsultException("No information about sensor 'Vehicle speed'");
				}
			}
			_consult.MonitoringSensors.Add(_sensSpeed);
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
			//_st = new Dictionary<KeyValuePair<int, int>, SpeedTrialRange>();
			_currentSpeedIntervals = new List<SpeedTrialRange>(_speedIntervals);

			//foreach (KeyValuePair<int, int> i in _speedIntervals)
			//	_st.Add(i, new SpeedTrialRange(i.Key, i.Value));

			_sensSpeed.NewDataFloat += new ConsultSensor.SensorNewDataFloatEven(sens_NewDataFloat);

			_consult.MonitoringSensors.SensorStartLive();
		}

		/// <summary>
		/// New Speed data 
		/// </summary>
		/// <param name="data"></param>
		void sens_NewDataFloat(float data)
		{
			DateTime now = DateTime.Now;
			List<SpeedTrialRange> tmpRemoveList = null;

			//correct speed koefficient
			data = ConsultSensor.CorrectSpeed(data);

			foreach (SpeedTrialRange i in _currentSpeedIntervals)
			{
				//SpeedTrialRange stTmp = _st[i];
				if (data > i.IntervalBegin && data <= i.IntervalEnd && !i.IsStarted)
					i.Start(now);
				else if (data >= i.IntervalEnd && i.IsStarted)
				{
					i.Stop(now);
					txtResult.Text = i.GetDescriptionTimeAccounted() + Environment.NewLine + txtResult.Text;
					if (tmpRemoveList == null)
						tmpRemoveList = new List<SpeedTrialRange>();
					tmpRemoveList.Add(i);
				}
			}
			if (tmpRemoveList != null)
				foreach (SpeedTrialRange i in tmpRemoveList)
					_currentSpeedIntervals.Remove(i);

			//update Speed label
			HelperClass.Invoke(this, delegate
			{
				lblCurrentSpeed.Text = Math.Round(data).ToString();
			});
		}

		/// <summary>
		/// Stop Button
		/// </summary>
		private void Stop()
		{
			_sensSpeed.NewDataFloat -= new ConsultSensor.SensorNewDataFloatEven(sens_NewDataFloat);
			if (_consult.MonitoringSensors.IsScanning)
				_consult.MonitoringSensors.SensorStopLive();
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
