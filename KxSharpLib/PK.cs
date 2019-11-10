namespace KxSharpLib.Kal
{
    public class PK
    {
        public string Name;
        public string Password;
        public string Path;
        public byte Key;

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
