using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using FRom.ConsultNS;
using FRom.ConsultNS.Data;
using FRom.Emulator;
using FRom.Grid;
using FRom.Logger;
using FRom.Logic;
using FRom.Properties;
using Helper;
using Helper.ProgressBar;

namespace FRom
{
	public partial class FormMain : Form, IDisposable
	{
#if DEBUG
		internal static bool debugFlag = true;
#else
		internal static bool debugFlag = false;
#endif

		internal From _bin;

		internal Romulator _emulator;

		internal Consult _consult;

		internal Log _log;

		/// <summary>
		/// Список интерфейсов для диагностики
		/// </summary>
		internal ListIndexString<IConsultData> _consltDataList;

		/// <summary>
		/// форма настроек
		/// </summary>
		internal FormSettings _frmOptions;

		/// <summary>
		/// Класс работы с интерфейсом consult
		/// </summary>
		internal FormLiveScan _frmConsult;

		/// <summary>
		/// Класс настроек приложения
		/// </summary>
		internal Settings _settings;

		FormSpeedTrial _frmSpeedTrial;

		/// <summary>
		/// Перехватчик событий главного меню.
		/// </summary>
		EventHandler _EHmainMenu;

		bool _pGridEnable;

		/// <summary>
		/// Окно отображения графиков
		/// </summary>
		FormGraph3D _graphForm;

		public FormMain()
		{
			InitializeComponent();				//Инициализация компонентов формы
			InitializeSettings();				//Инициализация настроек программы			
			InitializeEmulatorMenu();
			InitializeConsultMenu();
			InitializeMenu(mstrpMain);			//Инициализация обработчиков основного меню
			InitializeTabControl();				//Инициализация вкладок с картами

			Components.Add(this, 1000);
			Components.Add(_frmSpeedTrial);
		}

		ResourceDisposer Components
		{
			get
			{
				if (components == null)
					components = new Helper.ResourceDisposer();
				return (ResourceDisposer)components;
			}
			set
			{
				components = value;
			}
		}

		private void Dispose_Resources(bool disposing)
		{
			if (disposing)
			{
				if (frmLog != null)
					frmLog.Close();
				if (_frmConsult != null)
					_frmConsult.Close();
				if (_consult != null)
					_consult.Disconnect();
				if (_emulator != null)
					_emulator.Dispose();
				if (_bin != null)
					_bin.Clear();
				if (_settings != null)
					_settings.Save();
				if (_log != null)
					_log.Close();
			}
		}

		~FormMain()
		{
			Dispose_Resources(false);
			Dispose(false);
		}

		void IDisposable.Dispose()
		{
			//Dispose(true);
			Dispose_Resources(true);
			GC.SuppressFinalize(this);
		}

		private void InitializeConsultMenu()
		{
			bool flag = _consult.IsOnline;

			mnuConsultConnect.Checked =

			mnuConsultActiveTests.Enabled =
			mnuConsultSelfDiagnostic.Enabled =
			mnuConsultSensorsLive.Enabled =
			mnuConsultMode.Enabled =
				//ECU Info area
			txtConsultECUInfo.Visible =
			mnuConsultECUInfoSeparator.Visible =

				flag;

			if (flag)
			{
				txtConsultECUInfo.Text = _consult.GetECUInfo().ToString();
				mnuConsult.Image = Helper.Resources.pngAccept;
			}
			else
			{
				mnuConsult.Image = Helper.Resources.pngStop;
			}
		}

		private void InitializeSettings()
		{
			//Логи
			_log = Log.Instance;
			_log.CatchExceptions = true;
			_log.LogLevel = debugFlag ? EventEntryType.Debug : EventEntryType.Event;
			_log.LogFileEnabled = true;

			//делегат перехвата событий Click на ToolStripMenuItems
			_EHmainMenu = new EventHandler(menu_Click);

			//Инициализация класса настроек приложения
			_settings = new Settings();
			//Если настройки дефолтовые, то возможно обновили версию.
			//вытащим настройки из предыдущей версии и сменим флаг
			if (_settings.NeedUpgrade)
			{
				_settings.Upgrade();
				_settings.NeedUpgrade = false;
			}
			//Проверим на валидность последние пути диалогов
			if (_settings.cfgdlgADRPath == null
				|| _settings.cfgdlgADRPath.Length == 0
				|| !new DirectoryInfo(_settings.cfgdlgADRPath).Exists
			)
			{
				_settings.cfgdlgADRPath = Environment.CurrentDirectory;
			}
			if (_settings.cfgdlgROMPath == null
				|| _settings.cfgdlgROMPath.Length == 0
				|| !new DirectoryInfo(_settings.cfgdlgROMPath).Exists
			)
			{
				_settings.cfgdlgROMPath = Environment.CurrentDirectory;
			}

			//список доступных интерфейсов диагностики (устройств)
			_consltDataList = new ListIndexString<IConsultData>()
			{
				new ConsultData(new DataEngine()),
				new ConsultData(new DataAT()),
				new ConsultData(new DataHICAS()),
				new ConsultData(new DataAirCon()),
			};
			mnuConsultMode.DropDownItems.Clear();
			//Добавляем список режимов диагностики в меню
			foreach (IConsultData i in _consltDataList)
			{
				string name = i.ToString();
				ToolStripMenuItem mnu = new ToolStripMenuItem();
				//mnu.Click -= new EventHandler(menu_Click);
				//mnu.Click += new EventHandler(mnuConsultMode_Click);
				mnu.Name = mnu.Text = name;
				mnuConsultMode.DropDownItems.Add(mnu);
			}
			//Выбираем первый пункт меню по умолчанию
			mnuConsultMode_Click(
				mnuConsultMode.DropDownItems[_consltDataList[0].ToString()],
				new EventArgs());
			//класс работы через интерфейс consult
			_consult = new Consult(_consltDataList[0]);
			//Если стоит настройка на автоподключение - подключимся к консульту
			if (_settings.cfgConsultConnectAtStartup)
				menu_Click(mnuConsultConnect);

			//создаем класс работы с ROM/ADR Файлами
			_bin = new From();
			//подписываем функцию обновления интерфейса на событие смены источника данных класса
			_bin.DataSourceChanged += new From.FromEventHandler(InitFRomInterface);
			//InitInterface(_bin, null);

			//Откроем предыдущие файлы конфигурации если необходимо
			if (_settings.cfgOpenLastConfig)
			{
				if (_settings.cfgRecentAdrFiles.Count > 0 && File.Exists(_settings.cfgRecentAdrFiles[0]))
				{
					try { _bin.OpenAddressFile(_settings.cfgRecentAdrFiles[0]); }
					catch { }
				}
				if (_settings.cfgRecentBinFiles.Count > 0 && File.Exists(_settings.cfgRecentBinFiles[0]))
				{
					try { _bin.OpenROMFile(_settings.cfgRecentBinFiles[0]); }
					catch { _bin.Clear(); }
				}
			}
		}

		/// <summary>
		/// Инициализатор основного меню
		/// </summary>
		/// <param name="mstrpMain">Меню для инициализации</param>
		/// <param name="eventHandler">Обработчик событий элементов меню'Click'</param>
		private void InitializeMenu(MenuStrip menu)
		{
			btnRecentFiles_DropDownOpening(mnuRecentFilesROM, null);
			btnRecentFiles_DropDownOpening(mnuRecentFilesADR, null);

			foreach (object item in menu.Items)
				if (item is ToolStripMenuItem)
					HandleClickEvent(item as ToolStripMenuItem, _EHmainMenu);
		}

		/// <summary>
		/// Рекурсивная процедура подписывания DropDown Items на событие Click
		/// </summary>
		/// <param name="menu">MenuItem для подпиывания на событие</param>
		/// <param name="eventHandler">Обработчик события</param>
		private void HandleClickEvent(ToolStripMenuItem menu, EventHandler eventHandler)
		{
			if (menu.DropDown.Items != null)
				foreach (ToolStripItem item in menu.DropDown.Items)
				{
					if (item is ToolStripMenuItem)
					{
						HandleClickEvent(item as ToolStripMenuItem, eventHandler);
						item.Click += eventHandler;
					}
				}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{

#if DEBUG
			try
			{
				string romFileName = HelperClass.FindFolder("data") + "R32_rb26det.bin";
				_bin.OpenROMFile(romFileName);
				string addressFileName = HelperClass.FindFolder("data") + "HCR32_RB26_256_E_Z32_444cc.adr";
				_bin.OpenAddressFile(addressFileName);
				_log.LogLevel = EventEntryType.Debug;				
			}
			catch (Exception ex)
			{
				MessageBox.Show(HelperClass.GetExceptionInfo(ex));
			}
#else
			_log.LogLevel = EventEntryType.Event;
#endif
		}

		/// <summary>
		/// Инициализация интерфейса пользователя
		/// </summary>
		public void InitFRomInterface(object sender, FromEventArgs e)
		{
			if (sender is From)
			{
				From bin = sender as From;
				bool init = bin.Initialized;
				if (init)
				{
					InitComboBox();
					InitializeTabControl();
				}
				lblAddressFile.Text = _bin.DataSourceAddress == "" ? "Click to Load ADR" : bin.DataSourceAddress;
				lblBinFile.Text = _bin.DataSourceROM == "" ? "Cliack to Load BIN" : bin.DataSourceROM;

				cbMaps.Enabled =
				btnSaveBIN.Enabled =
				mnuSaveADRAs.Enabled =
				mnuSaveBIN.Enabled =
				mnuSaveBINAs.Enabled =
				mnuMapReload.Enabled = init;
			}
		}

		private void InitializeTabControl()
		{
			tabMain.TabPages.Clear();
		}

		private void InitializeEmulatorMenu()
		{
			bool emuEnable = _emulator != null;
			mnuEmulatorDownload.Enabled =
				mnuEmulatorUpload.Enabled =
				mnuEmulatorStreamMode.Enabled =
				btnEmulatorStreamMode.Enabled =
				emuEnable;
		}

		private void InitComboBox()
		{
			propertyGrid.SelectedObject = null;
			cbMaps.Items.Clear();
			cbMaps.SelectedIndex = -1;
			cbMaps.Text = " << SELECT MAP >> ";

			//Заполнение ComboBox картами
			Map[] maps = _bin.GetAllMaps();
			cbMaps.Items.AddRange(maps);
		}

		private void EmulatorUpload()
		{
			byte[] data = _bin.GetBin();
			if (data != null && (data.Length == 0x4000 || data.Length == 0x8000 || data.Length == 0x16000))
				try
				{
					_emulator.WriteBlockL(0, data);
				}
				catch (RomulatorException ex)
				{
					Error(ex, "Ошибка при загрузке данных в эмулятор", "Emulator Upload Error");
				}
		}

		private void EmulatorDownload()
		{
			byte[] data = null;
			try
			{
				data = _emulator.ReadBlockL(0, 0x8000);
			}
			catch (RomulatorException ex)
			{
				Error(ex, "Ошибка при скачке данных из эмулятора", "Emulator Download Error");
			}

			if (_settings.cfgEmulatorSaveFileAfterRead)
			{
				string fileName = Path.Combine(Environment.CurrentDirectory, DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_emulator.bin");

				try
				{
					//Попытка записи данных
					File.WriteAllBytes(fileName, data);
					_bin.OpenROMFile(fileName);
				}
				catch (Exception ex)
				{
					Error(ex, "Ошибка при сохранении данных в файл: " + fileName, "Error save Bin to file.");
				}
			}
			else
			{
				try { _bin.OpenROMFile(data); }
				catch (FromException ex)
				{
					Error(ex, "Ошибка при открытии данных скачанных из эмулятора", "Error open ROM data");
				}

			}
		}

		#region Error Handlers
		/// <summary>
		/// Уведомление пользователя об ошибке
		/// </summary>
		/// <param name="ex">Исключение</param>
		/// <param name="message">Сообщение об ошибке</param>
		/// <param name="caption">Заголовок окна сообщения</param>
		public void Error(Exception ex, string message, string caption)
		{
			message += HelperClass.GetExceptionInfo(ex);

			//Если отладка, то покажем лог
			if (debugFlag)
				UIViewLog();

			_log.WriteEntry(this, message, EventEntryType.Error);

			if (!debugFlag
				&& MessageBox.Show(message + "\n\nОтправить отчет об ошибке?", caption, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
			{
				List<string> att = new List<string>();
				bool enableAttach = false;
				if (MessageBox.Show("Прикрепить адресный файл к отчету?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					att.AddRange(new string[] {
						_bin.DataSourceAddress,
						_bin.DataSourceROM
					});
					enableAttach = true;
				}

				ShowBugReportWindow(message, att, enableAttach);
			}
		}

		private void ShowBugReportWindow(string message, List<string> att, bool enableAttach)
		{
			FormFeedBack frm = new FormFeedBack(this, message, att, enableAttach);
			frm.StartPosition = FormStartPosition.CenterParent;
			frm.ShowDialog(this);
		}

		private void Message(string msg, string caption = "", MessageBoxIcon icon = MessageBoxIcon.Information)
		{
			if (caption == "")
				caption = Application.ProductName;
			_log.WriteEntry(this, msg);
			MessageBox.Show(this, msg, caption, MessageBoxButtons.OK, icon);
		}

		private void Message(Exception ex, string caption = "", MessageBoxIcon icon = MessageBoxIcon.Information)
		{
			if (caption == "")
				caption = Application.ProductName;
			string exMsg = HelperClass.GetExceptionMessages(ex);
			HelperClass.Invoke(this, delegate()
			{
				MessageBox.Show(this, exMsg, caption, MessageBoxButtons.OK, icon);
			});
		}
		#endregion



		#region Interface Handler
		/// <summary>
		/// Обработчик нажатий клавиш на карте
		/// </summary>
		private void tabMain_KeyPress(object sender, KeyPressEventArgs e)
		{
			//this.Text= e.KeyChar.ToString();
			if (127 == e.KeyChar)
				tabMain.SelectedTab.Dispose();
		}

		/// <summary>
		/// Обработчик событий клика по табе
		/// </summary>
		private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			TabPage tab = tabMain.SelectedTab;
			if (tab == null)
				txtComment.Text = "Select Map to add Tab...";
			else
			{
				string cnst = (sender as TabControl).SelectedTab.Text;
				AddressInstance adr = _bin.GetAddressInstance(cnst);
				string name = adr.MapName;
				propertyGrid.SelectedObject = adr;	//property grid
				txtComment.Text = GetComment(adr);
				cbMaps.SelectedIndex = (int)tab.Tag;
			}
		}

		private string GetComment(AddressInstance adr)
		{
			return adr.MapName + " [" + adr.ConstName + "]";
		}

		/// <summary>
		/// Обработчик событий DropDownOpening открытия меню "Recent Files"
		/// </summary>
		private void btnRecentFiles_DropDownOpening(object sender, EventArgs e)
		{
			ToolStripDropDownItem menu = sender as ToolStripDropDownItem;
			if (menu == null)
				return;

			StringCollection files =
				(menu == mnuRecentFilesROM)
				? (_settings.cfgRecentBinFiles)
				: (_settings.cfgRecentAdrFiles);


			if (files.Count == 0)
			{
				menu.DropDownItems.Clear();
				menu.Enabled = false; //DropDownItems.Add(mnuRecentFilesEmpty);
			}
			else
			{
				menu.DropDownItems.Clear();
				foreach (var i in files)
					menu.DropDownItems.Add(i);
			}
		}

		private void InitAddAdrRecentFiles(string fileName)
		{
			if (_settings.cfgRecentAdrFiles == null) return;
			if (_settings.cfgRecentAdrFiles.Contains(fileName))
				_settings.cfgRecentAdrFiles.Remove(fileName);
			_settings.cfgRecentAdrFiles.Insert(0, fileName);
		}
		private void InitAddBinRecentFiles(string fileName)
		{
			if (_settings.cfgRecentBinFiles == null) return;
			if (_settings.cfgRecentBinFiles.Contains(fileName))
				_settings.cfgRecentBinFiles.Remove(fileName);
			_settings.cfgRecentBinFiles.Insert(0, fileName);
		}

		/// <summary>
		/// Обработчик событий 'Click' на меню
		/// </summary>
		void menu_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem menu = sender as ToolStripMenuItem;
			if (menu != null && menu.DropDownItems.Count > 0)
				return;

			ToolStripButton btn = sender as ToolStripButton;
			ToolStripLabel status = sender as ToolStripLabel;

			if (menu == null && btn == null && status == null)
				_log.WriteEntry(this, "Sender is NULL!\n" + Environment.StackTrace, EventEntryType.Warning);

			string selectedItem = menu == null
				? (btn == null)
					? (status == null)
						? ""
						: (status.Name)
					: (btn.Name)
				: (menu.Name);

			if (debugFlag)
				_log.WriteEntry(this,
					String.Format("Item clicked: {0} (Type:{1})",
					selectedItem,
					sender.GetType().ToString()),
					EventEntryType.Debug);

			try
			{
				//=================== FILE & STATUS STRIP =====================//
				if (menu == mnuSaveBIN || btn == btnSaveBIN)
					UISaveBin();
				else if (menu == mnuSaveBINAs)
					UISaveBinAs();
				else if (menu == mnuOpenADR || status == lblAddressFile || btn == btnOpenADR)
					UIOpenAddressFile();
				else if (menu == mnuOpenBIN || status == lblBinFile || btn == btnOpenBIN)
					UIOpenROMFile();
				else if (menu == mnuSettings)
					UISettings();
				else if (menu == mnuExit)
					UIExit();


				//=================== MAP =====================//
				else if (menu == mnuMapReload)
					_bin.Reload();
				else if (sender as ToolStripButton == btnToggleProp)
					UIMapToggleProperties();
				else if (menu == mnu3DMapViewToggle || btn == btnGraphShowSwitch)
					UIMapGraphSwitch();

				//=================== EMULATOR =====================//
				else if (menu == mnuEmulatorUpload)
					UIEmulatorUpload();
				else if (menu == mnuEmulatorDownload)
					UIEmulatorDownload();

				//=================== CONSULT =====================//
				else if (menu == mnuConsultSelfDiagnostic)
					UIConsultSelfDiagnostic();
				else if (menu == mnuConsultSensorsLive)
					UIConsultSensorsLive();
				else if (menu == mnuConsultSpeedTrial)
					UIConsultSpeedTrial();
				else if (menu == mnuConsultConnect)
					UIConsultConnect(menu);
				//mnuConsultMode item clicked
				else if (mnuConsultMode.DropDownItems.Contains(menu))
					mnuConsultMode_Click(sender, e);

				//=================== HELP =====================//
				else if (menu == mnuHelpAbout)
					new FormAboutBox().Show();
				else if (menu == mnuHelpSendFeedBack)
					UISendFeedBack();
				else if (menu == mnuHelpViewLog)
					UIViewLog();
				else
				{
#if DEBUG
					MessageBox.Show(@"Handle Event:  '" + selectedItem + "'  Not Found!");
#else
					MessageBox.Show(
						this, 
						"В текущей версии эта функциональность не реализована", 
						FormAboutBox.AssemblyProduct + " " + FormAboutBox.AssemblyVersion, 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Information
					);
#endif
				}
			}
			catch (Exception ex)
			{
				Error(ex, "Error operation '" + selectedItem + "'\n", "Error !");
			}
		}

		#region UserInterface functions
		private void UIConsultSpeedTrial()
		{
			if (_frmSpeedTrial == null || _frmSpeedTrial.IsDisposed)
				_frmSpeedTrial = new FormSpeedTrial(this._consult);
			_frmSpeedTrial.StartPosition = FormStartPosition.CenterParent;
			_frmSpeedTrial.Show(this);
		}

		private void UIConsultSelfDiagnostic()
		{
			FormDiagnosticCodes frm = new FormDiagnosticCodes(_consult);
			frm.ShowDialog(this);
		}

		FormLogView frmLog;
		private void UIViewLog()
		{
			if (frmLog == null || frmLog.IsDisposed)
				frmLog = new FormLogView(this);
			frmLog.Show();

		}

		private void UISaveBin()
		{
			if (_bin.Initialized)
			{
				string fileName = _bin.DataSourceROM;
				_bin.SaveBin();
				InitAddBinRecentFiles(fileName);
			}
		}

		private void UISettings()
		{
			if (_frmOptions == null || _frmOptions.IsDisposed)
				_frmOptions = new FormSettings(this);
			_frmOptions.ShowDialog(this);
			InitializeEmulatorMenu();
		}

		private void UIConsultSensorsLive()
		{
			if (_frmConsult == null || _frmConsult.IsDisposed)
				_frmConsult = new FormLiveScan(this._consult);
			_frmConsult.Show(this);
		}

		private void UISendFeedBack()
		{
			ShowBugReportWindow("!Custom user bug report",
				new List<string>(new string[] { _bin.DataSourceAddress, _bin.DataSourceROM }),
				false);
		}

		private void UIMapToggleProperties()
		{
			_pGridEnable = btnToggleProp.Checked = !btnToggleProp.Checked;
			if (_pGridEnable)
			{
				int tmp = this.Width;
				splitContMain.Panel2Collapsed = !_pGridEnable;

				this.Width += (int)splitContMain.Panel2.Tag;
				splitContMain.SplitterDistance = tmp;
			}
			else
			{
				splitContMain.Panel2.Tag = this.Width - splitContMain.SplitterDistance;
				this.Width = splitContMain.SplitterDistance;
				splitContMain.Panel2Collapsed = !_pGridEnable;
			}


			//Если проперти грид включен, нацеливаем его на активную табу
			if (_pGridEnable && tabMain.TabCount > 0)
				try
				{
					propertyGrid.SelectedObject = _bin.GetAddressInstance(tabMain.SelectedTab.Text);
				}
				catch (Exception ex)
				{
					Error(ex, null, null);
				}
		}

		private void UIOpenROMFile()
		{
			string fileName = HelperClass.ShowFileDialog(Resources.strBinFilesToShowOpen, false, _settings.cfgdlgROMPath, this);
			_settings.cfgdlgROMPath = fileName;
			if (fileName != "")
			{
				string recentFileName = _bin.DataSourceROM;
				_bin.OpenROMFile(fileName);
				InitAddBinRecentFiles(recentFileName);
				//InitInterface();
			}
		}

		private void UIOpenAddressFile()
		{
			string fileName = HelperClass.ShowFileDialog(Resources.strAdrFilesToShowOpen, false, _settings.cfgdlgADRPath, this);
			_settings.cfgdlgADRPath = fileName;
			if (fileName != "")
			{
				string recentFileName = _bin.DataSourceAddress;
				_bin.OpenAddressFile(fileName);
				InitAddAdrRecentFiles(recentFileName);
				//InitInterface();
			}
		}

		private void UISaveBinAs()
		{
			if (_bin.Initialized)
			{
				string fileName = HelperClass.ShowFileDialog(Resources.strBinFilesToShowSave, true, _settings.cfgdlgROMPath, this);
				_settings.cfgdlgROMPath = fileName;
				if (fileName != "")
				{
					_bin.SaveBin(fileName);
					InitAddBinRecentFiles(fileName);
				}
			}
		}

		private void UIMapGraphSwitch()
		{
			if (cbMaps.SelectedItem != null)
			{
				if (btnGraphShowSwitch.Checked)
				{
					cbMaps.SelectedIndexChanged -= _graphForm.mapChanged;
					_graphForm.Dispose();
					btnGraphShowSwitch.Checked
						= mnu3DMapViewToggle.Checked
						= false;
				}
				else
				{
					if (_graphForm != null)
						_graphForm.Dispose();
					_graphForm = new FormGraph3D((Map)cbMaps.SelectedItem);
					cbMaps.SelectedIndexChanged += new EventHandler(_graphForm.mapChanged);

					_graphForm.Left = this.Right;
					_graphForm.Top = this.Top;
					_graphForm.Show(this);
					this.Focus();
					btnGraphShowSwitch.Checked
						= mnu3DMapViewToggle.Checked
						= true;
				}
			}
		}

		private void UIEmulatorDownload()
		{
			DialogResult res = MessageBox.Show(
				this,
				"Перед продолжением, заглушите двигатель и выключите зажигание.\r\nПродолжить?",
				"Внимание !!!",
				MessageBoxButtons.YesNo);
			if (res == DialogResult.Yes)
				EmulatorDownload();
		}

		private void UIEmulatorUpload()
		{
			DialogResult choise = MessageBox.Show(
				this,
				"Перед продолжением, заглушите двигатель и выключите зажигание.\r\nПродолжить?",
				"Внимание !!!",
				MessageBoxButtons.YesNo);
			if (choise == DialogResult.Yes)
				EmulatorUpload();
		}

		private void UIExit()
		{
			base.Close();
		}

		private void UIConsultConnect(ToolStripMenuItem menu)
		{
			if (menu.Checked)
			{
				_consult.Disconnect();
			}
			else
			{
				using (IProgressBar _progressForm = new Form("Consult init..."))
				{
					ExecuteCallback _callback = delegate()
					{
						try
						{
							_consult.Initialise(_settings.cfgConsultPort);
						}
						catch (ConsultException ex)
						{
							Message(ex, "Connect to ECU failed!", MessageBoxIcon.Error);					
							_progressForm.StopProgressBar();
							//MessageBox.Show(ex.Message);
						}
					};
					_progressForm.ShowProgressBar(_callback);
				}
				
			}
			InitializeConsultMenu();
		}
		#endregion

		private void menu_Click(object sender, ToolStripItemClickedEventArgs e)
		{
			menu_Click(e.ClickedItem, e == null ? null : (EventArgs)e);
		}
		private void menu_Click(object sender)
		{
			menu_Click(sender, new EventArgs());
		}

		#endregion

		/// <summary>
		/// Обработчик событий ComboBox карт
		/// </summary>
		private void cbMaps_SelectedIndexChanged(object sender, EventArgs e)
		{
			//ComboBox combo = sender as ComboBox;
			ToolStripComboBox combo = sender as ToolStripComboBox;

			Map map = combo.SelectedItem as Map;
			if (map == null) return;

			int index = combo.SelectedIndex;

			//Property grid
			propertyGrid.SelectedObject = map.Address;
			string cnst = map.Address.ConstName;
			if (!tabMain.TabPages.ContainsKey(cnst)) //если таба с таким именем нет - добавляем
			{
				FRomGrid grid = new FRomGrid(_bin.GetMap(cnst));

				tabMain.TabPages.Add(cnst, cnst);
				TabPage tab = tabMain.TabPages[cnst];
				tab.Controls.Add(grid);     //добавляем на табу - грид

				//Запоминаю номер позиции комбо в табе, чтобы при переключении табов переключался комбо
				tab.Tag = combo.SelectedIndex;

				//подписываем грид на смену свойств в пропертигриде
				this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(grid.propertyGrid_PropertyValueChanged);

				txtComment.Text = GetComment(map.Address);
			}

			tabMain.SelectedTab = tabMain.TabPages[cnst];	//Активная та которую выбрали
		}

		private void MainForm_Move(object sender, EventArgs e)
		{
			if (_graphForm != null)
			{
				//if (Math.Abs(_graphForm.Left - this.Right) < 20 && Math.Abs(_graphForm.Top - this.Top) < 20)
				{
					_graphForm.Left = this.Right;
					_graphForm.Top = this.Top;
				}
			}
		}

		/// <summary>
		/// Процедура обработки нажатий на DropDownItems меню mnuConsultMode
		/// </summary>
		private void mnuConsultMode_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem mnuClicked = sender as ToolStripMenuItem;
			if (mnuClicked == null)
				return;

			IConsultData current = _consult == null ? null : _consult.DataSource;
			IConsultData clicked = _consltDataList[mnuClicked.Text];

			mnuClicked.Checked = true;
			mnuConsultMode.Text = mnuConsultMode.Tag + " [" + mnuClicked.Text + "]";

			//Если ничего выбрано небыло, то на этом все.
			if (current == null || current == clicked)
				return;

			_consult.DataSource = clicked;

			//Тип диагностики сменился
			string currentTxt = current.ToString();
			ToolStripMenuItem mnuCurrent =
				mnuConsultMode.DropDownItems[currentTxt] as ToolStripMenuItem;
			if (mnuCurrent == null)
				return;
			mnuCurrent.Checked = false;
		}
	}
}