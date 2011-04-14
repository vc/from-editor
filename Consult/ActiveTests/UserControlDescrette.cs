using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using FRom.ConsultNS.Data;
using Helper;

namespace FRom.ConsultNS.ActiveTests
{
	public partial class UserControlDescrette : UserControl
	{
		ConsultActiveTest _dataSource;
		ConsultTypeOfActiveTest _type = ConsultTypeOfActiveTest.Descrette;
		public event EventHandler<DescretteEventArgs> ApplyButtonClicked;

		List<CheckBox> _chkBoxes;
		List<RadioButton> _rButtons;		

		//Список радио-кнопок выбора количества "галок"
		private ActiveBits _countBinarySelector;		

		bool _showRadioButtons;

		byte _value;


		public UserControlDescrette()
		{
			InitializeComponent();

			_chkBoxes = new List<CheckBox>() { chk0, chk1, chk2, chk3, chk4, chk5, chk6, chk7 };
			_rButtons = new List<RadioButton>() { null, rb2, null, rb4, null, rb6, null, rb8 };

#if DEBUG
			_countBinarySelector = ActiveBits._4 | ActiveBits._6;
#endif
		}

		private void InitializeInterface()
		{
			//RadioButtons enable/disable
			for (int i = 1; i < 8; i += 2)
				_rButtons[i].Enabled = (1 == (((int)_countBinarySelector) >> i & 0x01))
					? true
					: false;

			//CheckBoxes visibility & text
			{
				int i = 0;
				for (; i < _dataSource._bitMap.Length; i++)
					_chkBoxes[i].Text = _dataSource._bitMap[i];
				for (; i < 8; i++)
				{
					_chkBoxes[i].Text = "";
					_chkBoxes[i].Visible = false;
				}
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

		/// <summary>
		/// Возвращает байт с битовыми полями в тех местах, в которых отмечены CheckBoxes
		/// </summary>
		/// <returns></returns>
		public byte GetCurrentValue()
		{
			byte tmp = 0;
			for (int i = 0; i < 8; i++)
				if (_chkBoxes[i].Checked)
					tmp |= (byte)(1 << i);

			return tmp;
		}

		

		public bool ShowRadioButtons { get; set; }

		private void btnApply_Click(object sender, EventArgs e)
		{
			ApplyButtonClicked(this, new DescretteEventArgs(this.GetCurrentValue()));
		}

		//[Editor("byte", "byte")]
		//public byte ActiveFields
		//{
		//    get { return _countBinary; }
		//    set { _countBinary = value; }
		//}

		enum ActiveBits
		{
			//_1 = 0x01,
			_2 = 0x02,
			//_3 = 0x04,
			_4 = 0x08,
			//_5 = 0x10,
			_6 = 0x20,
			//_7 = 0x40,
			_8 = 0x80,
		}

		private void rb_CheckedChanged(object sender, EventArgs e)
		{
			RadioButton sel = sender as RadioButton;
			if (sel == null)
				return;
			int i = 0;
			int n = Int32.Parse((string)sel.Tag);

			for (; i < n; i++)
			{
				_chkBoxes[i].Enabled = true;
			}
			for (; i < 8; i++)
			{
				_chkBoxes[i].Enabled = false;
				_chkBoxes[i].Checked = false;
			}
		}
	}
}
