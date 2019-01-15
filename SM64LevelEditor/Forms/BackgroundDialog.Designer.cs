namespace SM64LevelEditor
{
    partial class BackgroundDialog
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbSelectedBank = new System.Windows.Forms.ComboBox();
            this.lblSelect = new System.Windows.Forms.Label();
            this.chkCompress = new System.Windows.Forms.CheckBox();
            this.txtNewName = new System.Windows.Forms.TextBox();
            this.txtNewROMAddress = new System.Windows.Forms.TextBox();
            this.groupNew = new System.Windows.Forms.GroupBox();
            this.lblNewROMAddress = new System.Windows.Forms.Label();
            this.lblNewName = new System.Windows.Forms.Label();
            this.lblArgument = new System.Windows.Forms.Label();
            this.numericArgument = new System.Windows.Forms.NumericUpDown();
            this.groupNew.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericArgument)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(368, 177);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(287, 177);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cmbSelectedBank
            // 
            this.cmbSelectedBank.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSelectedBank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectedBank.FormattingEnabled = true;
            this.cmbSelectedBank.Location = new System.Drawing.Point(12, 33);
            this.cmbSelectedBank.Name = "cmbSelectedBank";
            this.cmbSelectedBank.Size = new System.Drawing.Size(431, 21);
            this.cmbSelectedBank.TabIndex = 1;
            this.cmbSelectedBank.SelectedIndexChanged += new System.EventHandler(this.cmbSelectedBank_SelectedIndexChanged);
            // 
            // lblSelect
            // 
            this.lblSelect.AutoSize = true;
            this.lblSelect.Location = new System.Drawing.Point(12, 13);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(98, 13);
            this.lblSelect.TabIndex = 2;
            this.lblSelect.Text = "Select Background";
            // 
            // chkCompress
            // 
            this.chkCompress.AutoSize = true;
            this.chkCompress.Location = new System.Drawing.Point(125, 78);
            this.chkCompress.Name = "chkCompress";
            this.chkCompress.Size = new System.Drawing.Size(72, 17);
            this.chkCompress.TabIndex = 4;
            this.chkCompress.Text = "Compress";
            this.chkCompress.UseVisualStyleBackColor = true;
            this.chkCompress.CheckedChanged += new System.EventHandler(this.chkCompress_CheckedChanged);
            // 
            // txtNewName
            // 
            this.txtNewName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNewName.Location = new System.Drawing.Point(6, 35);
            this.txtNewName.Name = "txtNewName";
            this.txtNewName.Size = new System.Drawing.Size(419, 20);
            this.txtNewName.TabIndex = 5;
            this.txtNewName.TextChanged += new System.EventHandler(this.txtNewName_TextChanged);
            // 
            // txtNewROMAddress
            // 
            this.txtNewROMAddress.Location = new System.Drawing.Point(6, 75);
            this.txtNewROMAddress.Name = "txtNewROMAddress";
            this.txtNewROMAddress.Size = new System.Drawing.Size(113, 20);
            this.txtNewROMAddress.TabIndex = 6;
            this.txtNewROMAddress.TextChanged += new System.EventHandler(this.txtNewROMAddress_TextChanged);
            // 
            // groupNew
            // 
            this.groupNew.Controls.Add(this.numericArgument);
            this.groupNew.Controls.Add(this.lblArgument);
            this.groupNew.Controls.Add(this.lblNewROMAddress);
            this.groupNew.Controls.Add(this.txtNewName);
            this.groupNew.Controls.Add(this.lblNewName);
            this.groupNew.Controls.Add(this.chkCompress);
            this.groupNew.Controls.Add(this.txtNewROMAddress);
            this.groupNew.Enabled = false;
            this.groupNew.Location = new System.Drawing.Point(12, 60);
            this.groupNew.Name = "groupNew";
            this.groupNew.Size = new System.Drawing.Size(431, 107);
            this.groupNew.TabIndex = 7;
            this.groupNew.TabStop = false;
            this.groupNew.Text = "New Background";
            // 
            // lblNewROMAddress
            // 
            this.lblNewROMAddress.AutoSize = true;
            this.lblNewROMAddress.Location = new System.Drawing.Point(6, 59);
            this.lblNewROMAddress.Name = "lblNewROMAddress";
            this.lblNewROMAddress.Size = new System.Drawing.Size(73, 13);
            this.lblNewROMAddress.TabIndex = 7;
            this.lblNewROMAddress.Text = "ROM Address";
            // 
            // lblNewName
            // 
            this.lblNewName.AutoSize = true;
            this.lblNewName.Location = new System.Drawing.Point(6, 19);
            this.lblNewName.Name = "lblNewName";
            this.lblNewName.Size = new System.Drawing.Size(35, 13);
            this.lblNewName.TabIndex = 2;
            this.lblNewName.Text = "Name";
            // 
            // lblArgument
            // 
            this.lblArgument.AutoSize = true;
            this.lblArgument.Location = new System.Drawing.Point(200, 59);
            this.lblArgument.Name = "lblArgument";
            this.lblArgument.Size = new System.Drawing.Size(76, 13);
            this.lblArgument.TabIndex = 10;
            this.lblArgument.Text = "Argument Byte";
            // 
            // numericArgument
            // 
            this.numericArgument.Hexadecimal = true;
            this.numericArgument.Location = new System.Drawing.Point(203, 75);
            this.numericArgument.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericArgument.Name = "numericArgument";
            this.numericArgument.Size = new System.Drawing.Size(69, 20);
            this.numericArgument.TabIndex = 11;
            this.numericArgument.ValueChanged += new System.EventHandler(this.numericArgument_ValueChanged);
            // 
            // BackgroundDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 212);
            this.Controls.Add(this.groupNew);
            this.Controls.Add(this.lblSelect);
            this.Controls.Add(this.cmbSelectedBank);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BackgroundDialog";
            this.Text = "BackgroundDialog";
            this.groupNew.ResumeLayout(false);
            this.groupNew.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericArgument)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbSelectedBank;
        private System.Windows.Forms.Label lblSelect;
        private System.Windows.Forms.CheckBox chkCompress;
        private System.Windows.Forms.TextBox txtNewName;
        private System.Windows.Forms.TextBox txtNewROMAddress;
        private System.Windows.Forms.GroupBox groupNew;
        private System.Windows.Forms.Label lblNewName;
        private System.Windows.Forms.Label lblNewROMAddress;
        private System.Windows.Forms.NumericUpDown numericArgument;
        private System.Windows.Forms.Label lblArgument;
    }
}