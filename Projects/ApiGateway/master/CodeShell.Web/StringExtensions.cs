using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CodeShell.Web
{
    public static class StringExtensions
    {
        static JavaScriptSerializer _Serializer;
        static JavaScriptSerializer Serializer
        {
            get
            {
                if (_Serializer == null) _Serializer = new JavaScriptSerializer();
                return _Serializer;
            }
        }


        /// <summary>
        /// Serializes object to json string
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public static string ToJson(this object ob)
        {
            return JsonConvert.SerializeObject(ob);
        }

        /// <summary>
        /// Deserializes object to T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="st"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string st)
        {
            return JsonConvert.DeserializeObject<T>(st);
        }
    }
}
