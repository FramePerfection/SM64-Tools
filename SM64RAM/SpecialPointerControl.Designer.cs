namespace SM64RAM
{
    partial class SpecialPointerControl
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
            this.btnAddRAMptr = new System.Windows.Forms.Button();
            this.listSpecialPointers = new System.Windows.Forms.ListBox();
            this.lblSpecialPointers = new System.Windows.Forms.Label();
            this.cmbSpecialPointers = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnAddRAMptr
            // 
            this.btnAddRAMptr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRAMptr.Location = new System.Drawing.Point(163, 83);
            this.btnAddRAMptr.Name = "btnAddRAMptr";
            this.btnAddRAMptr.Size = new System.Drawing.Size(29, 23);
            this.btnAddRAMptr.TabIndex = 20;
            this.btnAddRAMptr.Text = "+";
            this.btnAddRAMptr.UseVisualStyleBackColor = true;
            this.btnAddRAMptr.Click += new System.EventHandler(this.btnAddRAMptr_Click);
            // 
            // listSpecialPointers
            // 
            this.listSpecialPointers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listSpecialPointers.FormatString = "X8";
            this.listSpecialPointers.FormattingEnabled = true;
            this.listSpecialPointers.Location = new System.Drawing.Point(0, 13);
            this.listSpecialPointers.Name = "listSpecialPointers";
            this.listSpecialPointers.Size = new System.Drawing.Size(192, 69);
            this.listSpecialPointers.TabIndex = 19;
            // 
            // lblSpecialPointers
            // 
            this.lblSpecialPointers.AutoSize = true;
            this.lblSpecialPointers.Location = new System.Drawing.Point(-3, 0);
            this.lblSpecialPointers.Name = "lblSpecialPointers";
            this.lblSpecialPointers.Size = new System.Drawing.Size(83, 13);
            this.lblSpecialPointers.TabIndex = 18;
            this.lblSpecialPointers.Text = "Special Pointers";
            // 
            // cmbSpecialPointers
            // 
            this.cmbSpecialPointers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSpecialPointers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSpecialPointers.FormattingEnabled = true;
            this.cmbSpecialPointers.Location = new System.Drawing.Point(0, 85);
            this.cmbSpecialPointers.Name = "cmbSpecialPointers";
            this.cmbSpecialPointers.Size = new System.Drawing.Size(154, 21);
            this.cmbSpecialPointers.TabIndex = 21;
            // 
            // SpecialPointerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbSpecialPointers);
            this.Controls.Add(this.btnAddRAMptr);
            this.Controls.Add(this.listSpecialPointers);
            this.Controls.Add(this.lblSpecialPointers);
            this.Name = "SpecialPointerControl";
            this.Size = new System.Drawing.Size(195, 112);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddRAMptr;
        private System.Windows.Forms.ListBox listSpecialPointers;
        private System.Windows.Forms.Label lblSpecialPointers;
        private System.Windows.Forms.ComboBox cmbSpecialPointers;
    }
}
