namespace FRom.ConsultNS
{
	partial class FormSpeedTrial
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
			this.grpResults = new System.Windows.Forms.GroupBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.grpCurrentSpeed = new System.Windows.Forms.GroupBox();
			this.lblCurrentSpeed = new System.Windows.Forms.Label();
			this.btnStartStop = new System.Windows.Forms.Button();
			this.btnExit = new System.Windows.Forms.Button();
			this.grpResults.SuspendLayout();
			this.grpCurrentSpeed.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpResults
			// 
			this.grpResults.Controls.Add(this.textBox1);
			this.grpResults.Location = new System.Drawing.Point(12, 12);
			this.grpResults.Name = "grpResults";
			this.grpResults.Size = new System.Drawing.Size(284, 181);
			this.grpResults.TabIndex = 1;
			this.grpResults.TabStop = false;
			this.grpResults.Text = "Result";
			// 
			// textBox1
			// 
			this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.textBox1.Location = new System.Drawing.Point(3, 16);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(278, 162);
			this.textBox1.TabIndex = 1;
			// 
			// grpCurrentSpeed
			// 
			this.grpCurrentSpeed.Controls.Add(this.lblCurrentSpeed);
			this.grpCurrentSpeed.Location = new System.Drawing.Point(302, 12);
			this.grpCurrentSpeed.Name = "grpCurrentSpeed";
			this.grpCurrentSpeed.Size = new System.Drawing.Size(92, 67);
			this.grpCurrentSpeed.TabIndex = 1;
			this.grpCurrentSpeed.TabStop = false;
			this.grpCurrentSpeed.Text = "Current Speed";
			// 
			// lblCurrentSpeed
			// 
			this.lblCurrentSpeed.AutoSize = true;
			this.lblCurrentSpeed.Font = new System.Drawing.Font("Arial", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lblCurrentSpeed.Location = new System.Drawing.Point(6, 16);
			this.lblCurrentSpeed.Name = "lblCurrentSpeed";
			this.lblCurrentSpeed.Size = new System.Drawing.Size(74, 42);
			this.lblCurrentSpeed.TabIndex = 4;
			this.lblCurrentSpeed.Text = "- - -";
			// 
			// btnStartStop
			// 
			this.btnStartStop.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnStartStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnStartStop.Location = new System.Drawing.Point(303, 85);
			this.btnStartStop.Name = "btnStartStop";
			this.btnStartStop.Size = new System.Drawing.Size(91, 51);
			this.btnStartStop.TabIndex = 0;
			this.btnStartStop.Tag = "&Start|&Stop";
			this.btnStartStop.Text = "Start";
			this.btnStartStop.UseVisualStyleBackColor = true;
			this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
			// 
			// btnExit
			// 
			this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.btnExit.Location = new System.Drawing.Point(303, 142);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(91, 51);
			this.btnExit.TabIndex = 2;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// FormSpeedTrial
			// 
			this.AcceptButton = this.btnStartStop;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnExit;
			this.ClientSize = new System.Drawing.Size(406, 200);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.btnStartStop);
			this.Controls.Add(this.grpCurrentSpeed);
			this.Controls.Add(this.grpResults);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormSpeedTrial";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Speed Trial";
			this.grpResults.ResumeLayout(false);
			this.grpResults.PerformLayout();
			this.grpCurrentSpeed.ResumeLayout(false);
			this.grpCurrentSpeed.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox grpResults;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.GroupBox grpCurrentSpeed;
		private System.Windows.Forms.Label lblCurrentSpeed;
		private System.Windows.Forms.Button btnStartStop;
		private System.Windows.Forms.Button btnExit;

	}
}