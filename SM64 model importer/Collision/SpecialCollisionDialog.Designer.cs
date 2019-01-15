namespace SM64ModelImporter
{
    partial class SpecialCollisionDialog
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
            this.lstBoxes = new System.Windows.Forms.ListBox();
            this.lbType = new System.Windows.Forms.Label();
            this.lblX1 = new System.Windows.Forms.Label();
            this.numX1 = new System.Windows.Forms.NumericUpDown();
            this.numZ1 = new System.Windows.Forms.NumericUpDown();
            this.lblZ1 = new System.Windows.Forms.Label();
            this.numX2 = new System.Windows.Forms.NumericUpDown();
            this.lblX2 = new System.Windows.Forms.Label();
            this.numZ2 = new System.Windows.Forms.NumericUpDown();
            this.lblZ2 = new System.Windows.Forms.Label();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.lblY = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.btnAddBox = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZ1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZ2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // lstBoxes
            // 
            this.lstBoxes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstBoxes.FormattingEnabled = true;
            this.lstBoxes.Location = new System.Drawing.Point(12, 12);
            this.lstBoxes.Name = "lstBoxes";
            this.lstBoxes.Size = new System.Drawing.Size(120, 212);
            this.lstBoxes.TabIndex = 0;
            this.lstBoxes.SelectedIndexChanged += new System.EventHandler(this.lstBoxes_SelectedIndexChanged);
            // 
            // lbType
            // 
            this.lbType.AutoSize = true;
            this.lbType.Location = new System.Drawing.Point(139, 18);
            this.lbType.Name = "lbType";
            this.lbType.Size = new System.Drawing.Size(31, 13);
            this.lbType.TabIndex = 2;
            this.lbType.Text = "Type";
            // 
            // lblX1
            // 
            this.lblX1.AutoSize = true;
            this.lblX1.Location = new System.Drawing.Point(139, 42);
            this.lblX1.Name = "lblX1";
            this.lblX1.Size = new System.Drawing.Size(20, 13);
            this.lblX1.TabIndex = 2;
            this.lblX1.Text = "X1";
            // 
            // numX1
            // 
            this.numX1.Location = new System.Drawing.Point(183, 40);
            this.numX1.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numX1.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numX1.Name = "numX1";
            this.numX1.Size = new System.Drawing.Size(120, 20);
            this.numX1.TabIndex = 1;
            this.numX1.ValueChanged += new System.EventHandler(this.numX1_ValueChanged);
            // 
            // numZ1
            // 
            this.numZ1.Location = new System.Drawing.Point(183, 66);
            this.numZ1.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numZ1.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numZ1.Name = "numZ1";
            this.numZ1.Size = new System.Drawing.Size(120, 20);
            this.numZ1.TabIndex = 1;
            this.numZ1.ValueChanged += new System.EventHandler(this.numZ1_ValueChanged);
            // 
            // lblZ1
            // 
            this.lblZ1.AutoSize = true;
            this.lblZ1.Location = new System.Drawing.Point(139, 68);
            this.lblZ1.Name = "lblZ1";
            this.lblZ1.Size = new System.Drawing.Size(20, 13);
            this.lblZ1.TabIndex = 2;
            this.lblZ1.Text = "Z1";
            // 
            // numX2
            // 
            this.numX2.Location = new System.Drawing.Point(183, 92);
            this.numX2.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numX2.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numX2.Name = "numX2";
            this.numX2.Size = new System.Drawing.Size(120, 20);
            this.numX2.TabIndex = 1;
            this.numX2.ValueChanged += new System.EventHandler(this.numX2_ValueChanged);
            // 
            // lblX2
            // 
            this.lblX2.AutoSize = true;
            this.lblX2.Location = new System.Drawing.Point(139, 94);
            this.lblX2.Name = "lblX2";
            this.lblX2.Size = new System.Drawing.Size(20, 13);
            this.lblX2.TabIndex = 2;
            this.lblX2.Text = "X2";
            // 
            // numZ2
            // 
            this.numZ2.Location = new System.Drawing.Point(183, 118);
            this.numZ2.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numZ2.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numZ2.Name = "numZ2";
            this.numZ2.Size = new System.Drawing.Size(120, 20);
            this.numZ2.TabIndex = 1;
            this.numZ2.ValueChanged += new System.EventHandler(this.numZ2_ValueChanged);
            // 
            // lblZ2
            // 
            this.lblZ2.AutoSize = true;
            this.lblZ2.Location = new System.Drawing.Point(139, 120);
            this.lblZ2.Name = "lblZ2";
            this.lblZ2.Size = new System.Drawing.Size(20, 13);
            this.lblZ2.TabIndex = 2;
            this.lblZ2.Text = "Z2";
            // 
            // numHeight
            // 
            this.numHeight.Location = new System.Drawing.Point(183, 144);
            this.numHeight.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numHeight.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(120, 20);
            this.numHeight.TabIndex = 1;
            this.numHeight.ValueChanged += new System.EventHandler(this.numHeight_ValueChanged);
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(139, 146);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(38, 13);
            this.lblY.TabIndex = 2;
            this.lblY.Text = "Height";
            // 
            // cmbType
            // 
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            "Water",
            "Toxic",
            "Myst"});
            this.cmbType.Location = new System.Drawing.Point(183, 12);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(121, 21);
            this.cmbType.TabIndex = 3;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // btnAddBox
            // 
            this.btnAddBox.Location = new System.Drawing.Point(12, 230);
            this.btnAddBox.Name = "btnAddBox";
            this.btnAddBox.Size = new System.Drawing.Size(75, 23);
            this.btnAddBox.TabIndex = 4;
            this.btnAddBox.Text = "Add";
            this.btnAddBox.UseVisualStyleBackColor = true;
            this.btnAddBox.Click += new System.EventHandler(this.btnAddBox_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(246, 227);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // SpecialCollisionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 262);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnAddBox);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblY);
            this.Controls.Add(this.lblZ2);
            this.Controls.Add(this.lblX2);
            this.Controls.Add(this.lblZ1);
            this.Controls.Add(this.lblX1);
            this.Controls.Add(this.numHeight);
            this.Controls.Add(this.numZ2);
            this.Controls.Add(this.numX2);
            this.Controls.Add(this.numZ1);
            this.Controls.Add(this.lbType);
            this.Controls.Add(this.numX1);
            this.Controls.Add(this.lstBoxes);
            this.Name = "SpecialCollisionDialog";
            this.Text = "SpecialCollisionDialog";
            ((System.ComponentModel.ISupportInitialize)(this.numX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZ1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numX2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numZ2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstBoxes;
        private System.Windows.Forms.Label lbType;
        private System.Windows.Forms.Label lblX1;
        private System.Windows.Forms.NumericUpDown numX1;
        private System.Windows.Forms.NumericUpDown numZ1;
        private System.Windows.Forms.Label lblZ1;
        private System.Windows.Forms.NumericUpDown numX2;
        private System.Windows.Forms.Label lblX2;
        private System.Windows.Forms.NumericUpDown numZ2;
        private System.Windows.Forms.Label lblZ2;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Button btnAddBox;
        private System.Windows.Forms.Button btnOK;
    }
}