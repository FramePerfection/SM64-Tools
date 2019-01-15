namespace SM64LevelEditor
{
    partial class LevelSettingsDialog
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
            this.cmbBank0x9_0x12 = new System.Windows.Forms.ComboBox();
            this.lblBank0x9_0x12 = new System.Windows.Forms.Label();
            this.cmbBank0x5_0xC = new System.Windows.Forms.ComboBox();
            this.lblBank0x5_0xC = new System.Windows.Forms.Label();
            this.cmbBank0x6_0xD = new System.Windows.Forms.ComboBox();
            this.lblBank0x6_0xD = new System.Windows.Forms.Label();
            this.lstBankObjects0x9_0x12 = new System.Windows.Forms.ListBox();
            this.lstBankObjects0x5_0xC = new System.Windows.Forms.ListBox();
            this.lstBankObjects0x6_0xD = new System.Windows.Forms.ListBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbBank0x9_0x12
            // 
            this.cmbBank0x9_0x12.FormattingEnabled = true;
            this.cmbBank0x9_0x12.Location = new System.Drawing.Point(12, 25);
            this.cmbBank0x9_0x12.Name = "cmbBank0x9_0x12";
            this.cmbBank0x9_0x12.Size = new System.Drawing.Size(121, 21);
            this.cmbBank0x9_0x12.TabIndex = 0;
            this.cmbBank0x9_0x12.SelectedIndexChanged += new System.EventHandler(this.cmbBank0x9_0x12_SelectedIndexChanged);
            // 
            // lblBank0x9_0x12
            // 
            this.lblBank0x9_0x12.AutoSize = true;
            this.lblBank0x9_0x12.Location = new System.Drawing.Point(12, 9);
            this.lblBank0x9_0x12.Name = "lblBank0x9_0x12";
            this.lblBank0x9_0x12.Size = new System.Drawing.Size(86, 13);
            this.lblBank0x9_0x12.TabIndex = 1;
            this.lblBank0x9_0x12.Text = "Bank 0x9 / 0x12";
            // 
            // cmbBank0x5_0xC
            // 
            this.cmbBank0x5_0xC.FormattingEnabled = true;
            this.cmbBank0x5_0xC.Location = new System.Drawing.Point(139, 25);
            this.cmbBank0x5_0xC.Name = "cmbBank0x5_0xC";
            this.cmbBank0x5_0xC.Size = new System.Drawing.Size(121, 21);
            this.cmbBank0x5_0xC.TabIndex = 0;
            this.cmbBank0x5_0xC.SelectedIndexChanged += new System.EventHandler(this.cmbBank0x5_0xC_SelectedIndexChanged);
            // 
            // lblBank0x5_0xC
            // 
            this.lblBank0x5_0xC.AutoSize = true;
            this.lblBank0x5_0xC.Location = new System.Drawing.Point(139, 9);
            this.lblBank0x5_0xC.Name = "lblBank0x5_0xC";
            this.lblBank0x5_0xC.Size = new System.Drawing.Size(81, 13);
            this.lblBank0x5_0xC.TabIndex = 1;
            this.lblBank0x5_0xC.Text = "Bank 0x5 / 0xC";
            // 
            // cmbBank0x6_0xD
            // 
            this.cmbBank0x6_0xD.FormattingEnabled = true;
            this.cmbBank0x6_0xD.Location = new System.Drawing.Point(266, 25);
            this.cmbBank0x6_0xD.Name = "cmbBank0x6_0xD";
            this.cmbBank0x6_0xD.Size = new System.Drawing.Size(121, 21);
            this.cmbBank0x6_0xD.TabIndex = 0;
            this.cmbBank0x6_0xD.SelectedIndexChanged += new System.EventHandler(this.cmbBank0x6_0xD_SelectedIndexChanged);
            // 
            // lblBank0x6_0xD
            // 
            this.lblBank0x6_0xD.AutoSize = true;
            this.lblBank0x6_0xD.Location = new System.Drawing.Point(266, 9);
            this.lblBank0x6_0xD.Name = "lblBank0x6_0xD";
            this.lblBank0x6_0xD.Size = new System.Drawing.Size(82, 13);
            this.lblBank0x6_0xD.TabIndex = 1;
            this.lblBank0x6_0xD.Text = "Bank 0x6 / 0xD";
            // 
            // lstBankObjects0x9_0x12
            // 
            this.lstBankObjects0x9_0x12.FormattingEnabled = true;
            this.lstBankObjects0x9_0x12.Location = new System.Drawing.Point(15, 52);
            this.lstBankObjects0x9_0x12.Name = "lstBankObjects0x9_0x12";
            this.lstBankObjects0x9_0x12.Size = new System.Drawing.Size(118, 147);
            this.lstBankObjects0x9_0x12.TabIndex = 2;
            // 
            // lstBankObjects0x5_0xC
            // 
            this.lstBankObjects0x5_0xC.FormattingEnabled = true;
            this.lstBankObjects0x5_0xC.Location = new System.Drawing.Point(139, 52);
            this.lstBankObjects0x5_0xC.Name = "lstBankObjects0x5_0xC";
            this.lstBankObjects0x5_0xC.Size = new System.Drawing.Size(118, 147);
            this.lstBankObjects0x5_0xC.TabIndex = 2;
            // 
            // lstBankObjects0x6_0xD
            // 
            this.lstBankObjects0x6_0xD.FormattingEnabled = true;
            this.lstBankObjects0x6_0xD.Location = new System.Drawing.Point(266, 52);
            this.lstBankObjects0x6_0xD.Name = "lstBankObjects0x6_0xD";
            this.lstBankObjects0x6_0xD.Size = new System.Drawing.Size(118, 147);
            this.lstBankObjects0x6_0xD.TabIndex = 2;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(116, 205);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(167, 23);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // LevelSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 262);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lstBankObjects0x6_0xD);
            this.Controls.Add(this.lstBankObjects0x5_0xC);
            this.Controls.Add(this.lstBankObjects0x9_0x12);
            this.Controls.Add(this.lblBank0x6_0xD);
            this.Controls.Add(this.lblBank0x5_0xC);
            this.Controls.Add(this.cmbBank0x6_0xD);
            this.Controls.Add(this.cmbBank0x5_0xC);
            this.Controls.Add(this.lblBank0x9_0x12);
            this.Controls.Add(this.cmbBank0x9_0x12);
            this.Name = "LevelSettingsDialog";
            this.Text = "LevelSettingsDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbBank0x9_0x12;
        private System.Windows.Forms.Label lblBank0x9_0x12;
        private System.Windows.Forms.ComboBox cmbBank0x5_0xC;
        private System.Windows.Forms.Label lblBank0x5_0xC;
        private System.Windows.Forms.ComboBox cmbBank0x6_0xD;
        private System.Windows.Forms.Label lblBank0x6_0xD;
        private System.Windows.Forms.ListBox lstBankObjects0x9_0x12;
        private System.Windows.Forms.ListBox lstBankObjects0x5_0xC;
        private System.Windows.Forms.ListBox lstBankObjects0x6_0xD;
        private System.Windows.Forms.Button btnApply;

    }
}