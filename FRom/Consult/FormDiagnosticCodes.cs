using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FRom.Consult
{
	public partial class FormDiagnosticCodes : Form
	{
		ConsultProvider _consult;

		public FormDiagnosticCodes(ConsultProvider c)
		{
			InitializeComponent();
			InitializeMenu();

			_consult = c;
			ReadCodes(null, null);
		}

		/// <summary>
		/// Задает структуру меню
		/// </summary>
		private void InitializeMenu()
		{
			this.Menu = new MainMenu(
				new MenuItem[]{
				new MenuItem("&DTC Codes", new MenuItem[]{
					new MenuItem("&Read DTC", ReadCodes),
					new MenuItem("&Clear", mnuClear_Click),					
				}),
				new MenuItem("&Sensors", new MenuItem[]{
					new MenuItem("Show &Available", mnuShowSensors_Click),
				}),
				new MenuItem("&Copy", mnuCopy_Click),
				new MenuItem("E&xit", mnuExit_Click),
				}
			);
		}

		const string strFormattingDTC = "[{2}: ({1})]{0}{3}";
		private void ReadCodes(object sender, EventArgs e)
		{
			txtInfo.Text = String.Format(strFormattingDTC,
				Environment.NewLine,
				_consult.GetECUInfo().ToString(),
				_consult.DataSource.ToString(),
				_consult.DTCFaultCodesRead().ToString()
			);
		}

		private void mnuClear_Click(object sender, EventArgs e)
		{
			txtInfo.Text = String.Format(strFormattingDTC,
				Environment.NewLine,
				_consult.GetECUInfo().ToString(),
				_consult.DataSource.ToString(),
				_consult.DTCFaultCodesClear().ToString()
			);
		}

		private void mnuShowSensors_Click(object sender, EventArgs e)
		{
			txtInfo.Text = String.Format(strFormattingDTC,
				Environment.NewLine,
				_consult.GetECUInfo().ToString(),
				_consult.DataSource.ToString(),
				_consult.DataSource.ValidSensors
			);
		}

		private void mnuCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(txtInfo.Text, TextDataFormat.Text);
		}

		private void mnuExit_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
