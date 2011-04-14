using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace FRom.ConsultNS
{
	public partial class UserControlTyreParams : UserControl
	{
		static TyreParams _defaultTyre = new TyreParams(TyreWidth._205, TyreProfile._60, TyreRadius.R15);
		TyreParams _tyre;

		List<TyreWidth> _lstTyreWidth;
		List<TyreProfile> _lstTyreHeight;
		List<TyreRadius> _lstTyreRadius;

		public UserControlTyreParams()
		{
			_tyre = new TyreParams(_defaultTyre);

			InitializeComponent();
			InitializeInterface();
			UpdateInterface();
		}

		public UserControlTyreParams(TyreParams tyre)
		{
			if (tyre == null)
				_tyre = new TyreParams(_defaultTyre);

			InitializeComponent();
			InitializeInterface();
			UpdateInterface();
		}

		private void InitializeInterface()
		{
			_lstTyreWidth = new List<TyreWidth>();
			_lstTyreHeight = new List<TyreProfile>();
			_lstTyreRadius = new List<TyreRadius>();

			UpdateTextBoxesVisibles();

			foreach (TyreWidth i in Enum.GetValues(typeof(TyreWidth)))
				_lstTyreWidth.Add(i);

			cmbWidth.DataSource = _lstTyreWidth;

			foreach (TyreProfile i in Enum.GetValues(typeof(TyreProfile)))
				_lstTyreHeight.Add(i);

			cmbHeight.DataSource = _lstTyreHeight;

			foreach (TyreRadius i in Enum.GetValues(typeof(TyreRadius)))
				_lstTyreRadius.Add(i);

			cmbRadius.DataSource = _lstTyreRadius;

			UpdateSelectedItems();

			cmbWidth.SelectedIndexChanged += new System.EventHandler(cmb_SelectedIndexChanged);
			cmbHeight.SelectedIndexChanged += new System.EventHandler(cmb_SelectedIndexChanged);
			cmbRadius.SelectedIndexChanged += new System.EventHandler(cmb_SelectedIndexChanged);
		}

		private void UpdateSelectedItems()
		{

			cmbWidth.SelectedIndex = _lstTyreWidth.IndexOf(_tyre.TyreWidth);
			cmbHeight.SelectedIndex = _lstTyreHeight.IndexOf(_tyre.TyreProfile);
			cmbRadius.SelectedIndex = _lstTyreRadius.IndexOf(_tyre.TyreRadius);
		}

		private void UpdateInterface()
		{
			if (_showDiameter)
				txtDiameter.Text = Math.Round(_tyre.Diameter, 2).ToString();

			if (_showRecomendedParams)
			{
				txtRecomendRadius.Text = _tyre.Radius.ToString();
				txtRecomendWidth.Text = _tyre.RecomendedWidthOfDisk.ToString();
			}
		}

		private void UpdateTextBoxesVisibles()
		{
			txtRecomendRadius.Visible =
				txtRecomendWidth.Visible =
				lblRecomendedDiskSize.Visible =
				lblRecomendedSplitter.Visible = _showRecomendedParams;

			txtDiameter.Visible =
				lblDiameter.Visible = _showDiameter;
		}

		public delegate void SizeChangedEventHandler(object sender, TyreParams e);
		public event SizeChangedEventHandler SizesChanged;

		private void cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			_tyre.TyreWidth = (TyreWidth)cmbWidth.SelectedItem;
			_tyre.TyreProfile = (TyreProfile)cmbHeight.SelectedItem;
			_tyre.TyreRadius = (TyreRadius)cmbRadius.SelectedItem;

			UpdateInterface();

			if (SizesChanged != null)
				SizesChanged.DynamicInvoke(this, _tyre);
		}


		bool _showRecomendedParams = false;
		[DefaultValue(false)]
		public bool ShowRecomendedParams
		{
			get { return _showRecomendedParams; }
			set
			{
				_showRecomendedParams = value;
				UpdateTextBoxesVisibles();
				UpdateInterface();
			}
		}

		bool _showDiameter = true;
		[DefaultValue(true)]
		public bool ShowDiameter
		{
			get { return _showDiameter; }
			set
			{
				_showDiameter = value;
				UpdateTextBoxesVisibles();
				UpdateInterface();
			}
		}

		[Browsable(false), ReadOnly(true)]
		public TyreParams Tyre
		{
			get { return _tyre; }
			set
			{
				_tyre = value;
				UpdateSelectedItems();
			}
		}
		[Browsable(false)]
		public static TyreParams DefaultTyre
		{
			get { return _defaultTyre; }
		}
	}
}
