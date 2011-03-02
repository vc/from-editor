using SourceGrid;
namespace FRom.Grid
{
	public class FRomRows : RowsSimpleBase
	{
		public FRomRows(FRomGrid grid)
			: base(grid)
		{
		}

		public new FRomGrid Grid
		{
			get { return (FRomGrid)base.Grid; }
		}

		public override int Count
		{
			get
			{
				if (Grid.DataSource == null)
					return Grid.FixedRows;
				else
				{
					return Grid.DataSource.Rows + Grid.FixedRows;
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
	}
}
