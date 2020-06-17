using CodeShell.Globalization;
using CodeShell.Web.Razor.Angular;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeShell.Web.Razor.Angular.Validators
{
    public abstract class Validator
    {
        public string ModelType { get; set; }
        public string PropertyName { get; set; }
        public abstract string Attribute { get; }
        public abstract string ValidationMessage { get; }
        public string FormName { get; set; }
        public string FormFieldName { get; set; }
        public string PropertyTitle { get; set; }

        protected string MakeMessage(string index, string message)
        {
            string prop = FormFieldName != null ? FormFieldName : PropertyName.Replace(".", "_");

            return string.Format(NgOptions.ValidationMessageContainer, FormName, prop, index, message);
        }

        protected string GetLabel(string entity, string col)
        {
            col = col.Replace(".", "_");
            if (col.Contains("_"))
                return Strings.Column(col);
            return Strings.Column(entity + "_" + col);
        }

    }
}
