namespace FRom
{
	partial class FormTyreCalc
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTyreCalc));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.ctlTyresOriginal = new FRom.Consult.UserControlTyreParams();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.ctlTyresNew = new FRom.Consult.UserControlTyreParams();
			this.label1 = new System.Windows.Forms.Label();
			this.txtAccurancy = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(310, 320);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.ctlTyresOriginal);
			this.groupBox1.Location = new System.Drawing.Point(26, 339);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(281, 68);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Original Tyres";
			// 
			// ctlTyresOriginal
			// 
			this.ctlTyresOriginal.AutoSize = true;
			this.ctlTyresOriginal.Location = new System.Drawing.Point(6, 19);
			this.ctlTyresOriginal.Name = "ctlTyresOriginal";
			this.ctlTyresOriginal.Size = new System.Drawing.Size(268, 40);
			this.ctlTyresOriginal.TabIndex = 0;
			this.ctlTyresOriginal.SizesChanged += new FRom.Consult.UserControlTyreParams.SizeChangedEventHandler(this.ctlTyresOriginal_SizesChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.ctlTyresNew);
			this.groupBox2.Location = new System.Drawing.Point(26, 413);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(281, 89);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Current Tyres";
			// 
			// ctlTyresNew
			// 
			this.ctlTyresNew.AutoSize = true;
			this.ctlTyresNew.Location = new System.Drawing.Point(6, 19);
			this.ctlTyresNew.Name = "ctlTyresNew";
			this.ctlTyresNew.ShowRecomendedParams = true;
			this.ctlTyresNew.Size = new System.Drawing.Size(268, 61);
			this.ctlTyresNew.TabIndex = 0;
			this.ctlTyresNew.SizesChanged += new FRom.Consult.UserControlTyreParams.SizeChangedEventHandler(this.ctlTyresNew_SizesChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(103, 511);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Acuracy speed";
			// 
			// txtAccurancy
			// 
			this.txtAccurancy.Location = new System.Drawing.Point(187, 508);
			this.txtAccurancy.Name = "txtAccurancy";
			this.txtAccurancy.ReadOnly = true;
			this.txtAccurancy.Size = new System.Drawing.Size(57, 20);
			this.txtAccurancy.TabIndex = 4;
			this.txtAccurancy.Tag = "%";
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(88, 534);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(169, 534);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// FormTyreCalc
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(332, 566);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.txtAccurancy);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormTyreCalc";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Tyre Calc";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtAccurancy;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private FRom.Consult.UserControlTyreParams ctlTyresOriginal;
		private FRom.Consult.UserControlTyreParams ctlTyresNew;
	}
}