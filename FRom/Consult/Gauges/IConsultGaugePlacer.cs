using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace FRom.Consult.Gauges
{
	public interface IConsultGaugePlacer
	{
		void PlaceGauges(Control ctrl, IList<IConsultGauge> gauges);
	}
}
