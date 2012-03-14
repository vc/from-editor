using System;
using System.Collections.Generic;
using System.Text;

namespace FRom.Consult
{
	public interface ILiveScanForm
	{
		/// <summary>
		/// размер панели приборов
		/// </summary>
		System.Drawing.Size PanelSize { get; set; }

		/// <summary>
		/// минимально допустимый размер панели
		/// </summary>
		System.Drawing.Size PanelMinimumSize { get; set; }
	}
}
