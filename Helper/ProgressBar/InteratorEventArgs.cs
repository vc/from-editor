using System.ComponentModel;

namespace Helper.ProgressBar
{
	public class InteratorEventArgs : CancelEventArgs
	{
		private int index;

		public InteratorEventArgs(int currentIndex)
		{
			this.index = currentIndex;
		}

		public int Number
		{
			get { return this.index + 1; }
		}

		public int Index { get { return this.index; } }
	}
}
