using CodeShell.Data.Helpers;
using CodeShell.Globalization;
using CodeShell.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CodeShell.Web
{
    public static class Extenstions
    {
        public static HttpResponseMessage ToWebResponse(this SubmitResult data)
        {
            HttpResponseMessage mes = new HttpResponseMessage();
            if (data.Code == 0)
                mes.StatusCode = HttpStatusCode.OK;
            else
                mes.StatusCode = HttpStatusCode.ExpectationFailed;

            mes.Content = new ObjectContent<SubmitResult>(data, new JsonMediaTypeFormatter());
            return mes;
        }

        public static HttpResponseMessage ToWebResponse(this LoginResult data)
        {
            HttpResponseMessage mes = new HttpResponseMessage();
            if (data.Success)
                mes.StatusCode = HttpStatusCode.OK;
            else
                mes.StatusCode = HttpStatusCode.ExpectationFailed;

            mes.Content = new ObjectContent<LoginResult>(data, new JsonMediaTypeFormatter());
            return mes;
        }

        public static string GetHostName(this HttpRequest req)
        {
            return req.ServerVariables["SERVER_NAME"];
        }

        public static string GetHostPort(this HttpRequest req)
        {
            return req.ServerVariables["SERVER_PORT"];
        }

        public static void LoadCultureFromHeader(this Language obj)
        {
            if (HttpContext.Current.Request.Headers.AllKeys.Contains("Locale"))
            {
                string loc = HttpContext.Current.Request.Headers["Locale"];

                if (loc.Length == 2)
                    Language.SetCulture(loc);
            }
        }
    }
}
