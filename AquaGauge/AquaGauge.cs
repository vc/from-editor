namespace AquaControls
{
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.Globalization;
	using System.Windows.Forms;

	public class AquaGauge : UserControl
	{
		private Image backgroundImg;
		private IContainer components = null;
		private float currentValue;
		private int decimalPlaces;
		private int dialAlpha = 0xff;
		private Color dialBorderColor = Color.SlateGray;
		private Color dialColor = Color.MidnightBlue;
		private string dialText;
		private Color dialTextColor = Color.Gold;
		private Font dialTextFont;
		private int dialTextVOffset = 0;
		private float digitalValue;
		private int digitalValueBackAlpha = 1;
		private Color digitalValueBackColor = Color.White;
		private Color digitalValueColor = Color.Orange;
		private int digitalValueDecimalPlaces;
		private bool digitalValueVisible;
		private bool enableTransparentBackground;
		private float fromAngle = 135f;
		private float glossinessAlpha = 40f;
		private int height;
		private float maxValue;
		private float minValue;
		private int noOfDivisions;
		private int noOfSubDivisions;
		private int oldHeight;
		private int oldWidth;
		private Color pointerColor = Color.Black;
		private Rectangle rectImg;
		private bool requiresRedraw;
		private int rimAlpha = 0xff;
		private Color rimColor = Color.Gold;
		private Color scaleColor = Color.Gold;
		private int scaleFontSizeDivider = 0x16;
		private Color threshold1Color = Color.LawnGreen;
		private float threshold1Start = 0f;
		private float threshold1Stop = 0f;
		private Color threshold2Color = Color.Red;
		private float threshold2Start = 0f;
		private float threshold2Stop = 0f;
		private float toAngle = 405f;
		private bool valueToDigital;
		private int width;
		private int x;
		private int y;

		public AquaGauge()
		{
			this.InitializeComponent();
			this.x = 5;
			this.y = 5;
			base.Width = 200;
			base.Height = 200;
			this.width = base.Width - 10;
			this.height = base.Height - 10;
			this.digitalValueVisible = true;
			this.noOfDivisions = 10;
			this.noOfSubDivisions = 3;
			this.maxValue = 100f;
			this.dialTextFont = this.Font;
			this.decimalPlaces = 0;
			this.digitalValueDecimalPlaces = 0;
			this.ValueToDigital = true;
			base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			base.SetStyle(ControlStyles.ResizeRedraw, true);
			base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			base.SetStyle(ControlStyles.UserPaint, true);
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.BackColor = Color.Transparent;
			base.Resize += new EventHandler(this.AquaGauge_Resize);
			this.requiresRedraw = true;
		}

		private void AquaGauge_Resize(object sender, EventArgs e)
		{
			if (base.Width < 0x88)
			{
				base.Width = 0x88;
			}
			if (this.oldWidth != base.Width)
			{
				base.Height = base.Width;
				this.oldHeight = base.Width;
			}
			if (this.oldHeight != base.Height)
			{
				base.Width = base.Height;
				this.oldWidth = base.Width;
			}
			this.requiresRedraw = true;
			base.Invalidate();
		}

		public string DecimalFormat(int dp)
		{
			switch (dp)
			{
				case 1:
					return "0000.0";

				case 2:
					return "000.00";

				case 3:
					return "00.000";

				case 4:
					return "0.0000";
			}
			return "00000";
		}

		private void DisplayNumber(Graphics g, float number, RectangleF drect)
		{
			try
			{
				int dp = 0;
				if (this.ValueToDigital)
				{
					dp = this.decimalPlaces;
				}
				else
				{
					dp = this.digitalValueDecimalPlaces;
				}
				string str = number.ToString(this.DecimalFormat(dp), CultureInfo.InvariantCulture);
				str.PadLeft(5 - dp, '0');
				float num2 = 0f;
				if (number < 0f)
				{
					num2 -= this.width / 0x11;
				}
				bool flag = false;
				char[] chArray = str.ToCharArray();
				for (int i = 0; i < chArray.Length; i++)
				{
					char ch = chArray[i];
					if ((i < (chArray.Length - 1)) && ((chArray[i + 1] == '.')))
					{
						flag = true;
					}
					else
					{
						flag = false;
					}
					switch (ch)
					{
						case '.':
							break;

						default:
							{
								if (ch == '-')
								{
									this.DrawDigit(g, -1, new PointF(drect.X + num2, drect.Y), flag, drect.Height);
								}
								else
								{
									this.DrawDigit(g, short.Parse(ch.ToString()), new PointF(drect.X + num2, drect.Y), flag, drect.Height);
								}
								num2 += (15 * this.width) / 250;
								continue;
							}
					}
					num2 += (2 * this.width) / 250;
				}
			}
			catch (Exception)
			{
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && (this.components != null))
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void DrawCalibration(Graphics g, Rectangle rect, int cX, int cY)
		{
			int num = this.noOfDivisions + 1;
			int noOfSubDivisions = this.noOfSubDivisions;
			float radian = this.GetRadian(this.fromAngle);
			int num4 = (int)(base.Width * 0.01f);
			float num5 = base.Width / 0x19;
			Rectangle rectangle = new Rectangle(rect.Left + num4, rect.Top + num4, rect.Width - num4, rect.Height - num4);
			float num12 = (rectangle.Width / 2) - (num4 * 5);
			float num13 = this.toAngle - this.fromAngle;
			float num14 = this.GetRadian(num13 / ((float)((num - 1) * (noOfSubDivisions + 1))));
			Pen pen = new Pen(this.scaleColor, (float)(base.Width / 50));
			Pen pen2 = new Pen(this.scaleColor, (float)(base.Width / 100));
			float minValue = this.MinValue;
			for (int i = 0; i <= num; i++)
			{
				float num6 = (float)(cX + (num12 * Math.Cos((double)radian)));
				float num7 = (float)(cY + (num12 * Math.Sin((double)radian)));
				float num8 = (float)(cX + ((num12 - (base.Width / 20)) * Math.Cos((double)radian)));
				float num9 = (float)(cY + ((num12 - (base.Width / 20)) * Math.Sin((double)radian)));
				g.DrawLine(pen, num6, num7, num8, num9);
				string str = "N0";
				switch (this.decimalPlaces)
				{
					case 1:
						str = "N1";
						break;

					case 2:
						str = "N2";
						break;
				}
				int num17 = 10;
				if (this.decimalPlaces > 0)
				{
					num17 = 8;
				}
				float x = (float)(cX + ((num12 - (base.Width / num17)) * Math.Cos((double)radian)));
				float y = (cY - num5) + ((float)((num12 - (base.Width / 10)) * Math.Sin((double)radian)));
				Brush brush = new SolidBrush(this.scaleColor);
				StringFormat format = new StringFormat(StringFormatFlags.NoClip);
				format.Alignment = StringAlignment.Center;
				Font font = new Font(this.Font.FontFamily, (float)(base.Width / this.scaleFontSizeDivider), this.Font.Style);
				g.DrawString(minValue.ToString(str) + "", font, brush, new PointF(x, y), format);
				minValue += (this.MaxValue - this.MinValue) / ((float)(num - 1));
				minValue = (float)Math.Round((double)minValue, 2);
				if (i == (num - 1))
				{
					break;
				}
				for (int j = 0; j <= noOfSubDivisions; j++)
				{
					radian += num14;
					num6 = (float)(cX + (num12 * Math.Cos((double)radian)));
					num7 = (float)(cY + (num12 * Math.Sin((double)radian)));
					num8 = (float)(cX + ((num12 - (base.Width / 50)) * Math.Cos((double)radian)));
					num9 = (float)(cY + ((num12 - (base.Width / 50)) * Math.Sin((double)radian)));
					g.DrawLine(pen2, num6, num7, num8, num9);
				}
			}
		}

		private void DrawCenterPoint(Graphics g, Rectangle rect, int cX, int cY)
		{
			float width = base.Width / 5;
			RectangleF ef = new RectangleF(cX - (width / 2f), cY - (width / 2f), width, width);
			LinearGradientBrush brush = new LinearGradientBrush(rect, this.pointerColor, Color.FromArgb(100, this.dialColor), LinearGradientMode.Vertical);
			g.FillEllipse(brush, ef);
			width = base.Width / 7;
			ef = new RectangleF(cX - (width / 2f), cY - (width / 2f), width, width);
			brush = new LinearGradientBrush(rect, Color.SlateGray, this.pointerColor, LinearGradientMode.ForwardDiagonal);
			g.FillEllipse(brush, ef);
		}

		private void DrawDigit(Graphics g, int number, PointF position, bool dp, float height)
		{
			float width = (10f * height) / 13f;
			Pen pen = new Pen(Color.FromArgb(20, this.dialColor));
			Pen pen2 = new Pen(this.digitalValueColor);
			PointF[] points = new PointF[5];
			points[0] = points[4] = new PointF(position.X + this.GetX(2.8f, width), position.Y + this.GetY(1f, height));
			points[1] = new PointF(position.X + this.GetX(10f, width), position.Y + this.GetY(1f, height));
			points[2] = new PointF(position.X + this.GetX(8.8f, width), position.Y + this.GetY(2f, height));
			points[3] = new PointF(position.X + this.GetX(3.8f, width), position.Y + this.GetY(2f, height));
			PointF[] tfArray2 = new PointF[5];
			tfArray2[0] = tfArray2[4] = new PointF(position.X + this.GetX(10f, width), position.Y + this.GetY(1.4f, height));
			tfArray2[1] = new PointF(position.X + this.GetX(9.3f, width), position.Y + this.GetY(6.8f, height));
			tfArray2[2] = new PointF(position.X + this.GetX(8.4f, width), position.Y + this.GetY(6.4f, height));
			tfArray2[3] = new PointF(position.X + this.GetX(9f, width), position.Y + this.GetY(2.2f, height));
			PointF[] tfArray3 = new PointF[5];
			tfArray3[0] = tfArray3[4] = new PointF(position.X + this.GetX(9.2f, width), position.Y + this.GetY(7.2f, height));
			tfArray3[1] = new PointF(position.X + this.GetX(8.7f, width), position.Y + this.GetY(12.7f, height));
			tfArray3[2] = new PointF(position.X + this.GetX(7.6f, width), position.Y + this.GetY(11.9f, height));
			tfArray3[3] = new PointF(position.X + this.GetX(8.2f, width), position.Y + this.GetY(7.7f, height));
			PointF[] tfArray4 = new PointF[5];
			tfArray4[0] = tfArray4[4] = new PointF(position.X + this.GetX(7.4f, width), position.Y + this.GetY(12.1f, height));
			tfArray4[1] = new PointF(position.X + this.GetX(8.4f, width), position.Y + this.GetY(13f, height));
			tfArray4[2] = new PointF(position.X + this.GetX(1.3f, width), position.Y + this.GetY(13f, height));
			tfArray4[3] = new PointF(position.X + this.GetX(2.2f, width), position.Y + this.GetY(12.1f, height));
			PointF[] tfArray5 = new PointF[5];
			tfArray5[0] = tfArray5[4] = new PointF(position.X + this.GetX(2.2f, width), position.Y + this.GetY(11.8f, height));
			tfArray5[1] = new PointF(position.X + this.GetX(1f, width), position.Y + this.GetY(12.7f, height));
			tfArray5[2] = new PointF(position.X + this.GetX(1.7f, width), position.Y + this.GetY(7.2f, height));
			tfArray5[3] = new PointF(position.X + this.GetX(2.8f, width), position.Y + this.GetY(7.7f, height));
			PointF[] tfArray6 = new PointF[5];
			tfArray6[0] = tfArray6[4] = new PointF(position.X + this.GetX(3f, width), position.Y + this.GetY(6.4f, height));
			tfArray6[1] = new PointF(position.X + this.GetX(1.8f, width), position.Y + this.GetY(6.8f, height));
			tfArray6[2] = new PointF(position.X + this.GetX(2.6f, width), position.Y + this.GetY(1.3f, height));
			tfArray6[3] = new PointF(position.X + this.GetX(3.6f, width), position.Y + this.GetY(2.2f, height));
			PointF[] tfArray7 = new PointF[7];
			tfArray7[0] = tfArray7[6] = new PointF(position.X + this.GetX(2f, width), position.Y + this.GetY(7f, height));
			tfArray7[1] = new PointF(position.X + this.GetX(3.1f, width), position.Y + this.GetY(6.5f, height));
			tfArray7[2] = new PointF(position.X + this.GetX(8.3f, width), position.Y + this.GetY(6.5f, height));
			tfArray7[3] = new PointF(position.X + this.GetX(9f, width), position.Y + this.GetY(7f, height));
			tfArray7[4] = new PointF(position.X + this.GetX(8.2f, width), position.Y + this.GetY(7.5f, height));
			tfArray7[5] = new PointF(position.X + this.GetX(2.9f, width), position.Y + this.GetY(7.5f, height));
			g.FillPolygon(pen.Brush, points);
			g.FillPolygon(pen.Brush, tfArray2);
			g.FillPolygon(pen.Brush, tfArray3);
			g.FillPolygon(pen.Brush, tfArray4);
			g.FillPolygon(pen.Brush, tfArray5);
			g.FillPolygon(pen.Brush, tfArray6);
			g.FillPolygon(pen.Brush, tfArray7);
			int[] listOfNumbers = new int[8];
			listOfNumbers[1] = 2;
			listOfNumbers[2] = 3;
			listOfNumbers[3] = 5;
			listOfNumbers[4] = 6;
			listOfNumbers[5] = 7;
			listOfNumbers[6] = 8;
			listOfNumbers[7] = 9;
			if (this.IsNumberAvailable(number, listOfNumbers))
			{
				g.FillPolygon(pen2.Brush, points);
			}
			listOfNumbers = new int[8];
			listOfNumbers[1] = 1;
			listOfNumbers[2] = 2;
			listOfNumbers[3] = 3;
			listOfNumbers[4] = 4;
			listOfNumbers[5] = 7;
			listOfNumbers[6] = 8;
			listOfNumbers[7] = 9;
			if (this.IsNumberAvailable(number, listOfNumbers))
			{
				g.FillPolygon(pen2.Brush, tfArray2);
			}
			listOfNumbers = new int[9];
			listOfNumbers[1] = 1;
			listOfNumbers[2] = 3;
			listOfNumbers[3] = 4;
			listOfNumbers[4] = 5;
			listOfNumbers[5] = 6;
			listOfNumbers[6] = 7;
			listOfNumbers[7] = 8;
			listOfNumbers[8] = 9;
			if (this.IsNumberAvailable(number, listOfNumbers))
			{
				g.FillPolygon(pen2.Brush, tfArray3);
			}
			listOfNumbers = new int[7];
			listOfNumbers[1] = 2;
			listOfNumbers[2] = 3;
			listOfNumbers[3] = 5;
			listOfNumbers[4] = 6;
			listOfNumbers[5] = 8;
			listOfNumbers[6] = 9;
			if (this.IsNumberAvailable(number, listOfNumbers))
			{
				g.FillPolygon(pen2.Brush, tfArray4);
			}
			listOfNumbers = new int[4];
			listOfNumbers[1] = 2;
			listOfNumbers[2] = 6;
			listOfNumbers[3] = 8;
			if (this.IsNumberAvailable(number, listOfNumbers))
			{
				g.FillPolygon(pen2.Brush, tfArray5);
			}
			listOfNumbers = new int[7];
			listOfNumbers[1] = 4;
			listOfNumbers[2] = 5;
			listOfNumbers[3] = 6;
			listOfNumbers[4] = 7;
			listOfNumbers[5] = 8;
			listOfNumbers[6] = 9;
			if (this.IsNumberAvailable(number, listOfNumbers))
			{
				g.FillPolygon(pen2.Brush, tfArray6);
			}
			if (this.IsNumberAvailable(number, new int[] { 2, 3, 4, 5, 6, 8, 9, -1 }))
			{
				g.FillPolygon(pen2.Brush, tfArray7);
			}
			if (dp)
			{
				g.FillEllipse(pen2.Brush, new RectangleF(position.X + this.GetX(10f, width), position.Y + this.GetY(12f, height), width / 7f, width / 7f));
			}
		}

		private void DrawDigitalValue(Graphics gr)
		{
			RectangleF rect = new RectangleF((((float)base.Width) / 2f) - (((float)this.width) / 5f), (((float)this.height) / 1.2f) - 6f, ((float)this.width) / 2.5f, (((float)base.Height) / 9f) - 2f);
			RectangleF drect = new RectangleF((float)((base.Width / 2) - (this.width / 7)), (float)(((int)(((double)this.height) / 1.18)) - 6), (float)(this.width / 4), (float)((base.Height / 12) - 2));
			gr.FillRectangle(new SolidBrush(Color.FromArgb(this.digitalValueBackAlpha, this.digitalValueBackColor)), rect);
			if (this.valueToDigital)
			{
				this.DisplayNumber(gr, this.currentValue, drect);
			}
			else
			{
				this.DisplayNumber(gr, this.digitalValue, drect);
			}
		}

		private void DrawGloss(Graphics g)
		{
			RectangleF rect = new RectangleF(this.x + ((float)(this.width * 0.1)), this.y + ((float)(this.height * 0.07)), (float)(this.width * 0.8), (float)(this.height * 0.7));
			LinearGradientBrush brush = new LinearGradientBrush(rect, Color.FromArgb((int)this.glossinessAlpha, Color.White), Color.Transparent, LinearGradientMode.Vertical);
			g.FillEllipse(brush, rect);
			rect = new RectangleF(this.x + ((float)(this.width * 0.25)), (this.y + ((float)(this.height * 0.77))) - 5f, (float)(this.width * 0.5), ((float)(this.height * 0.2)) - 2f);
			int alpha = (int)(this.glossinessAlpha / 3f);
			brush = new LinearGradientBrush(rect, Color.Transparent, Color.FromArgb(alpha, this.BackColor), LinearGradientMode.Vertical);
			g.FillEllipse(brush, rect);
		}

		private void DrawPointer(Graphics gr, int cx, int cy)
		{
			float num = (base.Width / 2) - (base.Width * 0.12f);
			float theta = this.MaxValue - this.MinValue;
			Image image = new Bitmap(base.Width, base.Height);
			Graphics g = Graphics.FromImage(image);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			theta = (100f * (this.currentValue - this.MinValue)) / theta;
			theta = ((this.toAngle - this.fromAngle) * theta) / 100f;
			theta += this.fromAngle;
			float radian = this.GetRadian(theta);
			float num4 = radian;
			PointF[] points = new PointF[5];
			points[0].X = (float)(cx + (num * Math.Cos((double)radian)));
			points[0].Y = (float)(cy + (num * Math.Sin((double)radian)));
			points[4].X = (float)(cx + (num * Math.Cos(radian - 0.02)));
			points[4].Y = (float)(cy + (num * Math.Sin(radian - 0.02)));
			radian = this.GetRadian(theta + 20f);
			points[1].X = (float)(cx + ((base.Width * 0.09f) * Math.Cos((double)radian)));
			points[1].Y = (float)(cy + ((base.Width * 0.09f) * Math.Sin((double)radian)));
			points[2].X = cx;
			points[2].Y = cy;
			radian = this.GetRadian(theta - 20f);
			points[3].X = (float)(cx + ((base.Width * 0.09f) * Math.Cos((double)radian)));
			points[3].Y = (float)(cy + ((base.Width * 0.09f) * Math.Sin((double)radian)));
			Brush brush = new SolidBrush(this.pointerColor);
			g.FillPolygon(brush, points);
			PointF[] tfArray2 = new PointF[3];
			radian = this.GetRadian(theta);
			tfArray2[0].X = (float)(cx + (num * Math.Cos((double)radian)));
			tfArray2[0].Y = (float)(cy + (num * Math.Sin((double)radian)));
			radian = this.GetRadian(theta + 20f);
			tfArray2[1].X = (float)(cx + ((base.Width * 0.09f) * Math.Cos((double)radian)));
			tfArray2[1].Y = (float)(cy + ((base.Width * 0.09f) * Math.Sin((double)radian)));
			tfArray2[2].X = cx;
			tfArray2[2].Y = cy;
			LinearGradientBrush brush2 = new LinearGradientBrush(tfArray2[0], tfArray2[2], Color.SlateGray, this.pointerColor);
			g.FillPolygon(brush2, tfArray2);
			Rectangle rect = new Rectangle(this.x, this.y, this.width, this.height);
			this.DrawCenterPoint(g, rect, (this.width / 2) + this.x, (this.height / 2) + this.y);
			this.DrawGloss(g);
			gr.DrawImage(image, 0, 0);
		}

		public float GetRadian(float theta)
		{
			return ((theta * 3.141593f) / 180f);
		}

		private float GetX(float x, float width)
		{
			return ((x * width) / 12f);
		}

		private float GetY(float y, float height)
		{
			return ((y * height) / 15f);
		}

		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Name = "AquaGauge";
			base.ResumeLayout(false);
		}

		private bool IsNumberAvailable(int number, params int[] listOfNumbers)
		{
			if (listOfNumbers.Length > 0)
			{
				foreach (int num in listOfNumbers)
				{
					if (num == number)
					{
						return true;
					}
				}
			}
			return false;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			this.width = base.Width - (this.x * 2);
			this.height = base.Height - (this.y * 2);
			this.DrawPointer(e.Graphics, (this.width / 2) + this.x, (this.height / 2) + this.y);
			if (this.digitalValueVisible)
			{
				this.DrawDigitalValue(e.Graphics);
			}
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if (!this.enableTransparentBackground)
			{
				base.OnPaintBackground(e);
			}
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			e.Graphics.FillRectangle(new SolidBrush(Color.Transparent), new Rectangle(0, 0, base.Width, base.Height));
			if ((this.backgroundImg == null) || this.requiresRedraw)
			{
				float num4;
				float num5;
				this.backgroundImg = new Bitmap(base.Width, base.Height);
				Graphics g = Graphics.FromImage(this.backgroundImg);
				g.SmoothingMode = SmoothingMode.HighQuality;
				this.width = base.Width - (this.x * 2);
				this.height = base.Height - (this.y * 2);
				this.rectImg = new Rectangle(this.x, this.y, this.width, this.height);
				Brush brush = new SolidBrush(Color.FromArgb(this.dialAlpha, this.dialColor));
				if (this.enableTransparentBackground && (base.Parent != null))
				{
					float num = this.width / 60;
				}
				g.FillEllipse(brush, this.x, this.y, this.width, this.height);
				SolidBrush brush2 = new SolidBrush(Color.FromArgb(100, this.dialBorderColor));
				Pen pen = new Pen(brush2, (float)(this.width * 0.03));
				g.DrawEllipse(pen, this.rectImg);
				Pen pen2 = new Pen(this.dialBorderColor);
				g.DrawEllipse(pen2, this.x, this.y, this.width, this.height);
				this.DrawCalibration(g, this.rectImg, (this.width / 2) + this.x, (this.height / 2) + this.y);
				Pen pen3 = new Pen(Color.FromArgb(this.rimAlpha, this.rimColor), (float)(base.Width / 40));
				int num2 = (int)(base.Width * 0.03f);
				Rectangle rect = new Rectangle(this.rectImg.X + num2, this.rectImg.Y + num2, this.rectImg.Width - (num2 * 2), this.rectImg.Height - (num2 * 2));
				g.DrawArc(pen3, rect, 134f, 272f);
				float num3 = (this.toAngle - this.fromAngle) / (this.MaxValue - this.MinValue);
				if ((this.threshold2Stop - this.threshold2Start) > 0f)
				{
					pen3 = new Pen(Color.FromArgb(0xff, this.threshold2Color), (float)(base.Width / 40));
					rect = new Rectangle(this.rectImg.X + num2, this.rectImg.Y + num2, this.rectImg.Width - (num2 * 2), this.rectImg.Height - (num2 * 2));
					num4 = 135f + ((this.threshold2Start - this.minValue) * num3);
					num5 = (this.threshold2Stop - this.threshold2Start) * num3;
					if ((num4 + num5) > 405f)
					{
						num5 = 405f - num4;
					}
					g.DrawArc(pen3, rect, num4, num5);
				}
				if ((this.threshold1Stop - this.threshold1Start) > 0f)
				{
					pen3 = new Pen(Color.FromArgb(0xff, this.threshold1Color), (float)(base.Width / 40));
					rect = new Rectangle(this.rectImg.X + num2, this.rectImg.Y + num2, this.rectImg.Width - (num2 * 2), this.rectImg.Height - (num2 * 2));
					num4 = 135f + ((this.threshold1Start - this.minValue) * num3);
					num5 = (this.threshold1Stop - this.threshold1Start) * num3;
					if ((num4 + num5) > 405f)
					{
						num5 = 405f - num4;
					}
					g.DrawArc(pen3, rect, num4, num5);
				}
				if (this.DialText != "")
				{
					StringFormat format = new StringFormat();
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Center;
					SizeF ef = g.MeasureString(this.dialText, this.dialTextFont);
					RectangleF layoutRectangle = new RectangleF((base.Width / 2) - (ef.Width / 2f), (float)(((int)(((double)this.height) / 1.45)) + this.dialTextVOffset), ef.Width, ef.Height + 1f);
					g.DrawString(this.dialText, this.dialTextFont, new SolidBrush(this.dialTextColor), layoutRectangle, format);
				}
				this.requiresRedraw = false;
			}
			e.Graphics.DrawImage(this.backgroundImg, this.rectImg);
		}

		protected override System.Windows.Forms.CreateParams CreateParams
		{
			get
			{
				System.Windows.Forms.CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 0x20;
				return createParams;
			}
		}

		[Description("Decimal places for scale (and digital value if ValueToDigital=TRUE) (0-2)"), DefaultValue("0")]
		public int DecimalPlaces
		{
			get
			{
				return this.decimalPlaces;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				else if (value > 2)
				{
					value = 2;
				}
				this.decimalPlaces = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("Opacity of gauge background (0-255)"), DefaultValue("255")]
		public int DialAlpha
		{
			get
			{
				return this.dialAlpha;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				else if (value > 0xff)
				{
					value = 0xff;
				}
				this.dialAlpha = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("Dial Border Color")]
		public Color DialBorderColor
		{
			get
			{
				return this.dialBorderColor;
			}
			set
			{
				this.dialBorderColor = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("Background color of the dial")]
		public Color DialColor
		{
			get
			{
				return this.dialColor;
			}
			set
			{
				this.dialColor = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("Gets or Sets the Text to be displayed in the dial")]
		public string DialText
		{
			get
			{
				return this.dialText;
			}
			set
			{
				this.dialText = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("Dial Text Color")]
		public Color DialTextColor
		{
			get
			{
				return this.dialTextColor;
			}
			set
			{
				this.dialTextColor = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("Dial text font")]
		public Font DialTextFont
		{
			get
			{
				return this.dialTextFont;
			}
			set
			{
				this.dialTextFont = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("Vertical offset for dial text (text up if less than 0, text down if bigger than 0)"), DefaultValue("0")]
		public int DialTextVOffset
		{
			get
			{
				return this.dialTextVOffset;
			}
			set
			{
				this.dialTextVOffset = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[DefaultValue("0"), Description("Value used for DigitalValue if ValueToDigital is FALSE")]
		public float DigitalValue
		{
			get
			{
				return this.digitalValue;
			}
			set
			{
				this.digitalValue = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[DefaultValue("50"), Description("Opacity of digital value backcolor (0-255)")]
		public int DigitalValueBackAlpha
		{
			get
			{
				return this.digitalValueBackAlpha;
			}
			set
			{
				if (value > 0xff)
				{
					value = 0xff;
				}
				else if (value < 0)
				{
					value = 0;
				}
				this.digitalValueBackAlpha = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("Digital Value Background color")]
		public Color DigitalValueBackColor
		{
			get
			{
				return this.digitalValueBackColor;
			}
			set
			{
				this.digitalValueBackColor = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("Digital Value Color")]
		public Color DigitalValueColor
		{
			get
			{
				return this.digitalValueColor;
			}
			set
			{
				this.digitalValueColor = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("Set the decimal places used for digital values if ValueToDigital=FALSE. (0-2)"), DefaultValue("0")]
		public int DigitalValueDecimalPlaces
		{
			get
			{
				return this.digitalValueDecimalPlaces;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				else if (value > 2)
				{
					value = 2;
				}
				this.digitalValueDecimalPlaces = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[DefaultValue(true), Description("Show/Hide digital value")]
		public bool DigitalValueVisible
		{
			get
			{
				return this.digitalValueVisible;
			}
			set
			{
				this.digitalValueVisible = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[DefaultValue(false), Description("Enables or Disables Transparent Background color. Note: Enabling this will reduce the performance and may make the control flicker.")]
		public bool EnableTransparentBackground
		{
			get
			{
				return this.enableTransparentBackground;
			}
			set
			{
				this.enableTransparentBackground = value;
				base.SetStyle(ControlStyles.OptimizedDoubleBuffer, !this.enableTransparentBackground);
				this.requiresRedraw = true;
				this.Refresh();
			}
		}

		[DefaultValue(40), Description("Glossiness strength. Range: 0-255")]
		public float Glossiness
		{
			get
			{
				return this.glossinessAlpha;
			}
			set
			{
				if (value < 0f)
				{
					value = 0f;
				}
				else if (value > 255f)
				{
					value = 255f;
				}
				this.glossinessAlpha = value;
				this.Refresh();
			}
		}

		[DefaultValue(100), Description("Maximum value on the scale")]
		public float MaxValue
		{
			get
			{
				return this.maxValue;
			}
			set
			{
				if (value > this.minValue)
				{
					this.maxValue = value;
					if (this.currentValue > this.maxValue)
					{
						this.currentValue = this.maxValue;
					}
					if (this.threshold1Start > this.maxValue)
					{
						this.threshold1Start = this.maxValue;
					}
					if (this.threshold1Stop > this.maxValue)
					{
						this.threshold1Stop = this.maxValue;
					}
					if (this.threshold2Start > this.maxValue)
					{
						this.threshold2Start = this.maxValue;
					}
					if (this.threshold2Stop > this.maxValue)
					{
						this.threshold2Stop = this.maxValue;
					}
					this.requiresRedraw = true;
					base.Invalidate();
				}
			}
		}

		[Description("Mininum value on the scale"), DefaultValue(0)]
		public float MinValue
		{
			get
			{
				return this.minValue;
			}
			set
			{
				if (value < this.maxValue)
				{
					this.minValue = value;
					if (this.currentValue < this.minValue)
					{
						this.currentValue = this.minValue;
					}
					if (this.threshold1Start < this.minValue)
					{
						this.threshold1Start = this.minValue;
					}
					if (this.threshold1Stop < this.minValue)
					{
						this.threshold1Stop = this.minValue;
					}
					if (this.threshold2Start < this.minValue)
					{
						this.threshold2Start = this.minValue;
					}
					if (this.threshold2Stop < this.minValue)
					{
						this.threshold2Stop = this.minValue;
					}
					this.requiresRedraw = true;
					base.Invalidate();
				}
			}
		}

		[Description("Get or Sets the number of Divisions in the dial scale."), DefaultValue(10)]
		public int NoOfDivisions
		{
			get
			{
				return this.noOfDivisions;
			}
			set
			{
				if ((value > 1) && (value < 0x19))
				{
					this.noOfDivisions = value;
					this.requiresRedraw = true;
					base.Invalidate();
				}
			}
		}

		[Description("Gets or Sets the number of Sub Divisions in the scale per Division."), DefaultValue(3)]
		public int NoOfSubDivisions
		{
			get
			{
				return this.noOfSubDivisions;
			}
			set
			{
				if ((value > 0) && (value <= 10))
				{
					this.noOfSubDivisions = value;
					this.requiresRedraw = true;
					base.Invalidate();
				}
			}
		}

		[Description("Pointer Color")]
		public Color PointerColor
		{
			get
			{
				return this.pointerColor;
			}
			set
			{
				this.pointerColor = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[DefaultValue("255"), Description("Opacity of external rim (0-255)")]
		public int RimAlpha
		{
			get
			{
				return this.rimAlpha;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				else if (value > 0xff)
				{
					value = 0xff;
				}
				this.rimAlpha = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("External Rim Color")]
		public Color RimColor
		{
			get
			{
				return this.rimColor;
			}
			set
			{
				this.rimColor = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("Scale Color")]
		public Color ScaleColor
		{
			get
			{
				return this.scaleColor;
			}
			set
			{
				this.scaleColor = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("Divider used to resize scale font (15-25, 15=font bigger, 25=font smaller)"), DefaultValue("23")]
		public int ScaleFontSizeDivider
		{
			get
			{
				return this.scaleFontSizeDivider;
			}
			set
			{
				if (value < 15)
				{
					value = 15;
				}
				else if (value > 0x19)
				{
					value = 0x19;
				}
				this.scaleFontSizeDivider = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("Color for Threshold n\x00b01")]
		public Color Threshold1Color
		{
			get
			{
				return this.threshold1Color;
			}
			set
			{
				this.threshold1Color = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[DefaultValue("0"), Description("Start point for threshold arc n\x00b01")]
		public float Threshold1Start
		{
			get
			{
				return this.threshold1Start;
			}
			set
			{
				if (value > this.maxValue)
				{
					value = this.maxValue;
				}
				if (value < this.minValue)
				{
					value = this.minValue;
				}
				this.threshold1Start = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[DefaultValue("0"), Description("End point for threshold arc n\x00b01")]
		public float Threshold1Stop
		{
			get
			{
				return this.threshold1Stop;
			}
			set
			{
				if (value > this.maxValue)
				{
					value = this.maxValue;
				}
				if (value < this.minValue)
				{
					value = this.minValue;
				}
				this.threshold1Stop = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[Description("Color for threshold arc n\x00b02")]
		public Color Threshold2Color
		{
			get
			{
				return this.threshold2Color;
			}
			set
			{
				this.threshold2Color = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[DefaultValue("0"), Description("Start point for threshold arc n\x00b02")]
		public float Threshold2Start
		{
			get
			{
				return this.threshold2Start;
			}
			set
			{
				if (value > this.maxValue)
				{
					value = this.maxValue;
				}
				if (value < this.minValue)
				{
					value = this.minValue;
				}
				this.threshold2Start = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[DefaultValue("0"), Description("End point for threshold arc n\x00b02")]
		public float Threshold2Stop
		{
			get
			{
				return this.threshold2Stop;
			}
			set
			{
				if (value > this.maxValue)
				{
					value = this.maxValue;
				}
				if (value < this.minValue)
				{
					value = this.minValue;
				}
				this.threshold2Stop = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}

		[DefaultValue(0), Description("Value where the pointer will point to.")]
		public float Value
		{
			get
			{
				return this.currentValue;
			}
			set
			{
				if ((value >= this.minValue) && (value <= this.maxValue))
				{
					this.currentValue = value;
				}
			}
		}

		[Description("If TRUE, DigitalValue displays same value as the pointer. If FALSE digitalValue displays DigitalValue value")]
		public bool ValueToDigital
		{
			get
			{
				return this.valueToDigital;
			}
			set
			{
				this.valueToDigital = value;
				this.requiresRedraw = true;
				base.Invalidate();
			}
		}
	}
}

