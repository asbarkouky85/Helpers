using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Razor.Angular.Validators
{
    public class RestrictNumericValidator : Validator
    {
        public override string Attribute { get { return "is-number"; } }

        public override string ValidationMessage { get { return ""; } }
    }
}
