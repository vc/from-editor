using System;
using System.Windows.Forms;
using FRom.ConsultNS.Data;
using System.ComponentModel;

namespace FRom.ConsultNS.ActiveTests
{
	public partial class UserControlScallable : UserControl
	{
		ConsultTypeOfActiveTest _type = ConsultTypeOfActiveTest.Scallable;
		ConsultActiveTest _dataSource;
		public event EventHandler<ScallableEventArgs> ApplyButtonClicked;

		public UserControlScallable()
		{
			InitializeComponent();

			btnApply.Click += new EventHandler(btnApply_Click);
			txtCoolantTemp.DataBindings.Add("Text", trBarTemp, "Value");
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			ApplyButtonClicked(this, new ScallableEventArgs(this.GetValue()));
		}

		public float GetValue()
		{
			return trBarTemp.Value;
		}

		private void InitializeInterface()
		{
			lblDescr.Text = _dataSource._scale;
			trBarTemp.Minimum = (int)_dataSource._minScale;
			trBarTemp.Maximum = (int)_dataSource._maxScale;
			trBarTemp.Value = (int)_dataSource.GetValue(0x32);
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
	}
}
