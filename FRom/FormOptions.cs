using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FRom.Emulator;
using System.IO.Ports;
using Microsoft.Win32;
using System.Threading;
using FRom.ConsultNS;
using FRom.ConsultNS.Data;
using System.Runtime.CompilerServices;
using FRom.Properties;

namespace FRom
{
	public partial class FormSettings : Form
	{
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

		/// <summary>
		/// Родитель
		/// </summary>
		FormMain _frmParrent;

		//bool initEmulatorFlag = false;
		RomulatorVersion _emulatorVer;

		//bool initConsultFlag = false;
		ConsultECUPartNumber _consultECUInfo;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="emu"></param>
		public FormSettings(FormMain frmParrent)
		{
			_frmParrent = frmParrent;

			InitializeComponent();
		}

		/// <summary>
		/// Выполняется при загрузке формы
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormSettings_Load(object sender, EventArgs e)
		{

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

		/// <summary>
		/// Взять список всех COM портов
		/// </summary>
		/// <returns>Массив портов</returns>
		public static string[] GetPortNames()
		{
			List<string> serial_ports = new List<string>();
			using (RegistryKey subkey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DEVICEMAP\\SERIALCOMM"))
			{
				if (subkey != null)
				{
					string[] names = subkey.GetValueNames();
					foreach (string value in names)
					{
						string port = subkey.GetValue(value, "").ToString();
						if (port != "")
							serial_ports.Add(port);
					}
				}
			}
			return serial_ports.ToArray();
		}

		#region Properties
		public Romulator Emulator
		{
			get { return _emulator; }
		}
		#endregion

		private void btnOk_Click(object sender, EventArgs e)
		{
			Properties.Settings set = _frmParrent._settings;

			set.Initialize(
				new System.Configuration.SettingsContext(),
				new System.Configuration.SettingsPropertyCollection(),
				new System.Configuration.SettingsProviderCollection()
			);
			//_frmParrent._settings.Providers.Add(

			set.cfg_ADRFilesPath = this.txtADRFilesPath.Text;
			set.cfg_ROMFilesPath = this.txtBINFilesPath.Text;
			set.cfg_OpenLastConfig = this.chkAutoLoadFiles.Checked;
			set.cfg_ConsultPort = this.cbConsultPort.SelectedItem as string;
			set.cfg_EmulatorPort = this.cbEmulatorPort.SelectedItem as string;
			set.cfg_ConsultConnectAtStartup = this.chkConsultAutoConnect.Checked;
			set.cfg_EmulatorConnectAtStartup = this.chkEmulatorAutoConnect.Checked;
			set.cfg_EmulatorSaveFileAfterRead =
				this.chkEmulatorSaveROMToFileAfterDownload.Checked;
			set.cfg_ConsultKeepALive = chkConsultKeepALive.Enabled;

			set.Save();
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

		/// <summary>
		/// Заполняет sender списком доступных COM портов
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbPortItemsStore(object sender, EventArgs e)
		{
			ComboBox cb = (ComboBox)sender;
			cb.Items.AddRange(GetPortNames());
		}

		private void cbEmulatorPort_VisibleChanged(object sender, EventArgs e)
		{
			cbPortItemsStore(sender, e);
			List<string> ports = new List<string>(GetPortNames());
			int nPortEmulator = ports.BinarySearch(_frmParrent._settings.cfg_EmulatorPort);
			if (nPortEmulator >= 0)
				cbEmulatorPort.SelectedIndex = nPortEmulator;
			InitEmulator(_frmParrent._settings.cfg_EmulatorPort);
		}

		private void cbConsultPort_VisibleChanged(object sender, EventArgs e)
		{
			cbPortItemsStore(sender, e);
			List<string> ports = new List<string>(GetPortNames());
			int nPortConsult = ports.BinarySearch(_frmParrent._settings.cfg_ConsultPort);
			if (nPortConsult >= 0)
				cbConsultPort.SelectedIndex = nPortConsult;
			InitConsult(_frmParrent._settings.cfg_ConsultPort);
		}

		private void FormSettings_Shown(object sender, EventArgs e)
		{
			Settings set = _frmParrent._settings;

			//Загружаем форму из settings			
			this.txtADRFilesPath.Text = set.cfg_ADRFilesPath;
			this.txtBINFilesPath.Text = set.cfg_ROMFilesPath;
			this.chkAutoLoadFiles.Checked = set.cfg_OpenLastConfig;
			this.chkConsultAutoConnect.Checked = set.cfg_ConsultConnectAtStartup;
			this.chkEmulatorAutoConnect.Checked = set.cfg_EmulatorConnectAtStartup;
			this.chkEmulatorSaveROMToFileAfterDownload.Checked =
				set.cfg_EmulatorSaveFileAfterRead;
			this.chkConsultKeepALive.Enabled = set.cfg_ConsultKeepALive;


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
