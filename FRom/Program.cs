using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;

namespace FRom
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Form frm = new FormMain();
			frm.StartPosition = FormStartPosition.Manual;

			Application.Run(frm);
		}
	}
}