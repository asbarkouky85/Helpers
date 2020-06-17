using CodeShell.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Razor.Angular.Validators
{
    public class MaxValueValidator : Validator
    {
        protected int MAX;
        protected string Message;
        public override string Attribute { get { return "max='" + MAX + "'"; } }// onkeydown='dontExceedThis("+ MAX + ",this,event)' onkeyup='dontExceedThis("+ MAX + ",this,event)'

        public MaxValueValidator(int max)
        {
            MAX = max;
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
