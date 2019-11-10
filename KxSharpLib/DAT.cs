namespace KxSharpLib.Kal
{
    public class DAT
    {
        public int index;
        public string name;
        public string content;
        public override string ToString() { return name; }

        public DAT(int _index, string _name, byte[] _input)
        {
            index = _index;
            name = _name;
            content = Security.Kal.Crypto.DecryptDat(_input);
        }
    }
}
