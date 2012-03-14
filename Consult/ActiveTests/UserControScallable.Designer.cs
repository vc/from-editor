namespace FRom.Consult.ActiveTests
{
	partial class UserControlScallable
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
			this.btnApply = new System.Windows.Forms.Button();
			this.trBarTemp = new System.Windows.Forms.TrackBar();
			this.txtCoolantTemp = new System.Windows.Forms.TextBox();
			this.lblDescr = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.trBarTemp)).BeginInit();
			this.SuspendLayout();
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(102, 48);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(75, 23);
			this.btnApply.TabIndex = 0;
			this.btnApply.Text = "Apply";
			this.btnApply.UseVisualStyleBackColor = true;
			// 
			// trBarTemp
			// 
			this.trBarTemp.Location = new System.Drawing.Point(3, 3);
			this.trBarTemp.Maximum = 150;
			this.trBarTemp.Minimum = -50;
			this.trBarTemp.Name = "trBarTemp";
			this.trBarTemp.Size = new System.Drawing.Size(174, 42);
			this.trBarTemp.TabIndex = 1;
			this.trBarTemp.TickFrequency = 10;
			// 
			// txtCoolantTemp
			// 
			this.txtCoolantTemp.Location = new System.Drawing.Point(3, 51);
			this.txtCoolantTemp.Name = "txtCoolantTemp";
			this.txtCoolantTemp.Size = new System.Drawing.Size(36, 20);
			this.txtCoolantTemp.TabIndex = 2;
			// 
			// lblDescr
			// 
			this.lblDescr.AutoSize = true;
			this.lblDescr.Location = new System.Drawing.Point(45, 53);
			this.lblDescr.Name = "lblDescr";
			this.lblDescr.Size = new System.Drawing.Size(18, 13);
			this.lblDescr.TabIndex = 3;
			this.lblDescr.Text = "°C";
			// 
			// ActiveTestCoolantTemp
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.lblDescr);
			this.Controls.Add(this.txtCoolantTemp);
			this.Controls.Add(this.trBarTemp);
			this.Controls.Add(this.btnApply);
			this.Name = "ActiveTestCoolantTemp";
			this.Size = new System.Drawing.Size(180, 74);
			((System.ComponentModel.ISupportInitialize)(this.trBarTemp)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.TrackBar trBarTemp;
		private System.Windows.Forms.TextBox txtCoolantTemp;
		private System.Windows.Forms.Label lblDescr;
	}
}
