namespace SM64Patcher
{
    partial class Main
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

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.btnLoadROM = new System.Windows.Forms.Button();
            this.lblROM = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Enabled = false;
            this.btnLoadFile.Location = new System.Drawing.Point(12, 61);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(94, 23);
            this.btnLoadFile.TabIndex = 0;
            this.btnLoadFile.Text = "Load Patch File";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // btnLoadROM
            // 
            this.btnLoadROM.Location = new System.Drawing.Point(12, 32);
            this.btnLoadROM.Name = "btnLoadROM";
            this.btnLoadROM.Size = new System.Drawing.Size(94, 23);
            this.btnLoadROM.TabIndex = 1;
            this.btnLoadROM.Text = "Load ROM";
            this.btnLoadROM.UseVisualStyleBackColor = true;
            this.btnLoadROM.Click += new System.EventHandler(this.btnLoadROM_Click);
            // 
            // lblROM
            // 
            this.lblROM.AutoSize = true;
            this.lblROM.Location = new System.Drawing.Point(12, 13);
            this.lblROM.Name = "lblROM";
            this.lblROM.Size = new System.Drawing.Size(94, 13);
            this.lblROM.TabIndex = 2;
            this.lblROM.Text = "<no ROM loaded>";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(113, 96);
            this.Controls.Add(this.lblROM);
            this.Controls.Add(this.btnLoadROM);
            this.Controls.Add(this.btnLoadFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Main";
            this.Text = "SM64 Patcher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Button btnLoadROM;
        private System.Windows.Forms.Label lblROM;
    }
}

