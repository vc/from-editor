using System;
using FRom.Logic;

namespace FRom.Grid
{

	public class FRomColumnHeaderModel : SourceGrid.Cells.Models.IValueModel
	{
		public FRomColumnHeaderModel()
		{
		}
		#region IValueModel Members
		public virtual object GetValue(SourceGrid.CellContext cellContext)
		{
			AddressInstance adr = (cellContext.Grid as FRomGrid).DataSource.Address;

			//Порядковый номер с нуля
			int n = cellContext.Position.Column - cellContext.Grid.FixedColumns;

			//Если фиксированных больше одной и сейчас проходим по первой, 
			//заполняем ее порядковым номером
			if (cellContext.Grid.FixedRows > 1 && cellContext.Position.Row == 0)
				return n + 1;
			if (adr.XMapConstName.Length != 0)
			{
				From f = (cellContext.Grid as FRomGrid).DataSource.GetFRom();
				return f.GetMap(adr.XMapConstName)[n, 0, ViewEnum.Scale];
			}
			else
				return n + 1;
		}
		public virtual void SetValue(SourceGrid.CellContext cellContext, object p_Value)
		{
			throw new ApplicationException("Not supported");
		}
		#endregion
	}
}
