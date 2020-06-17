using CodeShell.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Razor.Angular.Validators
{
    public class RestrictArabicValidator : Validator
    {

        protected string Message;
        public override string Attribute { get { return "is-arabic"; } }

        public RestrictArabicValidator()
        {

        }

        public override string ValidationMessage
        {
            get
            {
                return "";
            }
        }
    }
}
