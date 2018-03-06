namespace SM64_model_importer
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
            this.btnImport = new System.Windows.Forms.Button();
            this.tabImports = new System.Windows.Forms.TabControl();
            this.tabPagePlus = new System.Windows.Forms.TabPage();
            this.globalsControl = new SM64_model_importer.GlobalsControl();
            this.btnAddCollision = new System.Windows.Forms.Button();
            this.btnAddDisplayList = new System.Windows.Forms.Button();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnLoadSettings = new System.Windows.Forms.Button();
            this.txtBaseOffset = new System.Windows.Forms.TextBox();
            this.lblBaseOffset = new System.Windows.Forms.Label();
            this.RAMControl = new SM64RAM.RAMControl();
            this.numScale = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.tabImports.SuspendLayout();
            this.tabPagePlus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).BeginInit();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImport.Enabled = false;
            this.btnImport.Location = new System.Drawing.Point(211, 730);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(118, 23);
            this.btnImport.TabIndex = 10;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // tabImports
            // 
            this.tabImports.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabImports.Controls.Add(this.tabPagePlus);
            this.tabImports.Location = new System.Drawing.Point(13, 151);
            this.tabImports.Multiline = true;
            this.tabImports.Name = "tabImports";
            this.tabImports.SelectedIndex = 0;
            this.tabImports.Size = new System.Drawing.Size(692, 573);
            this.tabImports.TabIndex = 11;
            // 
            // tabPagePlus
            // 
            this.tabPagePlus.Controls.Add(this.globalsControl);
            this.tabPagePlus.Controls.Add(this.btnAddCollision);
            this.tabPagePlus.Controls.Add(this.btnAddDisplayList);
            this.tabPagePlus.Location = new System.Drawing.Point(4, 22);
            this.tabPagePlus.Name = "tabPagePlus";
            this.tabPagePlus.Size = new System.Drawing.Size(684, 547);
            this.tabPagePlus.TabIndex = 0;
            this.tabPagePlus.Text = "+";
            this.tabPagePlus.UseVisualStyleBackColor = true;
            // 
            // globalsControl
            // 
            this.globalsControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.globalsControl.Location = new System.Drawing.Point(3, 195);
            this.globalsControl.Name = "globalsControl";
            this.globalsControl.Size = new System.Drawing.Size(675, 349);
            this.globalsControl.TabIndex = 1;
            // 
            // btnAddCollision
            // 
            this.btnAddCollision.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCollision.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddCollision.Location = new System.Drawing.Point(0, 99);
            this.btnAddCollision.Name = "btnAddCollision";
            this.btnAddCollision.Size = new System.Drawing.Size(678, 90);
            this.btnAddCollision.TabIndex = 0;
            this.btnAddCollision.Text = "New Collision Map";
            this.btnAddCollision.UseVisualStyleBackColor = true;
            this.btnAddCollision.Click += new System.EventHandler(this.btnAddCollision_Click);
            // 
            // btnAddDisplayList
            // 
            this.btnAddDisplayList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddDisplayList.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddDisplayList.Location = new System.Drawing.Point(3, 3);
            this.btnAddDisplayList.Name = "btnAddDisplayList";
            this.btnAddDisplayList.Size = new System.Drawing.Size(678, 90);
            this.btnAddDisplayList.TabIndex = 0;
            this.btnAddDisplayList.Text = "New Display List";
            this.btnAddDisplayList.UseVisualStyleBackColor = true;
            this.btnAddDisplayList.Click += new System.EventHandler(this.btnAddDisplayList_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveSettings.Location = new System.Drawing.Point(579, 730);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(89, 23);
            this.btnSaveSettings.TabIndex = 12;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnLoadSettings
            // 
            this.btnLoadSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadSettings.Location = new System.Drawing.Point(484, 730);
            this.btnLoadSettings.Name = "btnLoadSettings";
            this.btnLoadSettings.Size = new System.Drawing.Size(89, 23);
            this.btnLoadSettings.TabIndex = 12;
            this.btnLoadSettings.Text = "Load Settings";
            this.btnLoadSettings.UseVisualStyleBackColor = true;
            this.btnLoadSettings.Click += new System.EventHandler(this.btnLoadSettings_Click);
            // 
            // txtBaseOffset
            // 
            this.txtBaseOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtBaseOffset.Location = new System.Drawing.Point(105, 733);
            this.txtBaseOffset.Name = "txtBaseOffset";
            this.txtBaseOffset.Size = new System.Drawing.Size(100, 20);
            this.txtBaseOffset.TabIndex = 13;
            this.txtBaseOffset.Text = "0E000000";
            this.txtBaseOffset.TextChanged += new System.EventHandler(this.txtBaseOffset_TextChanged);
            // 
            // lblBaseOffset
            // 
            this.lblBaseOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBaseOffset.AutoSize = true;
            this.lblBaseOffset.Location = new System.Drawing.Point(9, 736);
            this.lblBaseOffset.Name = "lblBaseOffset";
            this.lblBaseOffset.Size = new System.Drawing.Size(90, 13);
            this.lblBaseOffset.TabIndex = 14;
            this.lblBaseOffset.Text = "Base RAM offset:";
            // 
            // RAMControl
            // 
            this.RAMControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RAMControl.Location = new System.Drawing.Point(12, 12);
            this.RAMControl.Name = "RAMControl";
            this.RAMControl.Size = new System.Drawing.Size(686, 133);
            this.RAMControl.TabIndex = 1;
            // 
            // numScale
            // 
            this.numScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numScale.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numScale.Location = new System.Drawing.Point(378, 734);
            this.numScale.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numScale.Name = "numScale";
            this.numScale.Size = new System.Drawing.Size(68, 20);
            this.numScale.TabIndex = 15;
            this.numScale.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numScale.ValueChanged += new System.EventHandler(this.numScale_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(335, 736);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Scale:";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(717, 758);
            this.Controls.Add(this.numScale);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblBaseOffset);
            this.Controls.Add(this.txtBaseOffset);
            this.Controls.Add(this.btnLoadSettings);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.tabImports);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.RAMControl);
            this.Name = "Main";
            this.Text = "SM64 Model Importer";
            this.tabImports.ResumeLayout(false);
            this.tabPagePlus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SM64RAM.RAMControl RAMControl;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TabControl tabImports;
        private System.Windows.Forms.TabPage tabPagePlus;
        private System.Windows.Forms.Button btnAddDisplayList;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnLoadSettings;
        private System.Windows.Forms.TextBox txtBaseOffset;
        private System.Windows.Forms.Label lblBaseOffset;
        private System.Windows.Forms.Button btnAddCollision;
        private System.Windows.Forms.NumericUpDown numScale;
        private System.Windows.Forms.Label label1;
        public GlobalsControl globalsControl;
    }
}

