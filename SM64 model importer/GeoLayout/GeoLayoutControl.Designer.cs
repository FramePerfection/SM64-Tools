namespace SM64ModelImporter
{
    partial class GeoLayoutControl
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
            this.pointerControl1 = new SM64RAM.PointerControl();
            this.specialPointerControl1 = new SM64RAM.SpecialPointerControl();
            this.lblEntries = new System.Windows.Forms.Label();
            this.treeEntries = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnLoadPreset = new System.Windows.Forms.Button();
            this.btnSavePreset = new System.Windows.Forms.Button();
            this.contextTreeEntries = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmAddCommand = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextTreeEntries.SuspendLayout();
            this.SuspendLayout();
            // 
            // pointerControl1
            // 
            this.pointerControl1.Location = new System.Drawing.Point(3, 3);
            this.pointerControl1.Name = "pointerControl1";
            this.pointerControl1.Size = new System.Drawing.Size(258, 114);
            this.pointerControl1.TabIndex = 0;
            // 
            // specialPointerControl1
            // 
            this.specialPointerControl1.Location = new System.Drawing.Point(267, 5);
            this.specialPointerControl1.Name = "specialPointerControl1";
            this.specialPointerControl1.Size = new System.Drawing.Size(195, 112);
            this.specialPointerControl1.TabIndex = 1;
            // 
            // lblEntries
            // 
            this.lblEntries.AutoSize = true;
            this.lblEntries.Location = new System.Drawing.Point(3, 129);
            this.lblEntries.Name = "lblEntries";
            this.lblEntries.Size = new System.Drawing.Size(39, 13);
            this.lblEntries.TabIndex = 3;
            this.lblEntries.Text = "Entries";
            // 
            // treeEntries
            // 
            this.treeEntries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeEntries.Location = new System.Drawing.Point(3, 3);
            this.treeEntries.Name = "treeEntries";
            this.treeEntries.Size = new System.Drawing.Size(362, 276);
            this.treeEntries.TabIndex = 4;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 145);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeEntries);
            this.splitContainer1.Size = new System.Drawing.Size(600, 282);
            this.splitContainer1.SplitterDistance = 368;
            this.splitContainer1.TabIndex = 5;
            // 
            // btnLoadPreset
            // 
            this.btnLoadPreset.Location = new System.Drawing.Point(48, 119);
            this.btnLoadPreset.Name = "btnLoadPreset";
            this.btnLoadPreset.Size = new System.Drawing.Size(75, 23);
            this.btnLoadPreset.TabIndex = 6;
            this.btnLoadPreset.Text = "Load Preset";
            this.btnLoadPreset.UseVisualStyleBackColor = true;
            // 
            // btnSavePreset
            // 
            this.btnSavePreset.Location = new System.Drawing.Point(129, 119);
            this.btnSavePreset.Name = "btnSavePreset";
            this.btnSavePreset.Size = new System.Drawing.Size(75, 23);
            this.btnSavePreset.TabIndex = 6;
            this.btnSavePreset.Text = "Save Preset";
            this.btnSavePreset.UseVisualStyleBackColor = true;
            // 
            // contextTreeEntries
            // 
            this.contextTreeEntries.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmAddCommand});
            this.contextTreeEntries.Name = "contextTreeEntries";
            this.contextTreeEntries.Size = new System.Drawing.Size(157, 26);
            // 
            // tsmAddCommand
            // 
            this.tsmAddCommand.Name = "tsmAddCommand";
            this.tsmAddCommand.Size = new System.Drawing.Size(156, 22);
            this.tsmAddCommand.Text = "Add Command";
            // 
            // GeoLayoutControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSavePreset);
            this.Controls.Add(this.btnLoadPreset);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.lblEntries);
            this.Controls.Add(this.specialPointerControl1);
            this.Controls.Add(this.pointerControl1);
            this.Name = "GeoLayoutControl";
            this.Size = new System.Drawing.Size(606, 430);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.contextTreeEntries.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SM64RAM.PointerControl pointerControl1;
        private SM64RAM.SpecialPointerControl specialPointerControl1;
        private System.Windows.Forms.Label lblEntries;
        private System.Windows.Forms.TreeView treeEntries;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnLoadPreset;
        private System.Windows.Forms.Button btnSavePreset;
        private System.Windows.Forms.ContextMenuStrip contextTreeEntries;
        private System.Windows.Forms.ToolStripMenuItem tsmAddCommand;
    }
}
