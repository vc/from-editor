using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Helper;
using Helper.Logger;

namespace FRom
{
	public partial class FormLogView : Form
	{
		Log _log = Log.Instance;
		FormMain _parrent;

		private AutoResetEvent flag;
		private Queue<string> _messageQueue;
		private Thread workThread;

		public FormLogView(FormMain parrent)
		{
			_parrent = parrent;

			InitializeComponent();
			InitializeMenu();

			workThread = new Thread(new ThreadStart(this.WorkFunction));
			workThread.Name = "Logger Thread";
			workThread.Priority = ThreadPriority.BelowNormal;
			workThread.Start();

			flag = new AutoResetEvent(false);
			_messageQueue = new Queue<string>();

			_log.LogLevel = EventEntryType.Debug;
			_log.NewMessage += new EventHandler<NewMessageEventArgs>(_log_NewMessage);

		}

		const string
			_cMenuStayOnTop = "Stay on &top",
			_cMenuExit = "E&xit",
			_cMenuAttachToMain = "&Attach to main";

		private void InitializeMenu()
		{
			//Menu LOG LEVEL
			string[] logLevels = Enum.GetNames(typeof(EventEntryType));
			MenuItem[] mnuLogLevels = new MenuItem[logLevels.Length];
			for (int i = 0; i < logLevels.Length; i++)
				mnuLogLevels[i] = new MenuItem(logLevels[i], MenuLogLevelClick);
			MenuItem mnuLogLevel = new MenuItem("Log Level", mnuLogLevels);

			foreach (MenuItem i in mnuLogLevels)
			{
				if (i.Text == _log.LogLevel.ToString())
				{
					i.Checked = true;
					_mnuLogLevelCurrent = i;
					break;
				}
			}

			_mnuStayOnTop = new MenuItem(_cMenuStayOnTop, MenuClick);
			_mnuStayOnTop.Checked = _parrent._cfg.cfgFormLogTopMost;
			MenuClick(_mnuStayOnTop, null);

			_mnuAttachToMain = new MenuItem(_cMenuAttachToMain, MenuClick);
			_mnuAttachToMain.Checked = _parrent._cfg.cfgFormLogAttachToMain;
			MenuClick(_mnuAttachToMain, null);

			//Main menu
			this.Menu = new MainMenu(new MenuItem[] {
				new MenuItem("Menu", new MenuItem[] { 
					_mnuStayOnTop,
					_mnuAttachToMain,
					new MenuItem("-"),
					new MenuItem(_cMenuExit, MenuClick),
				}),
				mnuLogLevel,
			});
		}

		MenuItem _mnuStayOnTop;
		MenuItem _mnuAttachToMain;

		MenuItem _mnuLogLevelCurrent;
		void MenuLogLevelClick(object sender, EventArgs e)
		{
			MenuItem menu = sender as MenuItem;
			if (menu == null)
				return;

			EventEntryType selected = (EventEntryType)Enum.Parse(typeof(EventEntryType), menu.Text);
			_log.LogLevel = selected;
			menu.Checked = true;
			_mnuLogLevelCurrent.Checked = false;
			_mnuLogLevelCurrent = menu;
		}

		void MenuClick(object sender, EventArgs e)
		{

			MenuItem menu = sender as MenuItem;
			if (menu == null)
				return;

			switch (menu.Text)
			{
				case _cMenuExit:
					Close();
					break;
				case _cMenuStayOnTop:
					if (e != null)
						menu.Checked = !menu.Checked;
					TopMost = menu.Checked;
					break;
				case _cMenuAttachToMain:
					if (e != null)
						menu.Checked = !menu.Checked;
					if (menu.Checked)
					{
						_parrent.Move += new EventHandler(_parrent_Move);
						_parrent_Move(_parrent, null);
					}
					else
						_parrent.Move -= new EventHandler(_parrent_Move);
					break;
			}
		}

		void _parrent_Move(object sender, EventArgs e)
		{
			this.Left = ((Form)sender).Right;
			this.Top = ((Form)sender).Top;
		}

		#region Log update functional
		/// <summary>
		/// New log message
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void _log_NewMessage(object sender, NewMessageEventArgs e)
		{
			_messageQueue.Enqueue(_log.LogNote(e.EventInstance));
			flag.Set();
		}

		/// <summary>
		/// Рабочая потоковая функция
		/// </summary>
		private void WorkFunction()
		{
			while (true)
			{
				this.flag.WaitOne();

				string message = null;
				StringBuilder grp = new StringBuilder();
				while ((message = Dequeue()) != null)
				{
					grp.Insert(0, message + Environment.NewLine);
				}

				HelperClass.Invoke(this, delegate
				{
					txtLog.Text = grp.ToString() + txtLog.Text;

					txtLog.Refresh();
				});
				Thread.Sleep(500);
			}
		}

		/// <summary>
		/// Выборка очередного сообщения из очереди
		/// </summary>
		/// <returns></returns>
		private string Dequeue()
		{
			lock (_messageQueue)
			{
				if (_messageQueue.Count > 0)
				{
					return _messageQueue.Dequeue();
				}
				return null;
			}
		}


		#endregion

		protected override void OnClosing(CancelEventArgs e)
		{
			//Save settings
			_parrent._cfg.cfgFormLogTopMost = _mnuStayOnTop.Checked;
			_parrent._cfg.cfgFormLogAttachToMain = _mnuAttachToMain.Checked;

			_log.NewMessage -= new EventHandler<NewMessageEventArgs>(_log_NewMessage);
			DisposeResources(true);
			base.OnClosing(e);
		}

		private void DisposeResources(bool disposing)
		{
			if (disposing)
			{
				if (workThread != null)
				{
					workThread.Abort();
					workThread.Join();
					workThread = null;
				}
				if (flag != null)
				{
					flag.Close();
					flag = null;
				}
			}
		}
	}
}
