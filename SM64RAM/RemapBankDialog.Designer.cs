namespace SM64RAM
{
    partial class RemapBankDialog
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
            this.cmbBank = new System.Windows.Forms.ComboBox();
            this.lblAdditionalPointers = new System.Windows.Forms.Label();
            this.lblBank = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtROMStart = new System.Windows.Forms.TextBox();
            this.lblROMStart = new System.Windows.Forms.Label();
            this.txtLength = new System.Windows.Forms.TextBox();
            this.lblLength = new System.Windows.Forms.Label();
            this.pointerControl1 = new SM64RAM.PointerControl();
            this.SuspendLayout();
            // 
            // cmbBank
            // 
            this.cmbBank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBank.FormattingEnabled = true;
            this.cmbBank.Location = new System.Drawing.Point(12, 29);
            this.cmbBank.Name = "cmbBank";
            this.cmbBank.Size = new System.Drawing.Size(258, 21);
            this.cmbBank.TabIndex = 0;
            this.cmbBank.SelectedIndexChanged += new System.EventHandler(this.cmbBank_SelectedIndexChanged);
            // 
            // lblAdditionalPointers
            // 
            this.lblAdditionalPointers.AutoSize = true;
            this.lblAdditionalPointers.Location = new System.Drawing.Point(12, 53);
            this.lblAdditionalPointers.Name = "lblAdditionalPointers";
            this.lblAdditionalPointers.Size = new System.Drawing.Size(155, 13);
            this.lblAdditionalPointers.TabIndex = 2;
            this.lblAdditionalPointers.Text = "Additional Pointers to this bank:";
            // 
            // lblBank
            // 
            this.lblBank.AutoSize = true;
            this.lblBank.Location = new System.Drawing.Point(12, 13);
            this.lblBank.Name = "lblBank";
            this.lblBank.Size = new System.Drawing.Size(32, 13);
            this.lblBank.TabIndex = 2;
            this.lblBank.Text = "Bank";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(197, 227);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(116, 227);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtROMStart
            // 
            this.txtROMStart.Location = new System.Drawing.Point(15, 199);
            this.txtROMStart.Name = "txtROMStart";
            this.txtROMStart.Size = new System.Drawing.Size(126, 20);
            this.txtROMStart.TabIndex = 4;
            this.txtROMStart.TextChanged += new System.EventHandler(this.txtROMStart_TextChanged);
            // 
            // lblROMStart
            // 
            this.lblROMStart.AutoSize = true;
            this.lblROMStart.Location = new System.Drawing.Point(12, 183);
            this.lblROMStart.Name = "lblROMStart";
            this.lblROMStart.Size = new System.Drawing.Size(82, 13);
            this.lblROMStart.TabIndex = 2;
            this.lblROMStart.Text = "New ROM Start";
            // 
            // txtLength
            // 
            this.txtLength.Location = new System.Drawing.Point(144, 199);
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(126, 20);
            this.txtLength.TabIndex = 4;
            this.txtLength.TextChanged += new System.EventHandler(this.txtLength_TextChanged);
            // 
            // lblLength
            // 
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point(141, 183);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(65, 13);
            this.lblLength.TabIndex = 2;
            this.lblLength.Text = "New Length";
            // 
            // pointerControl1
            // 
            this.pointerControl1.Location = new System.Drawing.Point(12, 69);
            this.pointerControl1.Name = "pointerControl1";
            this.pointerControl1.Size = new System.Drawing.Size(258, 114);
            this.pointerControl1.TabIndex = 1;
            // 
            // RemapBankDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.txtLength);
            this.Controls.Add(this.txtROMStart);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblLength);
            this.Controls.Add(this.lblROMStart);
            this.Controls.Add(this.lblBank);
            this.Controls.Add(this.lblAdditionalPointers);
            this.Controls.Add(this.pointerControl1);
            this.Controls.Add(this.cmbBank);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "RemapBankDialog";
            this.Text = "Remap Bank";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbBank;
        private PointerControl pointerControl1;
        private System.Windows.Forms.Label lblAdditionalPointers;
        private System.Windows.Forms.Label lblBank;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtROMStart;
        private System.Windows.Forms.Label lblROMStart;
        private System.Windows.Forms.TextBox txtLength;
        private System.Windows.Forms.Label lblLength;
    }
}