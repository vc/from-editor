using System;
using System.Threading;
using System.Windows.Forms;

namespace Helper.ProgressBar
{
	public partial class FormProgressBar : Form, IProgressBar
	{
		private int _count;
		private Thread _thread;
		private System.Windows.Forms.Timer _timer;

		/// <summary>
		/// Конструкто
		/// </summary>
		/// <param name="formCaption">Заголовок формы</param>
		protected FormProgressBar (string formCaption)
		{
			InitializeComponent ();
			this.Tag = this.Text = formCaption;
		}

		delegate void helpDelegateEvent (string text,int index,int? total);

		delegate void helpDelegateState (string str);

		delegate void helpDelegateInc ();

		/// <summary>
		/// Увеличить на один прогресс бар
		/// </summary>		
		private void IncProressBar ()
		{
			if (this.progressBar.InvokeRequired)
				this.progressBar.Invoke (new helpDelegateInc (this.IncProressBar));
			else {
				this.progressBar.PerformStep ();
				Application.DoEvents ();
			}
		}

		void timer_Tick (object sender, EventArgs e)
		{
			this.progressBar.PerformStep ();
			if (this.progressBar.Value == this.progressBar.Maximum)
				this.progressBar.Value = 0;
		}

		private void buttonCancel_Click (object sender, EventArgs e)
		{
			this.CloseThread ();
			this.DialogResult = DialogResult.Cancel;
		}

		/// <summary>
		/// Закрыть поток
		/// </summary>
		private void CloseThread ()
		{
			if (this._thread != null) {
				this._thread.Abort ();
				this._thread.Join ();
				this._thread = null;
			}
			if (this._timer != null) {
				this._timer.Stop ();
				this._timer.Dispose ();
				this._timer = null;
			}
		}

		private void ProgressBarForm_Shown (object sender, EventArgs e)
		{
			if (this._timer != null)
				this._timer.Start ();
			if (this._thread != null)
				this._thread.Start ();
		}

		/// <summary>
		/// итерационный Callback
		/// </summary>
		private IterationCallback _iterationCallback;

		/// <summary>
		/// Обычный callback без параметров
		/// </summary>
		private ExecuteCallback _executeCallback;
		
		public static IProgressBar GetInstance ()
		{
			return FormProgressBar.GetInstance ("");
		}
		
		public static IProgressBar GetInstance (string caption)
		{
			return new FormProgressBar (caption);
		}

		#region IProgressBar Members

		public void SetCurrentState (string currentState)
		{
			if (this.progressBar.InvokeRequired)
				this.progressBar.Invoke (new helpDelegateState (this.SetCurrentState), currentState);
			else {
				this.Text = String.Format ("{0} [{1}]",
					this.Tag,
					currentState);
				Application.DoEvents ();
			}
		}
		
		public void SetCurrentState (string text, int index)
		{
			this.SetCurrentState (text, index, null);
		}
		
		public void SetCurrentState (string text, int index, int? total)
		{
			HelperClass.BeginInvoke (this, delegate()
			{
				if (total != null)
					progressBar.Maximum = (int)total;
				progressBar.Value = index;

				this.Text = String.Format ("{0} [{1}]",
					this.Tag,
					text
					);
				this.Refresh ();
				//Application.DoEvents();
				//SetCurrentState(text);
			});
		}

		public void ShowProgressBar (int count, IterationCallback callback)
		{
			this._iterationCallback = callback;
			this._count = count;
			this.progressBar.Maximum = this._count;

			this._thread = new Thread (Execute);
			this._thread.Name = "ProgressForm Iteration Thread";

			this.ShowDialog ();
			this.CloseThread ();
		}

		public void ShowProgressBar (ExecuteCallback callback)
		{
			this._executeCallback = callback;
			this.progressBar.Maximum = 50;
			this.btnCancel.Enabled = false;

			this._timer = new System.Windows.Forms.Timer ();
			this._timer.Interval = 100;
			this._timer.Tick += new EventHandler (timer_Tick);

			this._thread = new Thread (ExecuteOnce);
			this._thread.Name = "ProgressForm ExecuteOnce Thread";

			this.ShowDialog ();
			this.CloseThread ();
		}

		public void ShowProgressBar ()
		{
			this.btnCancel.Enabled = false;

			this.ShowDialog ();
		}

		public void StopProgressBar ()
		{
			if (this._timer != null && this._timer.Enabled) {
				this._timer.Stop ();
			}
		}

		#endregion

		/// <summary>
		/// Функция потока
		/// </summary>
		/// <param name="sender"></param>
		private void Execute (object sender)
		{
			InteratorEventArgs e;
			for (int i = 0; i < this._count; i++) {
				e = new InteratorEventArgs (i);
				this._iterationCallback (e);
				if (e.Cancel)
					break;
				this.IncProressBar ();
			}
			this.DialogResult = DialogResult.OK;
		}

		/// <summary>
		/// Функция потока, 
		/// </summary>
		/// <param name="sender"></param>
		private void ExecuteOnce (object sender)
		{
			this._executeCallback ();
			this.DialogResult = DialogResult.OK;
		}
	}
}