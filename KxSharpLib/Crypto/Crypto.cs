using System;
using System.Text;
using System.IO;
using System.Globalization;

namespace KxSharpLib.Security.Kal
{
    public static class Crypto
    {
        public enum CodePage 
        {
            Korean = 949,
            WestEU = 1252,
        }

        public enum EUseCrypt
        {
            Unknown,
            c2006,
            c2015,
            c2018,
        };

        public enum ECryptTableType
        {
            Unknown,
            e2006,
            d2006,
            e2015,
            d2015,
            e2018,
            d2018,
        };

        [Flags]
        public enum EKeys : int
        {
            Unknown,
            CFG = 47,
            E = 4,
            CFGADD = 11,
            CFGNEW = CFG + CFGADD,
            MAX = 63,
        };

        public static int GKey
        {
            get;
            set;
        } = Convert.ToInt32(EKeys.CFG, CultureInfo.CurrentCulture);

        public static EUseCrypt GUseCrypt
        {
            get;
            set;
        } = EUseCrypt.Unknown;

        public static byte[] GetTable(ECryptTableType type)
        {
            switch (type)
            {
                case ECryptTableType.e2006:
                    return EncryptTables.e2006;
                case ECryptTableType.d2006:
                    return DecryptTables.d2006;
                case ECryptTableType.e2015:
                    return EncryptTables.e2015;
                case ECryptTableType.d2015:
                    return DecryptTables.d2015;
                case ECryptTableType.e2018:
                    return EncryptTables.e2018;
                case ECryptTableType.d2018:
                    return DecryptTables.d2018;
            }
            return null;
        }

        public static Encoding GetEncoding(string filename) 
        {
            using (var sr = new StreamReader(filename, Encoding.Default, true)) 
            {
                if (sr.Peek() >= 0)
                    sr.Read();
                return sr.CurrentEncoding;
            }
        }


        public static byte Encrypt(int key, byte input)
        {
            switch (GUseCrypt)
            {
                case EUseCrypt.c2006:
                    return Crypt(GetTable(ECryptTableType.e2006), key, input);
                case EUseCrypt.c2015:
                    return Crypt(GetTable(ECryptTableType.e2015), key, input);
                case EUseCrypt.c2018:
                    return Crypt(GetTable(ECryptTableType.e2018), key, input);
                default:
                    return Crypt(GetTable(ECryptTableType.e2006), key, input);
            }
        }
        public static byte Encrypt(byte input) 
        {
            return Encrypt(GKey, input);
        }


        public static byte Decrypt(int key, byte input)
        {
            switch (GUseCrypt)
            {
                case EUseCrypt.c2006:
                    return Crypt(GetTable(ECryptTableType.d2006), key, input);
                case EUseCrypt.c2015:
                    return Crypt(GetTable(ECryptTableType.d2015), key, input);
                case EUseCrypt.c2018:
                    return Crypt(GetTable(ECryptTableType.d2018), key, input);
                default:
                    return Crypt(GetTable(ECryptTableType.d2006), key, input);
            }
        }

        public static byte Decrypt(byte input)
        {
            return Decrypt(GKey, input);
        }


        public static byte[] EncryptDat(string input)
        {
            Encoding EUC = Encoding.GetEncoding((int)CodePage.Korean);
            Encoding UTF = Encoding.UTF8;

            byte[] buffer = UTF.GetBytes(input);
            buffer = Encoding.Convert(UTF, EUC, buffer);

            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = Convert.ToByte(Encrypt(buffer[i]));

            return buffer;
        }

        public static string DecryptDat(byte[] input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            byte[] _input = input;

            for (int i = 0; i < _input.Length; i++)
                _input[i] = Decrypt(input[i]);

            Encoding EUC = Encoding.GetEncoding((int)CodePage.Korean);
            Encoding UTF = Encoding.UTF8;
            byte[] utfcontent = Encoding.Convert(EUC, UTF, input);
            return UTF.GetString(utfcontent);
        }


        public static byte Crypt(byte[] table, int key, byte input)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            key &= (int)EKeys.MAX;
            return table[((key <<= 8) + input)];
        }
    }
}