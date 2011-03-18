using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FRom.Logger;
using Ionic.Zip;
using Helper;

namespace FRom
{
	public partial class FormFeedBack : Form
	{
		private List<string> _attachments;

		/// <summary>
		/// Лог
		/// </summary>
		private Log _log = Log.Instance;

		FormMain _frmParrent;

		public FormFeedBack(FormMain frm, string message, List<string> attachments)
		{
			InitializeComponent();

			_frmParrent = frm;

			Init(message, attachments, false);
		}
		public FormFeedBack(FormMain frm, string message, List<string> attachments, bool enableAttachements)
		{
			InitializeComponent();

			_frmParrent = frm;

			Init(message, attachments, enableAttachements);
		}

		private void Init(string message, List<string> attachments, bool enableAttacheents)
		{
			txtName.Text = _frmParrent._settings.cfgUserName;
			txtEmail.Text = _frmParrent._settings.cfgUserEmail;

			txtBugInfo.Text = message;

			_attachments = new List<string>();

			foreach (string att in attachments)
			{
				if (att != null && att != "")
					_attachments.Add(att);
			}

			lstFiles.DataSource = _attachments.ToArray();
			lstFiles.Enabled
				= btnDelAttach.Enabled
				= btnAddAttach.Enabled
				= chkAttachFiles.Checked
				= enableAttacheents;
		}

		private bool isValidEmail(string email)
		{
			string reg = "^['\\w_-]+(\\.['\\w_-]+)*@['\\w_-]+(\\.['\\w_-]+)*\\.[a-zA-Z]{2,4}$";
			return (Regex.IsMatch(email, reg));
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (txtName.Text == "")
			{
				MessageBox.Show(this, "Укажите пожалуйста имя", "Неправильно заполненное поле", MessageBoxButtons.OK);
				txtName.Focus();
				return;
			}

			if (txtEmail.Text == "" || !isValidEmail(txtEmail.Text))
			{
				MessageBox.Show(this, "Пустое или неверное поле 'eMail'", "Неправильно заполненное поле", MessageBoxButtons.OK);
				txtEmail.Focus();
				return;
			}

			if (!chkAttachFiles.Checked && _attachments == null)
				_attachments = new List<string>() { _log.LogFilePath };
			else
				_attachments.Add(_log.LogFilePath);

			MailAddress replyAddress = new MailAddress(txtEmail.Text, txtName.Text);
			string message = String.Format("{0}{1}{0}#User Description:{0}{2}",
				Environment.NewLine,
				txtBugInfo.Text,
				txtDescription.Text);

			SendEmail(
				replyAddress,
				message,
				_attachments);

			_frmParrent._settings.cfgUserName = txtName.Text;
			_frmParrent._settings.cfgUserEmail = txtEmail.Text;
			_frmParrent._settings.Save();

			this.Dispose(true);
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Dispose(true);
		}

		/// <summary>
		/// Отсылает письмо на from_logger@mail.ru
		/// </summary>
		/// <param name="message">Тело письма</param>
		/// <param name="attachments">Пути к прикрепляемым файлам</param>
		private void SendEmail(MailAddress replyAddress, string message, List<string> attachments)
		{
			do
			{
				string emailTo = "from_logger@mail.ru";
				SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.mail.ru");
				smtp.Port = 2525;
				smtp.Credentials = new NetworkCredential(emailTo, "simple");
				MailMessage msg = new MailMessage(emailTo, emailTo);

				//Добавляем аттачменты
				if (attachments != null)
				{
					ZipFile zip = new ZipFile();
					foreach (string file in attachments)
						try { zip.AddFile(file, ""); }
						catch { }

					MemoryStream st = new MemoryStream();

					zip.Save(st);

					ContentType ct = new ContentType();
					ct.MediaType = MediaTypeNames.Application.Zip;
					ct.Name = "archive.zip";

					st.Position = 0;

					msg.Attachments.Add(new Attachment(st, ct));
				}

				msg.ReplyTo = replyAddress;

				msg.Subject = String.Format("Bug report: {0} [{1}<{2}>]",
					FormAboutBox.AssemblyProduct,
					replyAddress.DisplayName,
					replyAddress.Address);

				msg.Body = String.Format("{0}{1}\tv.{2}{0}{3}{0}{4}",
					Environment.NewLine,
					FormAboutBox.AssemblyProduct,
					FormAboutBox.AssemblyVersion,
					"____________________________________________________________",
					message);
				try { smtp.Send(msg); }
				catch (Exception ex)
				{
					string errDelivery = String.Format("Ошибка при отправке:{0}{1}{0}{0}Повторить?", Environment.NewLine, HelperClass.GetExceptionInfo(ex));
					if (MessageBox.Show(this, errDelivery, "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
						continue;
				}
				break;
			} while (true);
		}

		private void lstFiles_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyValue)
			{
				case 45:	//INS
					AddAttachement();
					break;
				case 46:	//DEL
					DelAttachement();
					break;
				case 13:	//Enter
					btnOK_Click(sender, e);
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Удалить выбранный аттачмент в листбоксе lstFiles
		/// </summary>
		private void DelAttachement()
		{
			if (lstFiles.SelectedItem != null)
			{
				_attachments.Remove(lstFiles.SelectedItem.ToString());
				lstFiles.DataSource = _attachments.ToArray();
			}
		}

		/// <summary>
		/// Добавить аттачмент к письму и обновить его в листбоксе lstFiles
		/// </summary>
		private void AddAttachement()
		{
			_attachments.Add(
				HelperClass.ShowFileDialog(
					"Все известные файлы (*.adr;*.bin;*.from;*.xml)|*.adr;*.bin;*.from;*.xml|Все файлы (*.*)|*.*",
					false,
					Environment.CurrentDirectory, this
				)
			);
			lstFiles.DataSource = _attachments.ToArray();
		}

		private void chkAttachFiles_CheckedChanged(object sender, EventArgs e)
		{
			lstFiles.Enabled
				= btnAddAttach.Enabled
				= btnDelAttach.Enabled
				= chkAttachFiles.Checked;
		}

		private void txtName_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == 13)	//enter
				btnOK_Click(sender, e);
		}

		private void btnAddAttach_Click(object sender, EventArgs e)
		{
			AddAttachement();
		}

		private void btnDelAttach_Click(object sender, EventArgs e)
		{
			DelAttachement();
		}
	}
}
