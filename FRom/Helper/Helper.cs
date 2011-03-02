using System;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;

namespace FRom
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
		/// Ищет файл начиная с текущей папки и до корневой, выходя каждый раз на уровень выше
		/// </summary>
		/// <param name="name">Имя файла для поиска</param>
		/// <returns>Полный путь к файлу</returns>
		public static string FindFolder(string name)
		{
			string dir = Environment.CurrentDirectory,
					disk = Path.GetPathRoot(dir);

			for (char slash = '\\'; dir != null; dir = Path.GetDirectoryName(dir))
			{
				string res = dir.TrimEnd(slash) + slash + name;
				if (Directory.Exists(res))
					return res + slash;
			}
			return null;
		}

		/// <summary>
		/// Подробная информация об исключении со стектрейсом
		/// </summary>
		/// <param name="ex">Исключение</param>
		/// <returns>Строка с подрбной информацией</returns>
		public static string GetExceptionInfo(Exception ex)
		{
			string message = String.Format("{1}:{2}{0}{3}{0}",
				Environment.NewLine,
				ex.Message,
				ex.ToString(),
				ex.StackTrace
				);

			return message.ToString();
		}

		public static string GetExceptionMessages(Exception ex)
		{
			string msg = "";
			const string separator = "  ";
			int n = 0;
			while (ex != null)
			{
				for (int i = 0; i < n; i++)
					msg += separator;

				msg += String.Format("{1}{0}",
					Environment.NewLine,
					ex.Message
					);

				ex = ex.InnerException;
				n++;
			}

			return msg;
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
		public static void Invoke(Control ctrl, MethodInvoker methodToInvoke)
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
	}

}
