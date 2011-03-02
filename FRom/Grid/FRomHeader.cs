
namespace FRom.Grid
{
	/// <summary>
	/// A cell used for the top/left cell when using DataGridRowHeader.
	/// </summary>
	public class FRomHeader : SourceGrid.Cells.Virtual.Header
	{
		public FRomHeader()
		{
			Model.AddModel(new SourceGrid.Cells.Models.NullValueModel());
		}
	}
}
