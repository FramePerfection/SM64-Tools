using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SM64RAM;

namespace SM64_model_importer
{
    public partial class CollisionControl : UserControl, Importable, ReadWrite
    {
        public bool useCustomAddress { get { return false; } }
        Collision collision = new Collision();
        int segmentOffset;
        string sourceFileName;

        static Dictionary<byte, int> areaCollision = new Dictionary<byte, int>();
        static byte currentArea = 0xFF;

        public static void Init()
        {
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.START_AREA].Add((byte[] commandBytes) => { currentArea = commandBytes[2]; return true; });
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.END_AREA].Add((byte[] commandBytes) => { currentArea = 0xFF; return true; });
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.LOAD_COLLISION].Add((byte[] commandBytes) =>
            {
                if (currentArea < 0xFF)
                    areaCollision[currentArea] = LevelScriptReader.cursorPosition;
                return true;
            });
            EmulationState.instance.BankLoaded += (int bank) => {
                if (bank == 0x19)
                    areaCollision.Clear(); 
            };
        }

        public CollisionControl()
        {
            InitializeComponent();
            cmbTypeStyle.SelectedIndex = 0;
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.END_OF_LEVEL_LAYOUT].Add(UpdateSpecialPointers);
            UpdateSpecialPointers(null);
        }

        bool UpdateSpecialPointers(byte[] commandBytes)
        {
            AreaCollisionPointer[] arr = new AreaCollisionPointer[areaCollision.Count];
            int i = 0;
            foreach (KeyValuePair<byte, int> ack in areaCollision)
                arr[i] = new AreaCollisionPointer(ack.Key, ack.Value);
            specialPointerControl1.SetPointerSource(arr);
            return true;
        }

        private void btnLoadObj_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Wavefront obj files|*.obj";
            if (dlg.ShowDialog() != DialogResult.OK) return;
            sourceFileName = dlg.FileName;
            LoadFile();
        }

        public int PrepareForImport()
        {
            try
            {
                LoadFile();
            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public int Import(int segmentOffset)
        {
            int totalSize = collision.GetLength();
            this.segmentOffset = segmentOffset;
            int segment = segmentOffset >> 0x18;
            int offsetInBank = segmentOffset & 0xFFFFFF;
            EmulationState.RAMBank bank = EmulationState.instance.banks[segment];
            if (EmulationState.instance.AssertRead(segmentOffset, totalSize) && bank.compressed)
            {
                EmulationState.messages.AppendMessage("The selected bank 0x" + segment.ToString("X") + " is compressed in the ROM and can therefore not be altered.", "Error");
                return -1;
            }
            collision.Write(ref bank.value, offsetInBank);

            Array.Copy(bank.value, offsetInBank, EmulationState.instance.ROM, bank.ROMStart + offsetInBank, totalSize);
            pointerControl1.WritePointers(segmentOffset);
            specialPointerControl1.WritePointers(segmentOffset);
            return segmentOffset + totalSize;
        }

        void LoadFile()
        {
            if (sourceFileName == null) return;
#if !DEBUG
            try
            {
#endif
            collision.Import(sourceFileName, (Collision.PatchMode)cmbTypeStyle.SelectedIndex);
            lblObjFileName.Text = Path.GetFileName(sourceFileName);
            panelCollisionTypes.SuspendLayout();
            panelCollisionTypes.Controls.Clear();
            int y = 0;
            foreach (CollisionPatch patch in collision.patches)
            {
                CollisionTypeControl ctrl = new CollisionTypeControl();
                ctrl.Location = new Point(panelCollisionTypes.Margin.Left, panelCollisionTypes.Margin.Top + y);
                ctrl.Width = panelCollisionTypes.Width - panelCollisionTypes.Margin.Horizontal;
                ctrl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                ctrl.Patch = patch;
                y += ctrl.Size.Height + ctrl.Margin.Top + ctrl.Margin.Bottom;
                panelCollisionTypes.Controls.Add(ctrl);
            }
            panelCollisionTypes.ResumeLayout();
#if !DEBUG
            }
            catch
            {
                lblObjFileName.Text = "<Error>";
            }
#endif
        }
        #region Save/Load Settings

        public void SaveSettings(FileParser.Block block)
        {
            block.SetBool("Use Custom Address", useCustomAddress);
            if (useCustomAddress)
                block.SetInt("Custom Address", segmentOffset);
            block.SetIntArray("ROM Pointers", pointerControl1.GetROMPointers());
            block.SetIntArray("RAM Pointers", pointerControl1.GetRAMPointers());

            SpecialPointer[] specialPointers = specialPointerControl1.GetPointers();
            StringBuilder b = new StringBuilder();
            foreach (SpecialPointer p in specialPointers)
                b.Append(p.ToString() + "; ");
            if (b.Length > 0)
                block.SetString("Special Pointers", b.ToString());

            block.SetString("Obj File", sourceFileName);
            block.SetInt("Type Style", cmbTypeStyle.SelectedIndex, false);

            foreach (CollisionPatch patch in collision.patches)
                block.SetInt(patch.name, patch.type);
        }

        public void LoadSettings(FileParser.Block block)
        {
            sourceFileName = block.GetString("Obj File");
            if (sourceFileName != "")
                LoadFile();
            if (block.GetBool("Use Custom Address"))
            {
                //Set a checkbox here
                segmentOffset = block.GetInt("Custom Address");
            }
            pointerControl1.SetROMPointers(block.GetIntArray("ROM Pointers"));
            pointerControl1.SetRAMPointers(block.GetIntArray("RAM Pointers"));

            string specialPointerString = block.GetString("Special Pointers", false);
            if (specialPointerString != null)
            {
                string[] split = specialPointerString.Split(';');
                List<SpecialPointer> validPointers = new List<SpecialPointer>();
                foreach (string s in split)
                {
                    SpecialPointer newPointer = specialPointerControl1.PointerByString(s.Trim());
                    if (newPointer != null)
                        validPointers.Add(newPointer);
                }
                specialPointerControl1.SetPointers(validPointers.ToArray());
            }

            cmbTypeStyle.SelectedIndex = block.GetInt("Type Style", false);

            foreach (Control control in panelCollisionTypes.Controls)
            {
                CollisionTypeControl c = control as CollisionTypeControl;
                if (c != null) c.SetType(block.GetInt(c.Patch.name, false));
            }
        }

        #endregion

        private void cmbTypeStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFile();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.END_OF_LEVEL_LAYOUT].Remove(UpdateSpecialPointers);
        }
    }
}
