using CodeShell.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Razor.Angular.Validators
{
    public class MinMaxValidator : Validator
    {
        protected float? Min;
        protected float Max;
        protected string Message;
        public override string Attribute
        {
            get
            {
                string str = "";
                if (Min.HasValue)
                    str += "min='" + Min + "'";

                if (Max > 0)
                    str += "max='" + Max + "'";

                return str;
            }
        }

        public MinMaxValidator(float? min, float max = 0)
        {
            Min = min;
            Max = max;
        }

        public override string ValidationMessage
        {
            get
            {
                List<string> lst = new List<string>();
                string lab = GetLabel(ModelType, PropertyName);

                if (Min > 0)
                {
                    lst.Add(MakeMessage("min", Strings.Message(MessageIds.invalid_min, lab, Min.ToString())));
                }

                if (Max > 0)
                {
                    lst.Add(MakeMessage("max", Strings.Message(MessageIds.invalid_max, lab, Max.ToString())));
                }



                return string.Join("", lst);
            }
        }
    }
}
