namespace SM64_model_importer
{
    partial class PropertyControl<T>
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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox.Controls.Add(this.flowLayoutPanel);
            this.groupBox.Location = new System.Drawing.Point(3, 0);
            this.groupBox.MinimumSize = new System.Drawing.Size(100, 100);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(302, 121);
            this.groupBox.TabIndex = 1;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Render States";
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel.AutoScrollMargin = new System.Drawing.Size(10, 10);
            this.flowLayoutPanel.ColumnCount = 6;
            this.flowLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.flowLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.flowLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.flowLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.flowLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.flowLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.flowLayoutPanel.Location = new System.Drawing.Point(6, 19);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.RowCount = 4;
            this.flowLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.flowLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.flowLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.flowLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.flowLayoutPanel.Size = new System.Drawing.Size(290, 96);
            this.flowLayoutPanel.TabIndex = 0;
            // 
            // RenderStateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "RenderStateControl";
            this.Size = new System.Drawing.Size(302, 121);
            this.groupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.TableLayoutPanel flowLayoutPanel;
    }
}
