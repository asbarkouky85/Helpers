using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Mvc;
using System.Collections;

namespace CodeShell.Web.Razor.Core
{
    public class HtmlContainer : IDisposable
    {
        string _tag = "div";
        protected ViewContext Context { get; set; }
        public virtual string TagName { get { return _tag; } }

        public HtmlContainer(ViewContext cont = null) { }

        public HtmlContainer(ViewContext cont = null, string tag = "div", object attributes = null)
        {
            _tag = tag;
            Context = cont;
            init(attributes);
        }

        public HtmlContainer(ViewContext cont = null, object attributes = null)
        {
            Context = cont;
            init(attributes);

        }

        protected string GetAttibutesString(object attr) 
        {
            string attrs = "";
            if (attr != null)
            {
                if (attr is IDictionary<string, object>)
                {
                    IDictionary<string, object> dic = attr as IDictionary<string, object>;

                    foreach (KeyValuePair<string, object> inf in dic)
                    {
                        attrs += inf.Key + "=\"" + inf.Value.ToString() + "\"";
                    }

                }
                else
                {
                    PropertyInfo[] po = attr.GetType().GetProperties();
                    foreach (PropertyInfo inf in po)
                    {
                        attrs += inf.Name + "=\"" + inf.GetValue(attr, null) + "\"";
                    }
                }
            }
            return attrs;
        }

        protected virtual void init(object attr)
        {
            if (Context != null)
            {
                string attrs = GetAttibutesString(attr);
                Context.Writer.Write(string.Format("<{0} {1}>", TagName, attrs));
            }
        }

        void IDisposable.Dispose()
        {
            if (Context != null)
                TagClose();
        }

        protected virtual void TagClose()
        {
            Context.Writer.Write("</" + TagName + ">");
        }
    }
}
