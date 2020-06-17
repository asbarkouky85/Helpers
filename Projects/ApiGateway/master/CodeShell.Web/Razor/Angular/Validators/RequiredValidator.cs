using CodeShell.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeShell.Web.Razor.Angular.Validators
{
    public class RequiredValidator : Validator
    {
        string RequiredCondition;
        public RequiredValidator(string requiredIf = null)
        {
            RequiredCondition = requiredIf;
        }
        public override string ValidationMessage
        {
            get
            {
                string label = PropertyTitle == null ? GetLabel(ModelType, PropertyName) : PropertyTitle;
                return MakeMessage("required", Strings.Message(MessageIds.field_required, label));
            }
        }

        public override string Attribute { get { return RequiredCondition == null ? "required" : "ng-required=\"" + RequiredCondition + "\""; } }
    }
}
