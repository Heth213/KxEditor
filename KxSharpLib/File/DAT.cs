namespace KxSharpLib.File
{
    public class DAT
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string Content { get; set; }


        public DAT(int _index, string _name, byte[] _data) => 
            (Index, Name, Data, Content) = (_index, _name, _data, Decrypt(_data));


        public byte[] Encrypt() {
            return Security.Kal.Crypto.EncryptDat(Content);
        }

        public byte[] Encrypt(string _content) {
            return Security.Kal.Crypto.EncryptDat(_content);
        }

        public string Decrypt() {
            return Security.Kal.Crypto.DecryptDat(Data);
        }

        public string Decrypt(byte[] _data) {
            return Security.Kal.Crypto.DecryptDat(_data);
        }
    }
}
