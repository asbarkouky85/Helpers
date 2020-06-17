using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeShell.Web.Razor.Tables
{
    public class CellParameters
    {
        public string ModelName;
        public string MemberName;
        public string TextBoxType;
        public MvcHtmlString InputControl;
        public bool IsRequired;
        public string Attributes;
    }
}
