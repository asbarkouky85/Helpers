using CodeShell.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Razor.Validators
{
    public class IsEnglishValidator : Validator
    {
  
        protected string Message;
        public override string Attribute { get { return "onkeypress='EnglishOnly(event)'"; } }// onkeydown='dontExceed("+ MAX + ",this)' onkeyup='dontExceed("+ MAX + ",this)'

        public IsEnglishValidator()
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
