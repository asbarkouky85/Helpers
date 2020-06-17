using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Collections;
using System.Web.Routing;
using System.Reflection;
using System.Dynamic;
using CodeShell.Types;
using System.IO;
using CodeShell.Web.Razor.Angular.Validators;
using CodeShell.Globalization;
using CodeShell.Text;
using CodeShell.Web.Razor.Core;

namespace CodeShell.Web.Razor.Angular
{

    public class NgElement<T, TValue> : HtmlElement<T, TValue>
    {
        public NgElement(HtmlHelper<T> helper, Expression<Func<T, TValue>> exp) : base(helper, exp) { }

        public MvcHtmlString ControlGroup(ValidationCollection<T> coll, int size, string textType, string alternateLabel = null, string placeHolder = null, object attrs = null, object inputAttr = null, string classes = "", string value = "")
        {
            GroupModel.Label = alternateLabel == null ? GroupModel.Label : alternateLabel;
            GroupModel.Size = size;
            GroupModel.Attributes = ToAttributeString(attrs);

            InputModel.PlaceHolder = placeHolder != null ? placeHolder : GroupModel.Label;
            InputModel.Classes = classes;
            InputModel.AttributeObject = inputAttr;
            InputModel.TextBoxType = textType;

            if (textType == null)
            {
                Type t = typeof(TValue).RealType();
                if (t.IsDecimalType() || t.IsIntgerType())
                    InputModel.TextBoxType = "number";
                else
                    InputModel.TextBoxType = "text";
            }

            InitializeValidations(coll, alternateLabel);

            GroupModel.InputControl = GetInputControl(ComponentNames.TextBox);

            return GetView("Components/ControlGroup", GroupModel);
        }

        public MvcHtmlString TextAreaGroup(ValidationCollection<T> coll, int size, string alternateLabel, string placeHolder, object attrs, object inputAttr)
        {
            GroupModel.Label = alternateLabel == null ? GetLabel(typeof(T).Name, MemberName) : alternateLabel;
            GroupModel.Size = size;
            GroupModel.Attributes = ToAttributeString(attrs);

            InputModel.PlaceHolder = placeHolder != null ? placeHolder : GroupModel.Label;
            InputModel.AttributeObject = inputAttr;

            InitializeValidations(coll, alternateLabel);

            GroupModel.InputControl = GetInputControl(ComponentNames.Textarea);

            return GetView("Components/ControlGroup", GroupModel);
        }

        public MvcHtmlString FileGroup(string formFieldName, bool required, int size, string uploadUrl, string alternateLabel, string placeHolder, object attrs, object inputAttr)
        {
            GroupModel.Label = alternateLabel == null ? GetLabel(typeof(T).Name, MemberName) : alternateLabel;
            GroupModel.Size = size;
            GroupModel.Attributes = ToAttributeString(attrs);

            InputModel = new FileNgInput
            {
                MemberName = MemberName,
                NgModelName = ModelName,
                FormFieldName = formFieldName,
                UploadUrl = uploadUrl,
            };

            InputModel.PlaceHolder = placeHolder != null ? placeHolder : GroupModel.Label;
            InputModel.AttributeObject = inputAttr;

            if (required)
            {
                Validations = Helper.VCollection().AddRequired();
                Validations.SetMember(MemberName, typeof(T).Name, formFieldName);
                GroupModel.IsRequired = Validations.Validators.Any(d => d is RequiredValidator);
                GroupModel.ValidationMessages = Validations.GetMessages();
            }

            GroupModel.InputControl = GetInputControl(ComponentNames.FileTextBox);

            return GetView("Components/ControlGroup", GroupModel);
        }

        public MvcHtmlString CalendarGroup(
            bool isRequired,
            int size,
            string alternateLabel,
            string placeHolder,
            object attrs,
            object inputAttr,
            CalendarTypes type,
            DateRange range)
        {
            GroupModel.Label = alternateLabel == null ? GetLabel(typeof(T).Name, MemberName) : alternateLabel;
            GroupModel.Size = size;
            GroupModel.Attributes = ToAttributeString(attrs);

            InputModel.PlaceHolder = placeHolder != null ? placeHolder : GroupModel.Label;
            InputModel.AttributeObject = inputAttr;

            Validations = Helper.VCollection();

            if (type == CalendarTypes.Custom)
                Validations.Add(new DateValidator(NgOptions.DateValidationPattern, range));
            else
                Validations.Add(new DateValidator(NgOptions.DateValidationPattern, type));

            if (InputModel.AttributeObject == null && range != null)
                InputModel.AttributeObject = new { data_date_start_date = range.StartDate, data_date_end_date = range.EndDate };

            if (isRequired)
            {
                Validations.AddRequired();
                GroupModel.IsRequired = true;
            }

            Validations.SetMember(MemberName, typeof(T).Name);
            GroupModel.ValidationMessages = Validations.GetMessages();
            GroupModel.InputControl = GetInputControl(ComponentNames.CalendarTextBox);

            return GetView("Components/ControlGroup", GroupModel);
        }

        public MvcHtmlString HirjiCalendarGroup(
            bool isRequired,
            int size,
            string alternateLabel,
            string placeHolder,
            object attrs,
            object inputAttr,
            CalendarTypes type,
            DateRange range)
        {
            GroupModel.Label = alternateLabel == null ? GetLabel(typeof(T).Name, MemberName) : alternateLabel;
            GroupModel.Size = size;
            GroupModel.Attributes = ToAttributeString(attrs);

            InputModel.PlaceHolder = placeHolder != null ? placeHolder : GroupModel.Label;
            InputModel.AttributeObject = inputAttr;

            Validations = Helper.VCollection();

            if (type == CalendarTypes.Custom)
                Validations.Add(new DateValidator(NgOptions.DateValidationPattern, range));
            else
                Validations.Add(new DateValidator(NgOptions.DateValidationPattern, type));

            if (InputModel.AttributeObject == null && range != null)
                InputModel.AttributeObject = new { data_date_start_date = range.StartDate, data_date_end_date = range.EndDate };

            if (isRequired)
            {
                Validations.AddRequired();

                GroupModel.IsRequired = true;
            }

            Validations.SetMember(MemberName, typeof(T).Name);
            GroupModel.ValidationMessages = Validations.GetMessages();
            GroupModel.InputControl = GetInputControl(ComponentNames.HijriCalendarTextBox);

            return GetView("Components/ControlGroup", GroupModel);
        }

        public MvcHtmlString SelectControlGroup(string ngOpts, bool isRequired, int size, string alternateLabel, object attrs, object inputAttr = null, bool multi = false, string requiredIf = null)
        {
            GroupModel.Label = alternateLabel == null ? GetLabel(typeof(T).Name, MemberName) : alternateLabel;
            GroupModel.Size = size;
            GroupModel.Attributes = ToAttributeString(attrs);

            InputModel.AttributeObject = inputAttr;
            InputModel.NgOptions = ngOpts;

            if (multi)
                InputModel.Attributes = "chosen multiple='true' ";

            requiredIf = string.IsNullOrEmpty(requiredIf) ? null : requiredIf;
            if (isRequired || requiredIf!=null)
            {
                Validations = Helper.VCollection().AddRequired(requiredIf);
                Validations.SetMember(MemberName, typeof(T).Name);
                GroupModel.IsRequired = true;
                GroupModel.ValidationMessages = Validations.GetMessages();
            }



            GroupModel.InputControl = GetInputControl("Select");

            return GetView("Components/ControlGroup", GroupModel);
        }

        protected void InitializeValidations(ValidationCollection<T> coll, string alternateLabel = null)
        {
            Validations = coll;
            if (Validations == null)
                Validations = Helper.VCollection();

            if (InputModel.TextBoxType?.ToLower() == TextBoxTypes.Number && !Validations.Validators.Any(d => d is NumericTextValidator))
                Validations.AddNumeric();

            Validations.SetNumericDefaults();
            Validations.UseAnnotations(MemberExpression.Member.CustomAttributes);

            Validations.SetMember(MemberName, typeof(T).Name);

            if (alternateLabel != null)
                Validations.SetPropertyTitle(alternateLabel);

            GroupModel.IsRequired = Validations.Validators.Any(d => d is RequiredValidator);
            GroupModel.ValidationMessages = Validations.GetMessages();

        }
    }
}