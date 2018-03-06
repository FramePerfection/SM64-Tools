namespace SM64LevelEditor.Controls
{
    partial class ObjectControl
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
            this.propertyCommand = new System.Windows.Forms.PropertyGrid();
            this.cmbModelIDAlias = new System.Windows.Forms.ComboBox();
            this.cmbBehaviourAlias = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // propertyCommand
            // 
            this.propertyCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyCommand.Location = new System.Drawing.Point(0, 3);
            this.propertyCommand.Name = "propertyCommand";
            this.propertyCommand.Size = new System.Drawing.Size(150, 184);
            this.propertyCommand.TabIndex = 0;
            // 
            // cmbModelIDAlias
            // 
            this.cmbModelIDAlias.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbModelIDAlias.FormattingEnabled = true;
            this.cmbModelIDAlias.Location = new System.Drawing.Point(0, 220);
            this.cmbModelIDAlias.Name = "cmbModelIDAlias";
            this.cmbModelIDAlias.Size = new System.Drawing.Size(150, 21);
            this.cmbModelIDAlias.TabIndex = 4;
            this.cmbModelIDAlias.SelectedIndexChanged += new System.EventHandler(this.cmbModelAlias_SelectedIndexChanged);
            // 
            // cmbBehaviourAlias
            // 
            this.cmbBehaviourAlias.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbBehaviourAlias.FormattingEnabled = true;
            this.cmbBehaviourAlias.Location = new System.Drawing.Point(0, 193);
            this.cmbBehaviourAlias.Name = "cmbBehaviourAlias";
            this.cmbBehaviourAlias.Size = new System.Drawing.Size(150, 21);
            this.cmbBehaviourAlias.TabIndex = 3;
            this.cmbBehaviourAlias.SelectedIndexChanged += new System.EventHandler(this.cmbBehaviourAlias_SelectedIndexChanged);
            // 
            // ObjectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmbModelIDAlias);
            this.Controls.Add(this.cmbBehaviourAlias);
            this.Controls.Add(this.propertyCommand);
            this.Name = "ObjectControl";
            this.Size = new System.Drawing.Size(150, 251);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyCommand;
        private System.Windows.Forms.ComboBox cmbModelIDAlias;
        private System.Windows.Forms.ComboBox cmbBehaviourAlias;
    }
}
