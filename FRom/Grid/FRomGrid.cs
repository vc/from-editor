using System.ComponentModel;
using System.Drawing;
using FRom.Logic;
using SourceGrid;
using SourceGrid.Cells;

namespace FRom.Grid
{
	/// <summary>
	///  ласс дл€ отображени€ данных Map
	/// </summary>
	public class FRomGrid : SourceGrid.GridVirtual
	{
		/// <summary>
		/// »сточник данных дл€ отображени€.
		/// </summary>
		Map _dataSource;

		float _minVal, _maxVal;		//мин и макс значени€ €чеек

		public FRomGrid()
			: base() { }

		public FRomGrid(Map map)
			: base()
		{
			DataSource = map;
		}

		/// <summary>
		/// ѕереопределенное отображение €чеек дл€ раскраски
		/// </summary>
		protected override void PaintCell(DevAge.Drawing.GraphicsCache graphics,
			SourceGrid.CellContext cellContext,
			System.Drawing.RectangleF drawRectangle)
		{
			//if (cellContext.DisplayText != "")
			cellContext.Cell.View.BackColor = ColorCalcRGB(cellContext.DisplayText);
			//base.PaintCell(graphics, cellContext, drawRectangle);
			if (drawRectangle.Width > 0 && drawRectangle.Height > 0 &&
				cellContext.CanBeDrawn())
			{
				cellContext.Cell.View.DrawCell(cellContext, graphics, drawRectangle);
			}
		}

		/// <summary>
		/// ѕодсчитать минимум и максимум значений €чеек в карте
		/// </summary>
		private void CalcMinMaxElements()
		{
			if (_dataSource.Length == 0)
			{
				_minVal = _maxVal = 0;
				return;
			}
			else _minVal = _maxVal = _dataSource[0, 0];
			foreach (uint item in _dataSource)
				if (item < _minVal) _minVal = item;
		}

		public override bool EnableSort { get; set; }

		protected override SourceGrid.RowsBase CreateRowsObject()
		{
			return new FRomRows(this);
		}

		protected override SourceGrid.ColumnsBase CreateColumnsObject()
		{
			return new FRomColumns(this);
		}

		public override ICellVirtual GetCell(int p_iRow, int p_iCol)
		{
			if (p_iRow < FixedRows &&
				p_iCol < FixedColumns)
				return _header;
			else if (p_iRow < FixedRows)
				return _columnHeader;
			else if (p_iCol < FixedColumns)
				return _rowHeader;
			else
				return _valueCell;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new FRomRows Rows
		{
			get { return (FRomRows)base.Rows; }
		}
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new FRomColumns Columns
		{
			get { return (FRomColumns)base.Columns; }
		}

		/// <summary>
		/// Gets or sets the data source array used to bind the grid.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Map DataSource
		{

			get { return _dataSource; }
			set
			{
				if (value == null)
					throw new SourceGridException("Map not valid");

				_dataSource = value;
				Bind();
			}
		}

		/// <summary>
		/// ќтобразить данные
		/// </summary>
		protected virtual void Bind()
		{
			ValueCell = null;
			if (_dataSource == null) return;

			AcceptsInputChar = false;
			//Location = new System.Drawing.Point(0, 0);gg
			ClipboardMode = SourceGrid.ClipboardMode.Copy;
			Dock = System.Windows.Forms.DockStyle.Fill;
			Name = _dataSource.Address.ConstName;
			TabStop = true;
			SelectionMode = SourceGrid.GridSelectionMode.Cell;

			FixedColumns = _dataSource.Address.YMapConstName == "" ? 1 : 2;
			FixedRows = _dataSource.Address.XMapConstName == "" ? 1 : 2;

			Columns.AddColumns();

			ToolTipText = _dataSource.Address.Comment;

			_valueCell = new SourceGrid.Cells.Virtual.CellVirtual();
			_valueCell.Model.AddModel(new FRomValueModel());
			_valueCell.Editor = SourceGrid.Cells.Editors.Factory.Create(typeof(uint));

			Rows.RowsChanged();
			Columns.ColumnsChanged();

			Rows.AutoSize(true);
			Columns.AutoSize(true);

			RecalcColor();	//ѕересчет переменных дл€ задани€ цвета
		}

		private ICellVirtual _columnHeader = new FRomColumnHeader();
		/// <summary>
		/// Gets or sets the cell used for the column headers.  Only used when FixedRows is greater than 0.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ICellVirtual ColumnHeader
		{
			get { return _columnHeader; }
			set { _columnHeader = value; }
		}

		private ICellVirtual _rowHeader = new FRomRowHeader();
		/// <summary>
		/// Gets or sets the cell used for the row headers. Only used when FixedColumns is greater than 0.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ICellVirtual RowHeader
		{
			get { return _rowHeader; }
			set { _rowHeader = value; }
		}

		private ICellVirtual _header = new FRomHeader();
		/// <summary>
		/// Gets or sets the cell used for the left top position header. Only used when FixedRows and FixedColumns are greater than 0.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ICellVirtual Header
		{
			get { return _header; }
			set { _header = value; }
		}

		protected ICellVirtual _valueCell;
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ICellVirtual ValueCell
		{
			get { return _valueCell; }
			set { _valueCell = value; }
		}

		#region RGB реализаци€
		/// <summary>
		/// подсчет максимального и минимального элементов в массиве дл€ отрисовки цветов €чеек
		/// </summary>		
		private void RecalcColor()
		{
			if (_dataSource != null && _dataSource.Length > 0)
				_minElOfArr = _maxElOfArr = _dataSource[0, 0];
			else
				_minElOfArr = _maxElOfArr = 0;
			foreach (uint i in _dataSource)
			{
				if (_minElOfArr > i) _minElOfArr = i;
				if (_maxElOfArr < i) _maxElOfArr = i;
			}
			_countEl = (int)(_maxElOfArr - _minElOfArr);
			float _countElR = (float)_dataSource.Address.ColorHi.R / _countEl;
			float _countElG = (float)_dataSource.Address.ColorHi.G / _countEl;
			float _countElB = (float)_dataSource.Address.ColorHi.B / _countEl;


			//_countElColor = Color.FromArgb(255, (int)(_countEl * _countElR), (int)(_countEl * _countElG), (int)(_countEl * _countElB));

		}

		Color _countElColor;
		int _countEl;
		uint _minElOfArr;
		uint _maxElOfArr;
		byte _kColorMax = 255;
		byte _kColorMin = 0;	// оэффициент цвета
		float _kColor;
		struct ColorK
		{
			public float R, G, B;
		}
		ColorK _colorK;
		Color ColorCalcRGB(string str)
		{
			int _n = 0;
			try { _n = int.Parse(str); }
			catch { _n = 0; }
			return ColorCalcRGB(_n);
		}

		Color ColorCalcRGB(int n)
		{
			AddressInstance adr = _dataSource.Address;
			float arrK = _maxElOfArr - _minElOfArr;
			_kColor = arrK / (float)(_kColorMax - _kColorMin);

			_colorK.R = arrK / (float)(adr.ColorHi.R - adr.ColorLo.R);
			_colorK.G = arrK / (float)(adr.ColorHi.G - adr.ColorLo.G);
			_colorK.B = arrK / (float)(adr.ColorHi.B - adr.ColorLo.B);
			//_colorK = 
			if (n >= _minElOfArr && n <= _maxElOfArr)
			{
				byte R, G, B;
				Color absN = new Color();
				int x = n - (int)_minElOfArr;
				//absN = Color.FromArgb(x, x, x);
				R = (byte)(adr.ColorLo.R + (byte)((n - _minElOfArr) / _colorK.R));
				G = (byte)(adr.ColorLo.G + (byte)((n - _minElOfArr) / _colorK.G));
				B = (byte)(adr.ColorLo.B + (byte)((n - _minElOfArr) / _colorK.B));
				//Color ret = _adr.ColorLo + (absN / _colorK);

				return Color.FromArgb(255, R, G, B);
			}
			else
				return Color.FromArgb(255, 255, 255);
		}

		Color ColorCalc(int n)
		{
			if (n >= _minElOfArr && n <= _maxElOfArr)
			{
				byte R, G, B;

				R = (byte)(_kColorMin + (byte)((n - _minElOfArr) / _kColor));
				G = (byte)(_kColorMax - (byte)((n - _minElOfArr) / _kColor));
				B = 0;
				return Color.FromArgb(R, G, B);
			}
			else
				return Color.FromArgb(255, 255, 255);
		}
		Color ColorCalc(string str)
		{
			int _n = 0;
			try { _n = int.Parse(str); }
			catch { _n = 0; }
			return ColorCalc(_n);
		}
		#endregion

		public void propertyGrid_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			//TODO
			Bind();
		}
	}
}
