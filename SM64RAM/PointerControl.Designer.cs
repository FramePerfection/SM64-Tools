namespace SM64RAM
{
    partial class PointerControl
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
            this.txtROMPointer = new System.Windows.Forms.TextBox();
            this.txtRAMPointer = new System.Windows.Forms.TextBox();
            this.btnAddROMptr = new System.Windows.Forms.Button();
            this.btnAddRAMptr = new System.Windows.Forms.Button();
            this.listROMPointers = new System.Windows.Forms.ListBox();
            this.listRAMPointers = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtROMPointer
            // 
            this.txtROMPointer.Location = new System.Drawing.Point(131, 87);
            this.txtROMPointer.Name = "txtROMPointer";
            this.txtROMPointer.Size = new System.Drawing.Size(84, 20);
            this.txtROMPointer.TabIndex = 16;
            // 
            // txtRAMPointer
            // 
            this.txtRAMPointer.Location = new System.Drawing.Point(6, 86);
            this.txtRAMPointer.Name = "txtRAMPointer";
            this.txtRAMPointer.Size = new System.Drawing.Size(84, 20);
            this.txtRAMPointer.TabIndex = 17;
            // 
            // btnAddROMptr
            // 
            this.btnAddROMptr.Location = new System.Drawing.Point(221, 84);
            this.btnAddROMptr.Name = "btnAddROMptr";
            this.btnAddROMptr.Size = new System.Drawing.Size(29, 23);
            this.btnAddROMptr.TabIndex = 14;
            this.btnAddROMptr.Text = "+";
            this.btnAddROMptr.UseVisualStyleBackColor = true;
            this.btnAddROMptr.Click += new System.EventHandler(this.btnAddROMptr_Click);
            // 
            // btnAddRAMptr
            // 
            this.btnAddRAMptr.Location = new System.Drawing.Point(96, 84);
            this.btnAddRAMptr.Name = "btnAddRAMptr";
            this.btnAddRAMptr.Size = new System.Drawing.Size(29, 23);
            this.btnAddRAMptr.TabIndex = 15;
            this.btnAddRAMptr.Text = "+";
            this.btnAddRAMptr.UseVisualStyleBackColor = true;
            this.btnAddRAMptr.Click += new System.EventHandler(this.btnAddRAMptr_Click);
            // 
            // listROMPointers
            // 
            this.listROMPointers.FormatString = "X8";
            this.listROMPointers.FormattingEnabled = true;
            this.listROMPointers.Location = new System.Drawing.Point(131, 13);
            this.listROMPointers.Name = "listROMPointers";
            this.listROMPointers.Size = new System.Drawing.Size(119, 69);
            this.listROMPointers.TabIndex = 12;
            // 
            // listRAMPointers
            // 
            this.listRAMPointers.FormatString = "X8";
            this.listRAMPointers.FormattingEnabled = true;
            this.listRAMPointers.Location = new System.Drawing.Point(6, 13);
            this.listRAMPointers.Name = "listRAMPointers";
            this.listRAMPointers.Size = new System.Drawing.Size(119, 69);
            this.listRAMPointers.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(131, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "ROM Pointers";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Segmented Pointers";
            // 
            // PointerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtROMPointer);
            this.Controls.Add(this.txtRAMPointer);
            this.Controls.Add(this.btnAddROMptr);
            this.Controls.Add(this.btnAddRAMptr);
            this.Controls.Add(this.listROMPointers);
            this.Controls.Add(this.listRAMPointers);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PointerControl";
            this.Size = new System.Drawing.Size(258, 114);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtROMPointer;
        private System.Windows.Forms.TextBox txtRAMPointer;
        private System.Windows.Forms.Button btnAddROMptr;
        private System.Windows.Forms.Button btnAddRAMptr;
        private System.Windows.Forms.ListBox listROMPointers;
        private System.Windows.Forms.ListBox listRAMPointers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
