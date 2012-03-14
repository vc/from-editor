using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Ionic.Zip;
using Helper.ProgressBar;
using System.Drawing;
using Helper.Logger;

namespace Helper
{
	public class HelperClass
	{
		public static string ShowFileDialog (string filesToShow, bool saveDialog, string path, Form frm)
		{
			FileDialog _dlg;

			if (saveDialog)
				_dlg = new SaveFileDialog ();
			else
				_dlg = new OpenFileDialog ();
			_dlg.InitialDirectory = path;
			_dlg.Filter = filesToShow;
			_dlg.ShowDialog (frm);
			return _dlg.FileName;
		}

		/// <summary>
		/// Ищет папку начиная с текущей и до корневой, выходя каждый раз на уровень выше
		/// </summary>
		/// <param name="searchDir">Имя папки для поиска</param>
		/// <returns>Полный путь к папке со слешем в конце</returns>
		public static string FindFolder (string searchDir)
		{
			for (string currDir = Environment.CurrentDirectory/*char slash = '\\'*/; currDir != null; currDir = Path.GetDirectoryName(currDir)) {
				string res = Path.Combine (currDir, searchDir);

				if (Directory.Exists (res))
					return Path.GetFullPath (res) + "\\";
			}
			return null;
		}
		
		public static string FindFileInParrents (string fileName)
		{
			return HelperClass.FindFileInParrents (fileName, null);
		}
		
		
		/// <summary>
		/// Ищет файл начиная с текущей папки до корня
		/// </summary>
		/// <param name="fileName">Имя файла для поиска</param>
		/// <param name="path">Начальный путь для поиска</param>
		/// <returns>Полный путь к файлу</returns>		
		public static string FindFileInParrents (string fileName, string path)
		{
			if (String.IsNullOrEmpty (path))
				path = Environment.CurrentDirectory;

			for (; path != null; path = Path.GetDirectoryName(path)) {
				string res = Path.Combine (path, fileName);
				if (File.Exists (res))
					return res;
			}
			return null;
		}
		
		public static string FindFileInChilds (string fileName)
		{
			return HelperClass.FindFileInChilds (fileName, null);	
			
		}
		
		public static string FindFileInChilds (string fileName, string path)
		{
			return HelperClass.FindFileInChilds (fileName, path, 1);	
		}
		
		
		/// <summary>
		/// Ищет файл начиная с текущей папки во всех дочерних
		/// </summary>
		/// <param name="fileName">Имя файла для поиска</param>
		/// <param name="path">Начальный путь для поиска</param>
		/// <param name="level">Уровень поиска в дочерних папках 1=один уровень вверх(использовать с осторожностью!)</param>
		/// <returns>Полный путь к файлу</returns>		
		public static string FindFileInChilds (string fileName, string path, int level)
		{
			//Если путь не определен, используем текущий из окружения
			if (String.IsNullOrEmpty (path))
				path = Environment.CurrentDirectory;

			//Проверяем наличие файла в текущей папке
			string fullFileName = Path.Combine (path, fileName);
			if (File.Exists (fullFileName))
				return fullFileName;
			//глубина поиска исчерпана
			else if (level == -1)
				return null;

			//Рекурсивно вызываем себя для поиска в дочерних файлов
			foreach (string dir in Directory.GetDirectories(path))
				FindFileInChilds (fileName, dir, level - 1);

			return null;
		}

		/// <summary>
		/// Подробная информация об исключении со стектрейсом
		/// </summary>
		/// <param name="ex">Исключение</param>
		/// <returns>Строка с подрбной информацией</returns>
		public static string GetExceptionInfo (Exception ex)
		{
			string message = String.Format ("{4}Exception: {1} ({2}){0}{3}{0}",
				Environment.NewLine,
				ex.GetType ().ToString (),
				ex.Message,
				ex.StackTrace,
				ex.InnerException == null ? "" : GetExceptionInfo (ex.InnerException) + "  --\nInner"
				);

			return message;
		}

		public static string GetExceptionMessages (Exception ex)
		{
			string message = String.Format ("{2}Exception: {1}{0}",
				Environment.NewLine,
				ex.Message,
				ex.InnerException == null ? "" : GetExceptionMessages (ex.InnerException)
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
		public static void BeginInvoke (Control ctrl, MethodInvoker methodToInvoke)
		{
			try {
				if (ctrl == null || ctrl.IsDisposed)
					return;
				if (ctrl.InvokeRequired) {
					ctrl.BeginInvoke (methodToInvoke);
				} else {
					methodToInvoke ();
				}
			} catch (ObjectDisposedException) {
			}
		}

		public static void Invoke (Control ctrl, MethodInvoker methodToInvoke)
		{
			try {
				if (ctrl == null || ctrl.IsDisposed)
					return;
				if (ctrl.InvokeRequired) {
					ctrl.Invoke (methodToInvoke);
				} else {
					methodToInvoke ();
				}
			} catch (ObjectDisposedException) {
			}
		}
		
		public static DialogResult Message (Control ctrl, string msg)
		{
			return HelperClass.Message (ctrl, msg, null, MessageBoxIcon.Information, MessageBoxButtons.OK);
		}
		
		public static DialogResult Message (Control ctrl, string msg,
			string caption)
		{
			return HelperClass.Message (ctrl, msg, caption, MessageBoxIcon.Information, MessageBoxButtons.OK);
		}

		public static DialogResult Message (Control ctrl, string msg,
			string caption,
			MessageBoxIcon icon)
		{
			return HelperClass.Message (ctrl, msg, caption, icon, MessageBoxButtons.OK);
		}
		
		/// <summary>
		/// Интерактивное сообщение пользователю
		/// </summary>
		/// <param name="ctrl">Откуда сообщение</param>
		/// <param name="msg">Сообщение</param>
		/// <param name="caption">Заголовок</param>
		/// <param name="icon">Значек диалога</param>
		/// <param name="buttons">Кнопки</param>
		/// <returns>Резльтат диалога</returns>
		public static DialogResult Message (Control ctrl, string msg,
			string caption,
			MessageBoxIcon icon,
			MessageBoxButtons buttons)
		{
			if (caption == null)
				caption = GetProductInfo (ctrl);

			EventEntryType logEventType = icon == MessageBoxIcon.Error
				? EventEntryType.Error
				: EventEntryType.Event;
			Log.Instance.WriteEntry (ctrl, msg, logEventType);

			DialogResult res = System.Windows.Forms.DialogResult.OK;
			HelperClass.Invoke (ctrl, delegate()
			{
				res = MessageBox.Show (ctrl, msg, caption, buttons, icon);
			});
			Logger.Log.Instance.WriteEntry (ctrl, "DialogResult=" + res.ToString ());
			return res;
		}
		
		public static DialogResult Message (Control ctrl, Exception ex)
		{
			return HelperClass.Message (ctrl, ex, null, MessageBoxIcon.Information, MessageBoxButtons.OK);
		}
		
		public static DialogResult Message (Control ctrl, Exception ex, string caption
		)
		{
			return HelperClass.Message (ctrl, ex, caption, MessageBoxIcon.Information, MessageBoxButtons.OK);
		}
		
		public static DialogResult Message (Control ctrl, Exception ex, string caption, MessageBoxIcon icon)
		{
			return HelperClass.Message (ctrl, ex, caption, icon, MessageBoxButtons.OK);
		}
		
		/// <summary>
		/// Интерактивное сообщение пользователю
		/// </summary>
		/// <param name="ctrl">Откуда сообщение</param>
		/// <param name="ex">Исключение как источник сообщения</param>
		/// <param name="caption">Заголовок</param>
		/// <param name="icon">Значек диалога</param>
		/// <param name="buttons">Кнопки</param>
		/// <returns>Резльтат диалога</returns>
		public static DialogResult Message (Control ctrl, Exception ex,
			string caption, 
			MessageBoxIcon icon,
			MessageBoxButtons buttons)
		{
			string msg = HelperClass.GetExceptionMessages (ex);

			return Message (ctrl, msg, caption, icon, buttons);
		}

		/// <summary>
		/// Взять имя файла без расширения
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public static string GetFileName (FileInfo file)
		{
			return file.Name.Substring (0, file.Name.LastIndexOf ('.'));
		}

		static IProgressBar _progressZip;
		/// <summary>
		/// Сархивировать файлы в MemoryStream
		/// </summary>
		/// <param name="attachments">Список файлов</param>
		/// <returns>Поток данных</returns>
		public static MemoryStream CreateZipAttachement (List<string> attachments)
		{
			_progressZip = FormProgressBar.GetInstance ("Packing log file...");
			MemoryStream st = new MemoryStream ();
			ZipFile zip = new ZipFile ();


			lock (_progressZip) {
				foreach (string file in attachments)
					try {
						zip.AddFile (file, "");
					} catch {
					}
				_progressZip.ShowProgressBar (delegate() {
					zip.Save (st); });
			}

			_progressZip.Dispose ();
			_progressZip = null;
			return st;
		}
		
		public static FileInfo GetFirstFreeFileName (FileInfo fi)
		{
			return  HelperClass.GetFirstFreeFileName (fi, "{0}[{3:yyyy-MM-dd}]_{1}{2}");
		}
			
		/// <summary>
		/// Получить первое свободное имя файла в указанной папке
		/// </summary>
		/// <param name="fi">Имя файла для поиска первого незанятого имени</param>
		/// <param name="formatString">Строка формаирования индекса {0} - имя файла {1}-порядковый индекс, {2} - расширение {3}-дата</param>
		/// <returns>Имя файла, которого не существует			</returns>
		public static FileInfo GetFirstFreeFileName (FileInfo fi, string formatString)
		{
			FileInfo fiCurrent;
			int i = 0;

			do {
				string tmp = String.Format (formatString,
							HelperClass.GetFileName (fi),
							i++,
							fi.Extension,
							DateTime.Now
						);
				fiCurrent = new FileInfo (
					Path.Combine (
						fi.Directory.FullName,
						tmp
					)
				);
			} while (File.Exists(fiCurrent.FullName));

			return fiCurrent;
		}
		
		public static Form GetDefaultDialogForm ()
		{
			return HelperClass.GetDefaultDialogForm (null, null);
		}
		
		public static Form GetDefaultDialogForm (string caption)
		{
			return HelperClass.GetDefaultDialogForm (caption, null);
		}
		
		/// <summary>
		/// Получить дефолтовые настройки формы
		/// </summary>
		/// <param name="caption">Заголовок (если пустой - имя и версия продукта)</param>
		/// <param name="controls">Массив контролов для добавления на форму</param>
		public static Form GetDefaultDialogForm (string caption, Control[] controls)
		{
			Form frm = new Form ();
			frm.SuspendLayout ();
			//frm.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			frm.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			frm.ClientSize = new System.Drawing.Size (406, 200);
			frm.AutoSize = true;
			if (controls != null)
				foreach (Control i in controls)
					frm.Controls.Add (i);
			frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			frm.MaximizeBox = false;
			frm.MinimizeBox = false;
			//frm.Name = ;
			frm.ShowIcon = false;
			frm.ShowInTaskbar = false;
			frm.Text = caption == null ? GetProductInfo (frm) : caption;
			frm.ResumeLayout (false);
			return frm;
		}

		public static string GetProductInfo (Control ctrl)
		{
			return String.Format ("{0} ({1})",
								ctrl.ProductName,
								ctrl.ProductVersion
								);
		}

		public static DialogResult InputBox (string title, string promptText, ref string value)
		{
			Form form = new Form ();
			Label label = new Label ();
			TextBox textBox = new TextBox ();
			Button buttonOk = new Button ();
			Button buttonCancel = new Button ();

			form.Text = title;
			label.Text = promptText;
			textBox.Text = value;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds (9, 20, 372, 13);
			textBox.SetBounds (12, 36, 372, 20);
			buttonOk.SetBounds (228, 72, 75, 23);
			buttonCancel.SetBounds (309, 72, 75, 23);

			label.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new Size (396, 107);
			form.Controls.AddRange (new Control[] { label, textBox, buttonOk, buttonCancel });
			form.ClientSize = new Size (Math.Max (300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog ();
			value = textBox.Text;
			return dialogResult;
		}
	}
}
