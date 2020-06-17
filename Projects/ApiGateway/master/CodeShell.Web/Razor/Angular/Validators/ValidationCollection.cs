using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeShell.Web.Razor.Angular.Validators
{
    public class ValidationCollection<T>
    {
        private HtmlHelper<T> Helper;
        public List<Validator> Validators { get; private set; }
        string MemberName;

        private string formName;
        string formFieldName;

        public ValidationCollection(HtmlHelper<T> helper)
        {
            Helper = helper;
            if (helper.ViewData.ContainsKey("FormName"))
                formName = helper.ViewData["FormName"].ToString();
            else
                formName = "Form";
            Validators = new List<Validator>();
        }

        public ValidationCollection<T> AddWebsite()
        {
            Validators.Add(new PatternValidator(@"^([a-zA-Z]{3,9}:(\/){2})?([a-zA-Z0-9\.]*){1}(?::[a-zA-Z0-9]{1,4})?(?:\/[_\,\$#~\?\-\+=&;%@\.\w%]+)*(?:\/)?$"));
            return this;
        }

        public ValidationCollection<T> AddRequired(string reqCondition=null)
        {

            Validators.Add(new RequiredValidator(reqCondition));
            return this;
        }

        public ValidationCollection<T> AddAlphaNumeric()
        {
            Validators.Add(new RestrictAlphaNumeric());
            return this;
        }

        public ValidationCollection<T> AddEmail()
        {
            Validators.Add(new EmailValidator());
            return this;
        }
        public ValidationCollection<T> AddLength(int maxLength, int minLenth = 0)
        {
            Validators.Add(new LengthValidator(maxLength, minLenth));
            return this;
        }
        public ValidationCollection<T> MaxValue(int max)
        {
            Validators.Add(new MaxValueValidator(max));
            return this;
        }
        public ValidationCollection<T> MaxNonEditable(int max)
        {
            Validators.Add(new RestrictMaxValidator(max));
            return this;
        }
        public ValidationCollection<T> MinValue(int min)
        {
            Validators.Add(new MinMaxValidator(min));
            return this;
        }

        public ValidationCollection<T> AddMinMax(float? min, float max = 0)
        {
            Validators.Add(new MinMaxValidator(min, max));
            return this;
        }

        public ValidationCollection<T> IsEnglishOnly()
        {
            Validators.Add(new RestrictEnglishValidator());
            return this;
        }
        public ValidationCollection<T> IsArabicOnly()
        {
            Validators.Add(new RestrictArabicValidator());
            return this;
        }
        public ValidationCollection<T> AddPattern(string pattern, string message = null)
        {
            Validators.Add(new PatternValidator(pattern, message));
            return this;
        }

        public ValidationCollection<T> AddNumeric()
        {
            Validators.Add(new NumericTextValidator());
            
            return this;
        }

        public ValidationCollection<T> RestrictNumber()
        {
            Validators.Add(new RestrictNumericValidator());
            return this;
        }

        public ValidationCollection<T> AddCustom(string validationType, string message)
        {
            Validators.Add(new CustomValidator(validationType, message));
            return this;
        }

        public void Add(Validator v)
        {
            Validators.Add(v);
        }


        internal MvcHtmlString GetMessages()
        {
            string fName = formFieldName != null ? formFieldName : MemberName.Replace(".", "_");
            string messages = "<span ng-show='" + formName + "." + fName + ".$invalid &&" + formName + "." + fName + ".$touched'>\n";
            foreach (Validator v in Validators)
            {
                messages += v.ValidationMessage + "\n";
            }
            messages += "</span>\n";
            return new MvcHtmlString(messages);
        }

        internal void SetMember(string memberName, string modelType, string fieldName = null)
        {
            MemberName = memberName;
            formFieldName = fieldName;
            foreach (Validator v in Validators)
            {
                v.PropertyName = memberName;
                v.ModelType = modelType;
                v.FormName = formName;
                v.FormFieldName = fieldName;
            }
        }

        internal void SetPropertyTitle(string name)
        {
            foreach (Validator v in Validators)
            {
                v.PropertyTitle = name;
            }
        }

        internal string GetAttributes()
        {
            string attributes = "";
            foreach (Validator v in Validators)
            {
                attributes += v.Attribute + " ";
            }
            return attributes;
        }

        internal void SetNumericDefaults()
        {

            if (Validators.Any(d => d is NumericTextValidator || d is RestrictNumericValidator))
            {
                if (!Validators.Any(d => d is RestrictNumericValidator))
                    Validators.Add(new RestrictNumericValidator());
                if (!Validators.Any(d => d is LengthValidator))
                    AddLength(15);
            }
        }

        internal void UseAnnotations(IEnumerable<CustomAttributeData> customAttributes)
        {
            if (!Validators.Any(d => d is LengthValidator))
            {
                CustomAttributeData attr = customAttributes.FirstOrDefault(d => d.AttributeType == typeof(StringLengthAttribute));
                if (attr != null)
                {
                    int len= (int)attr.ConstructorArguments.Select(d => d.Value).FirstOrDefault();
                    Validators.Add(new LengthValidator(len));
                }
            }
        }
    }
}
