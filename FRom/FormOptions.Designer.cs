namespace FRom
{
	partial class FormSettings
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
			this.grpEmulator = new System.Windows.Forms.GroupBox();
			this.chkEmulatorSaveROMToFileAfterDownload = new System.Windows.Forms.CheckBox();
			this.chkEmulatorAutoConnect = new System.Windows.Forms.CheckBox();
			this.lblStatusEmulator = new System.Windows.Forms.Label();
			this.cbEmulatorPort = new System.Windows.Forms.ComboBox();
			this.btnEmulatorTest = new System.Windows.Forms.Button();
			this.lblPortEmulator = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.grpConsult = new System.Windows.Forms.GroupBox();
			this.chkConsultKeepALive = new System.Windows.Forms.CheckBox();
			this.chkConsultAutoConnect = new System.Windows.Forms.CheckBox();
			this.lblStatusConsult = new System.Windows.Forms.Label();
			this.btnConsultTest = new System.Windows.Forms.Button();
			this.cbConsultPort = new System.Windows.Forms.ComboBox();
			this.lblPortConsult = new System.Windows.Forms.Label();
			this.grpPathes = new System.Windows.Forms.GroupBox();
			this.chkBINPathRememberLast = new System.Windows.Forms.CheckBox();
			this.chkADRPathRememberLast = new System.Windows.Forms.CheckBox();
			this.chkAutoLoadFiles = new System.Windows.Forms.CheckBox();
			this.btnPathDialogBIN = new System.Windows.Forms.Button();
			this.btnPathDialogADR = new System.Windows.Forms.Button();
			this.txtBINFilesPath = new System.Windows.Forms.TextBox();
			this.txtADRFilesPath = new System.Windows.Forms.TextBox();
			this.lblPathBIN = new System.Windows.Forms.Label();
			this.lblPathADR = new System.Windows.Forms.Label();
			this.grpLogs = new System.Windows.Forms.GroupBox();
			this.btnConsultScan = new System.Windows.Forms.Button();
			this.grpEmulator.SuspendLayout();
			this.grpConsult.SuspendLayout();
			this.grpPathes.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpEmulator
			// 
			this.grpEmulator.Controls.Add(this.chkEmulatorSaveROMToFileAfterDownload);
			this.grpEmulator.Controls.Add(this.chkEmulatorAutoConnect);
			this.grpEmulator.Controls.Add(this.lblStatusEmulator);
			this.grpEmulator.Controls.Add(this.cbEmulatorPort);
			this.grpEmulator.Controls.Add(this.btnEmulatorTest);
			this.grpEmulator.Controls.Add(this.lblPortEmulator);
			this.grpEmulator.Location = new System.Drawing.Point(12, 12);
			this.grpEmulator.Name = "grpEmulator";
			this.grpEmulator.Size = new System.Drawing.Size(333, 99);
			this.grpEmulator.TabIndex = 5;
			this.grpEmulator.TabStop = false;
			this.grpEmulator.Text = "ROM Emulator Settings";
			// 
			// chkEmulatorSaveROMToFileAfterDownload
			// 
			this.chkEmulatorSaveROMToFileAfterDownload.AutoSize = true;
			this.chkEmulatorSaveROMToFileAfterDownload.Location = new System.Drawing.Point(9, 75);
			this.chkEmulatorSaveROMToFileAfterDownload.Name = "chkEmulatorSaveROMToFileAfterDownload";
			this.chkEmulatorSaveROMToFileAfterDownload.Size = new System.Drawing.Size(206, 17);
			this.chkEmulatorSaveROMToFileAfterDownload.TabIndex = 13;
			this.chkEmulatorSaveROMToFileAfterDownload.Text = "Save ROM data to file after Download";
			this.chkEmulatorSaveROMToFileAfterDownload.UseVisualStyleBackColor = true;
			// 
			// chkEmulatorAutoConnect
			// 
			this.chkEmulatorAutoConnect.AutoSize = true;
			this.chkEmulatorAutoConnect.Location = new System.Drawing.Point(9, 52);
			this.chkEmulatorAutoConnect.Name = "chkEmulatorAutoConnect";
			this.chkEmulatorAutoConnect.Size = new System.Drawing.Size(113, 17);
			this.chkEmulatorAutoConnect.TabIndex = 13;
			this.chkEmulatorAutoConnect.Text = "Connect at startup";
			this.chkEmulatorAutoConnect.UseVisualStyleBackColor = true;
			// 
			// lblStatusEmulator
			// 
			this.lblStatusEmulator.AutoSize = true;
			this.lblStatusEmulator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblStatusEmulator.Location = new System.Drawing.Point(6, 36);
			this.lblStatusEmulator.MaximumSize = new System.Drawing.Size(280, 26);
			this.lblStatusEmulator.Name = "lblStatusEmulator";
			this.lblStatusEmulator.Size = new System.Drawing.Size(154, 13);
			this.lblStatusEmulator.TabIndex = 10;
			this.lblStatusEmulator.Text = "Press \'Test\' to initialize ...";
			// 
			// cbEmulatorPort
			// 
			this.cbEmulatorPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbEmulatorPort.FormattingEnabled = true;
			this.cbEmulatorPort.Location = new System.Drawing.Point(121, 12);
			this.cbEmulatorPort.Name = "cbEmulatorPort";
			this.cbEmulatorPort.Size = new System.Drawing.Size(100, 21);
			this.cbEmulatorPort.TabIndex = 9;
			// 
			// btnEmulatorTest
			// 
			this.btnEmulatorTest.Location = new System.Drawing.Point(227, 11);
			this.btnEmulatorTest.Name = "btnEmulatorTest";
			this.btnEmulatorTest.Size = new System.Drawing.Size(75, 23);
			this.btnEmulatorTest.TabIndex = 8;
			this.btnEmulatorTest.Text = "Test";
			this.btnEmulatorTest.UseVisualStyleBackColor = true;
			this.btnEmulatorTest.Click += new System.EventHandler(this.btnEmulatorTest_Click);
			// 
			// lblPortEmulator
			// 
			this.lblPortEmulator.AutoSize = true;
			this.lblPortEmulator.Location = new System.Drawing.Point(6, 16);
			this.lblPortEmulator.Name = "lblPortEmulator";
			this.lblPortEmulator.Size = new System.Drawing.Size(50, 13);
			this.lblPortEmulator.TabIndex = 5;
			this.lblPortEmulator.Text = "Com Port";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(182, 342);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(101, 342);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 9;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// grpConsult
			// 
			this.grpConsult.Controls.Add(this.chkConsultKeepALive);
			this.grpConsult.Controls.Add(this.chkConsultAutoConnect);
			this.grpConsult.Controls.Add(this.lblStatusConsult);
			this.grpConsult.Controls.Add(this.btnConsultScan);
			this.grpConsult.Controls.Add(this.btnConsultTest);
			this.grpConsult.Controls.Add(this.cbConsultPort);
			this.grpConsult.Controls.Add(this.lblPortConsult);
			this.grpConsult.Location = new System.Drawing.Point(12, 117);
			this.grpConsult.Name = "grpConsult";
			this.grpConsult.Size = new System.Drawing.Size(333, 74);
			this.grpConsult.TabIndex = 10;
			this.grpConsult.TabStop = false;
			this.grpConsult.Text = "Nissan Consult Settings";
			// 
			// chkConsultKeepALive
			// 
			this.chkConsultKeepALive.AutoSize = true;
			this.chkConsultKeepALive.Location = new System.Drawing.Point(128, 53);
			this.chkConsultKeepALive.Name = "chkConsultKeepALive";
			this.chkConsultKeepALive.Size = new System.Drawing.Size(79, 17);
			this.chkConsultKeepALive.TabIndex = 12;
			this.chkConsultKeepALive.Text = "Keep a live";
			this.chkConsultKeepALive.UseVisualStyleBackColor = true;
			// 
			// chkConsultAutoConnect
			// 
			this.chkConsultAutoConnect.AutoSize = true;
			this.chkConsultAutoConnect.Location = new System.Drawing.Point(9, 53);
			this.chkConsultAutoConnect.Name = "chkConsultAutoConnect";
			this.chkConsultAutoConnect.Size = new System.Drawing.Size(113, 17);
			this.chkConsultAutoConnect.TabIndex = 12;
			this.chkConsultAutoConnect.Text = "Connect at startup";
			this.chkConsultAutoConnect.UseVisualStyleBackColor = true;
			// 
			// lblStatusConsult
			// 
			this.lblStatusConsult.AutoSize = true;
			this.lblStatusConsult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblStatusConsult.Location = new System.Drawing.Point(6, 37);
			this.lblStatusConsult.MaximumSize = new System.Drawing.Size(280, 26);
			this.lblStatusConsult.Name = "lblStatusConsult";
			this.lblStatusConsult.Size = new System.Drawing.Size(154, 13);
			this.lblStatusConsult.TabIndex = 10;
			this.lblStatusConsult.Text = "Press \'Test\' to initialize ...";
			// 
			// btnConsultTest
			// 
			this.btnConsultTest.Location = new System.Drawing.Point(171, 11);
			this.btnConsultTest.Name = "btnConsultTest";
			this.btnConsultTest.Size = new System.Drawing.Size(75, 23);
			this.btnConsultTest.TabIndex = 10;
			this.btnConsultTest.Text = "Test";
			this.btnConsultTest.UseVisualStyleBackColor = true;
			this.btnConsultTest.Click += new System.EventHandler(this.btnConsultTest_Click);
			// 
			// cbConsultPort
			// 
			this.cbConsultPort.BackColor = System.Drawing.SystemColors.Window;
			this.cbConsultPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbConsultPort.ForeColor = System.Drawing.SystemColors.WindowText;
			this.cbConsultPort.FormattingEnabled = true;
			this.cbConsultPort.Location = new System.Drawing.Point(65, 13);
			this.cbConsultPort.Name = "cbConsultPort";
			this.cbConsultPort.Size = new System.Drawing.Size(100, 21);
			this.cbConsultPort.TabIndex = 9;
			this.cbConsultPort.SelectedIndexChanged += new System.EventHandler(this.cbConsultPort_SelectedIndexChanged);
			// 
			// lblPortConsult
			// 
			this.lblPortConsult.AutoSize = true;
			this.lblPortConsult.Location = new System.Drawing.Point(6, 16);
			this.lblPortConsult.Name = "lblPortConsult";
			this.lblPortConsult.Size = new System.Drawing.Size(53, 13);
			this.lblPortConsult.TabIndex = 5;
			this.lblPortConsult.Text = "COM Port";
			// 
			// grpPathes
			// 
			this.grpPathes.Controls.Add(this.chkBINPathRememberLast);
			this.grpPathes.Controls.Add(this.chkADRPathRememberLast);
			this.grpPathes.Controls.Add(this.chkAutoLoadFiles);
			this.grpPathes.Controls.Add(this.btnPathDialogBIN);
			this.grpPathes.Controls.Add(this.btnPathDialogADR);
			this.grpPathes.Controls.Add(this.txtBINFilesPath);
			this.grpPathes.Controls.Add(this.txtADRFilesPath);
			this.grpPathes.Controls.Add(this.lblPathBIN);
			this.grpPathes.Controls.Add(this.lblPathADR);
			this.grpPathes.Location = new System.Drawing.Point(12, 197);
			this.grpPathes.Name = "grpPathes";
			this.grpPathes.Size = new System.Drawing.Size(333, 118);
			this.grpPathes.TabIndex = 11;
			this.grpPathes.TabStop = false;
			this.grpPathes.Text = "Default Directories";
			// 
			// chkBINPathRememberLast
			// 
			this.chkBINPathRememberLast.AutoSize = true;
			this.chkBINPathRememberLast.Location = new System.Drawing.Point(141, 54);
			this.chkBINPathRememberLast.Name = "chkBINPathRememberLast";
			this.chkBINPathRememberLast.Size = new System.Drawing.Size(125, 17);
			this.chkBINPathRememberLast.TabIndex = 3;
			this.chkBINPathRememberLast.Text = "Remember last folder";
			this.chkBINPathRememberLast.UseVisualStyleBackColor = true;
			// 
			// chkADRPathRememberLast
			// 
			this.chkADRPathRememberLast.AutoSize = true;
			this.chkADRPathRememberLast.Location = new System.Drawing.Point(141, 12);
			this.chkADRPathRememberLast.Name = "chkADRPathRememberLast";
			this.chkADRPathRememberLast.Size = new System.Drawing.Size(125, 17);
			this.chkADRPathRememberLast.TabIndex = 3;
			this.chkADRPathRememberLast.Text = "Remember last folder";
			this.chkADRPathRememberLast.UseVisualStyleBackColor = true;
			// 
			// chkAutoLoadFiles
			// 
			this.chkAutoLoadFiles.AutoSize = true;
			this.chkAutoLoadFiles.Location = new System.Drawing.Point(9, 97);
			this.chkAutoLoadFiles.Name = "chkAutoLoadFiles";
			this.chkAutoLoadFiles.Size = new System.Drawing.Size(142, 17);
			this.chkAutoLoadFiles.TabIndex = 3;
			this.chkAutoLoadFiles.Text = "Auto load files on startup";
			this.chkAutoLoadFiles.UseVisualStyleBackColor = true;
			// 
			// btnPathDialogBIN
			// 
			this.btnPathDialogBIN.Enabled = false;
			this.btnPathDialogBIN.Location = new System.Drawing.Point(272, 71);
			this.btnPathDialogBIN.Name = "btnPathDialogBIN";
			this.btnPathDialogBIN.Size = new System.Drawing.Size(30, 20);
			this.btnPathDialogBIN.TabIndex = 2;
			this.btnPathDialogBIN.Text = "...";
			this.btnPathDialogBIN.UseVisualStyleBackColor = true;
			// 
			// btnPathDialogADR
			// 
			this.btnPathDialogADR.Enabled = false;
			this.btnPathDialogADR.Location = new System.Drawing.Point(272, 32);
			this.btnPathDialogADR.Name = "btnPathDialogADR";
			this.btnPathDialogADR.Size = new System.Drawing.Size(30, 20);
			this.btnPathDialogADR.TabIndex = 2;
			this.btnPathDialogADR.Text = "...";
			this.btnPathDialogADR.UseVisualStyleBackColor = true;
			// 
			// txtBINFilesPath
			// 
			this.txtBINFilesPath.Location = new System.Drawing.Point(9, 71);
			this.txtBINFilesPath.Name = "txtBINFilesPath";
			this.txtBINFilesPath.Size = new System.Drawing.Size(257, 20);
			this.txtBINFilesPath.TabIndex = 1;
			// 
			// txtADRFilesPath
			// 
			this.txtADRFilesPath.Location = new System.Drawing.Point(9, 32);
			this.txtADRFilesPath.Name = "txtADRFilesPath";
			this.txtADRFilesPath.Size = new System.Drawing.Size(257, 20);
			this.txtADRFilesPath.TabIndex = 1;
			// 
			// lblPathBIN
			// 
			this.lblPathBIN.AutoSize = true;
			this.lblPathBIN.Location = new System.Drawing.Point(6, 55);
			this.lblPathBIN.Name = "lblPathBIN";
			this.lblPathBIN.Size = new System.Drawing.Size(73, 13);
			this.lblPathBIN.TabIndex = 0;
			this.lblPathBIN.Text = "BIN Files path";
			// 
			// lblPathADR
			// 
			this.lblPathADR.AutoSize = true;
			this.lblPathADR.Location = new System.Drawing.Point(6, 16);
			this.lblPathADR.Name = "lblPathADR";
			this.lblPathADR.Size = new System.Drawing.Size(78, 13);
			this.lblPathADR.TabIndex = 0;
			this.lblPathADR.Text = "ADR Files path";
			// 
			// grpLogs
			// 
			this.grpLogs.Location = new System.Drawing.Point(351, 12);
			this.grpLogs.Name = "grpLogs";
			this.grpLogs.Size = new System.Drawing.Size(293, 99);
			this.grpLogs.TabIndex = 12;
			this.grpLogs.TabStop = false;
			this.grpLogs.Text = "Log";
			this.grpLogs.Visible = false;
			// 
			// btnConsultScan
			// 
			this.btnConsultScan.Location = new System.Drawing.Point(252, 11);
			this.btnConsultScan.Name = "btnConsultScan";
			this.btnConsultScan.Size = new System.Drawing.Size(75, 23);
			this.btnConsultScan.TabIndex = 10;
			this.btnConsultScan.Text = "Detect";
			this.btnConsultScan.UseVisualStyleBackColor = true;
			this.btnConsultScan.Click += new System.EventHandler(this.btnConsultScan_Click);
			// 
			// FormSettings
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(359, 389);
			this.Controls.Add(this.grpLogs);
			this.Controls.Add(this.grpPathes);
			this.Controls.Add(this.grpConsult);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.grpEmulator);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.HelpButton = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(342, 121);
			this.Name = "FormSettings";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "FRom Configuration";
			this.grpEmulator.ResumeLayout(false);
			this.grpEmulator.PerformLayout();
			this.grpConsult.ResumeLayout(false);
			this.grpConsult.PerformLayout();
			this.grpPathes.ResumeLayout(false);
			this.grpPathes.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpEmulator;
		private System.Windows.Forms.Label lblStatusEmulator;
		private System.Windows.Forms.ComboBox cbEmulatorPort;
		private System.Windows.Forms.Button btnEmulatorTest;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.GroupBox grpConsult;
		private System.Windows.Forms.ComboBox cbConsultPort;
		private System.Windows.Forms.Label lblPortConsult;
		private System.Windows.Forms.Label lblStatusConsult;
		private System.Windows.Forms.Button btnConsultTest;
		private System.Windows.Forms.Label lblPortEmulator;
		private System.Windows.Forms.GroupBox grpPathes;
		private System.Windows.Forms.Label lblPathBIN;
		private System.Windows.Forms.Label lblPathADR;
		private System.Windows.Forms.Button btnPathDialogBIN;
		private System.Windows.Forms.Button btnPathDialogADR;
		private System.Windows.Forms.TextBox txtBINFilesPath;
		private System.Windows.Forms.TextBox txtADRFilesPath;
		private System.Windows.Forms.CheckBox chkAutoLoadFiles;
		private System.Windows.Forms.CheckBox chkEmulatorAutoConnect;
		private System.Windows.Forms.CheckBox chkConsultAutoConnect;
		private System.Windows.Forms.CheckBox chkBINPathRememberLast;
		private System.Windows.Forms.CheckBox chkADRPathRememberLast;
		private System.Windows.Forms.CheckBox chkEmulatorSaveROMToFileAfterDownload;
		private System.Windows.Forms.CheckBox chkConsultKeepALive;
		private System.Windows.Forms.GroupBox grpLogs;
		private System.Windows.Forms.Button btnConsultScan;

	}
}