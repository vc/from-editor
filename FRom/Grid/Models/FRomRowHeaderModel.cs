using System;
using SourceGrid;
using FRom.Logic;

namespace FRom.Grid
{
	public class FRomRowHeaderModel : SourceGrid.Cells.Models.IValueModel
	{
		public FRomRowHeaderModel()
		{
		}
		#region IValueModel Members
		public virtual object GetValue(CellContext cellContext)
		{
			AddressInstance adr = (cellContext.Grid as FRomGrid).DataSource.Address;

			//Порядковый номер с нуля 
			int n = cellContext.Position.Row - cellContext.Grid.FixedRows;

			//Если фиксированных больше одной и сейчас проходим по первой, 
			//заполняем ее порядковым номером
			if (cellContext.Grid.FixedColumns > 1 && cellContext.Position.Column == 0)
				return n + 1;
			if (adr.YMapConstName.Length != 0)
			{
				From f = (cellContext.Grid as FRomGrid).DataSource.GetFRom();
				return f.GetMap(adr.YMapConstName)[n, 0, ViewEnum.Scale];
			}
			else
				return n + 1;
		}
		public virtual void SetValue(CellContext cellContext, object p_Value)
		{
			throw new ApplicationException("Not supported");
		}
		#endregion
	}
}
