namespace FRom.ConsultNS
{
	partial class FormLiveScan
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLiveScan));
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripContainer2 = new System.Windows.Forms.ToolStripContainer();
			this.panel = new System.Windows.Forms.Panel();
			this.toolStripButtonsMain = new System.Windows.Forms.ToolStrip();
			this.btnStartStop = new System.Windows.Forms.ToolStripButton();
			this.btnClose = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnSensors = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripContainer1.SuspendLayout();
			this.toolStripContainer2.ContentPanel.SuspendLayout();
			this.toolStripContainer2.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer2.SuspendLayout();
			this.toolStripButtonsMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(600, 200);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.Size = new System.Drawing.Size(600, 225);
			this.toolStripContainer1.TabIndex = 0;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.exitToolStripMenuItem.Text = "&Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// toolStripContainer2
			// 
			// 
			// toolStripContainer2.ContentPanel
			// 
			this.toolStripContainer2.ContentPanel.Controls.Add(this.panel);
			this.toolStripContainer2.ContentPanel.Size = new System.Drawing.Size(600, 200);
			this.toolStripContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer2.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer2.Name = "toolStripContainer2";
			this.toolStripContainer2.Size = new System.Drawing.Size(600, 225);
			this.toolStripContainer2.TabIndex = 1;
			this.toolStripContainer2.Text = "toolStripContainer2";
			// 
			// toolStripContainer2.TopToolStripPanel
			// 
			this.toolStripContainer2.TopToolStripPanel.Controls.Add(this.toolStripButtonsMain);
			// 
			// panel
			// 
			this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel.Location = new System.Drawing.Point(0, 0);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(600, 200);
			this.panel.TabIndex = 1;
			// 
			// toolStripButtonsMain
			// 
			this.toolStripButtonsMain.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStripButtonsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStripButtonsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStartStop,
            this.btnClose,
            this.toolStripSeparator1,
            this.btnSensors,
            this.toolStripSeparator2});
			this.toolStripButtonsMain.Location = new System.Drawing.Point(0, 0);
			this.toolStripButtonsMain.Name = "toolStripButtonsMain";
			this.toolStripButtonsMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.toolStripButtonsMain.Size = new System.Drawing.Size(600, 25);
			this.toolStripButtonsMain.Stretch = true;
			this.toolStripButtonsMain.TabIndex = 1;
			// 
			// btnStartStop
			// 
			this.btnStartStop.CheckOnClick = true;
			this.btnStartStop.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnStartStop.Name = "btnStartStop";
			this.btnStartStop.Size = new System.Drawing.Size(35, 22);
			this.btnStartStop.Tag = "&Start|&Stop";
			this.btnStartStop.Text = "Start";
			this.btnStartStop.ToolTipText = "Start";
			this.btnStartStop.CheckedChanged += new System.EventHandler(this.mnuStartStop_CheckedChanged);
			this.btnStartStop.Click += new System.EventHandler(this.mnuStartStop_Click);
			// 
			// btnClose
			// 
			this.btnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
			this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(37, 22);
			this.btnClose.Text = "Close";
			this.btnClose.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnSensors
			// 
			this.btnSensors.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSensors.Image = global::Helper.Properties.Resources.pngPage_white_add;
			this.btnSensors.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSensors.Name = "btnSensors";
			this.btnSensors.Size = new System.Drawing.Size(23, 22);
			this.btnSensors.Text = "Sensors select";
			this.btnSensors.ToolTipText = "Select sensors to monitoring";
			this.btnSensors.Click += new System.EventHandler(this.btnSensors_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// FormLiveScan
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(600, 225);
			this.Controls.Add(this.toolStripContainer2);
			this.Controls.Add(this.toolStripContainer1);
			this.DoubleBuffered = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormLiveScan";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "ConsultForm";
			this.SizeChanged += new System.EventHandler(this.ConsultForm_SizeChanged);
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.toolStripContainer2.ContentPanel.ResumeLayout(false);
			this.toolStripContainer2.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer2.TopToolStripPanel.PerformLayout();
			this.toolStripContainer2.ResumeLayout(false);
			this.toolStripContainer2.PerformLayout();
			this.toolStripButtonsMain.ResumeLayout(false);
			this.toolStripButtonsMain.PerformLayout();
			this.ResumeLayout(false);

		}


		#endregion

		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripContainer toolStripContainer2;
		private System.Windows.Forms.ToolStrip toolStripButtonsMain;
		private System.Windows.Forms.ToolStripButton btnStartStop;
		private System.Windows.Forms.ToolStripButton btnClose;
		internal System.Windows.Forms.Panel panel;
		internal System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnSensors;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
	}
}