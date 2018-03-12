namespace SM64ModelImporter
{
    partial class DisplayListControl
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
            this.txtRawCommands = new System.Windows.Forms.RichTextBox();
            this.btnLoadObj = new System.Windows.Forms.Button();
            this.groupObjControls = new System.Windows.Forms.GroupBox();
            this.pointerControl = new SM64RAM.PointerControl();
            this.lblObjFileName = new System.Windows.Forms.Label();
            this.btnSavePreset = new System.Windows.Forms.Button();
            this.btnLoadPreset = new System.Windows.Forms.Button();
            this.tabSettings = new System.Windows.Forms.TabControl();
            this.tabObj = new System.Windows.Forms.TabPage();
            this.tabCombine = new System.Windows.Forms.TabPage();
            this.tabBlender = new System.Windows.Forms.TabPage();
            this.tabOtherModes = new System.Windows.Forms.TabPage();
            this.tabParams = new System.Windows.Forms.TabPage();
            this.lblLayer = new System.Windows.Forms.Label();
            this.numLayer = new System.Windows.Forms.NumericUpDown();
            this.combinerStateControl = new SM64ModelImporter.CombinerStateControl();
            this.blenderControl1 = new SM64ModelImporter.BlenderControl();
            this.renderStateControl = new SM64ModelImporter.RenderStateControl();
            this.paramFogIntensity = new SM64ModelImporter.ParameterControl();
            this.paramFogColor = new SM64ModelImporter.ParameterControl();
            this.paramColor = new SM64ModelImporter.ParameterControl();
            this.textureControl = new SM64ModelImporter.TextureControl();
            this.btnConversionOptions = new System.Windows.Forms.Button();
            this.groupObjControls.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabObj.SuspendLayout();
            this.tabCombine.SuspendLayout();
            this.tabBlender.SuspendLayout();
            this.tabOtherModes.SuspendLayout();
            this.tabParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numLayer)).BeginInit();
            this.SuspendLayout();
            // 
            // txtRawCommands
            // 
            this.txtRawCommands.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRawCommands.Location = new System.Drawing.Point(6, 365);
            this.txtRawCommands.Name = "txtRawCommands";
            this.txtRawCommands.Size = new System.Drawing.Size(655, 93);
            this.txtRawCommands.TabIndex = 0;
            this.txtRawCommands.Text = "";
            // 
            // btnLoadObj
            // 
            this.btnLoadObj.Location = new System.Drawing.Point(6, 35);
            this.btnLoadObj.Name = "btnLoadObj";
            this.btnLoadObj.Size = new System.Drawing.Size(100, 23);
            this.btnLoadObj.TabIndex = 1;
            this.btnLoadObj.Text = "Load Object";
            this.btnLoadObj.UseVisualStyleBackColor = true;
            this.btnLoadObj.Click += new System.EventHandler(this.btnLoadObj_Click);
            // 
            // groupObjControls
            // 
            this.groupObjControls.Controls.Add(this.pointerControl);
            this.groupObjControls.Controls.Add(this.lblLayer);
            this.groupObjControls.Controls.Add(this.numLayer);
            this.groupObjControls.Controls.Add(this.lblObjFileName);
            this.groupObjControls.Controls.Add(this.btnConversionOptions);
            this.groupObjControls.Controls.Add(this.btnLoadObj);
            this.groupObjControls.Location = new System.Drawing.Point(6, 6);
            this.groupObjControls.Name = "groupObjControls";
            this.groupObjControls.Size = new System.Drawing.Size(363, 125);
            this.groupObjControls.TabIndex = 6;
            this.groupObjControls.TabStop = false;
            this.groupObjControls.Text = "Object";
            // 
            // pointerControl
            // 
            this.pointerControl.Location = new System.Drawing.Point(109, 11);
            this.pointerControl.Name = "pointerControl";
            this.pointerControl.Size = new System.Drawing.Size(258, 114);
            this.pointerControl.TabIndex = 12;
            // 
            // lblObjFileName
            // 
            this.lblObjFileName.AutoSize = true;
            this.lblObjFileName.Location = new System.Drawing.Point(7, 16);
            this.lblObjFileName.Name = "lblObjFileName";
            this.lblObjFileName.Size = new System.Drawing.Size(82, 13);
            this.lblObjFileName.TabIndex = 6;
            this.lblObjFileName.Text = "<no file loaded>";
            // 
            // btnSavePreset
            // 
            this.btnSavePreset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSavePreset.Location = new System.Drawing.Point(586, 464);
            this.btnSavePreset.Name = "btnSavePreset";
            this.btnSavePreset.Size = new System.Drawing.Size(75, 23);
            this.btnSavePreset.TabIndex = 10;
            this.btnSavePreset.Text = "Save Preset";
            this.btnSavePreset.UseVisualStyleBackColor = true;
            this.btnSavePreset.Click += new System.EventHandler(this.btnSavePreset_Click);
            // 
            // btnLoadPreset
            // 
            this.btnLoadPreset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadPreset.Location = new System.Drawing.Point(505, 464);
            this.btnLoadPreset.Name = "btnLoadPreset";
            this.btnLoadPreset.Size = new System.Drawing.Size(75, 23);
            this.btnLoadPreset.TabIndex = 10;
            this.btnLoadPreset.Text = "Load Preset";
            this.btnLoadPreset.UseVisualStyleBackColor = true;
            this.btnLoadPreset.Click += new System.EventHandler(this.btnLoadPreset_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabSettings.Controls.Add(this.tabObj);
            this.tabSettings.Controls.Add(this.tabCombine);
            this.tabSettings.Controls.Add(this.tabBlender);
            this.tabSettings.Controls.Add(this.tabOtherModes);
            this.tabSettings.Controls.Add(this.tabParams);
            this.tabSettings.Location = new System.Drawing.Point(0, 3);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.SelectedIndex = 0;
            this.tabSettings.Size = new System.Drawing.Size(664, 180);
            this.tabSettings.TabIndex = 13;
            // 
            // tabObj
            // 
            this.tabObj.Controls.Add(this.groupObjControls);
            this.tabObj.Location = new System.Drawing.Point(4, 22);
            this.tabObj.Name = "tabObj";
            this.tabObj.Padding = new System.Windows.Forms.Padding(3);
            this.tabObj.Size = new System.Drawing.Size(656, 154);
            this.tabObj.TabIndex = 0;
            this.tabObj.Text = "Object";
            this.tabObj.UseVisualStyleBackColor = true;
            // 
            // tabCombine
            // 
            this.tabCombine.Controls.Add(this.combinerStateControl);
            this.tabCombine.Location = new System.Drawing.Point(4, 22);
            this.tabCombine.Name = "tabCombine";
            this.tabCombine.Size = new System.Drawing.Size(656, 154);
            this.tabCombine.TabIndex = 3;
            this.tabCombine.Text = "Combiner";
            this.tabCombine.UseVisualStyleBackColor = true;
            // 
            // tabBlender
            // 
            this.tabBlender.Controls.Add(this.blenderControl1);
            this.tabBlender.Location = new System.Drawing.Point(4, 22);
            this.tabBlender.Name = "tabBlender";
            this.tabBlender.Padding = new System.Windows.Forms.Padding(3);
            this.tabBlender.Size = new System.Drawing.Size(656, 154);
            this.tabBlender.TabIndex = 1;
            this.tabBlender.Text = "Blender";
            this.tabBlender.UseVisualStyleBackColor = true;
            // 
            // tabOtherModes
            // 
            this.tabOtherModes.Controls.Add(this.renderStateControl);
            this.tabOtherModes.Location = new System.Drawing.Point(4, 22);
            this.tabOtherModes.Name = "tabOtherModes";
            this.tabOtherModes.Size = new System.Drawing.Size(656, 154);
            this.tabOtherModes.TabIndex = 2;
            this.tabOtherModes.Text = "OtherModes";
            this.tabOtherModes.UseVisualStyleBackColor = true;
            // 
            // tabParams
            // 
            this.tabParams.Controls.Add(this.paramFogIntensity);
            this.tabParams.Controls.Add(this.paramFogColor);
            this.tabParams.Controls.Add(this.paramColor);
            this.tabParams.Location = new System.Drawing.Point(4, 22);
            this.tabParams.Name = "tabParams";
            this.tabParams.Size = new System.Drawing.Size(656, 154);
            this.tabParams.TabIndex = 4;
            this.tabParams.Text = "Parameters";
            this.tabParams.UseVisualStyleBackColor = true;
            // 
            // lblLayer
            // 
            this.lblLayer.AutoSize = true;
            this.lblLayer.Location = new System.Drawing.Point(25, 102);
            this.lblLayer.Name = "lblLayer";
            this.lblLayer.Size = new System.Drawing.Size(33, 13);
            this.lblLayer.TabIndex = 11;
            this.lblLayer.Text = "Layer";
            // 
            // numLayer
            // 
            this.numLayer.Location = new System.Drawing.Point(64, 99);
            this.numLayer.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numLayer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLayer.Name = "numLayer";
            this.numLayer.Size = new System.Drawing.Size(42, 20);
            this.numLayer.TabIndex = 10;
            this.numLayer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numLayer.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // combinerStateControl
            // 
            this.combinerStateControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.combinerStateControl.Location = new System.Drawing.Point(3, 3);
            this.combinerStateControl.Name = "combinerStateControl";
            this.combinerStateControl.Size = new System.Drawing.Size(645, 148);
            this.combinerStateControl.TabIndex = 0;
            // 
            // blenderControl1
            // 
            this.blenderControl1.Location = new System.Drawing.Point(3, 3);
            this.blenderControl1.Name = "blenderControl1";
            this.blenderControl1.Size = new System.Drawing.Size(593, 118);
            this.blenderControl1.TabIndex = 11;
            // 
            // renderStateControl
            // 
            this.renderStateControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.renderStateControl.Location = new System.Drawing.Point(3, 3);
            this.renderStateControl.Name = "renderStateControl";
            this.renderStateControl.Size = new System.Drawing.Size(650, 148);
            this.renderStateControl.TabIndex = 12;
            // 
            // paramFogIntensity
            // 
            this.paramFogIntensity.customValue = 0;
            this.paramFogIntensity.Location = new System.Drawing.Point(3, 62);
            this.paramFogIntensity.Name = "paramFogIntensity";
            this.paramFogIntensity.ParameterName = "Fog Intensity";
            this.paramFogIntensity.Size = new System.Drawing.Size(232, 53);
            this.paramFogIntensity.TabIndex = 0;
            // 
            // paramFogColor
            // 
            this.paramFogColor.customValue = 0;
            this.paramFogColor.Location = new System.Drawing.Point(241, 3);
            this.paramFogColor.Name = "paramFogColor";
            this.paramFogColor.ParameterName = "Fog Color";
            this.paramFogColor.Size = new System.Drawing.Size(232, 53);
            this.paramFogColor.TabIndex = 0;
            // 
            // paramColor
            // 
            this.paramColor.customValue = 0;
            this.paramColor.Location = new System.Drawing.Point(3, 3);
            this.paramColor.Name = "paramColor";
            this.paramColor.ParameterName = "Environment Color";
            this.paramColor.Size = new System.Drawing.Size(232, 53);
            this.paramColor.TabIndex = 0;
            // 
            // textureControl
            // 
            this.textureControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textureControl.Location = new System.Drawing.Point(0, 189);
            this.textureControl.materialLibrary = null;
            this.textureControl.Name = "textureControl";
            this.textureControl.Size = new System.Drawing.Size(664, 170);
            this.textureControl.TabIndex = 8;
            // 
            // btnConversionOptions
            // 
            this.btnConversionOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConversionOptions.Location = new System.Drawing.Point(6, 64);
            this.btnConversionOptions.Name = "btnConversionOptions";
            this.btnConversionOptions.Size = new System.Drawing.Size(100, 19);
            this.btnConversionOptions.TabIndex = 1;
            this.btnConversionOptions.Text = "Conversion Options";
            this.btnConversionOptions.UseVisualStyleBackColor = true;
            this.btnConversionOptions.Click += new System.EventHandler(this.btnConversionOptions_Click);
            // 
            // DisplayListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabSettings);
            this.Controls.Add(this.btnLoadPreset);
            this.Controls.Add(this.btnSavePreset);
            this.Controls.Add(this.textureControl);
            this.Controls.Add(this.txtRawCommands);
            this.Name = "DisplayListControl";
            this.Size = new System.Drawing.Size(664, 492);
            this.groupObjControls.ResumeLayout(false);
            this.groupObjControls.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.tabObj.ResumeLayout(false);
            this.tabCombine.ResumeLayout(false);
            this.tabBlender.ResumeLayout(false);
            this.tabOtherModes.ResumeLayout(false);
            this.tabParams.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numLayer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtRawCommands;
        private System.Windows.Forms.Button btnLoadObj;
        private System.Windows.Forms.GroupBox groupObjControls;
        private System.Windows.Forms.Label lblObjFileName;
        private TextureControl textureControl;
        private System.Windows.Forms.Button btnSavePreset;
        private BlenderControl blenderControl1;
        private RenderStateControl renderStateControl;
        private System.Windows.Forms.Button btnLoadPreset;
        private System.Windows.Forms.TabControl tabSettings;
        private System.Windows.Forms.TabPage tabObj;
        private System.Windows.Forms.TabPage tabBlender;
        private System.Windows.Forms.TabPage tabOtherModes;
        private System.Windows.Forms.TabPage tabCombine;
        private System.Windows.Forms.TabPage tabParams;
        private CombinerStateControl combinerStateControl;
        private ParameterControl paramColor;
        private ParameterControl paramFogIntensity;
        private ParameterControl paramFogColor;
        private SM64RAM.PointerControl pointerControl;
        private System.Windows.Forms.Label lblLayer;
        private System.Windows.Forms.NumericUpDown numLayer;
        private System.Windows.Forms.Button btnConversionOptions;
    }
}
