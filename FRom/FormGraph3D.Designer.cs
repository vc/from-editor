namespace FRom
{
	partial class FormGraph3D
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
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.cbViewMode = new System.Windows.Forms.ToolStripComboBox();
			this.tsMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// tsMain
			// 
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbViewMode});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.tsMain.Size = new System.Drawing.Size(626, 25);
			this.tsMain.TabIndex = 0;
			this.tsMain.Text = "toolStripMain";
			// 
			// cbViewMode
			// 
			this.cbViewMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbViewMode.Items.AddRange(new object[] {
            "GL_FILL",
            "GL_LINE",
            "GL_POINT"});
			this.cbViewMode.Name = "cbViewMode";
			this.cbViewMode.Size = new System.Drawing.Size(121, 25);
			this.cbViewMode.SelectedIndexChanged += new System.EventHandler(this.cbViewMode_SelectedIndexChanged);
			// 
			// Graph
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(626, 426);
			this.ControlBox = false;
			this.Controls.Add(this.tsMain);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Graph";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Graph";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Graph_FormClosed);
			this.Load += new System.EventHandler(this.Graph_Load);
			this.ResizeEnd += new System.EventHandler(this.Graph_ResizeEnd);
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.ToolStripComboBox cbViewMode;
	}
}