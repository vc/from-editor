using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ColorBarNS
{
	public class ColorTextBar : ColorBarNS.ColorBar
	{
		// Text Display members
		private Font _textFont;
		private Color _progressForeColor;

		//private bool _showPercent = true;
		private bool _showText = true;

		private string _customText = "Text";
		private string _customUnitMeasure = "Units";
		private string _textFormat = "{0} [{1}{2}]({3})";

		private float _minimum = 0;
		private float _maximum = 100;
		private float _ratio = 1;

		/// <summary>
		/// Смещение текста по Y.
		/// holds offset for Fixed3D borderstyle of panel
		/// </summary>
		const int _cTextYOffset = 1;

		public ColorTextBar()
			: base()
		{
			base.Maximum = 100;
			base.Minimum = 0;

			this._maximum = 100;
			this._minimum = 0;
			RatioReCalc();

			base.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

			SetColorListGradient(
				Color.Green
				//Color.LightGreen,
			);

			try { _textFont = new Font("Consolas", 10); }
			catch { new Font(this.Font, FontStyle.Bold); }
			_progressForeColor = Color.White;

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
		}

		/// <summary>
		/// Draw the text value of the progress to the screen
		/// </summary>
		/// <param name="e"></param>
		private void DrawText(Graphics e)
		{
			float dPercent = GetValueFloat() * 100;
			string textPercent = Math.Round(dPercent, 0).ToString() + "%";
			string text = String.Format(_textFormat,
				_customText,
				ValueF.ToString(),
				_customUnitMeasure,
				textPercent);

			SizeF strSize = e.MeasureString(text, _textFont);

			// draw the text
			e.DrawString(
				text,
				_textFont,
				new SolidBrush(_progressForeColor),
				(this.Width - strSize.Width) / 2,
				(this.Height - strSize.Height) / 2 - _cTextYOffset
				);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.DrawProgressBar(e.Graphics);
			if (_showText)
				DrawText(e.Graphics);

		}

		/// <summary>
		/// Sets the color of the progress display text
		/// </summary>
		public Color ProgressTextColor
		{
			get
			{ return _progressForeColor; }
			set
			{
				_progressForeColor = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Sets the display font for the prgress text
		/// </summary>
		public Font ProgressTextFont
		{
			get
			{ return _textFont; }
			set
			{
				_textFont = value;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Формат выводимого текста
		/// </summary>
		[Description("Text format string Ex:'{0} ({1}{2})' where: {0} - CustomText, {1} - Value, {2} - CustomUnitMeasure, {3} - PercentValue"), DefaultValue("{0} [{1}{2}]({3})")]
		public string TextFormat
		{
			get { return _textFormat; }
			set
			{
				try { string.Format(value, "text", 100, "%", 100); }
				catch { return; }

				_textFormat = value;
			}
		}

		[DefaultValue("Text")]
		public string CustomText
		{
			get { return _customText; }
			set { _customText = value; }
		}

		[DefaultValue("Units")]
		public string CustomUnitMeasure
		{
			get { return _customUnitMeasure; }
			set { _customUnitMeasure = value; }
		}

		/// <summary>
		/// Пересчет коэффициента преобразования реальных ед. в еденицы отображения
		/// </summary>
		protected void RatioReCalc()
		{
			_ratio = (base.Maximum - base.Minimum) / (_maximum - _minimum);
		}

		public float ValueF
		{
			get { return base.Value / _ratio; }
			set { base.Value = (int)(value * _ratio); }
		}

		public float MaximumF
		{
			get
			{
				return _maximum;
			}
			set
			{
				_maximum = value;
				RatioReCalc();
			}
		}

		public float MinimumF
		{
			get
			{
				return _minimum;
			}
			set
			{
				_minimum = value;
				RatioReCalc();
			}
		}

		[ReadOnly(true), Browsable(false), DefaultValue(100)]
		public override int Maximum
		{
			get
			{
				return base.Maximum;
			}
			set
			{
				base.Maximum = value;
			}
		}

		[ReadOnly(true), Browsable(false), DefaultValue(0)]
		public override int Minimum
		{
			get
			{
				return base.Minimum;
			}
			set
			{
				base.Minimum = value;
			}
		}

		[ReadOnly(true), Browsable(false), DefaultValue(100)]
		public override int Value
		{
			get
			{
				return base.Value;
			}
			set
			{
				base.Value = value;
			}
		}
	}
}
