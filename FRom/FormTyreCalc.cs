using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FRom.Properties;
using FRom.Consult;

namespace FRom
{
	public partial class FormTyreCalc : Form
	{
		internal TyreParams _tOrigin;
		internal TyreParams _tNew;

		public FormTyreCalc(TyreParams origin = null, TyreParams current = null)
		{
			InitializeComponent();

			_tOrigin = (origin == null)
				? UserControlTyreParams.DefaultTyre
				: origin;

			_tNew = (current == null)
				? UserControlTyreParams.DefaultTyre
				: current;

			ctlTyresOriginal.Tyre = _tOrigin;
			ctlTyresNew.Tyre = _tNew;

			CalcAcurancy();

			this.VisibleChanged += new EventHandler(FormTyreCalc_VisibleChanged);
		}

		void FormTyreCalc_VisibleChanged(object sender, EventArgs e)
		{
			CalcAcurancy();
		}

		void CalcAcurancy()
		{
			txtAccurancy.Text = (Math.Round(K * 100, 2)).ToString() + txtAccurancy.Tag;
		}

		private void ctlTyresOriginal_SizesChanged(object sender, TyreParams e)
		{
			_tOrigin = e;
			CalcAcurancy();
		}

		private void ctlTyresNew_SizesChanged(object sender, TyreParams e)
		{
			_tNew = e;
			CalcAcurancy();
		}

		/// <summary>
		/// Коэффициент корректировки скорости с учетом другого размера шин
		/// </summary>
		public double K
		{
			get { return TyreParams.CalcK(_tOrigin,_tNew); }
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Close();
		}
	}
}
