using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using FRom.Consult.Data;
using System.Windows.Forms;

namespace FRom.Consult.Gauges
{
	public interface IConsultGauge : IComponent
	{
		ConsultSensor Sensor { get; }
		void Close();

		void AddToControl(Control ctrl);
		void RemoveFromControl(Control ctrl);

		IConsultGaugePlacer GaugePlacer { get; }

		int Height { get; set; }
		int Width { get; set; }
		int Top { get; set; }
		int Left { get; set; }
	}
}
