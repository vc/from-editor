using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace FRom.ConsultNS
{
	public partial class UserControlTyreParams : UserControl
	{
		TyreParams _tyre = new TyreParams(TyreProfile._60, TyreWidth._205, TyreRadius.R15);

		public UserControlTyreParams()
		{
			InitializeComponent();
			InitInterface();
			UpdateInterface();
		}

		TyreParams DataSource
		{
			get { return _tyre; }
			set { _tyre = value; }
		}


		Array a = Enum.GetValues(typeof(TyreWidth));
		
		List<TyreWidth> _arrTyreWidth = new List<TyreWidth>();
		List<string> _arrTyreHeight = new List<string>(Enum.GetNames(typeof(TyreProfile)));
		List<string> _arrTyreRadius = new List<string>(Enum.GetNames(typeof(TyreRadius)));

		private void InitInterface()
		{
			IEnumerator = a.GetEnumerator();
			cmbWidth.DataSource = _arrTyreWidth;
			cmbHeight.DataSource = _arrTyreHeight;
			cmbRadius.DataSource = _arrTyreRadius;
		}

		private void UpdateInterface()
		{

			//cmbWidth.SelectedIndex = _arrTyreWidth.IndexOf(_tyre.Width.ToString());
			//cmbHeight.SelectedIndex = _arrTyreHeight.IndexOf(_tyre.Profile.ToString());
			//cmbRadius.SelectedIndex = _arrTyreRadius.IndexOf(_tyre.Radius.ToString());
		}
	}
}
