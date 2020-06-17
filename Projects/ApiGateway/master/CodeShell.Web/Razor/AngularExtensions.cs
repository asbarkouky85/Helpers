using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq;
using System.Reflection;
using CodeShell.Web.Razor.Angular;
using CodeShell.Web.Razor.Angular.Validators;
using CodeShell.Web.Razor.Core;

namespace CodeShell.Web.Razor
{
    /// <summary>
    /// <see cref="HtmlHelper{T}"/> extension methods for form building
    /// </summary>
    public static class AngularExtensions
    {
        /// <summary>
        /// Gets <see cref="HtmlHelper{TModel}"/> for the requested object
        /// </summary>
        /// <returns></returns>
        public static HtmlHelper<T> For<T>(this HtmlHelper h)
        {
            ViewDataDictionary dic = h.GetDictionary();
            ViewDataContainer cont = new ViewDataContainer(dic);
            return new HtmlHelper<T>(h.ViewContext, cont);
        }

        public static MvcHtmlString TextAreaGroup<T, TValue>(this HtmlHelper<T> helper, Expression<Func<T, TValue>> exp, ValidationCollection<T> coll = null, int size = 6, string alternateLabel = null, string placeHolder = null, object attrs = null, object inputAttr = null)
        {

            return new NgElement<T, TValue>(helper, exp).TextAreaGroup(coll, size, alternateLabel, placeHolder, attrs, inputAttr);
        }

        public static MvcHtmlString NumericControlGroup<T, TValue>(this HtmlHelper<T> helper, Expression<Func<T, TValue>> exp, bool required = false, bool numberTextBox = false, int maxLen = 15, int minLen = 0, float? minVal = null, float maxVal = 0, int size = 6, string alternateLabel = null, string placeHolder = null, object attrs = null, object inputAttr = null, string classes = "", string value = "")
        {
            var coll = helper.VCollection().AddNumeric();
            if (maxLen != 0 || minLen != 0)
            {
                coll.AddLength(maxLen, minLen);
            }

            if (minVal != null || maxVal != 0)
                coll.AddMinMax(minVal, maxVal);

            if (required)
                coll.AddRequired();
            string textType = numberTextBox ? "number" : "text";
            return new NgElement<T, TValue>(helper, exp).ControlGroup(coll, size, textType, alternateLabel, placeHolder, attrs, inputAttr, classes, value);
        }

        public static MvcHtmlString ControlGroup<T, TValue>(this HtmlHelper<T> helper, Expression<Func<T, TValue>> exp, ValidationCollection<T> coll = null, string textType = "text", int size = 6, string alternateLabel = null, string placeHolder = null, object attrs = null, object inputAttr = null, string classes = "", string value = "")
        {

            return new NgElement<T, TValue>(helper, exp).ControlGroup(coll, size, textType, alternateLabel, placeHolder, attrs, inputAttr, classes, value);
        }

        public static MvcHtmlString FileGroup<T, TValue>(this HtmlHelper<T> helper, Expression<Func<T, TValue>> exp, string formFieldName, bool required = false, string uploadUrl = "Files/Upload", int size = 6, string alternateLabel = null, string placeHolder = null, object attrs = null, object inputAttr = null)
        {

            return new NgElement<T, TValue>(helper, exp).FileGroup(formFieldName, required, size, uploadUrl, alternateLabel, placeHolder, attrs, inputAttr);
        }



        public static MvcHtmlString CalendarGroup<T, TValue>(
            this HtmlHelper<T> helper,
            Expression<Func<T, TValue>> exp,
            bool isRequired = false,
            int size = 6,
            string alternateLabel = null,
            string placeHolder = null,
            object attrs = null,
            object inputAttr = null,
            CalendarTypes calendarType = CalendarTypes.PastAndFuture,
            DateRange range = null)
        {

            return new NgElement<T, TValue>(helper, exp).CalendarGroup(isRequired, size, alternateLabel, placeHolder, attrs, inputAttr, calendarType, range);
        }

        public static MvcHtmlString HiriCalendarGroup<T, TValue>(
            this HtmlHelper<T> helper,
            Expression<Func<T, TValue>> exp,
            bool isRequired = false,
            int size = 6,
            string alternateLabel = null,
            string placeHolder = null,
            object attrs = null,
            object inputAttr = null,
            CalendarTypes calendarType = CalendarTypes.PastAndFuture,
            DateRange range = null)
        {

            return new NgElement<T, TValue>(helper, exp).HirjiCalendarGroup(isRequired, size, alternateLabel, placeHolder, attrs, inputAttr, calendarType, range);
        }

        public static MvcHtmlString SelectControlGroup<T, TValue>(this HtmlHelper<T> helper, Expression<Func<T, TValue>> exp, string ngOpts, bool isRequired = false, int size = 6, string alternateLabel = null, object attrs = null, object inputAttr = null, bool multi = false, string requiredIf = "")
        {
            return new NgElement<T, TValue>(helper, exp).SelectControlGroup(ngOpts, isRequired, size, alternateLabel, attrs, inputAttr, multi,requiredIf);
        }
    }
}
