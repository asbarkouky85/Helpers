using CodeShell.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Razor.Angular.Validators
{
    public class NumericTextValidator : Validator
    {
        public override string Attribute { get { return "ng-pattern='/[+-]?([0-9]*[.])?[0-9]+/'"; } }

        public override string ValidationMessage { get { return MakeMessage("pattern", Strings.Message(MessageIds.must_be_numeric, GetLabel(ModelType, PropertyName))); } }
    }
}
