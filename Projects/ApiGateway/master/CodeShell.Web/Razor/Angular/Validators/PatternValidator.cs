using CodeShell.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Razor.Angular.Validators
{
    public class PatternValidator : Validator
    {
        protected string Pattern;
        protected string Message;
        public override string Attribute { get { return "ng-pattern='/" + Pattern + "/'"; } }

        public PatternValidator(string pattern, string message = null)
        {
            Pattern = pattern;
            Message = message;
        }

        public override string ValidationMessage
        {
            get
            {
                if (Message == null)
                {
                    Message = Strings.Message(MessageIds.invalid_field, GetLabel(ModelType, PropertyName));
                }
                return MakeMessage("pattern", Message);
            }
        }
    }
}
