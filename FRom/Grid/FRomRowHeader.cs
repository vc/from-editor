
namespace FRom.Grid
{
	/// <summary>
	/// A cell used as left row selector. Usually used in the DataCell property of a DataGridColumn. If FixedColumns is grater than 0 and the columns are automatically created then the first column is created of this type.
	/// </summary>
	public class FRomRowHeader : SourceGrid.Cells.Virtual.RowHeader
	{
		public FRomRowHeader()
		{
			Model.AddModel(new FRomRowHeaderModel());
			ResizeEnabled = false;

			//RowSelectorEnabled = true;
		}
	}
}
