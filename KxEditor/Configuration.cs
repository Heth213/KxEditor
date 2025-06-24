using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JsonConfig;


namespace KxEditor
{
    public class Configuration
    {
        private KxSharpLib.Utility.Logger MainLogger => MainForm.Instance.logger;
        public enum DefaultEncodeKeys : byte
        {
            Config_2006 = 47,
            E_2006 = 4,

            Config_2015 = 47,
            E_2015 = 4,

            Config_2018 = 58,
            E_2018 = 4,

            Config_2025 = 1,
            E_2025 = 70,

            MAX = 63,
        }
        public static string File_Name => "Configuration.json";
        public static string Folder_Path => Path.Combine(Environment.CurrentDirectory, "Config");
        public static string Full_Path => Path.Combine(Folder_Path, File_Name);
        private byte _EncodeKey_ConfigPK;
        private byte _EncodeKey_EPK;
        public byte EncodeKey_ConfigPK {
            get => _EncodeKey_ConfigPK;
            set {
                if (value < (byte)DefaultEncodeKeys.MAX && value >= 0)
                    _EncodeKey_ConfigPK = value;
                else
                    throw Exception_EncodeKey();
            }
        }
        public byte EncodeKey_EPK
        {
            get => _EncodeKey_EPK;
            set
            {
                if (value < (byte)DefaultEncodeKeys.MAX && value >= 0)
                    _EncodeKey_EPK = value;
                else
                    throw Exception_EncodeKey();
            }
        }
        public bool UseConfigurationFile { get; set; }
        public List<string> MessageList { get; set; }

        private Exception Exception_EncodeKey() {
            throw new NotImplementedException();
        }

        public Configuration() => new Configuration(Folder_Path, File_Name);
        public Configuration(string folderPath, string fileName)
        {
            if (!CreateDirectoryIfNotExists(folderPath))
                MainLogger.Write("Configuration Directory not found and has been created!");

            if (!CreateFileIfNotExists(Path.Combine(folderPath, fileName)))
                MainLogger.Write("Configuration File not found, default Configuration.cfg has been created!");

            Read();

        }

        private bool CreateDirectoryIfNotExists(string path) {
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
                return false;
            }
            return true;
        }
        private bool CreateFileIfNotExists(string path) {
            if (File.Exists(path))
                return true;
            else {
                using (var textWriter = new StreamWriter(path, false)) {
                    textWriter.WriteLine("#=======================");
                    textWriter.WriteLine("#Default Config keys:");
                    textWriter.WriteLine("#(2005 - late 2017) = 47");
                    textWriter.WriteLine("#(2018+) = 58");
                    textWriter.WriteLine("#Default e.pk key: 4");
                    textWriter.WriteLine("#=======================");
                    textWriter.WriteLine("");
                    textWriter.WriteLine("");
                    textWriter.WriteLine("{\n\tUseConfigurationFile: \"false\",\n\tEncodingKeys: {");
                    textWriter.WriteLine(string.Format("\t\tConfig_PK: {0},\n\t\tE_PK: {1}", (byte)DefaultEncodeKeys.Config_2006, (byte)DefaultEncodeKeys.E_2006));
                    textWriter.WriteLine("\t}\n}");

                }
            }
            return false;
        }
        public void Read()
        {
            dynamic jsonCfg = Config.ApplyFromDirectory(Folder_Path, null, true);

            MessageList = ((string[])jsonCfg.MessageList).ToList<string>();


            if (jsonCfg.UseConfigurationFile.Equals("true"))
                UseConfigurationFile = true;
            else
                UseConfigurationFile = false;

            if (UseConfigurationFile) {
                MainLogger.Write("Reading Configuration...");
                MainLogger.Write(string.Format("Path:[{0}]", Folder_Path));
                MainLogger.Write(string.Format("FileName:[{0}]", File_Name));
                EncodeKey_ConfigPK = (byte)jsonCfg.EncodingKeys.Config_PK;
                EncodeKey_EPK = (byte)jsonCfg.EncodingKeys.E_PK;
                MainLogger.Write(string.Format("EncodingKeys => [Config.pk: ({0})], [E.pk: ({1})]", EncodeKey_ConfigPK, EncodeKey_EPK));
            }
            else {
                MainLogger.Write("Configuration file disabled!");
            }
        }
    }
}
