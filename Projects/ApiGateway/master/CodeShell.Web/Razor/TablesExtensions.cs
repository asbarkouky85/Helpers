using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using CodeShell.Web.Razor.Tables;

namespace CodeShell.Web.Razor
{

    public static class TablesHelperExtensions
    {

        public static MvcHtmlString HeaderRow<T>(this HtmlHelper<T> helper, params string[] parameters)
        {
            MvcHtmlString st = helper.GetBuilder().HeaderRow(parameters);
            return new MvcHtmlString("<thead>" + st.ToHtmlString() + "</thead>");
        }

        public static MvcHtmlString SelectCell<T, TValue>(this HtmlHelper<T> helper, Expression<Func<T, TValue>> exp, string source,bool enabled, string displayMember = "NAME", string valueMember = "ID")
        {
            CellMaker<T, TValue> cont = new CellMaker<T, TValue>(helper, exp);
            return cont.SelectCell(source, displayMember, valueMember);
        }
        
        public static MvcHtmlString TextBoxCell<T, TValue>(this HtmlHelper<T> helper, Expression<Func<T, TValue>> exp, bool required=false)
        {
            CellMaker<T, TValue> cont = new CellMaker<T, TValue>(helper, exp);
            return cont.TextBoxCell(required);
        }

        public static MvcHtmlString CalendarCell<T, TValue>(this HtmlHelper<T> helper, Expression<Func<T, TValue>> exp, object attr = null)
        {
            CellMaker<T, TValue> cont = new CellMaker<T, TValue>(helper, exp);
            return cont.CalendarCell(attr);
        }

        public static MvcHtmlString TextCell<T, TValue>(this HtmlHelper<T> helper,string modelName, Expression<Func<T, TValue>> exp,object attr=null)
        {
            CellMaker<T, TValue> cont = new CellMaker<T, TValue>(helper, exp);
            return cont.TextCell(modelName,attr);
        }

    }
}