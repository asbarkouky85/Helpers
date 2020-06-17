using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Razor.Angular
{
    public class NgInput
    {
        public string NgModelName { get; set; }
        public string NgOptions { get; set; }
        public string MemberName { get; set; }
        public string PlaceHolder { get; set; }
        public string TextBoxType { get; set; }
        public string Classes { get; set; }
        public string CalendarOptions { get; set; }
        public string Attributes { get; set; }
        public object AttributeObject { get; set; }
    }
}
