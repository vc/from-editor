using System.ComponentModel;
using System.Runtime.InteropServices;

namespace FRom
{
	public class Timer
	{
		[DllImport("KERNEL32")]
		private static extern bool QueryPerformanceCounter(
			out long lpPerformanceCount);

		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(out long lpFrequency);

		private long start;
		private long stop;
		private long frequency;
		private long[] counters;
		private int N;
		//Decimal multiplier = new Decimal(1.0e9);
		private double[] mem;
		public Timer(int N)
		{
			mem = new double[N];
			this.N = N;
			this.counters = new long[this.N];
			if (QueryPerformanceFrequency(out this.frequency) == false)
			{
				// Frequency not supported
				throw new Win32Exception();
			}
		}

		public void Start(int num)
		{
			QueryPerformanceCounter(out this.start);
			this.counters[num] = this.start;
		}

		public double Get(int num)
		{
			QueryPerformanceCounter(out this.stop);
			return ((((double)(this.stop - this.counters[num])) / (double)this.frequency));
		}

		public void Write(int num)
		{
			QueryPerformanceCounter(out this.stop);
			mem[num] += ((((double)(this.stop - this.counters[num])) / (double)this.frequency));
		}

		public double Read(int num)
		{
			return mem[num];
		}

		public double[] ReadAll()
		{
			return mem;
		}
	}
}
