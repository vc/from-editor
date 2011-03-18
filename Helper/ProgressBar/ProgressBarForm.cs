using System;
using System.Threading;
using System.Windows.Forms;

namespace Helper.ProgressBar
{
	public partial class FormProgressBar : FormProgressBar, IProgressBar
	{
		private int count;
		private Thread thread;
		private System.Windows.Forms.Timer timer;

		/// <summary>
		/// Конструкто
		/// </summary>
		/// <param name="formCaption">Заголовок формы</param>
		public FormProgressBar(string formCaption)
		{
			InitializeComponent();
			this.Tag = this.Text = formCaption;
		}

		delegate void helpDelegateState(string str);
		delegate void helpDelegateInc();

		/// <summary>
		/// Увеличить на один прогресс бар
		/// </summary>		
		private void IncProressBar()
		{
			if (this.progressBar.InvokeRequired)
				this.progressBar.Invoke(new helpDelegateInc(this.IncProressBar));
			else
			{
				this.progressBar.PerformStep();
				Application.DoEvents();
			}
		}

		void timer_Tick(object sender, EventArgs e)
		{
			this.progressBar.PerformStep();
			if (this.progressBar.Value == this.progressBar.Maximum)
				this.progressBar.Value = 0;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.CloseThread();
			this.DialogResult = DialogResult.Cancel;
		}

		/// <summary>
		/// Закрыть поток
		/// </summary>
		private void CloseThread()
		{
			if (this.thread != null)
			{
				this.thread.Abort();
				this.thread.Join();
				this.thread = null;
			}
			if (this.timer != null)
			{
				this.timer.Stop();
				this.timer.Dispose();
				this.timer = null;
			}
		}

		private void ProgressBarForm_Shown(object sender, EventArgs e)
		{
			if (this.timer != null)
				this.timer.Start();
			if (this.thread != null)
				this.thread.Start();
		}

		private IterationCallback iterationCallback;
		private ExecuteCallback executeCallback;

		#region IProgressBar Members

		public void SetCurrentState(string currentState)
		{
			if (this.progressBar.InvokeRequired)
				this.progressBar.Invoke(new helpDelegateState(this.SetCurrentState), currentState);
			else
			{
				this.Text = String.Format("{0} [{1}]",
					this.Tag,
					currentState);
				Application.DoEvents();
			}
		}

		public void ShowProgressBar(int count, IterationCallback callback)
		{
			this.iterationCallback = callback;
			this.count = count;
			this.progressBar.Maximum = this.count;
			this.thread = new Thread(Execute);
			this.thread.Name = "ProgressForm Thread";
			this.ShowDialog();
			this.CloseThread();
		}

		public void ShowProgressBar(ExecuteCallback callback)
		{
			this.executeCallback = callback;
			this.progressBar.Maximum = 50;
			this.btnCancel.Enabled = false;
			this.timer = new System.Windows.Forms.Timer();
			this.timer.Interval = 100;
			this.timer.Tick += new EventHandler(timer_Tick);
			this.thread = new Thread(ExecuteOnce);
			this.thread.Name = "ProgressForm Thread";
			this.ShowDialog();
			this.CloseThread();
		}

		public void StopProgressBar()
		{
			if (this.timer != null && this.timer.Enabled)
			{
				this.timer.Stop();
			}
		}

		#endregion

		/// <summary>
		/// Функция потока
		/// </summary>
		/// <param name="sender"></param>
		private void Execute(object sender)
		{
			//Delegate _dell = this.iterationCallback.GetInvocationList()[0];
			InteratorEventArgs _e;
			for (int _i = 0; _i < this.count; _i++)
			{
				_e = new InteratorEventArgs(_i);
				this.iterationCallback(_e);
				if (_e.Cancel)
					break;
				this.IncProressBar();
			}
			this.DialogResult = DialogResult.OK;
		}

		/// <summary>
		/// Функция потока, 
		/// </summary>
		/// <param name="sender"></param>
		private void ExecuteOnce(object sender)
		{
			//Delegate _dell = this.executeCallback.GetInvocationList()[0];
			//try
			//{
			//    _dell.Invoke(null);
			//}

			//catch (System.Reflection.TargetInvocationException ex)
			//{
			//    throw ex.InnerException;
			//}
			this.executeCallback();
			this.DialogResult = DialogResult.OK;
		}
	}

	public delegate void ExecuteCallback();

	public delegate void IterationCallback(InteratorEventArgs e);

	//public delegate void InteratotDelegate(InteratorEventArgs e);	

}