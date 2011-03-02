using System;
using FRom.Logic;
using SourceGrid;

namespace FRom.Grid
{
	public class FRomValueModel : SourceGrid.Cells.Models.IValueModel
	{
		private struct ColorK
		{
			public float? MaxV;
			public float? MinV;
			public float R, G, B;
			//public ColorK()
			//{
			//    R = G = B = 0;
			//    MinV = MaxV = null;
			//}
			//public Color ColorK(Color lo, Color hi)
			//{

			//}
		}

		ColorK _kCol;
		public FRomValueModel()
		{
		}
		#region IValueModel Members
		public object GetValue(CellContext cellContext)
		{
			Map map = (cellContext.Grid as FRom.Grid.FRomGrid).DataSource;
			return map[cellContext.Position.Column - cellContext.Grid.FixedColumns, cellContext.Position.Row - cellContext.Grid.FixedRows, null];
		}
		public void SetValue(CellContext cellContext, object p_Value)
		{
			//Выдергиваю текущее значение
			Map map = (cellContext.Grid as FRom.Grid.FRomGrid).DataSource;
			object oldValue = map[
				cellContext.Position.Column - cellContext.Grid.FixedColumns,
				cellContext.Position.Row - cellContext.Grid.FixedRows,
				null];

			SourceGrid.ValueChangeEventArgs valArgs = new SourceGrid.ValueChangeEventArgs(oldValue, p_Value);

			if (cellContext.Grid != null)
				cellContext.Grid.Controller.OnValueChanging(cellContext, valArgs);

			map[
				cellContext.Position.Column - cellContext.Grid.FixedColumns,
				cellContext.Position.Row - cellContext.Grid.FixedRows,
				null] = p_Value;

			if (cellContext.Grid != null)
				cellContext.Grid.Controller.OnValueChanged(cellContext, EventArgs.Empty);
		}
		#endregion
	}
}
