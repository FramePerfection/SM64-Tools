namespace SM64LevelEditor.Controls
{
    partial class WarpControl
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
            this.txtWarps = new System.Windows.Forms.RichTextBox();
            this.groupWarps = new System.Windows.Forms.GroupBox();
            this.groupWarps.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtWarps
            // 
            this.txtWarps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWarps.Location = new System.Drawing.Point(6, 19);
            this.txtWarps.Name = "txtWarps";
            this.txtWarps.Size = new System.Drawing.Size(232, 125);
            this.txtWarps.TabIndex = 0;
            this.txtWarps.Text = "";
            this.txtWarps.TextChanged += new System.EventHandler(this.txtWarps_TextChanged);
            // 
            // groupWarps
            // 
            this.groupWarps.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupWarps.Controls.Add(this.txtWarps);
            this.groupWarps.Location = new System.Drawing.Point(0, 0);
            this.groupWarps.Name = "groupWarps";
            this.groupWarps.Size = new System.Drawing.Size(244, 150);
            this.groupWarps.TabIndex = 1;
            this.groupWarps.TabStop = false;
            this.groupWarps.Text = "Warps";
            // 
            // WarpControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupWarps);
            this.Name = "WarpControl";
            this.Size = new System.Drawing.Size(244, 150);
            this.groupWarps.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtWarps;
        private System.Windows.Forms.GroupBox groupWarps;
    }
}
