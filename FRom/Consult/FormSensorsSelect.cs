using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FRom.Consult.Data;

namespace FRom.Consult
{
	public partial class FormSensorSelect : Form
	{
		const int _cButtonOffset = 95;
		const int _cGroupSensorsOffsetXY = 15;
		const int _cGroupBoxMinimumWidth = 50;
		IConsultData _data;
		GroupBox _grpSensors;
		List<CheckBox> _chkBoxes;
		Button[] _buttons;
		bool flagOKClicked = false;

		public FormSensorSelect(IConsultData data)
		{
			InitializeComponent();
			InitializeCustomControls();
			_data = data;
		}

		private void InitializeCustomControls()
		{
			_grpSensors = new GroupBox();
			_grpSensors.Location = new Point(_cGroupSensorsOffsetXY, _cGroupSensorsOffsetXY);
			_grpSensors.Text = "Available sensors";
			_grpSensors.Name = "grpSensors";
			this.Controls.Add(_grpSensors);

			_buttons = new Button[] {
				btnOK,
				btnCancel,
				btnSelectAll,
			};
		}

		private void FillForm()
		{
			if (_chkBoxes == null)
			{
				_chkBoxes = new List<CheckBox>();

				//fill and Placing check boxes
				int maxWidth = _cGroupBoxMinimumWidth;
				int yPosCheckBox = _cGroupSensorsOffsetXY;
				foreach (ConsultSensor i in _data.ValidSensors)
				{
					CheckBox chk = new CheckBox();
					chk.Name = chk.Text = i._name;
					chk.AutoSize = true;
					chk.Location = new Point(_cGroupSensorsOffsetXY, yPosCheckBox);

					_chkBoxes.Add(chk);
					_grpSensors.Controls.Add(chk);

					if (chk.Width > maxWidth)
						maxWidth = chk.Width;

					yPosCheckBox += chk.Height;
				}
				_grpSensors.Width = maxWidth + _cGroupSensorsOffsetXY * 2;
				_grpSensors.Height = yPosCheckBox + _cGroupSensorsOffsetXY;

				this.Width = _grpSensors.Width + _cButtonOffset + _cGroupSensorsOffsetXY * 2;
				this.Height = _grpSensors.Height + _cGroupSensorsOffsetXY * 4;
				frmSensorSelect_SizeChanged(this, null);
			}
		}

		private void frmSensorSelect_SizeChanged(object sender, EventArgs e)
		{
			foreach (Button i in _buttons)
				i.Left = this.Width - _cButtonOffset;
		}

		public List<ConsultSensor> GetListSensors(IWin32Window owner)
		{
			FillForm();

			base.ShowDialog(owner);

			if (flagOKClicked)
			{
				List<ConsultSensor> lst = new List<ConsultSensor>();
				foreach (CheckBox i in _chkBoxes)
				{
					if (i.Checked)
						lst.Add(_data.ValidSensors[i.Name]);
				}
				return lst;
			}
			else
				return null;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			flagOKClicked = true;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			flagOKClicked = false;
			this.Close();
		}

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			foreach (CheckBox i in _chkBoxes)
			{
				i.Checked = !i.Checked;
			}
		}
	}
}
