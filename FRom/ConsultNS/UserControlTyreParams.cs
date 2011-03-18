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
		TyreParams _tyre = new TyreParams(TyreWidth._205, TyreProfile._60, TyreRadius.R15);

		public TyreParams Tyre
		{
			get { return _tyre; }
			set { _tyre = value; }
		}

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

		List<TyreWidth> _lstTyreWidth = new List<TyreWidth>();
		List<TyreProfile> _lstTyreHeight = new List<TyreProfile>();
		List<TyreRadius> _lstTyreRadius = new List<TyreRadius>();

		private void InitInterface()
		{
			foreach (TyreWidth i in Enum.GetValues(typeof(TyreWidth)))
				_lstTyreWidth.Add(i);

			cmbWidth.DataSource = _lstTyreWidth;

			foreach (TyreProfile i in Enum.GetValues(typeof(TyreProfile)))
				_lstTyreHeight.Add(i);

			cmbHeight.DataSource = _lstTyreHeight;

			foreach (TyreRadius i in Enum.GetValues(typeof(TyreRadius)))
				_lstTyreRadius.Add(i);

			cmbRadius.DataSource = _lstTyreRadius;

			cmbWidth.SelectedIndex = _lstTyreWidth.IndexOf(_tyre.TWidth);
			cmbHeight.SelectedIndex = _lstTyreHeight.IndexOf(_tyre.TProfile);
			cmbRadius.SelectedIndex = _lstTyreRadius.IndexOf(_tyre.TRadius);

			cmbWidth.SelectedIndexChanged += new System.EventHandler(cmb_SelectedIndexChanged);
			cmbHeight.SelectedIndexChanged += new System.EventHandler(cmb_SelectedIndexChanged);
			cmbRadius.SelectedIndexChanged += new System.EventHandler(cmb_SelectedIndexChanged);
		}

		private void UpdateInterface()
		{
			txtDiameter.Text = Math.Round(_tyre.Diameter, 2).ToString();
		}

		public delegate void SizeChangedEventHandler(object sender, TyreParams e);
		public event SizeChangedEventHandler SizesChanged;

		private void cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			_tyre.TWidth = (TyreWidth)cmbWidth.SelectedItem;
			_tyre.TProfile = (TyreProfile)cmbHeight.SelectedItem;
			_tyre.TRadius = (TyreRadius)cmbRadius.SelectedItem;

			UpdateInterface();

			if (SizesChanged != null)
				SizesChanged.DynamicInvoke(this, _tyre);
		}
	}
}
