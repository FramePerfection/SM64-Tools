using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using SM64RAM;
using System.Globalization;

namespace SM64LevelEditor
{
    public class ProjectSettings
    {
       public List<BankDescription> banks = new List<BankDescription>();
        List<Alias<int>> commonBehaviourAlias = new List<Alias<int>>();
        List<Alias<byte>> commonModelIDAlias = new List<Alias<byte>>();
        public Dictionary<int, List<Alias<int>>> behaviourAliasLists = new Dictionary<int, List<Alias<int>>>();
        public Dictionary<int, List<Alias<byte>>> modelIDAliasLists = new Dictionary<int, List<Alias<byte>>>();
        Dictionary<string, int> levelNameToROM = new Dictionary<string, int>();
        Dictionary<int, string> ROMtoLevelName = new Dictionary<int, string>();

        public ProjectSettings(string ROM_fileName)
        {
            string rootPath = Path.GetDirectoryName(ROM_fileName);
            string ROM_Name = Path.GetFileNameWithoutExtension(ROM_fileName);

            string banksFile = GetSettingsFile(rootPath + "\\" + ROM_Name + ".banks.txt", "banks.txt", "RAM Banks");
            string levelNamesFile = GetSettingsFile(rootPath + "\\" + ROM_Name + ".levels.txt", "Alias_levels.txt", "Level Alias");
            string behaviourAliasFile = GetSettingsFile(rootPath + "\\" + ROM_Name + ".behaviourScripts.txt", "Alias_behaviourScripts.txt", "Behaviour Script Alias");
            string modelIDAliasFile = GetSettingsFile(rootPath + "\\" + ROM_Name + ".modelIDs.txt", "Alias_modelIDs.txt", "Model ID Alias");

            LoadBanks(banksFile);
            LoadLevelNames(levelNamesFile);
            LoadAliasLists(behaviourAliasFile, AddBehaviourAlias);
            LoadAliasLists(modelIDAliasFile, AddModelIDAlias);
        }

        string GetSettingsFile(string localPath, string defaultPath, string fileIdentifier)
        {
            if (!File.Exists(localPath))
            {
                if (MessageBox.Show("No " + fileIdentifier + " file has been found. Copy Default file?", fileIdentifier, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    File.Copy(defaultPath, localPath);
                else
                    return defaultPath;
            }
            return localPath;
        }

        #region Getter Methods
        public string GetLevelName(int ROMAddress)
        {
            string output;
            if (ROMtoLevelName.TryGetValue(ROMAddress, out output))
                return output;
            return "<Unknown>";
        }
        public int GetROMAddress(string levelName)
        {
            int output;
            if (levelNameToROM.TryGetValue(levelName, out output))
                return output;
            return -1;
        }

        public List<Alias<int>> GetBehaviourAliasList(int levelAddress)
        {
            List<Alias<int>> output;
            if (behaviourAliasLists.TryGetValue(levelAddress, out output))
                return output;
            return commonBehaviourAlias;
        }
        public List<Alias<byte>> GetModelIDAliasList(int levelAddress)
        {
            List<Alias<byte>> output;
            if (modelIDAliasLists.TryGetValue(levelAddress, out output))
                return output;
            return commonModelIDAlias;
        }
        public BankDescription UpdateBank(BankDescription bankDescription)
        {
            foreach (BankDescription bank in banks)
                if (bank.ROM_Start == bankDescription.ROM_Start)
                {
                    bank.ROM_End = bankDescription.ROM_End;
                    bank.ID = bankDescription.ID;
                    bank.arg = bankDescription.arg;
                    bank.compressed = bankDescription.compressed;
                    return bank;
                }
            bankDescription.name = "<Unknown>";
            banks.Add(bankDescription);
            return bankDescription;
        }
        #endregion

        void LoadBanks(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            try
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    line = line.Trim();
                    BankDescription newBank = BankDescription.Parse(line);
                    if (newBank != null) banks.Add(newBank);
                }
            }
            catch (Exception ex)
            {
                EmulationState.messages.AppendMessage("Failed to level name file!\nException was:\n" + ex.ToString(), "Error");
            }
            finally
            {
                reader.Close();
            }
        }

        void LoadLevelNames(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            try
            {
                List<Alias<int>> currentTargetList;
                List<Alias<int>> commonBehaviourAliasList = new List<Alias<int>>();
                currentTargetList = commonBehaviourAliasList;

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    line = line.Trim();
                    if (line == "") continue;
                    string[] split = line.Split(new string[] { ":=" }, StringSplitOptions.None);
                    if (split.Length != 2) continue;
                    int address;
                    if (!int.TryParse(split[1].Trim().Remove(0, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out address))
                        continue;
                    string name = split[0].Trim();
                    levelNameToROM[name] = address;
                    ROMtoLevelName[address] = name;
                }
            }
            catch (Exception ex)
            {
                EmulationState.messages.AppendMessage("Failed to level name file!\nException was:\n" + ex.ToString(), "Error");
            }
            finally
            {
                reader.Close();
            }
        }

        delegate void AddAliasFunc(int levelAddress, int value, string alias);
        void LoadAliasLists(string fileName, AddAliasFunc addAlias)
        {
            StreamReader reader = new StreamReader(fileName);
            try
            {
                int currentLevelAddress = 0;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    line = line.Trim();
                    if (line == "") continue;
                    if (line == "Common:")
                    {
                        currentLevelAddress = 0;
                        continue;
                    }
                    if (line.StartsWith("Level ") && line.EndsWith(":"))
                    {
                        string levelString = line.Substring("Level ".Length, line.Length - "Level ".Length - 1).Trim();
                        int address;
                        if (!levelString.StartsWith("0x") || !int.TryParse(levelString, out address))
                            if ((address = GetROMAddress(levelString)) == -1)
                            {
                                EmulationState.messages.AppendMessage("Invalid level identifier in behaviour script file.", "Error");
                                currentLevelAddress = -1;
                                continue;
                            }
                        currentLevelAddress = address;
                        continue;
                    }
                    string[] split = line.Split(new string[] { ":=" }, StringSplitOptions.None);
                    if (split.Length == 2)
                    {
                        int address;
                        if (int.TryParse(split[1].Trim().Remove(0, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out address))
                            addAlias(currentLevelAddress, address, split[0].Trim());
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                EmulationState.messages.AppendMessage("Failed to load behaviour script alias file!\nException was:\n" + ex.ToString(), "Error");
            }
            finally
            {
                reader.Close();
            }
        }

        void AddBehaviourAlias(int levelAddress, int value, string alias)
        {
            if (levelAddress == -1) return;
            Alias<int>.ToStringFunc toString = (int v) => "0x" + v.ToString("X8");
            Alias<int> newAlias = new Alias<int>(alias, value, toString);
            if (levelAddress == 0)
            {
                commonBehaviourAlias.Add(newAlias);
                foreach (KeyValuePair<int, List<Alias<int>>> levelList in behaviourAliasLists)
                    levelList.Value.Add(newAlias);
            }
            else
            {
                List<Alias<int>> targetList;
                if (!behaviourAliasLists.TryGetValue(levelAddress, out targetList))
                    targetList = behaviourAliasLists[levelAddress] = new List<Alias<int>>(commonBehaviourAlias);
                targetList.Add(newAlias);
            }
        }

        void AddModelIDAlias(int levelAddress, int value, string alias)
        {
            if (levelAddress == -1) return;
            Alias<byte>.ToStringFunc toString = (byte v) => "0x" + v.ToString("X8");
            Alias<byte> newAlias = new Alias<byte>(alias, (byte)value, toString);
            if (levelAddress == 0)
            {
                commonModelIDAlias.Add(newAlias);
                foreach (KeyValuePair<int, List<Alias<byte>>> levelList in modelIDAliasLists)
                    levelList.Value.Add(newAlias);
            }
            else
            {
                List<Alias<byte>> targetList;
                if (!modelIDAliasLists.TryGetValue(levelAddress, out targetList))
                    targetList = modelIDAliasLists[levelAddress] = new List<Alias<byte>>(commonModelIDAlias);
                targetList.Add(newAlias);
            }
        }
    }
}
