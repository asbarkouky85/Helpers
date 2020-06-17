using CodeShell.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Razor.Validators
{
    public class IsArabicValidator : Validator
    {

        protected string Message;
        public override string Attribute { get { return "onkeypress='ArabicOnly(event)'"; } }// onkeydown='dontExceed("+ MAX + ",this)' onkeyup='dontExceed("+ MAX + ",this)'

        public IsArabicValidator()
        {

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
