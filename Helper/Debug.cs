using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace FRom
{
	partial class FormMain
	{
		string _str;
		void Print(string s) { _str += s + "\n"; }
		private void _Debug()
		{
			//string s;
			//s = Helper.Helper.ToHex(16);
			//s = Helper.Helper.ToHex(17);
			//s = Helper.Helper.ToHex(32);

			//int n = 1000000;
			//Helper.Timer tmr = new FRom.Helper.Timer(3);
			
			//long b = DateTime.Now.Ticks;
			//System.Diagnostics.Stopwatch b = new System.Diagnostics.Stopwatch().;

			//tmr.Start(0);
			//for (uint i = 0; i < n; i++)
			//    Helper.Helper.ToHex(i);
			//tmr.Write(0); 

			//tmr.Start(1);
			//for (uint i = 0; i < n; i++)
			//    i.ToString("x");
			//tmr.Write(1);

			//tmr.Start(2);
			//for (uint i = 0; i < n; i++)
			//    Convert.ToString(i, 16);
			//tmr.Write(2);
			
			//double[] d = tmr.ReadAll();
		}
	}
}
