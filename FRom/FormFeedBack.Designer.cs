namespace FRom
{
	partial class FormFeedBack
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lblDescription = new System.Windows.Forms.Label();
			this.lblEmail = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.txtEmail = new System.Windows.Forms.TextBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lstFiles = new System.Windows.Forms.ListBox();
			this.chkAttachFiles = new System.Windows.Forms.CheckBox();
			this.btnDelAttach = new System.Windows.Forms.Button();
			this.btnAddAttach = new System.Windows.Forms.Button();
			this.txtBugInfo = new System.Windows.Forms.TextBox();
			this.lblBugInfo = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(12, 411);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(12, 385);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "Send";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// lblDescription
			// 
			this.lblDescription.AutoSize = true;
			this.lblDescription.Location = new System.Drawing.Point(12, 67);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(60, 13);
			this.lblDescription.TabIndex = 7;
			this.lblDescription.Text = "Description";
			// 
			// lblEmail
			// 
			this.lblEmail.AutoSize = true;
			this.lblEmail.Location = new System.Drawing.Point(12, 41);
			this.lblEmail.Name = "lblEmail";
			this.lblEmail.Size = new System.Drawing.Size(32, 13);
			this.lblEmail.TabIndex = 8;
			this.lblEmail.Text = "Email";
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(12, 15);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(35, 13);
			this.lblName.TabIndex = 6;
			this.lblName.Text = "Name";
			// 
			// txtDescription
			// 
			this.txtDescription.Location = new System.Drawing.Point(112, 64);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtDescription.Size = new System.Drawing.Size(514, 150);
			this.txtDescription.TabIndex = 2;
			this.txtDescription.WordWrap = false;
			// 
			// txtEmail
			// 
			this.txtEmail.Location = new System.Drawing.Point(112, 38);
			this.txtEmail.Name = "txtEmail";
			this.txtEmail.Size = new System.Drawing.Size(514, 20);
			this.txtEmail.TabIndex = 1;
			this.txtEmail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(112, 12);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(514, 20);
			this.txtName.TabIndex = 0;
			this.txtName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtName_KeyDown);
			// 
			// lstFiles
			// 
			this.lstFiles.Enabled = false;
			this.lstFiles.FormattingEnabled = true;
			this.lstFiles.Location = new System.Drawing.Point(112, 366);
			this.lstFiles.Name = "lstFiles";
			this.lstFiles.Size = new System.Drawing.Size(514, 69);
			this.lstFiles.TabIndex = 4;
			this.lstFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstFiles_KeyDown);
			// 
			// chkAttachFiles
			// 
			this.chkAttachFiles.AutoSize = true;
			this.chkAttachFiles.Location = new System.Drawing.Point(112, 343);
			this.chkAttachFiles.Name = "chkAttachFiles";
			this.chkAttachFiles.Size = new System.Drawing.Size(399, 17);
			this.chkAttachFiles.TabIndex = 3;
			this.chkAttachFiles.Text = "Прикрепить к отчету следующие файлы: (INS, DEL - для редактирования)";
			this.chkAttachFiles.UseVisualStyleBackColor = true;
			this.chkAttachFiles.CheckedChanged += new System.EventHandler(this.chkAttachFiles_CheckedChanged);
			this.chkAttachFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstFiles_KeyDown);
			// 
			// btnDelAttach
			// 
			this.btnDelAttach.Enabled = false;
			this.btnDelAttach.Location = new System.Drawing.Point(603, 343);
			this.btnDelAttach.Name = "btnDelAttach";
			this.btnDelAttach.Size = new System.Drawing.Size(23, 23);
			this.btnDelAttach.TabIndex = 12;
			this.btnDelAttach.TabStop = false;
			this.btnDelAttach.Text = "-";
			this.btnDelAttach.UseVisualStyleBackColor = true;
			this.btnDelAttach.Click += new System.EventHandler(this.btnDelAttach_Click);
			// 
			// btnAddAttach
			// 
			this.btnAddAttach.Enabled = false;
			this.btnAddAttach.Location = new System.Drawing.Point(574, 343);
			this.btnAddAttach.Name = "btnAddAttach";
			this.btnAddAttach.Size = new System.Drawing.Size(23, 23);
			this.btnAddAttach.TabIndex = 13;
			this.btnAddAttach.TabStop = false;
			this.btnAddAttach.Text = "+";
			this.btnAddAttach.UseVisualStyleBackColor = true;
			this.btnAddAttach.Click += new System.EventHandler(this.btnAddAttach_Click);
			// 
			// txtBugInfo
			// 
			this.txtBugInfo.Location = new System.Drawing.Point(112, 220);
			this.txtBugInfo.Multiline = true;
			this.txtBugInfo.Name = "txtBugInfo";
			this.txtBugInfo.ReadOnly = true;
			this.txtBugInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtBugInfo.Size = new System.Drawing.Size(514, 117);
			this.txtBugInfo.TabIndex = 14;
			this.txtBugInfo.WordWrap = false;
			// 
			// lblBugInfo
			// 
			this.lblBugInfo.AutoSize = true;
			this.lblBugInfo.Location = new System.Drawing.Point(12, 223);
			this.lblBugInfo.Name = "lblBugInfo";
			this.lblBugInfo.Size = new System.Drawing.Size(50, 13);
			this.lblBugInfo.TabIndex = 7;
			this.lblBugInfo.Text = "Error Info";
			// 
			// BugReport
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(638, 449);
			this.Controls.Add(this.txtBugInfo);
			this.Controls.Add(this.btnAddAttach);
			this.Controls.Add(this.btnDelAttach);
			this.Controls.Add(this.chkAttachFiles);
			this.Controls.Add(this.lstFiles);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblBugInfo);
			this.Controls.Add(this.lblDescription);
			this.Controls.Add(this.lblEmail);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.txtDescription);
			this.Controls.Add(this.txtEmail);
			this.Controls.Add(this.txtName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BugReport";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "BugReport";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstFiles_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.Label lblEmail;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.TextBox txtEmail;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.ListBox lstFiles;
		private System.Windows.Forms.CheckBox chkAttachFiles;
		private System.Windows.Forms.Button btnDelAttach;
		private System.Windows.Forms.Button btnAddAttach;
		private System.Windows.Forms.TextBox txtBugInfo;
		private System.Windows.Forms.Label lblBugInfo;
	}
}