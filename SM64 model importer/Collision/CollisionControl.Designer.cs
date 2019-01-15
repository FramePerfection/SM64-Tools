namespace SM64ModelImporter
{
    partial class CollisionControl
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
            this.btnLoadObj = new System.Windows.Forms.Button();
            this.pointerControl1 = new SM64RAM.PointerControl();
            this.groupObject = new System.Windows.Forms.GroupBox();
            this.specialPointerControl1 = new SM64RAM.SpecialPointerControl();
            this.lblObjFileName = new System.Windows.Forms.Label();
            this.cmbTypeStyle = new System.Windows.Forms.ComboBox();
            this.groupCollisionTypes = new System.Windows.Forms.GroupBox();
            this.panelCollisionTypes = new System.Windows.Forms.Panel();
            this.btnSpecialCollision = new System.Windows.Forms.Button();
            this.groupObject.SuspendLayout();
            this.groupCollisionTypes.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnLoadObj
            // 
            this.btnLoadObj.Location = new System.Drawing.Point(6, 35);
            this.btnLoadObj.Name = "btnLoadObj";
            this.btnLoadObj.Size = new System.Drawing.Size(96, 23);
            this.btnLoadObj.TabIndex = 0;
            this.btnLoadObj.Text = "Load File";
            this.btnLoadObj.UseVisualStyleBackColor = true;
            this.btnLoadObj.Click += new System.EventHandler(this.btnLoadObj_Click);
            // 
            // pointerControl1
            // 
            this.pointerControl1.Location = new System.Drawing.Point(108, 19);
            this.pointerControl1.Name = "pointerControl1";
            this.pointerControl1.Size = new System.Drawing.Size(258, 114);
            this.pointerControl1.TabIndex = 1;
            // 
            // groupObject
            // 
            this.groupObject.Controls.Add(this.specialPointerControl1);
            this.groupObject.Controls.Add(this.lblObjFileName);
            this.groupObject.Controls.Add(this.btnLoadObj);
            this.groupObject.Controls.Add(this.pointerControl1);
            this.groupObject.Location = new System.Drawing.Point(3, 3);
            this.groupObject.Name = "groupObject";
            this.groupObject.Size = new System.Drawing.Size(565, 140);
            this.groupObject.TabIndex = 2;
            this.groupObject.TabStop = false;
            this.groupObject.Text = "Object";
            // 
            // specialPointerControl1
            // 
            this.specialPointerControl1.Location = new System.Drawing.Point(364, 19);
            this.specialPointerControl1.Name = "specialPointerControl1";
            this.specialPointerControl1.Size = new System.Drawing.Size(195, 112);
            this.specialPointerControl1.TabIndex = 8;
            // 
            // lblObjFileName
            // 
            this.lblObjFileName.AutoSize = true;
            this.lblObjFileName.Location = new System.Drawing.Point(6, 19);
            this.lblObjFileName.Name = "lblObjFileName";
            this.lblObjFileName.Size = new System.Drawing.Size(82, 13);
            this.lblObjFileName.TabIndex = 7;
            this.lblObjFileName.Text = "<no file loaded>";
            // 
            // cmbTypeStyle
            // 
            this.cmbTypeStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTypeStyle.FormattingEnabled = true;
            this.cmbTypeStyle.Items.AddRange(new object[] {
            "By Object",
            "By Material"});
            this.cmbTypeStyle.Location = new System.Drawing.Point(574, 14);
            this.cmbTypeStyle.Name = "cmbTypeStyle";
            this.cmbTypeStyle.Size = new System.Drawing.Size(103, 21);
            this.cmbTypeStyle.TabIndex = 3;
            this.cmbTypeStyle.SelectedIndexChanged += new System.EventHandler(this.cmbTypeStyle_SelectedIndexChanged);
            // 
            // groupCollisionTypes
            // 
            this.groupCollisionTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupCollisionTypes.Controls.Add(this.panelCollisionTypes);
            this.groupCollisionTypes.Location = new System.Drawing.Point(3, 149);
            this.groupCollisionTypes.Margin = new System.Windows.Forms.Padding(3, 17, 3, 3);
            this.groupCollisionTypes.Name = "groupCollisionTypes";
            this.groupCollisionTypes.Size = new System.Drawing.Size(674, 292);
            this.groupCollisionTypes.TabIndex = 4;
            this.groupCollisionTypes.TabStop = false;
            this.groupCollisionTypes.Text = "Collision Types";
            // 
            // panelCollisionTypes
            // 
            this.panelCollisionTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCollisionTypes.AutoScroll = true;
            this.panelCollisionTypes.Location = new System.Drawing.Point(0, 19);
            this.panelCollisionTypes.Name = "panelCollisionTypes";
            this.panelCollisionTypes.Size = new System.Drawing.Size(674, 273);
            this.panelCollisionTypes.TabIndex = 0;
            // 
            // btnSpecialCollision
            // 
            this.btnSpecialCollision.Location = new System.Drawing.Point(574, 41);
            this.btnSpecialCollision.Name = "btnSpecialCollision";
            this.btnSpecialCollision.Size = new System.Drawing.Size(103, 23);
            this.btnSpecialCollision.TabIndex = 5;
            this.btnSpecialCollision.Text = "Special Collision";
            this.btnSpecialCollision.UseVisualStyleBackColor = true;
            this.btnSpecialCollision.Click += new System.EventHandler(this.btnSpecialCollision_Click);
            // 
            // CollisionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSpecialCollision);
            this.Controls.Add(this.groupCollisionTypes);
            this.Controls.Add(this.cmbTypeStyle);
            this.Controls.Add(this.groupObject);
            this.Name = "CollisionControl";
            this.Size = new System.Drawing.Size(680, 444);
            this.groupObject.ResumeLayout(false);
            this.groupObject.PerformLayout();
            this.groupCollisionTypes.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadObj;
        private SM64RAM.PointerControl pointerControl1;
        private System.Windows.Forms.GroupBox groupObject;
        private System.Windows.Forms.Label lblObjFileName;
        private System.Windows.Forms.ComboBox cmbTypeStyle;
        private System.Windows.Forms.GroupBox groupCollisionTypes;
        private System.Windows.Forms.Panel panelCollisionTypes;
        private SM64RAM.SpecialPointerControl specialPointerControl1;
        private System.Windows.Forms.Button btnSpecialCollision;
    }
}
