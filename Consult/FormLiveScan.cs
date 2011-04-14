using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using FRom.ConsultNS.Data;
using FRom.ConsultNS.Gauges;
using Helper;
using Helper.Properties;

namespace FRom.ConsultNS
{
	public partial class FormLiveScan : Form, ILiveScanForm
	{
		internal Consult _consult;
		internal List<IConsultGauge> _gauges;

		List<ConsultSensor> _lstSensors;

		public FormLiveScan(Consult cnslt)
		{
			InitializeComponent();

			_consult = cnslt;

			Tag = Text;

			if (_lstSensors == null)
				_lstSensors = FormSensors.GetListSensors(this);

			InitializeGauges();
			_consult.ClassStateChanged +=
				new Consult.HandleConsultClassStateChange(_consult_ClassStateChanged);

			mnuStartStop_CheckedChanged(btnStartStop, new EventArgs());

			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.DoubleBuffer, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}

		delegate void TextUpdate(string t);
		void _consult_ClassStateChanged(ConsultClassState state)
		{
			string text = String.Format("{0} [{1}]",
				this.Tag,
				state.ToString()
				);

			HelperClass.BeginInvoke(this, delegate { Text = text; });
		}

		void InitializeGauges()
		{
			if (_lstSensors == null)
				throw new ApplicationException("Нет доступных сенсоров для сканирования");

			//Если идет сканирование - останавливаем.
			if (_consult.MonitoringSensors.IsScanning)
				StartLiveScanSwitch(false);

			//Первая инициализация
			if (_gauges == null)
				_gauges = new List<IConsultGauge>();
			//повторная инициализация
			else
			{
				foreach (IConsultGauge i in _gauges)
				{
					Components.Remove(i);
					_consult.MonitoringSensors.SensorRemove(i.Sensor);
					i.RemoveFromControl(this);
					i.Close();
				}
				_gauges.Clear();
			}

			// заполнение списка
			foreach (ConsultSensor i in _lstSensors)
			{
				IConsultGauge tmp = new ConsultAquaGauge(i);
				_gauges.Add(tmp);
				Components.Add(tmp);
				tmp.AddToControl(panel);
			}

			_consult.MonitoringSensors.SensorAdd(_lstSensors);

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
		/// Начало приема данных в реальном времени
		/// </summary>
		private void mnuStartStop_Click(object sender, EventArgs e)
		{
			StartLiveScanSwitch();
		}

		/// <summary>
		/// Старт или стоп сканирования
		/// </summary>
		/// <param name="stop">true=START, flase=START. По умолчанию - переключить</param>
		private void StartLiveScanSwitch(bool? start = null)
		{
			lock (_consult)
			{
				try
				{
					switch (start)
					{
						// Начало приема данных в реальном времени
						case true:
							if (!_consult.MonitoringSensors.IsScanning)
							{
								_consult.MonitoringSensors.SensorStartLive();
								btnStartStop.Checked = true;
							}
							break;

						// Конец приема данных вреальном времени
						case false:
							if (_consult.MonitoringSensors.IsScanning)
							{
								_consult.MonitoringSensors.SensorStopLive();
								btnStartStop.Checked = false;
							}
							break;
						case null:
							if (_consult.MonitoringSensors.IsScanning)
							{
								_consult.MonitoringSensors.SensorStopLive();
								btnStartStop.Checked = false;
							}
							else
							{
								_consult.MonitoringSensors.SensorStartLive();
								btnStartStop.Checked = true;
							}
							break;
					}
				}
				catch (Exception ex)
				{
					throw new ConsultException("Ошибка при попытке запуска/останова SensorStartLive", ex);
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
				StartLiveScanSwitch(false);

			base.OnClosing(e);
		}

		/// <summary>
		/// Изменение размеров формы
		/// </summary>
		private void ConsultForm_SizeChanged(object sender, EventArgs e)
		{
			if (_gauges.Count < 1)
				return;
			_gauges[0].GaugePlacer.PlaceGauges(this, _gauges);
		}

		/// <summary>
		/// Функция обновления интерфейса кнопки Старт/Стоп
		/// </summary>
		private void mnuStartStop_CheckedChanged(object sender, EventArgs e)
		{
			ToolStripButton btn = sender as ToolStripButton;
			if (btn == null)
				return;

			if (btn.Checked)
			{
				btn.Image = Resources.pngAccept;
				btn.CheckState = CheckState.Checked;
				string sw = btn.Tag as string;
				btn.Text = sw.Substring(sw.IndexOf('|') + 1);
			}
			else
			{
				btn.Image = Resources.pngStop;
				btn.CheckState = CheckState.Unchecked;
				string sw = btn.Tag as string;
				btn.Text = sw.Substring(0, sw.IndexOf('|'));
			}
		}

		private void btnSensors_Click(object sender, EventArgs e)
		{
			bool flag = _consult.MonitoringSensors.IsScanning;

			//Если запущено сканирование - остановим
			if (flag)
				StartLiveScanSwitch(false);

			List<ConsultSensor> lst = FormSensors.GetListSensors(this);
			if (lst != null)
			{
				_lstSensors = lst;
				InitializeGauges();
				if (flag)
					StartLiveScanSwitch(true);
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

		#region ILiveScanForm Members

		/// <summary>
		/// Установка размера формы по размеру панели
		/// </summary>
		public System.Drawing.Size PanelSize
		{
			get
			{
				//Разница размеров формы и клиентской области
				System.Drawing.Size diffFormClient = this.Size - this.ClientSize;
				//Разница размеров клиентской области и панели
				System.Drawing.Size diffClientPanel = this.ClientSize - panel.Size;

				return Size - diffClientPanel - diffFormClient;
			}
			set
			{
				//Разница размеров формы и клиентской области
				System.Drawing.Size diffFormClient = this.Size - this.ClientSize;
				//Разница размеров клиентской области и панели
				System.Drawing.Size diffClientPanel = this.ClientSize - panel.Size;

				this.Size = value + diffClientPanel + diffFormClient;
			}
		}

		/// <summary>
		/// Минимальный размер панели
		/// </summary>
		public System.Drawing.Size PanelMinimumSize
		{
			get
			{
				//Разница размеров формы и клиентской области
				System.Drawing.Size diffFormClient = this.Size - this.ClientSize;
				//Разница размеров клиентской области и панели
				System.Drawing.Size diffClientPanel = this.ClientSize - panel.Size;

				return MinimumSize - diffClientPanel - diffFormClient;
			}
			set
			{
				//Разница размеров формы и клиентской области
				System.Drawing.Size diffFormClient = this.Size - this.ClientSize;
				//Разница размеров клиентской области и панели
				System.Drawing.Size diffClientPanel = this.ClientSize - panel.Size;

				MinimumSize = value + diffClientPanel + diffFormClient;
			}
		}

		#endregion
	}
}
