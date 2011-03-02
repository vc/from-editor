using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FRom.ConsultNS.Data;
using System.ComponentModel;
using FRom.Helper;

namespace FRom.ConsultNS
{
	public partial class FormLiveScan : Form
	{
		internal FormMain _frmParrent;
		internal List<ConsultAquaGauge> _gauges;
		/// <summary>
		/// Высота и ширина датчиков (д.б. квадрат)
		/// </summary>
		internal int _agHeightWidth = 200;

		List<ConsultSensor> _lstSensors;

		public FormLiveScan(FormMain parrent)
		{
			_frmParrent = parrent;

			InitializeComponent();
			if (_lstSensors == null)
				_lstSensors = FormSensors.GetListSensors(this);
			InitializeGauges();
			mnuStartStop_CheckedChanged(btnStartStop, new EventArgs());

			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}

		void InitializeGauges()
		{
			//Если идет сканирование - сотанавливаем.
			if (_consult.State == ConsultClassState.ECU_STREAMING_MONITORS)
				mnuStartStop_Click(btnStartStop, null);

			if (_gauges == null)
				_gauges = new List<ConsultAquaGauge>();
			else
			{
				foreach (ConsultAquaGauge i in _gauges)
				{
					Components.Remove(i);
					panel.Controls.Remove(i);
					i.Close();
				}
			}

			// заполнение списка
			foreach (ConsultSensor i in _lstSensors)
			{
				ConsultAquaGauge tmp = new ConsultAquaGauge(i);
				_gauges.Add(tmp);
				Components.Add(tmp);
			}

			_consult.SensorAddRange(_lstSensors);

			panel.Controls.AddRange(_gauges.ToArray());
			ConsultForm_SizeChanged(this, new EventArgs());
		}

		private IContainer Components
		{
			get
			{
				if (components == null)
					components = new ResourceDisposer();
				return components;
			}
		}

		/// <summary>
		/// Consult интерфейс
		/// </summary>
		Consult _consult
		{
			get { return _frmParrent._consult; }
		}

		/// <summary>
		/// Начало приема данных в реальном времени
		/// </summary>
		private void mnuStartStop_Click(object sender, EventArgs e)
		{
			ToolStripButton btn = sender as ToolStripButton;
			if (btn == null)
				return;

			// Начало приема данных в реальном времени
			if (btn.Checked)
			{
				try
				{
					_consult.SensorStartLive();
					btn.Checked = true;
				}
				catch (Exception ex)
				{
					_frmParrent.Error(ex,
						"Ошибка при попытке запуска SensorStartLive",
						"Sensor Live Capture ERROR");
				}
			}
			// Конец приема данных вреальном времени
			else
			{
				try
				{
					_consult.SensorStopLive();
					btn.Checked = false;
				}
				catch (Exception ex)
				{
					_frmParrent.Error(ex,
						"Ошибка при попытке остановки SensorStartLive",
						"Sensor Live Capture ERROR");
				}
			}
		}

		/// <summary>
		/// Кнопка выхода
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Останавливаем прием данных
		/// </summary>
		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			if (btnStartStop.Checked == true)
				mnuStartStop_CheckedChanged(btnStartStop, new EventArgs());
			base.OnClosing(e);
		}

		/// <summary>
		/// Изменение размеров формы
		/// </summary>
		private void ConsultForm_SizeChanged(object sender, EventArgs e)
		{
			ConsultAquaGauge.PlaceGauges(this, _gauges);
		}

		private void mnuStartStop_CheckedChanged(object sender, EventArgs e)
		{
			ToolStripButton btn = sender as ToolStripButton;
			if (btn == null)
				return;

			if (btn.Checked)
			{
				btn.Image = Properties.Resources.Button_Blank_Green_01;
				btn.CheckState = CheckState.Checked;
				string sw = btn.Tag as string;
				btn.Text = sw.Substring(sw.IndexOf('|') + 1);
			}
			else
			{
				btn.Image = Properties.Resources.Button_Blank_Red_01;
				btn.CheckState = CheckState.Unchecked;
				string sw = btn.Tag as string;
				btn.Text = sw.Substring(0, sw.IndexOf('|'));
			}
		}

		private void btnSensors_Click(object sender, EventArgs e)
		{
			List<ConsultSensor> lst = FormSensors.GetListSensors(this);
			if (lst != null)
			{
				_lstSensors = lst;
				InitializeGauges();
			}
		}

		FormSensorSelect _frmSensors;
		internal FormSensorSelect FormSensors
		{
			get
			{
				if (_frmSensors == null)
					_frmSensors = new FormSensorSelect(_consult.DataSource);
				return _frmSensors;
			}
		}

	}
}
