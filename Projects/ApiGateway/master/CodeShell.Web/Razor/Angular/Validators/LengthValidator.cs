using CodeShell.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Razor.Angular.Validators
{

    public class LengthValidator : Validator
    {
        protected int maxLength = 0;
        protected int minLength = 0;
        public override string Attribute
        {
            get
            {
                string data = "", sep = "";

                if (maxLength > 0)
                {
                    data += "maxlength=\"" + maxLength + "\"";
                    sep = " ";
                }


                if (minLength > 0)
                    data += sep + "minlength=\"" + minLength + "\"";
                return data;

            }
        }

        public LengthValidator(int _maxLength, int _minLength = 0)
        {
            maxLength = _maxLength;
            minLength = _minLength;
        }

        public override string ValidationMessage
        {
            get
            {
                string messages = "", sep = "";
                if (maxLength > 0)
                {
                    messages = MakeMessage("maxlength", Strings.Message(MessageIds.invalid_max_length, maxLength.ToString()));
                    sep = "\n";
                }
                if (minLength > 0)
                {
                    messages += sep + MakeMessage("minlength", Strings.Message(MessageIds.invalid_min_length, minLength.ToString()));
                }
                return messages;
            }
        }
    }
}
