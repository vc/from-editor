﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FRom.Properties;

namespace FRom.ConsultNS
{
	public partial class FormTyreCalc : Form
	{
		TyreParams _tOrigin;
		TyreParams _tNew;

		FormMain _main;

		public FormTyreCalc(FormMain main = null)
		{
			InitializeComponent();

			_main = main;
			Settings cfg = _main._settings;

			if (cfg.cfgTyreOrigin == null)
				_tOrigin = ctlTyresOriginal.Tyre;
			else
				_tOrigin = cfg.cfgTyreOrigin;

			if (cfg.cfgTyreCurrent == null)
				_tNew = ctlTyresNew.Tyre;
			else
				_tNew = cfg.cfgTyreCurrent;

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
			get { return _tOrigin / _tNew; }
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (_main != null)
			{
				Settings cfg = _main._settings;
				cfg.cfgTyreOrigin = _tOrigin;
				cfg.cfgTyreCurrent = _tNew;
				cfg.Save();
			}
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
