using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FRom.Consult.Data;
using FRom.Consult;

namespace FRom
{
	public partial class FormConsultActiveTest : Form
	{
		ConsultProvider _consult;
		public FormConsultActiveTest(ConsultProvider consult)
		{
			_consult = consult;

			InitializeComponent();
			InitializeMenu();

			//ConsultData data = new ConsultData(new DataEngine);

			ctrlScallable.Datasource = _consult.DataSource.AllActiveTests["COOLANT TEMP TEST"];
		}

		private void InitializeMenu()
		{
			base.Menu = new MainMenu(new MenuItem[] { 
				new MenuItem("Exit", MenuExitEventHandler),
			});
		}

		private void MenuExitEventHandler(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
