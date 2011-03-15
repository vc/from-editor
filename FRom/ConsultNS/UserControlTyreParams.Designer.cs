namespace FRom.ConsultNS
{
	partial class UserControlTyreParams
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
			this.lblWidth = new System.Windows.Forms.Label();
			this.cmbWidth = new System.Windows.Forms.ComboBox();
			this.lblHeight = new System.Windows.Forms.Label();
			this.cmbHeight = new System.Windows.Forms.ComboBox();
			this.lblRadius = new System.Windows.Forms.Label();
			this.cmbRadius = new System.Windows.Forms.ComboBox();
			this.txtDiameter = new System.Windows.Forms.TextBox();
			this.lblDiameter = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblWidth
			// 
			this.lblWidth.AutoSize = true;
			this.lblWidth.Location = new System.Drawing.Point(-3, 0);
			this.lblWidth.Name = "lblWidth";
			this.lblWidth.Size = new System.Drawing.Size(51, 13);
			this.lblWidth.TabIndex = 0;
			this.lblWidth.Text = "Width (P)";
			// 
			// cmbWidth
			// 
			this.cmbWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbWidth.FormattingEnabled = true;
			this.cmbWidth.Location = new System.Drawing.Point(0, 16);
			this.cmbWidth.Name = "cmbWidth";
			this.cmbWidth.Size = new System.Drawing.Size(74, 21);
			this.cmbWidth.TabIndex = 2;
			// 
			// lblHeight
			// 
			this.lblHeight.AutoSize = true;
			this.lblHeight.Location = new System.Drawing.Point(77, 0);
			this.lblHeight.Name = "lblHeight";
			this.lblHeight.Size = new System.Drawing.Size(55, 13);
			this.lblHeight.TabIndex = 0;
			this.lblHeight.Text = "Height (H)";
			// 
			// cmbHeight
			// 
			this.cmbHeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbHeight.FormattingEnabled = true;
			this.cmbHeight.Location = new System.Drawing.Point(80, 16);
			this.cmbHeight.Name = "cmbHeight";
			this.cmbHeight.Size = new System.Drawing.Size(74, 21);
			this.cmbHeight.TabIndex = 2;
			// 
			// lblRadius
			// 
			this.lblRadius.AutoSize = true;
			this.lblRadius.Location = new System.Drawing.Point(157, 0);
			this.lblRadius.Name = "lblRadius";
			this.lblRadius.Size = new System.Drawing.Size(56, 13);
			this.lblRadius.TabIndex = 0;
			this.lblRadius.Text = "Radius (C)";
			// 
			// cmbRadius
			// 
			this.cmbRadius.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbRadius.FormattingEnabled = true;
			this.cmbRadius.Location = new System.Drawing.Point(160, 16);
			this.cmbRadius.Name = "cmbRadius";
			this.cmbRadius.Size = new System.Drawing.Size(53, 21);
			this.cmbRadius.TabIndex = 2;
			// 
			// txtDiameter
			// 
			this.txtDiameter.Location = new System.Drawing.Point(219, 16);
			this.txtDiameter.Name = "txtDiameter";
			this.txtDiameter.ReadOnly = true;
			this.txtDiameter.Size = new System.Drawing.Size(60, 20);
			this.txtDiameter.TabIndex = 3;
			// 
			// lblDiameter
			// 
			this.lblDiameter.AutoSize = true;
			this.lblDiameter.Location = new System.Drawing.Point(216, 0);
			this.lblDiameter.Name = "lblDiameter";
			this.lblDiameter.Size = new System.Drawing.Size(66, 13);
			this.lblDiameter.TabIndex = 0;
			this.lblDiameter.Text = "Diameter (D)";
			// 
			// UserControlTyreParams
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.Controls.Add(this.txtDiameter);
			this.Controls.Add(this.cmbRadius);
			this.Controls.Add(this.cmbHeight);
			this.Controls.Add(this.cmbWidth);
			this.Controls.Add(this.lblDiameter);
			this.Controls.Add(this.lblRadius);
			this.Controls.Add(this.lblHeight);
			this.Controls.Add(this.lblWidth);
			this.Name = "UserControlTyreParams";
			this.Size = new System.Drawing.Size(285, 40);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblWidth;
		private System.Windows.Forms.ComboBox cmbWidth;
		private System.Windows.Forms.Label lblHeight;
		private System.Windows.Forms.ComboBox cmbHeight;
		private System.Windows.Forms.Label lblRadius;
		private System.Windows.Forms.ComboBox cmbRadius;
		private System.Windows.Forms.TextBox txtDiameter;
		private System.Windows.Forms.Label lblDiameter;
	}
}
