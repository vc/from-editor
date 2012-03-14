using System;
using System.Collections.Generic;
using System.Text;

namespace Helper.ProgressBar
{
	public delegate void IterationCallback (InteratorEventArgs e);

	public delegate void ExecuteCallback ();

	public interface IProgressBar : IDisposable
	{
		/// <summary>
		/// Показать процесс выполнения. 
		/// </summary>
		/// <param name="count">число итераций</param>
		/// <param name="handler">то что будет выполняться в итерации</param>
		void ShowProgressBar (int count, IterationCallback callback);

		/// <summary>
		/// Установить надпись текущей итерации
		/// </summary>
		/// <param name="currentState"></param>
		void SetCurrentState (string currentState);

		/// <summary>
		/// Показать прогрес бар когда нужно ждать. При этом не известно сколько времени надо
		/// </summary>
		/// <param name="callback">То что будет выполняться в потоке</param>
		void ShowProgressBar (ExecuteCallback callback);
		
		void SetCurrentState (string text, int index);
		
		void SetCurrentState (string text, int index, int? total);

		void ShowProgressBar ();

		/// <summary>
		/// Остановить прогрес бар 
		/// </summary>
		void StopProgressBar ();
	}
}
