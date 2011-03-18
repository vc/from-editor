using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FRom.ConsultNS;
using FRom.ConsultNS.Data;
using FRom.Emulator;
using FRom.Properties;
using Microsoft.Win32;
using Helper;

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
		List<COMPortsList> _portsList;


		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="emu"></param>
		public FormSettings(FormMain frmParrent)
		{
			_frmParrent = frmParrent;
			_portsList = COMPortsList.GetPortNames();

			InitializeComponent();
			InitializeMenu();
			InitializeConsult();
			InitializeEmulator();
			InitializeSettings();
		}

		private void cbEmulatorPort_VisibleChanged(object sender, EventArgs e)
		{
			COMPortsList com = new COMPortsList(_frmParrent._settings.cfgEmulatorPort);
			int nPortEmulator = _portsList.BinarySearch(com, com);
			if (nPortEmulator >= 0)
				cbEmulatorPort.SelectedIndex = nPortEmulator;
			InitEmulator(_frmParrent._settings.cfgEmulatorPort);
		}

		private void cbConsultPort_VisibleChanged(object sender, EventArgs e)
		{
			COMPortsList com = new COMPortsList(_frmParrent._settings.cfgConsultPort);
			int nPortConsult = _portsList.BinarySearch(com, com);
			if (nPortConsult >= 0)
				cbConsultPort.SelectedIndex = nPortConsult;
			InitConsult(_frmParrent._settings.cfgConsultPort);
		}

		private void InitializeEmulator()
		{
			cbEmulatorPort.Items.Clear();
			cbEmulatorPort.Items.AddRange(_portsList.ToArray());
			cbEmulatorPort_VisibleChanged(cbEmulatorPort, null);
		}

		private void InitializeConsult()
		{
			cbConsultPort.Items.Clear();
			cbConsultPort.Items.AddRange(_portsList.ToArray());
			cbConsultPort_VisibleChanged(cbConsultPort, null);
		}

		/// <summary>
		/// Загружаем форму из settings
		/// </summary>
		private void InitializeSettings()
		{
			this.txtADRFilesPath.Text = cfg.cfgADRFilesPath;
			this.txtBINFilesPath.Text = cfg.cfgROMFilesPath;
			this.chkAutoLoadFiles.Checked = cfg.cfgOpenLastConfig;
			this.chkConsultAutoConnect.Checked = cfg.cfgConsultConnectAtStartup;
			this.chkEmulatorAutoConnect.Checked = cfg.cfgEmulatorConnectAtStartup;
			this.chkEmulatorSaveROMToFileAfterDownload.Checked =
				cfg.cfgEmulatorSaveFileAfterRead;
			this.chkConsultKeepALive.Enabled = cfg.cfgConsultKeepALive;
		}

		private void InitializeMenu()
		{
			Menu = new MainMenu(new MenuItem[]{
				new MenuItem("Tyre Calculator", MenuClickTyreCalculator),
			});
		}

		Settings cfg
		{
			get { return _frmParrent._settings; }
		}

		protected Romulator _emulator
		{
			get { return _frmParrent._emulator; }
			set { _frmParrent._emulator = value; }
		}

		protected Consult _consult
		{
			get { return _frmParrent._consult; }
			set { _frmParrent._consult = value; }
		}

		private void MenuClickTyreCalculator(object sender, EventArgs e)
		{
			FormTyreCalc frmTyreCalc = new FormTyreCalc(cfg.cfgTyreOrigin, cfg.cfgTyreCurrent);
			if (frmTyreCalc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				cfg.cfgTyreOrigin = frmTyreCalc._tOrigin;
				cfg.cfgTyreCurrent = frmTyreCalc._tNew;
			}
		}

		/// <summary>
		/// Попытка инициализации эмулятора
		/// </summary>
		/// <param name="port"></param>
		void InitEmulator(string port)
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
				InitEmulator(port);
			}
		}

		private void InitConsult(string port)
		{
			_consultECUInfo = null;
			//Если Класса не существует - внештатная ситуация
			if (_consult == null)
				throw new NullReferenceException("Не создан экземпляр класса Consult!");
			if (port == null)
				return;

			//выбран конкретный порт. пробуем к нему подключиться
			if (port != "")
				try
				{
					//Если подключены - отключаемся
					if (_consult.State != ConsultClassState.ECU_OFFLINE)
						_consult.Disconnect();
					_consult.Initialise(port);
					_consultECUInfo = _consult.GetECUInfo();
					StatusLabel(StatusCommunications.Found, lblStatusConsult, _consultECUInfo.ToString());
					StatusLabel(StatusCommunications.Found, cbConsultPort, _consultECUInfo.ToString());
				}
				catch (ConsultException ex)
				{
					StatusLabel(ex.Message, Color.Red, lblStatusConsult);
					StatusLabel(ex.Message, Color.Red, cbConsultPort);
				}
			//Переоткрываем последний выбранный
			else if (port == "")
				try
				{
					_consultECUInfo = _consult.GetECUInfo();
					StatusLabel(StatusCommunications.Found, lblStatusEmulator, _consultECUInfo.ToString());
					string consPort = _consult.COMPort.ToUpper();
					foreach (var i in cbConsultPort.Items)
					{
						if (i.ToString().CompareTo(consPort) == 0)
						{
							cbConsultPort.SelectedItem = i;
							break;
						}
					}
				}
				catch (Exception ex)
				{
					StatusLabel(ex.Message, Color.Red, lblStatusConsult);
				}
			else
				StatusLabel(StatusCommunications.Default, lblStatusConsult);
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
					textLabel = "Обнаружен: v";
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

			if (lbl != null)
			{
				lbl.Text = text;
				lbl.ForeColor = col;
			}
			else if (cb != null)
			{
				cb.BackColor = col;
				ToolTip toolTip1 = new ToolTip();
				//toolTip1.SetToolTip(cb, text);
				toolTip1.Show(text, cb);
			}
			this.Update();
		}

		#region Properties
		public Romulator Emulator
		{
			get { return _emulator; }
		}
		#endregion

		private void btnOk_Click(object sender, EventArgs e)
		{
			cfg.Initialize(
				new System.Configuration.SettingsContext(),
				new System.Configuration.SettingsPropertyCollection(),
				new System.Configuration.SettingsProviderCollection()
			);
			//_frmParrent._settings.Providers.Add(

			cfg.cfgADRFilesPath = this.txtADRFilesPath.Text;
			cfg.cfgROMFilesPath = this.txtBINFilesPath.Text;
			cfg.cfgOpenLastConfig = this.chkAutoLoadFiles.Checked;
			cfg.cfgConsultPort = this.cbConsultPort.SelectedItem as string;
			cfg.cfgEmulatorPort = this.cbEmulatorPort.SelectedItem as string;
			cfg.cfgConsultConnectAtStartup = this.chkConsultAutoConnect.Checked;
			cfg.cfgEmulatorConnectAtStartup = this.chkEmulatorAutoConnect.Checked;
			cfg.cfgEmulatorSaveFileAfterRead =
				this.chkEmulatorSaveROMToFileAfterDownload.Checked;
			cfg.cfgConsultKeepALive = chkConsultKeepALive.Enabled;

			cfg.Save();
		}

		private void btnConsultTest_Click(object sender, EventArgs e)
		{
			if (cbConsultPort.SelectedItem == null)
				return;
			string port = cbConsultPort.SelectedItem.ToString();

			StatusLabel(StatusCommunications.Search, lblStatusConsult);
			StatusLabel(StatusCommunications.Search, cbConsultPort);

			InitConsult(port);
		}
	}


	enum StatusCommunications
	{
		Default,
		Found,
		NotFound,
		Search
	}
}
