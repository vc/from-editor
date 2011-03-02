using SourceGrid;

namespace FRom.Grid
{
	public class FRomColumns : ColumnInfoCollection/*ColumnsSimpleBase//ColumnInfoCollection*/
	{
		public FRomColumns(FRomGrid grid) : base(grid) { }

		public new FRomGrid Grid
		{
			get { return (FRomGrid)base.Grid; }
		}

		private bool mColunsAdded = false;
		public void AddColumns()
		{
			//Добалвяем колонки НАХЕРА?!?!
			if (mColunsAdded) return;
			for (int i = 0; i < Grid.FixedColumns; i++)
				Add(new ColumnInfo(base.Grid));
			for (int i = 0; i < Grid.DataSource.Columns; i++)
				Add(new ColumnInfo(base.Grid));
			mColunsAdded = true;
		}
		/*
		public override int Count
		{
			get
			{
				if (Grid.DataSource == null)
					return Grid.FixedColumns;
				else
				{
					return Grid.DataSource.Columns + Grid.FixedColumns;
				}
			}
		}

		private SourceGrid.AutoSizeMode mAutoSizeMode = SourceGrid.AutoSizeMode.Default;
		public SourceGrid.AutoSizeMode AutoSizeMode
		{
			get { return mAutoSizeMode; }
			set { mAutoSizeMode = value; }
		}

		public override SourceGrid.AutoSizeMode GetAutoSizeMode(int row)
		{
			return mAutoSizeMode;
		}
		//*/
	}
}
