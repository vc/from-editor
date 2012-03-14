using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using AquaControls;
using FRom.Consult.Data;
using System.Windows.Forms;

namespace FRom.Consult.Gauges
{
	class ConsultAquaGauge : AquaGauge, IComponent, IConsultGauge, IConsultGaugePlacer
	{
		ConsultSensor _sens;

		Queue<float> _queue;
		private AutoResetEvent _flag;
		private Thread _workThread;
		int _roundTime = 100;

		/// <summary>
		/// Конструктор для дизайнера
		/// </summary>
		public ConsultAquaGauge()
			: base() { }

		public ConsultAquaGauge(ConsultSensor sens)
			: base()
		{
			Init(sens);
		}

		private ScaleDescription ScaleDescription;

		public void Init(ConsultSensor sens)
		{
			_queue = new Queue<float>(50);
			_flag = new AutoResetEvent(false);
			_sens = sens;

			InitDefaultAquaGaugeToConsult();

			this.ScaleDescription = sens.ConvFunc(1).Scale;

			MaxValue = ScaleDescription.Max;
			MinValue = ScaleDescription.Min;

			this.DialText = _sens._name + Environment.NewLine + ScaleDescription.Scale;

			_sens.NewDataFloat += NewDataUpdate;

			_workThread = new Thread(new ThreadStart(this.WorkFunction));
			_workThread.Name = "AquagaugeThreadSensor:[" + sens._name + "]";
			_workThread.Priority = ThreadPriority.BelowNormal;
			_workThread.Start();
		}

		~ConsultAquaGauge()
		{
			Dispose(true);
		}

		void IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Close()
		{
			Dispose(true);
		}

		public void RemoveFromControl(Control ctrl)
		{
			ctrl.Controls.Remove(this);
		}

		public void AddToControl(Control ctrl)
		{
			ctrl.Controls.Add(this);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				//Отписываемся от событий обновления датчиков				
				_sens.NewDataFloat -= NewDataUpdate;

				//останавливаем основной поток
				if (this._workThread != null)
				{
					this._workThread.Abort();
					this._workThread.Join();
					this._workThread = null;
				}
				if (this._flag != null)
				{
					this._flag.Close();
					this._flag = null;
				}
			}
			base.Dispose(disposing);
		}

		/// <summary>
		/// Дефолтовые настройки для consult
		/// </summary>
		void InitDefaultAquaGaugeToConsult()
		{
			//this.Location = new System.Drawing.Point(12, 76);
			//this.Size = new System.Drawing.Size(200, 200);
			this.BackColor = System.Drawing.Color.Transparent;
			this.DecimalPlaces = 0;
			this.DialAlpha = 255;
			this.DialBorderColor = System.Drawing.Color.SlateGray;
			this.DialColor = System.Drawing.Color.Lavender;
			this.DialTextColor = System.Drawing.Color.Black;
			this.DialTextVOffset = -10;
			this.DigitalValueBackAlpha = 10;
			this.DigitalValueBackColor = System.Drawing.Color.White;
			this.DigitalValueColor = System.Drawing.Color.Green;
			this.DigitalValueDecimalPlaces = 2;
			this.Glossiness = 50F;
			this.NoOfDivisions = 6;
			this.NoOfSubDivisions = 4;
			this.PointerColor = System.Drawing.Color.Black;
			this.RimAlpha = 150;
			this.RimColor = System.Drawing.SystemColors.ControlLightLight;
			this.ScaleColor = System.Drawing.Color.Orange;
			this.ScaleFontSizeDivider = 22;
			this.TabIndex = 6;
			this.Threshold1Color = System.Drawing.Color.LawnGreen;
			this.Threshold1Start = 0F;
			this.Threshold1Stop = 0F;
			this.Threshold2Color = System.Drawing.Color.Red;
			this.Threshold2Start = 0F;
			this.Threshold2Stop = 0F;
			this.ValueToDigital = false;
		}

		public ConsultSensor Sensor
		{
			get { return _sens; }
		}

		void NewDataUpdate(float data)
		{
			_queue.Enqueue(data);
			_flag.Set();
		}

		/// <summary>
		/// Рабочая потоковая функция
		/// </summary>
		private void WorkFunction()
		{
			DateTime lastUpdate = DateTime.Now;
			int counter = 0;
			double val = 0;
			while (true)
			{
				this._flag.WaitOne();
				float? message;
				while ((message = this.Dequeue()) != null)
				{
					DateTime now = DateTime.Now;
					TimeSpan span = (TimeSpan)(now - lastUpdate);
					val += (float)message;
					counter++;
					if (span.TotalMilliseconds > _roundTime)
					{
						this.Value = (float)val / counter;
						this.DigitalValue = (float)val / counter;
						lastUpdate = now;
						val = 0;
						counter = 0;
					}
				}
			}
		}

		/// <summary>
		/// Выборка очередного значения из очереди
		/// </summary>
		/// <returns></returns>
		private float? Dequeue()
		{
			lock (this._queue)
			{
				if (this._queue.Count > 0)
				{
					return this._queue.Dequeue();
				}
				return null;
			}
		}

		/// <summary>
		/// Высота и ширина датчиков
		/// </summary>
		internal static int _heightWidth = 200;

		static IConsultGaugePlacer _gaugePlacer;
		public IConsultGaugePlacer GaugePlacer
		{
			get
			{
				if (_gaugePlacer == null)
					_gaugePlacer = new ConsultAquaGauge();
				return this;
			}
		}

		public void PlaceGauges(Control ctrl, IList<IConsultGauge> gauges)
		{
			ILiveScanForm f = ctrl as ILiveScanForm;
			if (f == null)
				return;

			//ctrl.Controls[.cou
			//Минимальные размеры формы
			int count = gauges.Count;
			// Необходимое количество слотов по осям для размещения датчиков в текущих размерах
			int countNeedX = f.PanelSize.Width / _heightWidth;
			if (countNeedX == 0) countNeedX = 1;
			int countNeedY = (count % countNeedX) > 0
				? count / countNeedX + 1
				: count / countNeedX;
			//Минимальные размеры панели
			f.PanelMinimumSize = new System.Drawing.Size(
				_heightWidth,
				countNeedY * _heightWidth);

			f.PanelSize = new System.Drawing.Size(
				f.PanelSize.Width,
				f.PanelMinimumSize.Height);

			//расстановка датчиков по форме
			for (int i = 0, x = 0, y = 0; i < gauges.Count; i++)
			{
				//Если выходим за правые границамы, то строка шириной с датчик вниз.
				if (x + _heightWidth > f.PanelSize.Width)
				{
					y += _heightWidth;
					x = 0;
				}

				//Выставляем размеры датчиков
				gauges[i].Height = gauges[i].Width = _heightWidth;

				//Выставляем позицию датчиков				
				gauges[i].Left = x;
				gauges[i].Top = y;

				//приращаю по иксу
				x += _heightWidth;
			}
		}
	}
}
