namespace ColorBarNS
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.Windows.Forms;

	//using Microsoft.VisualBasic.CompilerServices;

	//[DesignerGenerated]
	public class ColorBar : UserControl
	{
		private const int BorderWidth = 2;
		private IContainer components;
		private List<SolidBrush> _lstBrushes;
		private List<Color> _lstColorListDefault;
		private List<Color> _colorList;
		private float m_HeightThickness;
		private int _maxValue;
		private int _minValue;
		private enumOrientation _orientation;
		private bool _reversed = false;
		private int _smoothness;
		private BarStyle _style;
		/// <summary>
		/// Current value progress bar
		/// </summary>
		private int _value;
		private float m_WidthThickness;
		public const int MaxSmoothness = 7;
		public const float MaxThickness = 0.5f;
		public const int MinSmoothness = 0;
		public const float MinThickness = 0.1f;

		public ColorBar ()
		{
			this.InitializeComponent ();
			this.BorderStyle = BorderStyle.Fixed3D;
			this.SetStyle (
				ControlStyles.DoubleBuffer |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.Opaque |
				ControlStyles.UserPaint, true);
			this.UpdateStyles ();

			this._lstColorListDefault = new List<Color> (
				new Color[]{
				Color.Red,
				Color.Orange,
				Color.Yellow,
				Color.Green,
				Color.Cyan,
				Color.Blue,
				Color.Indigo,
				Color.Violet,
			});

			this.Minimum = 0;
			this.Maximum = 100;
			this.Smoothness = 4;
			this.Value = this.Maximum;
			this.Style = BarStyle.Flow;
			this.Orientation = enumOrientation.Horizontal;
			this.Reversed = false;
			this.WidthThickness = 0.2f;
			this.HeightThickness = 0.2f;
		}

		private void BuildColorList (List<Color> lstAdd)
		{
			List<Color> list = new List<Color> (lstAdd);
			_lstBrushes = new List<SolidBrush> ();
			for (int s = 0; s <= _smoothness; s++) {
				for (int i = list.Count - 1, c = 0; c < i; i++, c += 2) {
					list.Insert (c + 1, this.InterpolateColors (list [c], list [c + 1]));
				}
			}
			foreach (Color i in list)
				_lstBrushes.Add (new SolidBrush (i));
		}

		private Color InterpolateColors (Color c1, Color c2)
		{
			return Color.FromArgb (
			//(int)Math.Round((double)(((double)(c1.A + c2.A)) / 2.0)),
				(int)Math.Round ((double)(((double)(c1.R + c2.R)) / 2.0)),
				(int)Math.Round ((double)(((double)(c1.G + c2.G)) / 2.0)),
				(int)Math.Round ((double)(((double)(c1.B + c2.B)) / 2.0)));
		}

		protected override void Dispose (bool disposing)
		{
			try {
				if (disposing && (this.components != null)) {
					this.components.Dispose ();
				}
			} finally {
				base.Dispose (disposing);
			}
		}

		private void InitializeComponent ()
		{
			this.SuspendLayout ();
			SizeF ef = new SizeF (6f, 13f);
			this.AutoScaleDimensions = ef;
			this.AutoScaleMode = AutoScaleMode.Font;
			this.Name = "ColorBar";
			Size size = new Size (200, 0x19);
			this.Size = size;
			this.ResumeLayout (false);
		}

		internal float GetValueFloat ()
		{
			return (float)(
				((double)(_value - _minValue)) /
				((double)(_maxValue - _minValue))
			);
		}

		protected void DrawProgressBar (Graphics g)
		{
			// Background paint
			g.FillRectangle (new SolidBrush (this.BackColor), this.ClientRectangle);

			float valueF = GetValueFloat ();

			if (valueF > 0f) {
				float colorCount = 0;
				float lengthBar;

				if (valueF > 1f)
					valueF = 1f;

				if (this._orientation == enumOrientation.Horizontal)
					lengthBar = base.ClientRectangle.Width - 2;
				else
					lengthBar = base.ClientRectangle.Height - 2;

				float currentValueLengthF = lengthBar * valueF;

				if (_style == BarStyle.Expand)
					colorCount = currentValueLengthF;
				else if ((_style == BarStyle.Flow) | (_style == BarStyle.Block))
					colorCount = lengthBar;

				//count of color zones on bar
				colorCount /= (float)this._lstBrushes.Count;
				const float y = 1f;
				//int num5 = 0;

				switch (_orientation) {
				case enumOrientation.Horizontal:
					{
						float h = base.ClientRectangle.Height - 2;
						float stepValue = colorCount;
						float i;
						int lstB = 0;
						for (i = y; i <= currentValueLengthF; i += stepValue) {
							g.FillRectangle (_lstBrushes [lstB], i, y, colorCount, h);
							//Paint block rectangles
							if ((colorCount > 4f) && (this.Style == BarStyle.Block)) {
								Rectangle clientRectangle =
										new Rectangle ((int)Math.Round ((double)i),
										(int)Math.Round ((double)y),
										(int)Math.Round ((double)colorCount),
										(int)Math.Round ((double)h));
								ControlPaint.DrawBorder (
										g,
										clientRectangle,
										Color.Gray,
										ButtonBorderStyle.Outset);
							}
							if (lstB < this._lstBrushes.Count)
								lstB++;
						}
						if ((i < (this.ClientRectangle.Width - y) && (valueF == 1.0))
								&& (lstB < this._lstBrushes.Count)
							) {
							g.FillRectangle (
									this._lstBrushes [lstB],
									i,
									y,
									(this.ClientRectangle.Width - y) - i,
									h);
						}
						return;
					}
					#region Vertical
				case enumOrientation.Vertical:
					{
						float w = this.ClientRectangle.Width - 2;
						float num29 = colorCount;
						float num28 = currentValueLengthF;
						float num8 = y;
						int lstB = 0;
						while ((num29 >= 0f) ? (num8 <= num28) : (num8 >= num28)) {
							g.FillRectangle (this._lstBrushes [lstB], y, (this.ClientRectangle.Bottom - colorCount) - num8, w, colorCount);
							if ((colorCount > 4f) & (this.Style == BarStyle.Block)) {
								Rectangle clientRectangle = new Rectangle ((int)Math.Round ((double)y), (int)Math.Round ((double)((this.ClientRectangle.Bottom - colorCount) - num8)), (int)Math.Round ((double)w), (int)Math.Round ((double)colorCount));
								ControlPaint.DrawBorder (g, clientRectangle, Color.Gray, ButtonBorderStyle.Outset);
							}
							if (lstB < this._lstBrushes.Count) {
								lstB++;
							}
							num8 += num29;
						}
						if (((num8 < (this.ClientRectangle.Top - y)) & (valueF == 1.0)) && (lstB < this._lstBrushes.Count)) {
							g.FillRectangle (this._lstBrushes [lstB], y, num8, w, num8 - (this.ClientRectangle.Top - y));
						}
						return;
					}
					#endregion
					#region Circular
				case enumOrientation.Circular:
					{
						float num9;
						float num10 = 0;
						float num11;
						float num12;
						float num13;
						PointF[] tfArray;
						float num14;
						float num15;
						float num16;
						float num17;

						int lstB = 0;
						num11 = (float)(((double)this.ClientRectangle.Width) / 2.0);
						num12 = (float)(((double)this.ClientRectangle.Height) / 2.0);
						num14 = (float)(((double)this.ClientRectangle.Width) / 2.0);
						num15 = ((float)(((double)this.ClientRectangle.Width) / 2.0))
								- (num14 * this.m_WidthThickness);
						num16 = (float)(((double)this.ClientRectangle.Height) / 2.0);
						num17 = ((float)(((double)this.ClientRectangle.Height) / 2.0))
								- (num16 * this.m_HeightThickness);
						num9 = 0f;
						num13 = 360f * valueF;
						tfArray = new PointF[4];
						if (this._style != BarStyle.Expand) {
							if ((this._style == BarStyle.Flow) | (this._style == BarStyle.Block)) {
								num10 = 360f / ((float)this._lstBrushes.Count);
							}
							break;
						}
						num10 = (360f * valueF) / ((float)this._lstBrushes.Count);
						float num18 = ((float)(num14 * Math.Sin ((double)(0.01745329f * num9)))) + num11;
						float num22 = ((float)(num16 * Math.Cos ((double)(0.01745329f * num9)))) + num12;
						float num19 = ((float)(num15 * Math.Sin ((double)(0.01745329f * num9)))) + num11;
						float num23 = ((float)(num17 * Math.Cos ((double)(0.01745329f * num9)))) + num12;
						do {
							num9 += num10;
							float num20 = ((float)(num14 * Math.Sin ((double)(0.01745329f * num9)))) + num11;
							float num24 = ((float)(num16 * Math.Cos ((double)(0.01745329f * num9)))) + num12;
							float num21 = ((float)(num15 * Math.Sin ((double)(0.01745329f * num9)))) + num11;
							float num25 = ((float)(num17 * Math.Cos ((double)(0.01745329f * num9)))) + num12;
							tfArray [0].X = num18;
							tfArray [0].Y = num22;
							tfArray [1].X = num20;
							tfArray [1].Y = num24;
							tfArray [2].X = num21;
							tfArray [2].Y = num25;
							tfArray [3].X = num19;
							tfArray [3].Y = num23;
							num18 = num20;
							num22 = num24;
							num19 = num21;
							num23 = num25;
							if (lstB < this._lstBrushes.Count) {
								g.FillPolygon (this._lstBrushes [lstB], tfArray);
								lstB++;
							}
						} while (num9 < num13);
					}
					return;
					#endregion
				default:
					return;
				}
			}
		}

		protected override void OnResize (EventArgs e)
		{
			base.OnResize (e);
			this.Invalidate (false);
		}

		private void Swap (ref int val1, ref int val2)
		{
			int num = val1;
			val1 = val2;
			val2 = num;
		}

		protected override void WndProc (ref Message m)
		{
			if (m.Msg != 20) {
				base.WndProc (ref m);
			}
		}

		public void SetColorListGradient (Color c)
		{
			SetColorList (GetDarkToLightColor (c));
		}

		public void SetColorList (List<Color> list)
		{
			_colorList = list;

			if (_colorList == null || _colorList.Count < 2) {
				this.BuildColorList (_lstColorListDefault);
			} else {
				this.BuildColorList (_colorList);
			}
			this.Invalidate (false);
		}

		private List<Color> GetDarkToLightColor (Color c)
		{
			return new List<Color> (new Color[]
			
			{
				Color.FromArgb (c.R / 2, c.G / 2, c.B / 2),
				c,				
			});
		}
		
		public virtual int Maximum {
			get {
				return this._maxValue;
			}
			set {
				this._maxValue = value;
				if (this._minValue > this._maxValue) {
					this.Swap (ref this._minValue, ref this._maxValue);
				}
				if (this._value > this._maxValue) {
					this._value = this._maxValue;
				}
				this.Invalidate (false);
			}
		}

		public virtual int Minimum {
			get {
				return this._minValue;
			}
			set {
				this._minValue = value;
				if (this._minValue > this._maxValue) {
					this.Swap (ref this._minValue, ref this._maxValue);
				}
				if (this._value < this._minValue) {
					this._value = this._minValue;
				}
				this.Invalidate (false);
			}
		}

		public enumOrientation Orientation {
			get {
				return this._orientation;
			}
			set {
				this._orientation = value;
				this.Invalidate (false);
			}
		}

		public bool Reversed {
			get {
				return this._reversed;
			}
			set {
				if (value != this._reversed) {
					this._reversed = value;
					this._lstBrushes.Reverse ();
					this.Invalidate (false);
				}
			}
		}

		[Description("Smoothness. Min:1 Max:7"), DefaultValue(4)]
		public int Smoothness {
			get {
				return this._smoothness;
			}
			set {
				if (value < 0) {
					value = 0;
				}
				if (value > 7) {
					value = 7;
				}
				this._smoothness = value;
				if (this._colorList != null) {
					this.BuildColorList (this._colorList);
				} else {
					this.BuildColorList (this._lstColorListDefault);
				}
				this.Invalidate (false);
			}
		}

		public BarStyle Style {
			get {
				return this._style;
			}
			set {
				this._style = value;
				this.Invalidate (false);
			}
		}

		public virtual int Value {
			get {
				return this._value;
			}
			set {
				this._value = value;
				if (this._value < this._minValue) {
					this._value = this._minValue;
				}
				if (this._value > this._maxValue) {
					this._value = this._maxValue;
				}
				this.Invalidate (false);
			}
		}

		public float WidthThickness {
			get {
				return this.m_WidthThickness;
			}
			set {
				if (value != this.m_WidthThickness) {
					if (value < 0.1f) {
						value = 0.1f;
					}
					if (value > 0.5f) {
						value = 0.5f;
					}
					this.m_WidthThickness = value;
					this.Invalidate (false);
				}
			}
		}

		public float HeightThickness {
			get {
				return this.m_HeightThickness;
			}
			set {
				if (value != this.m_HeightThickness) {
					if (value < 0.1f) {
						value = 0.1f;
					}
					if (value > 0.5f) {
						value = 0.5f;
					}
					this.m_HeightThickness = value;
					this.Invalidate (false);
				}
			}
		}

		public enum BarStyle
		{
			Flow,
			Expand,
			Block
		}

		public enum enumOrientation
		{
			Horizontal,
			Vertical,
			Circular
		}
	}
}

