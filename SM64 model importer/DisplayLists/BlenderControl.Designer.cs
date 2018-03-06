namespace SM64_model_importer
{
    partial class BlenderControl
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
            this.components = new System.ComponentModel.Container();
            this.cmbP1 = new System.Windows.Forms.ComboBox();
            this.lblFirstCycle = new System.Windows.Forms.Label();
            this.cmbCycleMode = new System.Windows.Forms.ComboBox();
            this.lblP = new System.Windows.Forms.Label();
            this.cmbA1 = new System.Windows.Forms.ComboBox();
            this.cmbM1 = new System.Windows.Forms.ComboBox();
            this.cmbB1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbP2 = new System.Windows.Forms.ComboBox();
            this.cmbA2 = new System.Windows.Forms.ComboBox();
            this.cmbM2 = new System.Windows.Forms.ComboBox();
            this.cmbB2 = new System.Windows.Forms.ComboBox();
            this.lblSecondCycle = new System.Windows.Forms.Label();
            this.addressModeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBlender = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.addressModeBindingSource)).BeginInit();
            this.groupBlender.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbP1
            // 
            this.cmbP1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP1.FormattingEnabled = true;
            this.cmbP1.Location = new System.Drawing.Point(80, 45);
            this.cmbP1.MaxDropDownItems = 4;
            this.cmbP1.Name = "cmbP1";
            this.cmbP1.Size = new System.Drawing.Size(121, 21);
            this.cmbP1.TabIndex = 0;
            this.cmbP1.SelectedIndexChanged += new System.EventHandler(this.blendModeChanged);
            // 
            // lblFirstCycle
            // 
            this.lblFirstCycle.AutoSize = true;
            this.lblFirstCycle.Location = new System.Drawing.Point(3, 48);
            this.lblFirstCycle.Name = "lblFirstCycle";
            this.lblFirstCycle.Size = new System.Drawing.Size(58, 13);
            this.lblFirstCycle.TabIndex = 1;
            this.lblFirstCycle.Text = "First Cycle:";
            // 
            // cmbCycleMode
            // 
            this.cmbCycleMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCycleMode.Enabled = false;
            this.cmbCycleMode.FormattingEnabled = true;
            this.cmbCycleMode.Location = new System.Drawing.Point(6, 19);
            this.cmbCycleMode.Name = "cmbCycleMode";
            this.cmbCycleMode.Size = new System.Drawing.Size(89, 21);
            this.cmbCycleMode.TabIndex = 2;
            this.cmbCycleMode.SelectedIndexChanged += new System.EventHandler(this.cmbCycleMode_SelectedIndexChanged);
            // 
            // lblP
            // 
            this.lblP.AutoSize = true;
            this.lblP.Location = new System.Drawing.Point(101, 22);
            this.lblP.Name = "lblP";
            this.lblP.Size = new System.Drawing.Size(14, 13);
            this.lblP.TabIndex = 1;
            this.lblP.Text = "P";
            // 
            // cmbA1
            // 
            this.cmbA1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbA1.FormattingEnabled = true;
            this.cmbA1.Location = new System.Drawing.Point(207, 45);
            this.cmbA1.MaxDropDownItems = 4;
            this.cmbA1.Name = "cmbA1";
            this.cmbA1.Size = new System.Drawing.Size(121, 21);
            this.cmbA1.TabIndex = 0;
            this.cmbA1.SelectedIndexChanged += new System.EventHandler(this.blendModeChanged);
            // 
            // cmbM1
            // 
            this.cmbM1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbM1.FormattingEnabled = true;
            this.cmbM1.Location = new System.Drawing.Point(334, 45);
            this.cmbM1.MaxDropDownItems = 4;
            this.cmbM1.Name = "cmbM1";
            this.cmbM1.Size = new System.Drawing.Size(121, 21);
            this.cmbM1.TabIndex = 0;
            this.cmbM1.SelectedIndexChanged += new System.EventHandler(this.blendModeChanged);
            // 
            // cmbB1
            // 
            this.cmbB1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbB1.FormattingEnabled = true;
            this.cmbB1.Location = new System.Drawing.Point(461, 45);
            this.cmbB1.MaxDropDownItems = 4;
            this.cmbB1.Name = "cmbB1";
            this.cmbB1.Size = new System.Drawing.Size(121, 21);
            this.cmbB1.TabIndex = 0;
            this.cmbB1.SelectedIndexChanged += new System.EventHandler(this.blendModeChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(228, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "A";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(355, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "M";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(482, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "B";
            // 
            // cmbP2
            // 
            this.cmbP2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbP2.FormattingEnabled = true;
            this.cmbP2.Location = new System.Drawing.Point(80, 81);
            this.cmbP2.MaxDropDownItems = 4;
            this.cmbP2.Name = "cmbP2";
            this.cmbP2.Size = new System.Drawing.Size(121, 21);
            this.cmbP2.TabIndex = 0;
            this.cmbP2.SelectedIndexChanged += new System.EventHandler(this.blendModeChanged);
            // 
            // cmbA2
            // 
            this.cmbA2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbA2.FormattingEnabled = true;
            this.cmbA2.Location = new System.Drawing.Point(207, 81);
            this.cmbA2.MaxDropDownItems = 4;
            this.cmbA2.Name = "cmbA2";
            this.cmbA2.Size = new System.Drawing.Size(121, 21);
            this.cmbA2.TabIndex = 0;
            this.cmbA2.SelectedIndexChanged += new System.EventHandler(this.blendModeChanged);
            // 
            // cmbM2
            // 
            this.cmbM2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbM2.FormattingEnabled = true;
            this.cmbM2.Location = new System.Drawing.Point(334, 81);
            this.cmbM2.MaxDropDownItems = 4;
            this.cmbM2.Name = "cmbM2";
            this.cmbM2.Size = new System.Drawing.Size(121, 21);
            this.cmbM2.TabIndex = 0;
            this.cmbM2.SelectedIndexChanged += new System.EventHandler(this.blendModeChanged);
            // 
            // cmbB2
            // 
            this.cmbB2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbB2.FormattingEnabled = true;
            this.cmbB2.Location = new System.Drawing.Point(461, 81);
            this.cmbB2.MaxDropDownItems = 4;
            this.cmbB2.Name = "cmbB2";
            this.cmbB2.Size = new System.Drawing.Size(121, 21);
            this.cmbB2.TabIndex = 0;
            this.cmbB2.SelectedIndexChanged += new System.EventHandler(this.blendModeChanged);
            // 
            // lblSecondCycle
            // 
            this.lblSecondCycle.AutoSize = true;
            this.lblSecondCycle.Location = new System.Drawing.Point(3, 84);
            this.lblSecondCycle.Name = "lblSecondCycle";
            this.lblSecondCycle.Size = new System.Drawing.Size(76, 13);
            this.lblSecondCycle.TabIndex = 1;
            this.lblSecondCycle.Text = "Second Cycle:";
            // 
            // addressModeBindingSource
            // 
            this.addressModeBindingSource.DataSource = typeof(SM64_model_importer.TextureInfo.AddressMode);
            // 
            // groupBlender
            // 
            this.groupBlender.Controls.Add(this.cmbCycleMode);
            this.groupBlender.Controls.Add(this.cmbP1);
            this.groupBlender.Controls.Add(this.label3);
            this.groupBlender.Controls.Add(this.cmbA1);
            this.groupBlender.Controls.Add(this.label2);
            this.groupBlender.Controls.Add(this.cmbP2);
            this.groupBlender.Controls.Add(this.label1);
            this.groupBlender.Controls.Add(this.cmbM1);
            this.groupBlender.Controls.Add(this.lblP);
            this.groupBlender.Controls.Add(this.cmbA2);
            this.groupBlender.Controls.Add(this.lblSecondCycle);
            this.groupBlender.Controls.Add(this.cmbB1);
            this.groupBlender.Controls.Add(this.cmbB2);
            this.groupBlender.Controls.Add(this.cmbM2);
            this.groupBlender.Controls.Add(this.lblFirstCycle);
            this.groupBlender.Location = new System.Drawing.Point(3, 3);
            this.groupBlender.Name = "groupBlender";
            this.groupBlender.Size = new System.Drawing.Size(587, 110);
            this.groupBlender.TabIndex = 3;
            this.groupBlender.TabStop = false;
            this.groupBlender.Text = "Blender Settings (output color = (P * A + M - B) / (A + B))";
            // 
            // BlenderControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBlender);
            this.Name = "BlenderControl";
            this.Size = new System.Drawing.Size(593, 118);
            ((System.ComponentModel.ISupportInitialize)(this.addressModeBindingSource)).EndInit();
            this.groupBlender.ResumeLayout(false);
            this.groupBlender.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbP1;
        private System.Windows.Forms.BindingSource addressModeBindingSource;
        private System.Windows.Forms.Label lblFirstCycle;
        private System.Windows.Forms.ComboBox cmbCycleMode;
        private System.Windows.Forms.Label lblP;
        private System.Windows.Forms.ComboBox cmbA1;
        private System.Windows.Forms.ComboBox cmbM1;
        private System.Windows.Forms.ComboBox cmbB1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbP2;
        private System.Windows.Forms.ComboBox cmbA2;
        private System.Windows.Forms.ComboBox cmbM2;
        private System.Windows.Forms.ComboBox cmbB2;
        private System.Windows.Forms.Label lblSecondCycle;
        private System.Windows.Forms.GroupBox groupBlender;
    }
}
