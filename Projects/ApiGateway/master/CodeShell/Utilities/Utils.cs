using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeShell.Utilities
{
    public static class Utils
    {
        static Random r = new Random();
        public static void CreatePropertyDictionary<T>(string folderPath)
        {
            List<string> st = new List<string>();
            foreach (PropertyInfo inf in typeof(T).GetProperties().OrderBy(d => d.Name))
            {
                st.Add(typeof(T).Name + "_" + inf.Name + "\t");
            }
            File.WriteAllLines(Path.Combine(folderPath, typeof(T).Name + ".csv"), st);
        }

        public static string RandomNumber(int digits)
        {
            string st = "";

            for (int i = 0; i < digits; i++)
            {
                st += r.Next(0, 9);
            }
            return st;
        }
        public static string RandomAlphabet(int numOfChars, int CapitalAndSmall = 0) //CapitalAndSmall( 0 = capital and small ,1=capital,2 =small)
        {
            string allalpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //26
            bool Case = CapitalAndSmall == 2 ? false : true;
            string st = "";

            for (int i = 0; i < numOfChars; i++)
            {
               
                if (Case && (CapitalAndSmall<2))//Capital
                {
                    st += allalpha[r.Next(0, 25)].ToString().ToUpper();
                }
                else  //Small
                {
                    st += allalpha[r.Next(0, 25)].ToString().ToLower();
                }
                if(CapitalAndSmall==0)
                    Case = !Case;
            }
            return st;

        }
        public static string CombineUrl(params string[] parts)
        {
            string ret = "";
            Regex reg = new Regex("^/");
            
            for (var i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (i != 0)
                    part = reg.Replace(part,"");
                if (i < (parts.Length - 1))
                {
                    char last = part[part.Length - 1];
                    part += (last != '/') ? "/" : "";
                }
                ret += part;
            }
            return ret;
        }
    }
}
