using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeShell.Web.Razor.Angular
{
    public class NgControlGroup
    {
        public string GroupCssClass { get; set; }
        public bool IsRequired { get; set; }
        public string Label { get; set; }
        public string PlaceHolder { get; set; }
        public string Attributes { get; set; }
        public MvcHtmlString InputControl { get; set; }
        public MvcHtmlString ValidationMessages { get; set; }
        public int Size { get; set; }

        public NgControlGroup()
        {
            Size = 6;
        }


    }
}
