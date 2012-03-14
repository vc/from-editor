namespace FRom
{
	partial class FormConsultActiveTest
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
            this.userControlOnOff1 = new FRom.Consult.ActiveTests.UserControlOnOff();
            this.userControlDescrette1 = new FRom.Consult.ActiveTests.UserControlDescrette();
            this.ctrlScallable = new FRom.Consult.ActiveTests.UserControlScallable();
            this.SuspendLayout();
            // 
            // userControlOnOff1
            // 
            this.userControlOnOff1.AutoSize = true;
            this.userControlOnOff1.Location = new System.Drawing.Point(199, 12);
            this.userControlOnOff1.Name = "userControlOnOff1";
            this.userControlOnOff1.Size = new System.Drawing.Size(180, 53);
            this.userControlOnOff1.TabIndex = 2;
            this.userControlOnOff1.Value = false;
            // 
            // userControlDescrette1
            // 
            this.userControlDescrette1.AutoSize = true;
            this.userControlDescrette1.Location = new System.Drawing.Point(13, 94);
            this.userControlDescrette1.Name = "userControlDescrette1";
            this.userControlDescrette1.ShowRadioButtons = false;
            this.userControlDescrette1.Size = new System.Drawing.Size(180, 128);
            this.userControlDescrette1.TabIndex = 1;
            // 
            // ctrlScallable
            // 
            this.ctrlScallable.AutoSize = true;
            this.ctrlScallable.Location = new System.Drawing.Point(13, 13);
            this.ctrlScallable.Name = "ctrlScallable";
            this.ctrlScallable.Size = new System.Drawing.Size(180, 74);
            this.ctrlScallable.TabIndex = 0;
            // 
            // FormConsultActiveTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 235);
            this.Controls.Add(this.userControlOnOff1);
            this.Controls.Add(this.userControlDescrette1);
            this.Controls.Add(this.ctrlScallable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConsultActiveTest";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FormConsultActiveTest";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private global::FRom.Consult.ActiveTests.UserControlScallable ctrlScallable;
		private global::FRom.Consult.ActiveTests.UserControlDescrette userControlDescrette1;
        private Consult.ActiveTests.UserControlOnOff userControlOnOff1;

	}
}