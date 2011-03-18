using System;
using System.IO;
using System.Windows.Forms;

namespace Helper
{
	public class HelperClass
	{
		public static string ShowFileDialog(string filesToShow, bool saveDialog, string path, Form frm)
		{
			FileDialog _dlg;

			if (saveDialog) _dlg = new SaveFileDialog();
			else _dlg = new OpenFileDialog();
			_dlg.InitialDirectory = path;
			_dlg.Filter = filesToShow;
			_dlg.ShowDialog(frm);
			return _dlg.FileName;
		}

		/// <summary>
		/// Ищет папку начиная с текущей и до корневой, выходя каждый раз на уровень выше
		/// </summary>
		/// <param name="searchDir">Имя папки для поиска</param>
		/// <returns>Полный путь к папке со слешем в конце</returns>
		public static string FindFolder(string searchDir)
		{
			for (string currDir = Environment.CurrentDirectory/*char slash = '\\'*/; currDir != null; currDir = Path.GetDirectoryName(currDir))
			{
				string res = Path.Combine(currDir, searchDir);

				if (Directory.Exists(res))
					return Path.GetFullPath(res) + "\\";
			}
			return null;
		}

		/// <summary>
		/// Ищет файл начиная с текущей папки до корня
		/// </summary>
		/// <param name="fileName">Имя файла для поиска</param>
		/// <param name="path">Начальный путь для поиска</param>
		/// <returns>Полный путь к файлу</returns>		
		public static string FindFileInParrents(string fileName, string path = null)
		{
			if (String.IsNullOrEmpty(path))
				path = Environment.CurrentDirectory;

			for (; path != null; path = Path.GetDirectoryName(path))
			{
				string res = Path.Combine(path, fileName);
				if (File.Exists(res))
					return res;
			}
			return null;
		}

		/// <summary>
		/// Ищет файл начиная с текущей папки во всех дочерних
		/// </summary>
		/// <param name="fileName">Имя файла для поиска</param>
		/// <param name="path">Начальный путь для поиска</param>
		/// <param name="level">Уровень поиска в дочерних папках 1=один уровень вверх(использовать с осторожностью!)</param>
		/// <returns>Полный путь к файлу</returns>		
		public static string FindFileInChilds(string fileName, string path = null, int level = 1)
		{
			//Если путь не определен, используем текущий из окружения
			if (String.IsNullOrEmpty(path))
				path = Environment.CurrentDirectory;

			//Проверяем наличие файла в текущей папке
			string fullFileName = Path.Combine(path, fileName);
			if (File.Exists(fullFileName))
				return fullFileName;
			//глубина поиска исчерпана
			else if (level == -1)
				return null;

			//Рекурсивно вызываем себя для поиска в дочерних файлов
			foreach (string dir in Directory.GetDirectories(path))
				FindFileInChilds(fileName, dir, level - 1);

			return null;
		}

		/// <summary>
		/// Подробная информация об исключении со стектрейсом
		/// </summary>
		/// <param name="ex">Исключение</param>
		/// <returns>Строка с подрбной информацией</returns>
		public static string GetExceptionInfo(Exception ex)
		{
			string message = String.Format("{4}Exception: {1} ({2}){0}{3}{0}",
				Environment.NewLine,
				ex.GetType().ToString(),
				ex.Message,
				ex.StackTrace,
				ex.InnerException == null ? "" : GetExceptionInfo(ex.InnerException) + "\t--\nInner"
				);

			return message;
		}

		public static string GetExceptionMessages(Exception ex)
		{
			string message = String.Format("{2}Exception: {1}{0}",
				Environment.NewLine,
				ex.Message,
				ex.InnerException == null ? "" : GetExceptionMessages(ex.InnerException)
				);

			return message;
		}

		//http://rsdn.ru/forum/dotnet/2285015.aspx
		/// <summary>
		/// private void SetIntSignalLevel(int SigValue)
		/// {
		/// 	SafeInvoker.Invoke(SigLevel,
		/// 		delegate
		///			{
		/// 				SigLevel.Value = SigValue;
		/// 		}
		///		);
		/// }
		/// </summary>
		/// <param name="ctrl"></param>
		/// <param name="methodToInvoke"> delegate { ProgressBar.Value = value; } </param>
		public static void BeginInvoke(Control ctrl, MethodInvoker methodToInvoke)
		{
			try
			{
				if (ctrl == null || ctrl.IsDisposed)
					return;
				if (ctrl.InvokeRequired)
				{
					ctrl.BeginInvoke(methodToInvoke);
				}
				else
				{
					methodToInvoke();
				}
			}
			catch (ObjectDisposedException) { }
		}

		public static void Invoke(Control ctrl, MethodInvoker methodToInvoke)
		{
			try
			{
				if (ctrl == null || ctrl.IsDisposed)
					return;
				if (ctrl.InvokeRequired)
				{
					ctrl.Invoke(methodToInvoke);
				}
				else
				{
					methodToInvoke();
				}
			}
			catch (ObjectDisposedException) { }
		}

	}

}
