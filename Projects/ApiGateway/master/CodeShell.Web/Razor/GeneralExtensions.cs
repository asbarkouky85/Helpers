using CodeShell.Globalization;
using CodeShell.Web.Razor.Angular.Validators;
using CodeShell.Web.Razor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;

namespace CodeShell.Web.Razor
{
    public static class GeneralExtensions
    {
        public static ViewDataDictionary GetDictionary(this HtmlHelper helper)
        {

            ViewDataDictionary dic = new ViewDataDictionary();

            foreach (KeyValuePair<string, object> kv in helper.ViewData)
                dic[kv.Key] = kv.Value;

            return dic;
        }

        public static HtmlElementBuilder<T> GetBuilder<T>(this HtmlHelper<T> helper)
        {
            if (!helper.ViewData.ContainsKey("Builder"))
                helper.ViewData["Builder"] = new HtmlElementBuilder<T>();

            return (HtmlElementBuilder<T>)helper.ViewData["Builder"];
        }

        public static void SetViewData(this HtmlHelper helper, string key, object value)
        {
            helper.ViewData[key] = value;
        }

        public static void SetNgModel(this HtmlHelper helper, string name)
        {
            helper.ViewData["NgModelName"] = name;
        }

        public static void SetNgForm(this HtmlHelper helper, string name)
        {
            helper.ViewData["FormName"] = name;
        }

        public static ValidationCollection<T> VCollection<T>(this HtmlHelper<T> helper)
        {
            return new ValidationCollection<T>(helper);
        }

        public static string Column(this HtmlHelper helper,string id)
        {
            return Strings.Column(id);
        }
        public static string Column<T,TValue>(this HtmlHelper<T> helper, Expression<Func<T,TValue>> exp)
        {
            string index = typeof(T).Name + "_" + ((MemberExpression)exp.Body).Member.Name;
            return Strings.Column(index);
        }
        public static string Message(this HtmlHelper helper, string id,params string[] paramateres)
        {
            return Strings.Message(id,paramateres);
        }

        public static MvcHtmlString HiddenMessage(this HtmlHelper helper, string id, params string[] parameters)
        {
            string mes = Strings.Message(id, parameters);
            string str = "<div style = \"display: none\" id = \"msg__"+id+"\" >"+mes+"</div>";

            return new MvcHtmlString(str);
        }
        public static MvcHtmlString HiddenWord(this HtmlHelper helper, string id)
        {
            string mes = Strings.Word(id);
            string str = "<div style = \"display: none\" id = \"wrd__" + id + "\" >" + mes + "</div>";

            return new MvcHtmlString(str);
        }
        public static string Word(this HtmlHelper helper, string id)
        {
            return Strings.Word(id);
        }
        public static string Page(this HtmlHelper helper, string id)
        {
            return Strings.Page(id);
        }
    }
}
