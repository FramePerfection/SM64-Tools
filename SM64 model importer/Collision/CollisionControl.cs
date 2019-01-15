using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SM64RAM;

namespace SM64ModelImporter
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
            EmulationState.instance.BankLoaded += (int bank) =>
            {
                if (bank == 0x19)
                    areaCollision.Clear();
            };
        }

        public CollisionControl()
        {
            InitializeComponent();
            cmbTypeStyle.SelectedIndex = 1;
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.END_OF_LEVEL_LAYOUT].Add(UpdateSpecialPointers);
            UpdateSpecialPointers(null);
        }

        bool UpdateSpecialPointers(byte[] commandBytes)
        {
            AreaCollisionPointer[] arr = new AreaCollisionPointer[areaCollision.Count];
            int i = 0;
            foreach (KeyValuePair<byte, int> ack in areaCollision)
                arr[i++] = new AreaCollisionPointer(ack.Key, ack.Value);
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

        private void cmbTypeStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFile();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            LevelScriptReader.executors[(byte)LEVEL_SCRIPT_COMMANDS.END_OF_LEVEL_LAYOUT].Remove(UpdateSpecialPointers);
        }

        private void btnSpecialCollision_Click(object sender, EventArgs e)
        {
            SpecialCollisionDialog dlg = new SpecialCollisionDialog(collision.specialBoxes);
            if (dlg.ShowDialog() != DialogResult.OK) return;
            collision.specialBoxes = dlg.cmd;
        }

        public int PrepareForImport()
        {
            try
            {
                FileParser.Block old_settings = new FileParser.Block(this); //Restore settings for collision after loading
                LoadFile();
                LoadSettings(old_settings);
            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public int Import(int segmentOffset)
        {
            foreach (Control ctrl in panelCollisionTypes.Controls)
            {
                CollisionTypeControl patchControl = ctrl as CollisionTypeControl;
                if (patchControl != null && !patchControl.enableImport)
                    collision.patches.Remove(patchControl.patch);
            }
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
                ctrl.patch = patch;
                if (cmbTypeStyle.SelectedIndex == 1) //By Material
                    ctrl.previewImage = patch.materialImage;
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

            foreach (Control control in panelCollisionTypes.Controls)
            {
                CollisionTypeControl c = control as CollisionTypeControl;
                if (c != null)
                {
                    block.SetInt(c.patch.name, c.patch.type);
                    block.SetBool(c.patch.name + " enabled", c.enableImport);
                }
            }

            if (collision.specialBoxes != null)
            {
                int num = collision.specialBoxes.boxes.Count;
                int[] type = new int[num], x1 = new int[num], x2 = new int[num], z1 = new int[num], z2 = new int[num], y = new int[num];
                int i = 0;
                foreach (SpecialCollisionBox box in collision.specialBoxes.boxes)
                {
                    type[i] = box.type;
                    x1[i] = box.x1;
                    x2[i] = box.x2;
                    z1[i] = box.z1;
                    z2[i] = box.z2;
                    y[i] = box.y;
                    i++;
                }
                block.SetIntArray("Special Boxes", type, true);
                block.SetIntArray("Special Boxes X1", x1, false);
                block.SetIntArray("Special Boxes X2", x2, false);
                block.SetIntArray("Special Boxes Z1", z1, false);
                block.SetIntArray("Special Boxes Z2", z2, false);
                block.SetIntArray("Special Boxes Height", y, false);
            }
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
                if (c != null)
                {
                    c.SetType(block.GetInt(c.patch.name, false));
                    c.enableImport = block.GetBool(c.patch.name + " enabled", false);
                }
            }

            int[] specialBoxTypes = block.GetIntArray("Special Boxes", false);
            int[] specialBoxX1 = block.GetIntArray("Special Boxes X1", false);
            int[] specialBoxX2 = block.GetIntArray("Special Boxes X2", false);
            int[] specialBoxZ1 = block.GetIntArray("Special Boxes Z1", false);
            int[] specialBoxZ2 = block.GetIntArray("Special Boxes Z2", false);
            int[] specialBoxY = block.GetIntArray("Special Boxes Height", false);
            if (specialBoxTypes.Length > 0)
                collision.specialBoxes = new SpecialBoxes();
            for (int i = 0; i < specialBoxTypes.Length; i++)
            {
                SpecialCollisionBox box = new SpecialCollisionBox();
                box.type = (short)specialBoxTypes[i];
                if (i < specialBoxX1.Length) box.x1 = (short)specialBoxX1[i];
                if (i < specialBoxX2.Length) box.x2 = (short)specialBoxX2[i];
                if (i < specialBoxZ1.Length) box.z1 = (short)specialBoxZ1[i];
                if (i < specialBoxZ2.Length) box.z2 = (short)specialBoxZ2[i];
                if (i < specialBoxY.Length) box.y = (short)specialBoxY[i];
                collision.specialBoxes.boxes.Add(box);
            }
        }

        #endregion
    }
}
