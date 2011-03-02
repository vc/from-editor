using System;
using System.Drawing;


namespace FRom.Grid
{
	/// <summary>
	/// Класс для отображения данных Map
	/// </summary>
	public class Grid : SourceGrid.Grid
	{

		/// <summary>
		/// Источник данных для отображения.
		/// </summary>
		Map _dataSource;

		float _minVal, _maxVal;		//мин и макс значения ячеек

		public Grid()
			: base() { }

		public Grid(Map map)
			: base()
		{
			_dataSource = map;
			Bind();
		}

		/// <summary>
		/// Основная инициалзация контрола
		/// </summary>
		void Init()
		{
			AcceptsInputChar = false;
			//Location = new System.Drawing.Point(0, 0);gg
			ClipboardMode = SourceGrid.ClipboardMode.Copy;
			Dock = System.Windows.Forms.DockStyle.Fill;
			Name = _dataSource.Address.ConstName;
			TabStop = true;
			SelectionMode = SourceGrid.GridSelectionMode.Cell;

			//Rows.AutoSize(false, 0, 0);
			//Rows.RowHeight = _dataSource.Address.SizeOfCellY;
			//Columns.ColumnWidth = _dataSource.Address.SizeOfCellX;

			FixedColumns = 1;// _dataSource.Address.YMapConstName == "" ? 0 : 1;
			FixedRows = 1;// _dataSource.Address.XMapConstName == "" ? 0 : 1;

			ToolTipText = _dataSource.Address.Comment;
		}


		/// <summary>
		/// Gets or sets the data source array used to bind the grid.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Map DataSource
		{

			get { return _dataSource; }
			set
			{
				if (value == null)
					throw new SourceGrid.SourceGridException("Map not valid");

				_dataSource = value;
				Bind();
			}

		}

		/// <summary>
		/// Переопределенное отображение ячеек для раскраски
		/// </summary>
		protected override void PaintCell(DevAge.Drawing.GraphicsCache graphics, SourceGrid.CellContext cellContext, System.Drawing.RectangleF drawRectangle)
		{
			//if (cellContext.DisplayText != "")
			cellContext.Cell.View.BackColor = ColorCalcRGB(cellContext.DisplayText);
			base.PaintCell(graphics, cellContext, drawRectangle);
		}

		/// <summary>
		/// Отобразить данные
		/// </summary>
		protected virtual void Bind()
		{
			ValueCell = null;
			if (_dataSource != null)
			{
				Init();
				_valueCell = new SourceGrid.Cells.Virtual.CellVirtual();
				_valueCell.Model.AddModel(new FRomValueModel());
				_valueCell.Editor = SourceGrid.Cells.Editors.Factory.Create(typeof(uint));
			}
			Rows.RowsChanged();
			Columns.ColumnsChanged();

			RecalcColor();	//Пересчет переменных для задания цвета
		}

		/// <summary>
		/// Подсчитать минимум и максимум значений ячеек в карте
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

		protected SourceGrid.Cells.ICellVirtual _valueCell;
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public SourceGrid.Cells.ICellVirtual ValueCell
		{
			get { return _valueCell; }
			set { _valueCell = value; }
		}

		#region RGB реализация
		/// <summary>
		/// подсчет максимального и минимального элементов в массиве для отрисовки цветов ячеек
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
		byte _kColorMin = 0;	//Коэффициент цвета
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
	}
}
