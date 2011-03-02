using System;
using System.Collections.Generic;
using System.Text;
using FRom.ConsultNS.Data;
using System.Threading;
using System.Drawing;
using System.ComponentModel;

namespace FRom.ConsultNS
{
	class ConsultColorBar : ColorBarNS.ColorTextBar, IComponent
	{
		ConsultSensor _sens;

		Queue<float> _queue;
		private AutoResetEvent _flag;
		private Thread _workThread;

		/// <summary>
		/// Конструктор для дизайнера
		/// </summary>
		public ConsultColorBar()
			: base()
		{
			base.Maximum = 100;
			base.Minimum = 0;
			base.SetColorListGradient(Color.YellowGreen);
		}

		public ConsultColorBar(ConsultSensor sens)
			: base()
		{

			Init(sens);
		}

		private void Dispose_Resources(bool disposing)
		{
			if (disposing)
			{
				//Отписываемся от событий обновления датчиков
				_sens.MaxScaleValueChanged -= delegate(float val) { MaximumF = val; };
				_sens.MinScaleValueChanged -= delegate(float val) { MinimumF = val; };
				_sens.NewDataFloat -= NewDataUpdate;

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
		}

		~ConsultColorBar()
		{
			Dispose_Resources(false);
		}

		void IDisposable.Dispose()
		{
			Dispose_Resources(true);
			GC.SuppressFinalize(this);
		}

		protected override void Dispose(bool disposing)
		{
			Dispose_Resources(true);
			base.Dispose(disposing);
		}

		public void Init(ConsultSensor sens)
		{
			_queue = new Queue<float>(50);
			_flag = new AutoResetEvent(false);
			_sens = sens;

			InitDefaults();

			MaximumF = _sens.MaximumScallable;
			MinimumF = _sens.MinimumScallable;

			this.Text = _sens._name;
			this.CustomUnitMeasure = _sens._scaling;

			_sens.MaxScaleValueChanged += delegate(float t) { MaximumF = t; };
			_sens.MinScaleValueChanged += delegate(float t) { MinimumF = t; };

			_sens.NewDataFloat += new ConsultSensor.SensorNewDataFloatEven(NewDataUpdate);

			_workThread = new Thread(new ThreadStart(this.WorkFunction));
			_workThread.Name = "ColorBarThreadSensor:'" + sens._name + "'";
			_workThread.Priority = ThreadPriority.BelowNormal;
			_workThread.Start();
		}

		private void InitDefaults()
		{
			//base.Size = new System.Drawing.Size(306, 20);
			//base.Value = 100;
			//base.Name = "pgbConsult1";
			//base.Location = new System.Drawing.Point(78, 121);
			//base.SetColorList(new List<Color> {
			//    Color.Red,
			//    Color.Green,
			//    Color.Yellow,});
			SetColorListGradient(Color.Red);

			base.BackColor = System.Drawing.SystemColors.Control;
			base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			base.Orientation = ColorBarNS.ColorBar.enumOrientation.Horizontal;
			base.Reversed = false;
			base.Smoothness = 4;
			base.Style = ColorBarNS.ColorBar.BarStyle.Flow;
			base.HeightThickness = 0.1F;
			base.WidthThickness = 0.2F;
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
					if (span.TotalMilliseconds > 100.0)
					{
						this.ValueF = (float)val / counter;
						//this.DigitalValue = (float)val / counter;
						lastUpdate = DateTime.Now;
						val = 0;
						counter = 0;

					}
				}
			}
		}

		/// <summary>
		/// Выборка очередного щначения из очереди
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
		/// Добавление зчередного значения в очередь
		/// </summary>
		/// <param name="data">данные</param>
		void NewDataUpdate(float data)
		{
			_queue.Enqueue(data);
			_flag.Set();
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// ConsultColorBar
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.CustomUnitMeasure = "Unit";
			this.Name = "ConsultColorBar";
			this.Size = new System.Drawing.Size(366, 15);
			this.ResumeLayout(false);

		}


	}
}
