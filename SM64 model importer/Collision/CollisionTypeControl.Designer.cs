namespace SM64ModelImporter
{
    partial class CollisionTypeControl
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
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.groupType = new System.Windows.Forms.GroupBox();
            this.chkEnableImport = new System.Windows.Forms.CheckBox();
            this.picMaterial = new System.Windows.Forms.PictureBox();
            this.groupType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMaterial)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbType
            // 
            this.cmbType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(6, 19);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(236, 21);
            this.cmbType.TabIndex = 0;
            this.cmbType.TextChanged += new System.EventHandler(this.cmbType_SelectedTextChanged);
            // 
            // groupType
            // 
            this.groupType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupType.Controls.Add(this.picMaterial);
            this.groupType.Controls.Add(this.chkEnableImport);
            this.groupType.Controls.Add(this.cmbType);
            this.groupType.Location = new System.Drawing.Point(0, 0);
            this.groupType.Name = "groupType";
            this.groupType.Size = new System.Drawing.Size(355, 52);
            this.groupType.TabIndex = 1;
            this.groupType.TabStop = false;
            this.groupType.Text = "Object";
            // 
            // chkEnableImport
            // 
            this.chkEnableImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkEnableImport.AutoSize = true;
            this.chkEnableImport.Checked = true;
            this.chkEnableImport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableImport.Location = new System.Drawing.Point(294, 21);
            this.chkEnableImport.Name = "chkEnableImport";
            this.chkEnableImport.Size = new System.Drawing.Size(55, 17);
            this.chkEnableImport.TabIndex = 1;
            this.chkEnableImport.Text = "Import";
            this.chkEnableImport.UseVisualStyleBackColor = true;
            // 
            // picMaterial
            // 
            this.picMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picMaterial.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.picMaterial.Location = new System.Drawing.Point(248, 9);
            this.picMaterial.Name = "picMaterial";
            this.picMaterial.Size = new System.Drawing.Size(40, 37);
            this.picMaterial.TabIndex = 2;
            this.picMaterial.TabStop = false;
            // 
            // CollisionTypeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupType);
            this.Name = "CollisionTypeControl";
            this.Size = new System.Drawing.Size(355, 55);
            this.groupType.ResumeLayout(false);
            this.groupType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMaterial)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.GroupBox groupType;
        private System.Windows.Forms.CheckBox chkEnableImport;
        private System.Windows.Forms.PictureBox picMaterial;
    }
}
