namespace SM64_model_importer
{
    partial class GlobalsControl
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
            this.btnFogColor = new System.Windows.Forms.Button();
            this.numFogIntensity = new System.Windows.Forms.NumericUpDown();
            this.groupFog = new System.Windows.Forms.GroupBox();
            this.numFogOffset = new System.Windows.Forms.NumericUpDown();
            this.panelPreview = new System.Windows.Forms.Panel();
            this.lblFogOffset = new System.Windows.Forms.Label();
            this.lblFogIntensity = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numFogIntensity)).BeginInit();
            this.groupFog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFogOffset)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFogColor
            // 
            this.btnFogColor.Location = new System.Drawing.Point(6, 19);
            this.btnFogColor.Name = "btnFogColor";
            this.btnFogColor.Size = new System.Drawing.Size(52, 23);
            this.btnFogColor.TabIndex = 0;
            this.btnFogColor.Text = "Color";
            this.btnFogColor.UseVisualStyleBackColor = true;
            this.btnFogColor.Click += new System.EventHandler(this.btnFogColor_Click);
            // 
            // numFogIntensity
            // 
            this.numFogIntensity.DecimalPlaces = 8;
            this.numFogIntensity.Location = new System.Drawing.Point(64, 35);
            this.numFogIntensity.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numFogIntensity.Name = "numFogIntensity";
            this.numFogIntensity.Size = new System.Drawing.Size(107, 20);
            this.numFogIntensity.TabIndex = 1;
            this.numFogIntensity.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // groupFog
            // 
            this.groupFog.Controls.Add(this.numFogOffset);
            this.groupFog.Controls.Add(this.panelPreview);
            this.groupFog.Controls.Add(this.lblFogOffset);
            this.groupFog.Controls.Add(this.lblFogIntensity);
            this.groupFog.Controls.Add(this.btnFogColor);
            this.groupFog.Controls.Add(this.numFogIntensity);
            this.groupFog.Location = new System.Drawing.Point(3, 3);
            this.groupFog.Name = "groupFog";
            this.groupFog.Size = new System.Drawing.Size(294, 120);
            this.groupFog.TabIndex = 2;
            this.groupFog.TabStop = false;
            this.groupFog.Text = "Fog";
            // 
            // numFogOffset
            // 
            this.numFogOffset.DecimalPlaces = 8;
            this.numFogOffset.Location = new System.Drawing.Point(177, 35);
            this.numFogOffset.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numFogOffset.Name = "numFogOffset";
            this.numFogOffset.Size = new System.Drawing.Size(107, 20);
            this.numFogOffset.TabIndex = 1;
            this.numFogOffset.Value = new decimal(new int[] {
            232,
            0,
            0,
            0});
            // 
            // panelPreview
            // 
            this.panelPreview.Location = new System.Drawing.Point(6, 61);
            this.panelPreview.Name = "panelPreview";
            this.panelPreview.Size = new System.Drawing.Size(282, 53);
            this.panelPreview.TabIndex = 3;
            // 
            // lblFogOffset
            // 
            this.lblFogOffset.AutoSize = true;
            this.lblFogOffset.Location = new System.Drawing.Point(174, 16);
            this.lblFogOffset.Name = "lblFogOffset";
            this.lblFogOffset.Size = new System.Drawing.Size(35, 13);
            this.lblFogOffset.TabIndex = 2;
            this.lblFogOffset.Text = "Offset";
            // 
            // lblFogIntensity
            // 
            this.lblFogIntensity.AutoSize = true;
            this.lblFogIntensity.Location = new System.Drawing.Point(61, 16);
            this.lblFogIntensity.Name = "lblFogIntensity";
            this.lblFogIntensity.Size = new System.Drawing.Size(46, 13);
            this.lblFogIntensity.TabIndex = 2;
            this.lblFogIntensity.Text = "Intensity";
            // 
            // GlobalsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupFog);
            this.Name = "GlobalsControl";
            this.Size = new System.Drawing.Size(407, 279);
            ((System.ComponentModel.ISupportInitialize)(this.numFogIntensity)).EndInit();
            this.groupFog.ResumeLayout(false);
            this.groupFog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFogOffset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnFogColor;
        private System.Windows.Forms.NumericUpDown numFogIntensity;
        private System.Windows.Forms.GroupBox groupFog;
        private System.Windows.Forms.Panel panelPreview;
        private System.Windows.Forms.Label lblFogOffset;
        private System.Windows.Forms.Label lblFogIntensity;
        private System.Windows.Forms.NumericUpDown numFogOffset;
    }
}
