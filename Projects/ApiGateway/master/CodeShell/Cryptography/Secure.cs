using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace CodeShell.Cryptography
{

    public class Encryptor
    {
        private string KeyString { get; set; }
        private string IVString { get; set; }
        public Encryptor(string key, string iv= "IVzhJbYM5uo=")
        {
            KeyString = key;
            IVString = iv;
        }

        TripleDES _Cryptor;

        TripleDES Cryptor
        {
            get
            {
                if (_Cryptor == null)
                {
                    _Cryptor = TripleDES.Create();
                    _Cryptor.Key = Convert.FromBase64String(KeyString);
                    _Cryptor.IV = Convert.FromBase64String(IVString);
                }
                return _Cryptor;
            }
            set
            {
                _Cryptor = value;
            }
        }

        public byte[] DecryptToBytes(byte[] mStr)
        {
            string dat = Decrypt(mStr);
            byte[] str = Encoding.ASCII.GetBytes(dat);
            return str;
        }

        public string Decrypt(byte[] byts)
        {
            ICryptoTransform trns = Cryptor.CreateDecryptor();

            try
            {
                MemoryStream mStr = new MemoryStream(byts);
                CryptoStream str = new CryptoStream(mStr, trns, CryptoStreamMode.Read);

                StreamReader sRead = new StreamReader(str);

                string read = sRead.ReadToEnd();
                mStr.Close();
                str.Close();

                return read;
            }
            catch
            {
                return null;
            }
        }

        public byte[] DecryptToBytes(string mStr)
        {
            string dec = Decrypt(mStr);
            return Encoding.ASCII.GetBytes(dec);
        }

        public string Decrypt(string st)
        {

            ICryptoTransform trns = Cryptor.CreateDecryptor();

            try
            {
                byte[] bytes = Convert.FromBase64String(st);
                MemoryStream mStr = new MemoryStream(bytes);
                CryptoStream str = new CryptoStream(mStr, trns, CryptoStreamMode.Read);

                StreamReader sRead = new StreamReader(str);

                string read = sRead.ReadToEnd();
                mStr.Close();
                str.Close();

                return read;
            }
            catch
            {
                return null;
            }
        }

        public string Encrypt(byte[] st)
        {
            return Convert.ToBase64String(EncryptToBytes(st));
        }

        public byte[] EncryptToBytes(byte[] inArray)
        {
            ICryptoTransform trns = Cryptor.CreateEncryptor();

            MemoryStream outStream = new MemoryStream();
            CryptoStream encStream = new CryptoStream(outStream, trns, CryptoStreamMode.Write);

            encStream.Write(inArray, 0, inArray.Length);
            encStream.FlushFinalBlock();
            encStream.Close();

            return outStream.ToArray();
        }

        public string Encrypt(string st)
        {
            return Convert.ToBase64String(EncryptToBytes(st));
        }

        public byte[] EncryptToBytes(string st)
        {
            ICryptoTransform trns = Cryptor.CreateEncryptor();

            MemoryStream mStr = new MemoryStream();
            CryptoStream str = new CryptoStream(mStr, trns, CryptoStreamMode.Write);

            byte[] cString = Encoding.ASCII.GetBytes(st);
            str.Write(cString, 0, cString.Length);

            str.FlushFinalBlock();
            str.Close();

            return mStr.ToArray();
        }
    }
}
