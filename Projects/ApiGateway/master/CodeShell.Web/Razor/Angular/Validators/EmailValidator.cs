using CodeShell.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Razor.Angular.Validators
{
    public class EmailValidator : Validator
    {
        public override string Attribute { get { return @"ng-pattern=""/[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/"""; } }

        public override string ValidationMessage
        {
            get
            {
                return MakeMessage("pattern",Strings.Message(MessageIds.invalid_email));
            }
        }
    }
}
