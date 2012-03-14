using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FRom.Consult;
using FRom.Consult.Data;
using FRom.Emulator;
using FRom.Properties;
using Helper;
using Helper.ProgressBar;

namespace FRom
{
	public partial class FormSettings : Form
	{
		/// <summary>
		/// Родитель
		/// </summary>
		FormMain _frmParrent;

		RomulatorVersion _emulatorVer;

		ConsultECUPartNumber _consultECUInfo;

		/// <summary>
		/// Список доступных COM портов
		/// </summary>
		List<COMPortName> _portsList;


		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="emu"></param>
		public FormSettings(FormMain frmParrent)
		{
			_frmParrent = frmParrent;
			_portsList = COMPortName.GetPortNames();

			InitializeComponent();

			cbConsultPort.SelectedIndexChanged += new EventHandler(cbPorts_SelectedIndexChanged);
			cbEmulatorPort.SelectedIndexChanged += new EventHandler(cbPorts_SelectedIndexChanged);

			InitializeMenu();

			InitializeConsult();
			UpdateConsultInterface();

			InitializeEmulator();

			LoadSettings();
			UpdateButtons();
		}

		void cbPorts_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateButtons();
		}

		/// <summary>
		/// Выбрать COM в ComboBox
		/// </summary>
		/// <param name="port"></param>
		/// <param name="cb"></param>
		void ComboBoxSelectedIndexUpdate(COMPortName port, ComboBox cb)
		{
			int nPort = _portsList.BinarySearch(port, port);
			if (nPort >= 0)
				cb.SelectedIndex = nPort;
		}

		private void InitializeEmulator()
		{
			cbEmulatorPort.Items.Clear();
			cbEmulatorPort.Items.AddRange(_portsList.ToArray());
			ComboBoxSelectedIndexUpdate(new COMPortName(cfg.cfgEmulatorPort), cbEmulatorPort);
		}

		private void InitializeConsult()
		{
			cbConsultPort.Items.Clear();
			cbConsultPort.Items.AddRange(_portsList.ToArray());
			ComboBoxSelectedIndexUpdate(new COMPortName(cfg.cfgConsultPort), cbConsultPort);
		}

		private void UpdateConsultInterface()
		{
			btnConsultTest.Enabled = cbConsultPort.SelectedIndex != null;
		}

		private void UpdateButtons()
		{
			const string scan = "Scan";
			const string test = "Test";

			btnConsultTest.Text = cbConsultPort.SelectedItem == null
				? scan
				: test;

			btnEmulatorTest.Text = cbEmulatorPort.SelectedItem == null
				? scan
				: test;
		}

		private void InitializeMenu()
		{
			Menu = new MainMenu(new MenuItem[]{
				new MenuItem("Tyre Calculator", MenuClickTyreCalculator),
			});
		}

		private void MenuClickTyreCalculator(object sender, EventArgs e)
		{
			FormTyreCalc frmTyreCalc = new FormTyreCalc(cfg.cfgTyreOrigin, cfg.cfgTyreCurrent);
			if (frmTyreCalc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				cfg.cfgTyreOrigin = frmTyreCalc._tOrigin;
				cfg.cfgTyreCurrent = frmTyreCalc._tNew;

				ConversionFunctions.SpeedCorrectCoefficient = TyreParams.CalcK(frmTyreCalc._tOrigin, frmTyreCalc._tNew);
			}
		}

		/// <summary>
		/// Попытка инициализации эмулятора
		/// </summary>
		/// <param name="port"></param>
		void ConnectEmulator(string port)
		{
			_emulatorVer = null;
			//Класса эмулятор не существует и выбран COM порт
			if (_emulator == null && port != "")
				try
				{
					_emulator = new Romulator(port);
					//initEmulatorFlag = true;
					_emulatorVer = _emulator.GetVersion();
					StatusLabel(StatusCommunications.Found, lblStatusEmulator);
				}
				catch (Exception ex)
				{
					StatusLabel(ex.Message, Color.Red, lblStatusEmulator);
				}
			//эмулятор уже был создан но порт не выбран. Переоткрываем
			else if (_emulator != null && port == "")
				try
				{
					_emulatorVer = _emulator.GetVersion();
					StatusLabel(StatusCommunications.Found, lblStatusEmulator);
					string emuPort = _emulator.Port.ToUpper();
					foreach (var i in cbEmulatorPort.Items)
					{
						if (i.ToString().CompareTo(emuPort) == 0)
						{
							cbEmulatorPort.SelectedItem = i;
							break;
						}
					}
				}
				catch (Exception ex)
				{
					StatusLabel(ex.Message, Color.Red, lblStatusEmulator);
				}
			else
				StatusLabel(StatusCommunications.Default, lblStatusEmulator);
		}

		/// <summary>
		/// Обработчик нажатия на кнопку TEST
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnEmulatorTest_Click(object sender, EventArgs e)
		{
			if (_emulator != null)
				_emulator.Dispose();
			_emulator = null;

			string port = cbEmulatorPort.SelectedItem == null ? cbEmulatorPort.Text : cbEmulatorPort.SelectedItem.ToString();

			//если порт актуальный
			if (port != null && port != "" && port.Substring(0, 3).ToLower().Equals("com"))
			{
				StatusLabel(StatusCommunications.Search, lblStatusEmulator);
				this.Update();
				ConnectEmulator(port);
			}
		}

		private void ConsultConnect(COMPortName comPort = null)
		{
			StatusLabel(StatusCommunications.Search, lblStatusConsult);
			StatusLabel(StatusCommunications.Search, cbConsultPort);

			string port = comPort == null ? "" : comPort.PortName;

			_consultECUInfo = null;
			//Если Класса не существует - внештатная ситуация
			if (_consult == null)
				throw new NullReferenceException("Не создан экземпляр класса Consult!");

			//Порт пуст. будем искать консульт на всех свободных портах
			if (String.IsNullOrEmpty(port))
			{
				if (!_consult.IsOnline)
				{
					List<COMPortName> lstAccesiblePorts = COMPortName.GetPortNames(true);
					using (IProgressBar progressCOMSearch = FormProgressBar.GetInstance("Initialize consult on "))
					{
						progressCOMSearch.ShowProgressBar(delegate()
						{
							foreach (COMPortName i in lstAccesiblePorts)
							{
								progressCOMSearch.SetCurrentState(i.PortName);
								try
								{
									_consult.Initialise(i.PortName, true);
									_consultECUInfo = _consult.GetECUInfo();
									break;
								}
								catch (ConsultException)
								{ continue; }
							}
						});
					}

					if (_consult.IsOnline)
					{
						StatusLabel(StatusCommunications.Found, lblStatusConsult, _consultECUInfo.ToString());
						StatusLabel(StatusCommunications.Found, cbConsultPort, _consultECUInfo.ToString());

						port = _consult.COMPort;
						_consult.Disconnect();
						ComboBoxSelectedIndexUpdate(new COMPortName(_consult.COMPort), cbConsultPort);
					}
					else
					{
						StatusLabel(StatusCommunications.NotFound, lblStatusConsult);
						StatusLabel(StatusCommunications.NotFound, cbConsultPort);
					}
				}
			}
			//выбран конкретный порт
			else
			{
				try
				{
					//выбранный порт отличается от того по которому сейчас работает консульт				
					//выбранный порт новый и consult уже подключен, отключимся
					if (port != _consult.COMPort && _consult.IsOnline)
						_consult.Disconnect();

					using (IProgressBar progress = FormProgressBar.GetInstance("Consult initialization on " + port))
					{
						progress.ShowProgressBar(delegate()
						{
							try
							{
								_consult.Initialise(port);
								_consultECUInfo = _consult.GetECUInfo();

								StatusLabel(StatusCommunications.Found, lblStatusConsult, _consultECUInfo.ToString());
								StatusLabel(StatusCommunications.Found, cbConsultPort, _consultECUInfo.ToString());
							}
							catch (ConsultException ex)
							{
								StatusLabel(StatusCommunications.NotFound, lblStatusConsult, ex.Message);
								StatusLabel(StatusCommunications.NotFound, cbConsultPort, ex.Message);
							}
						});
					}
				}
				catch (ConsultException ex)
				{
					StatusLabel(ex.Message, Color.Red, lblStatusConsult);
					StatusLabel(ex.Message, Color.Red, cbConsultPort);
					return;
				}
			}
		}

		/// <summary>
		/// Выставляет состояние индикатора эмулятора
		/// </summary>
		/// <param name="statusEmulator"></param>
		private void StatusLabel(StatusCommunications status, object o, string text = "")
		{


			string textLabel = "";
			Color col = Color.Black;
			switch (status)
			{
				case StatusCommunications.Default:
					textLabel = "Выбирете порт и нажмите 'Test'";
					col = Color.Black;
					break;
				case StatusCommunications.Found:
					textLabel = "Обнаружен: ";
					col = Color.Green;
					break;
				case StatusCommunications.NotFound:
					textLabel = "Устройство не обнаружено";
					col = Color.Red;
					break;
				case StatusCommunications.Search:
					textLabel = "Поиск...";
					col = Color.Yellow;
					break;
				default:
					break;
			}
			StatusLabel(textLabel + " " + text, col, o);
		}
		private void StatusLabel(string text, Color col, object o)
		{
			Label lbl = o as Label;
			ComboBox cb = o as ComboBox;

			HelperClass.Invoke(this, delegate()
			{
				if (lbl != null)
				{
					lbl.Text = text;
					//ToolTip tt = new ToolTip(Components);
					//tt.IsBalloon = true;
					//tt.SetToolTip(lbl, text);
					lbl.ForeColor = col;
				}
				else if (cb != null)
				{
					cb.BackColor = col;
					//ToolTip toolTip1 = new ToolTip();
					//toolTip1.SetToolTip(cb, text);
					//toolTip1.Show(text, cb, 50);
				}
				this.Update();
			});
		}

		#region Properties
		Settings cfg
		{
			get { return _frmParrent._cfg; }
		}

		protected Romulator _emulator
		{
			get { return _frmParrent._emulator; }
			set { _frmParrent._emulator = value; }
		}

		protected ConsultProvider _consult
		{
			get { return _frmParrent._consult; }
			set { _frmParrent._consult = value; }
		}
		#endregion

		private void btnOk_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void SaveSettings()
		{
			cfg.Initialize(
				new System.Configuration.SettingsContext(),
				new System.Configuration.SettingsPropertyCollection(),
				new System.Configuration.SettingsProviderCollection()
			);

			cfg.cfgADRFilesPath = this.txtADRFilesPath.Text;
			cfg.cfgROMFilesPath = this.txtBINFilesPath.Text;
			cfg.cfgOpenLastConfig = this.chkAutoLoadFiles.Checked;

			cfg.cfgConsultPort = (this.cbConsultPort.SelectedItem as COMPortName) == null
				? ""
				: (this.cbConsultPort.SelectedItem as COMPortName).PortName;

			cfg.cfgEmulatorPort = (this.cbEmulatorPort.SelectedItem as COMPortName) == null
				? ""
				: (this.cbEmulatorPort.SelectedItem as COMPortName).PortName;

			cfg.cfgConsultConnectAtStartup = this.chkConsultAutoConnect.Checked;
			cfg.cfgEmulatorConnectAtStartup = this.chkEmulatorAutoConnect.Checked;
			cfg.cfgEmulatorSaveFileAfterRead =
				this.chkEmulatorSaveROMToFileAfterDownload.Checked;
			cfg.cfgConsultKeepALive = chkConsultKeepALive.Enabled;

			cfg.Save();
		}

		/// <summary>
		/// Загружаем форму из settings
		/// </summary>
		private void LoadSettings()
		{
			this.txtADRFilesPath.Text = cfg.cfgADRFilesPath;
			this.txtBINFilesPath.Text = cfg.cfgROMFilesPath;
			this.chkAutoLoadFiles.Checked = cfg.cfgOpenLastConfig;

			this.chkConsultAutoConnect.Checked = cfg.cfgConsultConnectAtStartup;
			this.chkConsultKeepALive.Enabled = cfg.cfgConsultKeepALive;
			ComboBoxSelectedIndexUpdate(new COMPortName(cfg.cfgConsultPort), cbConsultPort);

			this.chkEmulatorAutoConnect.Checked = cfg.cfgEmulatorConnectAtStartup;
			this.chkEmulatorSaveROMToFileAfterDownload.Checked = cfg.cfgEmulatorSaveFileAfterRead;
			ComboBoxSelectedIndexUpdate(new COMPortName(cfg.cfgEmulatorPort), cbEmulatorPort);
		}

		private void btnConsultTest_Click(object sender, EventArgs e)
		{
			COMPortName port = cbConsultPort.SelectedItem as COMPortName;

			ConsultConnect(port);
		}

		enum StatusCommunications
		{
			Default,
			Found,
			NotFound,
			Search
		}

		public System.ComponentModel.IContainer Components
		{
			get
			{
				if (components == null)
					components = new Helper.ResourceDisposer();
				return components;
			}
			set { components = value; }
		}

		private void btnConsultScan_Click(object sender, EventArgs e)
		{
			ConsultConnect();
		}

		private void cbConsultPort_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateConsultInterface();
		}
	}



}
