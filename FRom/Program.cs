
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace FRom
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main ()
		{
			Application.ThreadException += Program.ExceptionHandler;
			
			Application.EnableVisualStyles ();
			Application.SetCompatibleTextRenderingDefault (false);

			Form frm = new FormMain ();
			frm.StartPosition = FormStartPosition.Manual;

			Application.Run (frm);
		}
		
		static void ExceptionHandler (object sender, ThreadExceptionEventArgs ex)
		{
			MessageBox.Show (ex.ToString());
		}
	}
}