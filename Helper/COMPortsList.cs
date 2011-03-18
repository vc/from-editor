using System;
using System.Collections.Generic;
using System.IO.Ports;
using Microsoft.Win32;

namespace Helper
{

	public class COMPortsList : IComparer<COMPortsList>
	{
		string _port;
		bool _isUsed;
		public COMPortsList(string port, bool isUsed = false)
		{
			_port = port;
			_isUsed = isUsed;
		}

		public override string ToString()
		{
			return String.Format("{0} {1}",
				_port,
				_isUsed ? "<<used>>" : ""
				);

		}

		#region IComparer<Coms> Members

		public int Compare(COMPortsList x, COMPortsList y)
		{
			return x._port == y._port ? 0 : -1;
		}

		#endregion

		/// <summary>
		/// Взять список всех COM портов
		/// </summary>
		/// <returns>Список портов</returns>
		public static List<COMPortsList> GetPortNames()
		{
			List<COMPortsList> serial_ports = new List<COMPortsList>();
			using (RegistryKey subkey = Registry.LocalMachine.OpenSubKey("HARDWARE\\DEVICEMAP\\SERIALCOMM"))
			{
				if (subkey != null)
				{
					string[] names = subkey.GetValueNames();
					foreach (string value in names)
					{
						string port = subkey.GetValue(value, "").ToString();
						if (port != "")
						{
							serial_ports.Add(new COMPortsList(port, !CheckAccessPort(port)));
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
		public static bool CheckAccessPort(string port)
		{
			using (SerialPort tmp = new SerialPort(port))
				try
				{
					tmp.Open();
					return true;
				}
				catch
				{
					return false;
				}
				finally
				{
					tmp.Close();
				}
		}
	}

}
