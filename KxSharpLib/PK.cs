using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KxSharpLib.Kal
{
    public class PK
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Path { get; set; }
        public byte Key { get; set; }

        public PK() => new PK("", "", "", 0);
        public PK(string name, string path, string pw, byte key)
        {
            Name = name;
            Password = pw;
            Path = path;
            Key = key;
        }
    }
}
