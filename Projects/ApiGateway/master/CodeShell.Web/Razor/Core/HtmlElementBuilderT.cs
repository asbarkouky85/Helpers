using CodeShell.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeShell.Web.Razor.Core
{
    public class HtmlElementBuilder<T> 
    {
        public MvcHtmlString HeaderRow(params string[] heads)
        {
            string st = "<tr>";
            Type classType = typeof(T);

            foreach (string col in heads)
            {

                st += "<th><label>" + Strings.Column(classType.Name + "_" + col) + "</label></th>";
            }
            st += "</tr>";
            return new MvcHtmlString(st);
        }
    }
}
