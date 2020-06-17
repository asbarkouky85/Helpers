using System;
using System.Security.Cryptography;
using System.Text;

namespace CodeShell.Text
{
    public static class StringExtensions
    {
        static MD5 _md5;
        static MD5 Md5Hash
        {
            get
            {
                if (_md5 == null)
                    _md5 = System.Security.Cryptography.MD5.Create();
                return _md5;
            }
        }

        public static string ToMD5(this string input)
        {

            byte[] data = Md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            
            return sBuilder.ToString();

        }

        



        /// <summary>
        /// Substracts a string to get content before a certain charachter
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetBefore(this string subject, string str)
        {
            int ind = subject.IndexOf(str);
            if (ind != 0)
                return subject.Substring(0, ind);
            else
                return subject;
        }

        /// <summary>
        /// Substracts a string to get content after a certain charachter
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetAfter(this string subject, string str)
        {
            int ind = subject.LastIndexOf(str);
            if (ind != 0)
                return subject.Substring(ind + 1);
            else
                return subject;
        }

        public static int ConvertToInt(this String str)
        {
            int value = 0;
            int.TryParse(str, out value);
            return value;
        }

        public static double ConvertToDouble(this String str)
        {
            double value = 0;
            double.TryParse(str, out value);
            return value;
        }

        public static long ConvertToLong(this String str)
        {
            long value = 0;
            long.TryParse(str, out value);
            return value;
        }
    }
}
