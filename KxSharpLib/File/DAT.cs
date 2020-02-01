namespace KxSharpLib.File
{
    public class DAT
    {
        private int m_index;
        public int Index
        {
            get { return m_index; }
            set { m_index = value; }
        }

        private string m_name;
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        private string m_content;
        public string Content
        {
            get { return m_content; }
            set { m_content = value; }
        }

        public DAT(int _index, string _name, byte[] _data)
        {
            m_index = _index;
            m_name = _name;
            m_content = Security.Kal.Crypto.DecryptDat(_data);
        }

        public byte[] Encrypt()
        {
            return Security.Kal.Crypto.EncryptDat(m_content);
        }
    }
}
