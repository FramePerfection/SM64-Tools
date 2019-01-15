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
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabObjects = new System.Windows.Forms.TabPage();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.txtMusic = new System.Windows.Forms.TextBox();
            this.lblMusic = new System.Windows.Forms.Label();
            this.tabMain.SuspendLayout();
            this.tabObjects.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbLevel
            // 
            this.cmbLevel.FormatString = "X";
            this.cmbLevel.FormattingEnabled = true;
            this.cmbLevel.Location = new System.Drawing.Point(3, 6);
            this.cmbLevel.Name = "cmbLevel";
            this.cmbLevel.Size = new System.Drawing.Size(204, 21);
            this.cmbLevel.TabIndex = 0;
            this.cmbLevel.SelectedIndexChanged += new System.EventHandler(this.cmbLevel_SelectedIndexChanged);
            // 
            // warpControl
            // 
            this.warpControl.Location = new System.Drawing.Point(6, 3);
            this.warpControl.Name = "warpControl";
            this.warpControl.Size = new System.Drawing.Size(212, 192);
            this.warpControl.TabIndex = 2;
            // 
            // objectControl1
            // 
            this.objectControl1.Location = new System.Drawing.Point(3, 33);
            this.objectControl1.Name = "objectControl1";
            this.objectControl1.Size = new System.Drawing.Size(204, 514);
            this.objectControl1.TabIndex = 1;
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabObjects);
            this.tabMain.Controls.Add(this.tabSettings);
            this.tabMain.Location = new System.Drawing.Point(2, 12);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(232, 580);
            this.tabMain.TabIndex = 3;
            // 
            // tabObjects
            // 
            this.tabObjects.Controls.Add(this.objectControl1);
            this.tabObjects.Controls.Add(this.cmbLevel);
            this.tabObjects.Location = new System.Drawing.Point(4, 22);
            this.tabObjects.Name = "tabObjects";
            this.tabObjects.Padding = new System.Windows.Forms.Padding(3);
            this.tabObjects.Size = new System.Drawing.Size(224, 554);
            this.tabObjects.TabIndex = 0;
            this.tabObjects.Text = "Objects";
            this.tabObjects.UseVisualStyleBackColor = true;
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.lblMusic);
            this.tabSettings.Controls.Add(this.txtMusic);
            this.tabSettings.Controls.Add(this.warpControl);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(224, 554);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Area Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // txtMusic
            // 
            this.txtMusic.Location = new System.Drawing.Point(146, 201);
            this.txtMusic.Name = "txtMusic";
            this.txtMusic.Size = new System.Drawing.Size(71, 20);
            this.txtMusic.TabIndex = 3;
            this.txtMusic.TextChanged += new System.EventHandler(this.txtMusic_TextChanged);
            // 
            // lblMusic
            // 
            this.lblMusic.AutoSize = true;
            this.lblMusic.Location = new System.Drawing.Point(6, 204);
            this.lblMusic.Name = "lblMusic";
            this.lblMusic.Size = new System.Drawing.Size(130, 13);
            this.lblMusic.TabIndex = 4;
            this.lblMusic.Text = "Music Sequence Number:";
            // 
            // ToolBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 593);
            this.Controls.Add(this.tabMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ToolBox";
            this.Text = "ToolBox";
            this.tabMain.ResumeLayout(false);
            this.tabObjects.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbLevel;
        public Controls.ObjectControl objectControl1;
        public Controls.WarpControl warpControl;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabObjects;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.Label lblMusic;
        private System.Windows.Forms.TextBox txtMusic;
    }
}