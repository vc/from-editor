namespace FRom.ConsultNS.ActiveTests
{
	partial class UserControlOnOff
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.grpOnOff = new System.Windows.Forms.GroupBox();
			this.chkOnOff = new System.Windows.Forms.CheckBox();
			this.btnApply = new System.Windows.Forms.Button();
			this.grpOnOff.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpOnOff
			// 
			this.grpOnOff.Controls.Add(this.chkOnOff);
			this.grpOnOff.Controls.Add(this.btnApply);
			this.grpOnOff.Location = new System.Drawing.Point(3, 3);
			this.grpOnOff.Name = "grpOnOff";
			this.grpOnOff.Size = new System.Drawing.Size(174, 47);
			this.grpOnOff.TabIndex = 0;
			this.grpOnOff.TabStop = false;
			// 
			// chkOnOff
			// 
			this.chkOnOff.AutoSize = true;
			this.chkOnOff.Location = new System.Drawing.Point(6, 19);
			this.chkOnOff.Name = "chkOnOff";
			this.chkOnOff.Size = new System.Drawing.Size(59, 17);
			this.chkOnOff.TabIndex = 5;
			this.chkOnOff.Tag = "On|Off";
			this.chkOnOff.Text = "On/Off";
			this.chkOnOff.UseVisualStyleBackColor = true;
			this.chkOnOff.CheckedChanged += new System.EventHandler(this.chkOnOff_CheckedChanged);
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(93, 15);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 23);
			this.btnApply.TabIndex = 4;
			this.btnApply.Text = "Apply";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// UserControlOnOff
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.grpOnOff);
			this.Name = "UserControlOnOff";
			this.Size = new System.Drawing.Size(180, 53);
			this.grpOnOff.ResumeLayout(false);
			this.grpOnOff.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpOnOff;
		private System.Windows.Forms.CheckBox chkOnOff;
		private System.Windows.Forms.Button btnApply;

	}
}
