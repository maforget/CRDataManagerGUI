using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataManagerGUI;

namespace DataManagerGUI
{

    public enum DebugLevel : int
    {
        StartupOnly = 0,
        Minimal = 1,
        Verbose = 2,
    }

    public class dmUserInfo
    {
        Dictionary<string, string> KeyStorage { get; set; }
        DebugLevel DebugLevel { get; set; }

        public bool SessionAutoComplete
        {
            get
            {
                if (KeyStorage.ContainsKey("SessionAutoComplete"))
                    return Boolean.Parse(KeyStorage["SessionAutoComplete"]);
                else
                    return false;
            }
            set
            {
                if (KeyStorage.ContainsKey("SessionAutoComplete"))
                    KeyStorage["SessionAutoComplete"] = value.ToString();
                else
                    KeyStorage.Add("SessionAutoComplete", value.ToString());
            }
        }

        public bool ShowStartupDialog
        {
            get
            {
                if (!KeyStorage.ContainsKey("ShowStartupDialog"))
                    KeyStorage.Add("ShowStartupDialog", true.ToString());
                return Boolean.Parse(KeyStorage["ShowStartupDialog"]);
            }
            set
            {
                if (!KeyStorage.ContainsKey("ShowStartupDialog"))
                    KeyStorage.Add("ShowStartupDialog", value.ToString());
                KeyStorage["ShowStartupDialog"] = value.ToString();
            }
        }

        public string DateTimeFormat
        {
            get
            {
                if (!KeyStorage.ContainsKey("DateTimeFormat"))
                    KeyStorage.Add("DateTimeFormat", "yyyy/MM/dd hh:mm:ss");
                return KeyStorage["DateTimeFormat"];
            }
            set
            {
                if (!KeyStorage.ContainsKey("DateTimeFormat"))
                    KeyStorage.Add("DateTimeFormat", value.ToString());
                KeyStorage["DateTimeFormat"] = value;
            }
        }

        public bool BreakAfterFirstError
        {
            get
            {
                if (!KeyStorage.ContainsKey("BreakAfterFirstError"))
                    KeyStorage.Add("BreakAfterFirstError", true.ToString());
                return Boolean.Parse(KeyStorage["BreakAfterFirstError"]);
            }
            set
            {
                if (!KeyStorage.ContainsKey("BreakAfterFirstError"))
                    KeyStorage.Add("BreakAfterFirstError", value.ToString());
                KeyStorage["BreakAfterFirstError"] = value.ToString();
            }
        }

        public bool WriteDataManagerProcessed
        {
            get
            {
                if (!KeyStorage.ContainsKey("WriteDataManagerProcessed"))
                    KeyStorage.Add("WriteDataManagerProcessed", true.ToString());
                return Boolean.Parse(KeyStorage["WriteDataManagerProcessed"]);
            }
            set
            {
                if (!KeyStorage.ContainsKey("WriteDataManagerProcessed"))
                    KeyStorage.Add("WriteDataManagerProcessed", value.ToString());
                KeyStorage["WriteDataManagerProcessed"] = value.ToString();
            }
        }

        public bool DebugMode
        {
            get
            {
                bool bReturn = false;
                if (KeyStorage.ContainsKey("Debug"))
                    Boolean.TryParse(KeyStorage["Debug"], out bReturn);
                return bReturn;
            }
        }

        public bool LogBookOnlyWhenValuesChanged
        {
            get
            {
                if (KeyStorage.ContainsKey("LogBookOnlyWhenValuesChanged"))
                    return bool.Parse(KeyStorage["LogBookOnlyWhenValuesChanged"]);
                else
                    return false;
            }
            set
            {
                if (KeyStorage.ContainsKey("LogBookOnlyWhenValuesChanged"))
                    KeyStorage["LogBookOnlyWhenValuesChanged"] = value.ToString();
                else
                    KeyStorage.Add("LogBookOnlyWhenValuesChanged", value.ToString());
            }
        }

        public bool SaveLocationInStartupDir
        {
            get
            {
                if (KeyStorage.ContainsKey("SaveLocation"))
                    return bool.Parse(KeyStorage["SaveLocation"]);
                else
                    return false;
            }
            set
            {
                KeyStorage["SaveLocation"] = value.ToString();
            }
        }
        Dictionary<string, List<string>> AutoCompleteStrings { get; set; }

        public dmUserInfo()
        {
            KeyStorage = new Dictionary<string, string>();
            ConfirmOverwrite = true;
            DateTimeFormat = "yyyy/MM/dd hh:mm:dd";
            BreakAfterFirstError = false;
            AutoCompleteStrings = new Dictionary<string, List<string>>();
            DebugLevel = DebugLevel.StartupOnly;
            CaseSensitive = false;
            WriteDataManagerProcessed = false;
            SaveLocationInStartupDir = false;
        }

        public bool ConfirmOverwrite 
        { 
            get
            {
                if (KeyStorage.ContainsKey("ConfirmOverwrite"))
                    return bool.Parse(KeyStorage["ConfirmOverwrite"]);
                else
                    return true;
            }
            set
            {
                if (!KeyStorage.ContainsKey("ConfirmOverwrite"))
                    KeyStorage.Add("ConfirmOverwrite", ConfirmOverwrite.ToString());
                else
                    KeyStorage["ConfirmOverwrite"] = value.ToString();
            }
        }

        public bool CaseSensitive
        {
            get
            {
                if (KeyStorage.ContainsKey("CaseSensitive"))
                    return bool.Parse(KeyStorage["CaseSensitive"]);
                else
                    return false;
            }
            set
            {
                if (!KeyStorage.ContainsKey("CaseSensitive"))
                    KeyStorage.Add("CaseSensitive", CaseSensitive.ToString());
                else
                    KeyStorage["CaseSensitive"] = value.ToString();
            }
        }

        public dmUserInfo(string strFilePath)
            : this()
        {
            ReadFile(strFilePath);
        }

        public dmUserInfo(string strFilePath, string strBackupFilePath)
            : this()
        {
            if (!System.IO.File.Exists(strFilePath))
            {
                Program.DebugAppend("User file does not exist, searching for backup...");
                if (!System.IO.File.Exists(strBackupFilePath))
                {
                    Program.DebugAppend("Backup User file does not exist, creating default...");
                    ReadFile(strFilePath);
                    WriteFile(strFilePath);
                }
                else
                {
                    Program.DebugAppend("Backup found copying to folder...");
                    ReadFile(strBackupFilePath);
                    WriteFile(strBackupFilePath);
                }
            }
            else
            {
                Program.DebugAppend("User file found, loading user settings...");
                ReadFile(strFilePath);
            }
        }


        public void ReadFile(string strFilePath)
        {
            Dictionary<string, string> tmpKeys = ConfigHandler.ReadAllKeys(strFilePath);
            foreach (KeyValuePair<string, string> item in tmpKeys)
            {
                if (item.Key.StartsWith("AutoComplete_"))
                    AutoCompleteStrings.Add(item.Key.Replace("AutoComplete_", ""), new List<string>(item.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)));
                else 
                    AddToKeyStorage(item.Key, item.Value);
            }
        }

        private void AddToKeyStorage(string strKey, string strValue)
        {
            if (!KeyStorage.ContainsKey(strKey))
                KeyStorage.Add(strKey, strValue);
            else
                KeyStorage[strKey] = strValue;
        }

        public void WriteFile(string strFilePath)
        {
            Dictionary<string, string> tmpKeys = new Dictionary<string, string>();

            foreach (KeyValuePair<string, string> item in KeyStorage)
                tmpKeys.Add(item.Key, item.Value);
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(strFilePath)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(strFilePath));

            ConfigHandler.WriteAllKeys(tmpKeys, strFilePath);
        }
    }
}