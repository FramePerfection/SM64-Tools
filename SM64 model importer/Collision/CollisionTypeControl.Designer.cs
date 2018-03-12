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
            this.numExtra2 = new System.Windows.Forms.NumericUpDown();
            this.numExtra1 = new System.Windows.Forms.NumericUpDown();
            this.lblExtraData = new System.Windows.Forms.Label();
            this.groupType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numExtra2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numExtra1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbType
            // 
            this.cmbType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(6, 19);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(121, 21);
            this.cmbType.TabIndex = 0;
            this.cmbType.TextChanged += new System.EventHandler(this.cmbType_SelectedTextChanged);
            // 
            // groupType
            // 
            this.groupType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupType.Controls.Add(this.numExtra2);
            this.groupType.Controls.Add(this.numExtra1);
            this.groupType.Controls.Add(this.lblExtraData);
            this.groupType.Controls.Add(this.cmbType);
            this.groupType.Location = new System.Drawing.Point(0, 0);
            this.groupType.Name = "groupType";
            this.groupType.Size = new System.Drawing.Size(355, 52);
            this.groupType.TabIndex = 1;
            this.groupType.TabStop = false;
            this.groupType.Text = "Object";
            // 
            // numExtra2
            // 
            this.numExtra2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numExtra2.Hexadecimal = true;
            this.numExtra2.Location = new System.Drawing.Point(272, 20);
            this.numExtra2.Name = "numExtra2";
            this.numExtra2.Size = new System.Drawing.Size(70, 20);
            this.numExtra2.TabIndex = 2;
            // 
            // numExtra1
            // 
            this.numExtra1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numExtra1.Hexadecimal = true;
            this.numExtra1.Location = new System.Drawing.Point(196, 19);
            this.numExtra1.Name = "numExtra1";
            this.numExtra1.Size = new System.Drawing.Size(70, 20);
            this.numExtra1.TabIndex = 2;
            // 
            // lblExtraData
            // 
            this.lblExtraData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExtraData.AutoSize = true;
            this.lblExtraData.Location = new System.Drawing.Point(133, 22);
            this.lblExtraData.Name = "lblExtraData";
            this.lblExtraData.Size = new System.Drawing.Size(57, 13);
            this.lblExtraData.TabIndex = 1;
            this.lblExtraData.Text = "Extra Data";
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
            ((System.ComponentModel.ISupportInitialize)(this.numExtra2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numExtra1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.GroupBox groupType;
        private System.Windows.Forms.NumericUpDown numExtra1;
        private System.Windows.Forms.Label lblExtraData;
        private System.Windows.Forms.NumericUpDown numExtra2;
    }
}
