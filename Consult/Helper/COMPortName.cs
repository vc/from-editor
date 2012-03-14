using System;
using System.Collections.Generic;
using System.IO.Ports;
using Microsoft.Win32;

namespace FRom.Consult.Helper
{
	public class COMPortName : IComparer<COMPortName>
	{
		string _port;
		bool _isUsed;

		public COMPortName (string port)
		:this(port,false)
		{
		}
		
		public COMPortName (string port, bool isUsed)
		{
			_port = port;
			_isUsed = isUsed;
		}

		public override string ToString ()
		{
			return String.Format ("{0}{1}",
				_port,
				_isUsed ? " <<used>>" : ""
				);

		}

		/// <summary>
		/// Имя COM порта
		/// </summary>
		public string PortName {
			get { return _port; }
			set { _port = value; }
		}

		/// <summary>
		/// Занят ли порт
		/// </summary>
		public bool IsUsed {
			get { return _isUsed; }
			set { _isUsed = value; }
		}


		#region IComparer<Coms> Members

		public int Compare (COMPortName x, COMPortName y)
		{
			return x._port == y._port ? 0 : -1;
		}

		#endregion
		
		public static List<COMPortName> GetPortNames ()
		{
			return COMPortName.GetPortNames (false);
		}
		
		/// <summary>
		/// Взять список всех COM портов
		/// </summary>
		/// <returns>Список портов</returns>
		public static List<COMPortName> GetPortNames (bool openPortsOnly)
		{
			List<COMPortName> serial_ports = new List<COMPortName> ();
			using (RegistryKey subkey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DEVICEMAP\\SERIALCOMM")) {
				if (subkey != null) {
					string[] names = subkey.GetValueNames ();
					foreach (string value in names) {
						string port = subkey.GetValue (value, "").ToString ();
						if (port != "") {
							bool isAccesible = IsPortFree (port);
							if (openPortsOnly && !isAccesible)
								continue;
							serial_ports.Add (new COMPortName (port, !isAccesible));
						}
					}
				}
			}
			return serial_ports;
		}

		/// <summary>
		/// Проверить доступность порта на открытие
		/// </summary>
		/// <param name="port">имя порта</param>
		/// <returns>true - порт доступен. false - порт НЕ доступен</returns>
		public static bool IsPortFree (string port)
		{
			using (SerialPort tmp = new SerialPort(port))
				try {
					tmp.Open ();
					return true;
				} catch {
					return false;
				} finally {
					tmp.Close ();
				}
		}
	}

}
