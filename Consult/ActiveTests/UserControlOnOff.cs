using System;
using System.Windows.Forms;
using FRom.Consult.Data;
using System.ComponentModel;

namespace FRom.Consult.ActiveTests
{
	public partial class UserControlOnOff : UserControl
	{
		ConsultTypeOfActiveTest _type = ConsultTypeOfActiveTest.OnOff;
		ConsultActiveTest _dataSource;
		public event EventHandler<OnOffEventArgs> ApplyButtonClicked;

		bool _value;

		public UserControlOnOff()
		{
			InitializeComponent();
		}

		private void InitializeInterface()
		{
			grpOnOff.Text = _dataSource._name;
			_value = chkOnOff.Checked = _dataSource._onOffDefaultValue;
			CheckEnableButton();
		}

		/// <summary>
		/// Последнее примененное значение
		/// </summary>
		public bool Value
		{
			get { return _value; }
			set
			{
				_value = value;
				CheckEnableButton();
			}
		}

		[ReadOnly(true), Browsable(false)]
		public ConsultActiveTest Datasource
		{
			get { return _dataSource; }
			set
			{
				if (value == null)
					throw new NullReferenceException();

				if (value._type != this._type)
					throw new InvalidOperationException("Ожидается тип активного сенсора - " + _type.ToString());
				_dataSource = value;
				InitializeInterface();
			}
		}


		private void btnApply_Click(object sender, EventArgs e)
		{
			Value = GetCurrentValue();
			ApplyButtonClicked(this, new OnOffEventArgs(Value));
		}

		/// <summary>
		/// Текущее значение
		/// </summary>
		/// <returns></returns>
		private bool GetCurrentValue()
		{
			return chkOnOff.Checked;
		}

		private void chkOnOff_CheckedChanged(object sender, EventArgs e)
		{
			CheckEnableButton();
		}

		private void CheckEnableButton()
		{
			btnApply.Enabled = !_value == GetCurrentValue();
		}
	}
}
