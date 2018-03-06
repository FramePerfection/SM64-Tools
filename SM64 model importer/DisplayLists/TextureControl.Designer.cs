namespace SM64_model_importer
{
    partial class TextureControl
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
            this.panelView = new System.Windows.Forms.Panel();
            this.txtCustomAddress = new System.Windows.Forms.TextBox();
            this.chkCustomAddress = new System.Windows.Forms.CheckBox();
            this.numericCustomWidth = new System.Windows.Forms.NumericUpDown();
            this.numericCustomHeight = new System.Windows.Forms.NumericUpDown();
            this.cmbFormat = new System.Windows.Forms.ComboBox();
            this.cmbAddressX = new System.Windows.Forms.ComboBox();
            this.lblAddressX = new System.Windows.Forms.Label();
            this.cmbAddressY = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pointerScrollingTextures = new SM64RAM.PointerControl();
            this.groupScrollingTextures = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericCustomWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericCustomHeight)).BeginInit();
            this.groupScrollingTextures.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelView
            // 
            this.panelView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelView.Location = new System.Drawing.Point(0, 3);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(398, 102);
            this.panelView.TabIndex = 0;
            this.panelView.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTextureDisplay_Paint);
            // 
            // txtCustomAddress
            // 
            this.txtCustomAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtCustomAddress.Enabled = false;
            this.txtCustomAddress.Location = new System.Drawing.Point(110, 134);
            this.txtCustomAddress.Name = "txtCustomAddress";
            this.txtCustomAddress.Size = new System.Drawing.Size(100, 20);
            this.txtCustomAddress.TabIndex = 1;
            this.txtCustomAddress.Text = "07000000";
            this.txtCustomAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCustomAddress.TextChanged += new System.EventHandler(this.txtCustomAddress_TextChanged);
            // 
            // chkCustomAddress
            // 
            this.chkCustomAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkCustomAddress.AutoSize = true;
            this.chkCustomAddress.Enabled = false;
            this.chkCustomAddress.Location = new System.Drawing.Point(3, 137);
            this.chkCustomAddress.Name = "chkCustomAddress";
            this.chkCustomAddress.Size = new System.Drawing.Size(101, 17);
            this.chkCustomAddress.TabIndex = 2;
            this.chkCustomAddress.Text = "custom Address";
            this.chkCustomAddress.UseVisualStyleBackColor = true;
            this.chkCustomAddress.CheckedChanged += new System.EventHandler(this.chkCustomAddress_CheckedChanged);
            // 
            // numericCustomWidth
            // 
            this.numericCustomWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericCustomWidth.Enabled = false;
            this.numericCustomWidth.Location = new System.Drawing.Point(216, 134);
            this.numericCustomWidth.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericCustomWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericCustomWidth.Name = "numericCustomWidth";
            this.numericCustomWidth.Size = new System.Drawing.Size(72, 20);
            this.numericCustomWidth.TabIndex = 3;
            this.numericCustomWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericCustomWidth.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericCustomWidth.ValueChanged += new System.EventHandler(this.numericCustomWidth_ValueChanged);
            // 
            // numericCustomHeight
            // 
            this.numericCustomHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericCustomHeight.Enabled = false;
            this.numericCustomHeight.Location = new System.Drawing.Point(294, 134);
            this.numericCustomHeight.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericCustomHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericCustomHeight.Name = "numericCustomHeight";
            this.numericCustomHeight.Size = new System.Drawing.Size(72, 20);
            this.numericCustomHeight.TabIndex = 4;
            this.numericCustomHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericCustomHeight.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericCustomHeight.ValueChanged += new System.EventHandler(this.numericHeight_ValueChanged);
            // 
            // cmbFormat
            // 
            this.cmbFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormat.FormattingEnabled = true;
            this.cmbFormat.Items.AddRange(new object[] {
            "16bit RGBA",
            "32bit RGBA",
            "4bit Color Indexed",
            "8bit Color Indexed",
            "4bit Intensity Alpha",
            "8bit Intensity Alpha",
            "16bit Intensity Alpha",
            "4bit Intensity",
            "8bit Intensity"});
            this.cmbFormat.Location = new System.Drawing.Point(3, 107);
            this.cmbFormat.Name = "cmbFormat";
            this.cmbFormat.Size = new System.Drawing.Size(121, 21);
            this.cmbFormat.TabIndex = 5;
            this.cmbFormat.SelectedIndexChanged += new System.EventHandler(this.cmbFormat_SelectedIndexChanged);
            // 
            // cmbAddressX
            // 
            this.cmbAddressX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbAddressX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAddressX.FormattingEnabled = true;
            this.cmbAddressX.Items.AddRange(new object[] {
            "Wrap",
            "Mirror",
            "Clamp"});
            this.cmbAddressX.Location = new System.Drawing.Point(162, 107);
            this.cmbAddressX.Name = "cmbAddressX";
            this.cmbAddressX.Size = new System.Drawing.Size(80, 21);
            this.cmbAddressX.TabIndex = 6;
            this.cmbAddressX.SelectedIndexChanged += new System.EventHandler(this.cmbAddressX_SelectedIndexChanged);
            // 
            // lblAddressX
            // 
            this.lblAddressX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAddressX.AutoSize = true;
            this.lblAddressX.Location = new System.Drawing.Point(139, 110);
            this.lblAddressX.Name = "lblAddressX";
            this.lblAddressX.Size = new System.Drawing.Size(17, 13);
            this.lblAddressX.TabIndex = 7;
            this.lblAddressX.Text = "X:";
            // 
            // cmbAddressY
            // 
            this.cmbAddressY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbAddressY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAddressY.FormattingEnabled = true;
            this.cmbAddressY.Items.AddRange(new object[] {
            "Wrap",
            "Mirror",
            "Clamp"});
            this.cmbAddressY.Location = new System.Drawing.Point(271, 107);
            this.cmbAddressY.Name = "cmbAddressY";
            this.cmbAddressY.Size = new System.Drawing.Size(80, 21);
            this.cmbAddressY.TabIndex = 6;
            this.cmbAddressY.SelectedIndexChanged += new System.EventHandler(this.cmbAddressY_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(248, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Y:";
            // 
            // pointerScrollingTextures
            // 
            this.pointerScrollingTextures.Location = new System.Drawing.Point(6, 19);
            this.pointerScrollingTextures.Name = "pointerScrollingTextures";
            this.pointerScrollingTextures.Size = new System.Drawing.Size(258, 116);
            this.pointerScrollingTextures.TabIndex = 8;
            this.pointerScrollingTextures.ValueChanged += new System.EventHandler(this.pointerScrollingTextures_ValueChanged);
            // 
            // groupScrollingTextures
            // 
            this.groupScrollingTextures.Controls.Add(this.pointerScrollingTextures);
            this.groupScrollingTextures.Location = new System.Drawing.Point(404, 3);
            this.groupScrollingTextures.Name = "groupScrollingTextures";
            this.groupScrollingTextures.Size = new System.Drawing.Size(265, 151);
            this.groupScrollingTextures.TabIndex = 9;
            this.groupScrollingTextures.TabStop = false;
            this.groupScrollingTextures.Text = "Scrolling Textures";
            // 
            // TextureControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupScrollingTextures);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbAddressY);
            this.Controls.Add(this.lblAddressX);
            this.Controls.Add(this.cmbAddressX);
            this.Controls.Add(this.cmbFormat);
            this.Controls.Add(this.numericCustomHeight);
            this.Controls.Add(this.numericCustomWidth);
            this.Controls.Add(this.chkCustomAddress);
            this.Controls.Add(this.txtCustomAddress);
            this.Controls.Add(this.panelView);
            this.Name = "TextureControl";
            this.Size = new System.Drawing.Size(672, 160);
            ((System.ComponentModel.ISupportInitialize)(this.numericCustomWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericCustomHeight)).EndInit();
            this.groupScrollingTextures.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelView;
        private System.Windows.Forms.TextBox txtCustomAddress;
        private System.Windows.Forms.CheckBox chkCustomAddress;
        private System.Windows.Forms.NumericUpDown numericCustomWidth;
        private System.Windows.Forms.NumericUpDown numericCustomHeight;
        private System.Windows.Forms.ComboBox cmbFormat;
        private System.Windows.Forms.ComboBox cmbAddressX;
        private System.Windows.Forms.Label lblAddressX;
        private System.Windows.Forms.ComboBox cmbAddressY;
        private System.Windows.Forms.Label label1;
        private SM64RAM.PointerControl pointerScrollingTextures;
        private System.Windows.Forms.GroupBox groupScrollingTextures;
    }
}
