using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace SM64RAM
{
    public static class PatchEngine
    {
        public static void Run(string patchString, string patchFileDirectory)
        {
            if (patchString != "")
            {
                    string[] patches = patchString.Split(';');
                    foreach (string patch in patches)
                    {
                        string[] patchSplitA = patch.Split(new char[] { ':' }, 2);
                        string patchName = patchSplitA.Length > 1 ? patchSplitA[0] : patch;
                        if (patchName.EndsWith("?") && (MessageBox.Show("Apply patch " + patchName, "Patch ROM", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)) continue;
                        string[] patchParts = (patchSplitA.Length > 1 ? patchSplitA[1] : patch).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string patchPart in patchParts)
                        {
                            string[] patchSplit = patchPart.Split(new string[] { "->" }, 2, StringSplitOptions.None);
                            if (patchSplit.Length != 2) { MessageBox.Show("Invalid patch " + patchName); continue; }
                            int patchOffset;
                            string ack = patchSplit[0].Trim().Substring(2);
                            if (!cvt.ParseIntHex(ack, out patchOffset))
                            { MessageBox.Show("Invalid patch offset " + patchName); continue; }

                            patchSplit[1] = patchSplit[1].ToLower().Trim();
                            if (patchSplit[1].StartsWith("file:")) //Patch file
                            {
                                string patchFileName = patchSplit[1].Substring("file:".Length).Trim();
                                patchFileName = System.IO.Path.IsPathRooted(patchFileName) ? patchFileName : patchFileDirectory + "\\" + patchFileName;
                                if (!File.Exists(patchFileName)) { MessageBox.Show("File not found: " + patchFileName + "\nfor patch " + patchName); continue; }
                                byte[] patchBytes = File.ReadAllBytes(patchFileName);
                                Array.Copy(patchBytes, 0, EmulationState.instance.ROM, patchOffset, patchBytes.Length);
                            }
                            else //Patch bytes
                            {
                                string byteString = Regex.Replace(patchSplit[1], @"\s+", string.Empty);
                                if (byteString.Length % 2 != 0) { MessageBox.Show("Invalid length of byte patch in patch " + patchName); continue; }
                                byte[] patchBytes = new byte[byteString.Length / 2];
                                for (int i = 0; i < patchBytes.Length; i++)
                                    if (!byte.TryParse(byteString.Substring(i * 2, 2), System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.InvariantCulture, out patchBytes[i]))
                                    { MessageBox.Show("Invalid byte in patch " + patchName); goto Continue2; }
                                Array.Copy(patchBytes, 0, EmulationState.instance.ROM, patchOffset, patchBytes.Length);
                            }
                        }
                    Continue2: ;
                    }
                    RecalculateChecksum();
                    File.WriteAllBytes(EmulationState.instance.ROMName, EmulationState.instance.ROM);
            }
        }

        public static void RecalculateChecksum()
        {
            Process.Start("rn64crc.exe", "-u \"" + EmulationState.instance.ROMName + "\"");
        }
    }
}
