using CodeShell.Globalization;
using CodeShell.Web.Razor.Angular;
using CodeShell.Web.Razor.Angular.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CodeShell.Web.Razor.Core
{

    public class HtmlElement<T, TValue>
    {
        protected NgControlGroup GroupModel { get; set; }
        protected NgInput InputModel { get; set; }
        protected ValidationCollection<T> Validations { get; set; }
        protected HtmlHelper<T> Helper { get; set; }
        protected MemberExpression MemberExpression { get; set; }

        protected string ModelName { get; set; }
        protected string MemberName { get; set; }

        
        public HtmlElement(HtmlHelper<T> helper, Expression<Func<T, TValue>> exp)
        {
            Helper = helper;

            MemberExpression ex = (MemberExpression)exp.Body;

            MemberName = GetMemberName(ex);

            
            ModelName = GetModelName();

            GroupModel = new NgControlGroup
            {
                Label = GetLabel(typeof(T).Name, MemberName)
            };

            InputModel = new NgInput
            {
                MemberName = MemberName,
                NgModelName = ModelName
            };
        }

        protected MvcHtmlString GetView(string template, object model = null)
        {
            return Helper.Partial(Path.Combine(Shell.AppRootUrl, "Views/" + template + ".cshtml"), model);
        }

        protected string GetMemberName(MemberExpression exp)
        {
            string name = "";

            if (exp.Expression is MemberExpression)
            {
                name = ((MemberExpression)exp.Expression).Member.Name + "." + exp.Member.Name;
                MemberExpression = (MemberExpression)exp.Expression;
            }

            else
            {
                name = exp.Member.Name;
                MemberExpression = exp;
            }
            
            return name;
        }

        protected string GetModelName()
        {
            if (Helper.ViewData.ContainsKey("NgModelName"))
                return Helper.ViewData["NgModelName"].ToString();
            else
                return NgOptions.ModelName;
        }


        protected string GetLabel(string entity, string col)
        {
            bool isSub = col.Contains(".");
            col = col.Replace(".", "_");
            if (isSub)
                return Strings.Column(col);
            return Strings.Column(entity + "_" + col);
        }

        protected string ToAttributeString(object attr)
        {
            if (attr == null)
                return "";
            List<string> lst = new List<string>();
            PropertyInfo[] props = attr.GetType().GetProperties();
            foreach (var pair in props)
            {
                string name = pair.Name.ToLower().Replace('_', '-');

                lst.Add(name + "=\"" + pair.GetValue(attr) + "\"");
            }
            string st = string.Join(" ", lst);

            return st;
        }
        
        protected MvcHtmlString GetInputControl(string componentName)
        {
            if (string.IsNullOrEmpty(InputModel.Attributes))
                InputModel.Attributes = "";

            if (Validations != null)
                InputModel.Attributes += Validations.GetAttributes();

            InputModel.Attributes += ToAttributeString(InputModel.AttributeObject);

            return GetView("Components/"+componentName, InputModel);
        }
        
    }
}
