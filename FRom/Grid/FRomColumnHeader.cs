using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.Grid
{
	/// <summary>
	/// A cell header used for the columns. Usually used in the HeaderCell property of a DataGridColumn.
	/// </summary>
	public class FRomColumnHeader : SourceGrid.Cells.Virtual.ColumnHeader
	{
		public FRomColumnHeader()
		{
			Model.AddModel(new FRomColumnHeaderModel());
			AutomaticSortEnabled = false;
			ResizeEnabled = false;

			//            ColumnSelectorEnabled = true;
			//            ColumnFocusEnabled = true;
		}
	}
}
