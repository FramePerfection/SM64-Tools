namespace SM64RAM
{
    partial class RAMControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBank = new System.Windows.Forms.GroupBox();
            this.btnClearRAMBanks = new System.Windows.Forms.Button();
            this.btnRunLevelScript = new System.Windows.Forms.Button();
            this.lblBanks = new System.Windows.Forms.Label();
            this.txtROMOffset = new System.Windows.Forms.TextBox();
            this.lblBankROMOffset = new System.Windows.Forms.Label();
            this.btnLoadROM = new System.Windows.Forms.Button();
            this.lblROMName = new System.Windows.Forms.Label();
            this.btnRemapBank = new System.Windows.Forms.Button();
            this.groupBank.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBank
            // 
            this.groupBank.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBank.Controls.Add(this.btnClearRAMBanks);
            this.groupBank.Controls.Add(this.btnRunLevelScript);
            this.groupBank.Controls.Add(this.lblBanks);
            this.groupBank.Controls.Add(this.txtROMOffset);
            this.groupBank.Controls.Add(this.lblBankROMOffset);
            this.groupBank.Location = new System.Drawing.Point(103, 0);
            this.groupBank.Name = "groupBank";
            this.groupBank.Size = new System.Drawing.Size(444, 125);
            this.groupBank.TabIndex = 5;
            this.groupBank.TabStop = false;
            this.groupBank.Text = "Level script";
            // 
            // btnClearRAMBanks
            // 
            this.btnClearRAMBanks.Location = new System.Drawing.Point(3, 93);
            this.btnClearRAMBanks.Name = "btnClearRAMBanks";
            this.btnClearRAMBanks.Size = new System.Drawing.Size(103, 23);
            this.btnClearRAMBanks.TabIndex = 4;
            this.btnClearRAMBanks.Text = "Clear RAM Banks";
            this.btnClearRAMBanks.UseVisualStyleBackColor = true;
            this.btnClearRAMBanks.Click += new System.EventHandler(this.btnClearRAMBanks_Click);
            // 
            // btnRunLevelScript
            // 
            this.btnRunLevelScript.Location = new System.Drawing.Point(3, 64);
            this.btnRunLevelScript.Name = "btnRunLevelScript";
            this.btnRunLevelScript.Size = new System.Drawing.Size(103, 23);
            this.btnRunLevelScript.TabIndex = 4;
            this.btnRunLevelScript.Text = "Run from here";
            this.btnRunLevelScript.UseVisualStyleBackColor = true;
            this.btnRunLevelScript.Click += new System.EventHandler(this.btnRunLevelScript_Click);
            // 
            // lblBanks
            // 
            this.lblBanks.AutoSize = true;
            this.lblBanks.Location = new System.Drawing.Point(123, 16);
            this.lblBanks.Name = "lblBanks";
            this.lblBanks.Size = new System.Drawing.Size(79, 13);
            this.lblBanks.TabIndex = 6;
            this.lblBanks.Text = "Loaded Banks:";
            // 
            // txtROMOffset
            // 
            this.txtROMOffset.Location = new System.Drawing.Point(6, 38);
            this.txtROMOffset.Name = "txtROMOffset";
            this.txtROMOffset.Size = new System.Drawing.Size(100, 20);
            this.txtROMOffset.TabIndex = 2;
            // 
            // lblBankROMOffset
            // 
            this.lblBankROMOffset.AutoSize = true;
            this.lblBankROMOffset.Location = new System.Drawing.Point(6, 16);
            this.lblBankROMOffset.Name = "lblBankROMOffset";
            this.lblBankROMOffset.Size = new System.Drawing.Size(72, 13);
            this.lblBankROMOffset.TabIndex = 3;
            this.lblBankROMOffset.Text = "ROM address";
            // 
            // btnLoadROM
            // 
            this.btnLoadROM.Location = new System.Drawing.Point(3, 19);
            this.btnLoadROM.Name = "btnLoadROM";
            this.btnLoadROM.Size = new System.Drawing.Size(94, 23);
            this.btnLoadROM.TabIndex = 7;
            this.btnLoadROM.Text = "Load ROM";
            this.btnLoadROM.UseVisualStyleBackColor = true;
            this.btnLoadROM.Click += new System.EventHandler(this.btnLoadROM_Click);
            // 
            // lblROMName
            // 
            this.lblROMName.AutoSize = true;
            this.lblROMName.Location = new System.Drawing.Point(3, 3);
            this.lblROMName.Name = "lblROMName";
            this.lblROMName.Size = new System.Drawing.Size(94, 13);
            this.lblROMName.TabIndex = 8;
            this.lblROMName.Text = "<no ROM loaded>";
            // 
            // btnRemapBank
            // 
            this.btnRemapBank.Location = new System.Drawing.Point(3, 48);
            this.btnRemapBank.Name = "btnRemapBank";
            this.btnRemapBank.Size = new System.Drawing.Size(94, 23);
            this.btnRemapBank.TabIndex = 9;
            this.btnRemapBank.Text = "Remap Bank";
            this.btnRemapBank.UseVisualStyleBackColor = true;
            this.btnRemapBank.Click += new System.EventHandler(this.btbRemapBank_Click);
            // 
            // RAMControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnRemapBank);
            this.Controls.Add(this.lblROMName);
            this.Controls.Add(this.btnLoadROM);
            this.Controls.Add(this.groupBank);
            this.Name = "RAMControl";
            this.Size = new System.Drawing.Size(550, 133);
            this.groupBank.ResumeLayout(false);
            this.groupBank.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBank;
        private System.Windows.Forms.Button btnClearRAMBanks;
        private System.Windows.Forms.Button btnRunLevelScript;
        private System.Windows.Forms.TextBox txtROMOffset;
        private System.Windows.Forms.Label lblBankROMOffset;
        private System.Windows.Forms.Label lblBanks;
        private System.Windows.Forms.Button btnLoadROM;
        private System.Windows.Forms.Label lblROMName;
        private System.Windows.Forms.Button btnRemapBank;
    }
}
