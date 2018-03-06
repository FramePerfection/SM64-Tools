namespace SM64LevelEditor
{
    partial class ToolBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbLevel = new System.Windows.Forms.ComboBox();
            this.warpControl = new SM64LevelEditor.Controls.WarpControl();
            this.objectControl1 = new SM64LevelEditor.Controls.ObjectControl();
            this.SuspendLayout();
            // 
            // cmbLevel
            // 
            this.cmbLevel.FormatString = "X";
            this.cmbLevel.FormattingEnabled = true;
            this.cmbLevel.Location = new System.Drawing.Point(12, 12);
            this.cmbLevel.Name = "cmbLevel";
            this.cmbLevel.Size = new System.Drawing.Size(204, 21);
            this.cmbLevel.TabIndex = 0;
            this.cmbLevel.SelectedIndexChanged += new System.EventHandler(this.cmbLevel_SelectedIndexChanged);
            // 
            // warpControl
            // 
            this.warpControl.Location = new System.Drawing.Point(12, 358);
            this.warpControl.Name = "warpControl";
            this.warpControl.Size = new System.Drawing.Size(204, 192);
            this.warpControl.TabIndex = 2;
            // 
            // objectControl1
            // 
            this.objectControl1.Location = new System.Drawing.Point(12, 39);
            this.objectControl1.Name = "objectControl1";
            this.objectControl1.Size = new System.Drawing.Size(204, 313);
            this.objectControl1.TabIndex = 1;
            // 
            // ToolBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(228, 562);
            this.Controls.Add(this.warpControl);
            this.Controls.Add(this.objectControl1);
            this.Controls.Add(this.cmbLevel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ToolBox";
            this.Text = "ToolBox";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbLevel;
        public Controls.ObjectControl objectControl1;
        public Controls.WarpControl warpControl;
    }
}