using CodeShell.Globalization;
using CodeShell.Web.Razor.Angular;
using CodeShell.Web.Razor.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CodeShell.Web.Razor.Tables
{
    public class CellMaker<T, TValue> : HtmlElement<T, TValue>
    {
        
        public CellMaker(HtmlHelper<T> helper, Expression<Func<T, TValue>> exp) : base(helper, exp)
        {
            
        }

        public MvcHtmlString SelectCell(string source, string display, string value)
        {
            SelectCellParameters par = new SelectCellParameters();

            par.MemberName = MemberName;
            par.SourceName = source;
            par.DisplayMember = display;
            par.ValueMember = value;

            return GetView("Components/TableCells/ComboCell", par);
        }


        public MvcHtmlString TextBoxCell(bool attr)
        {
            CellParameters par = new CellParameters();
            par.MemberName = MemberName;
            par.ModelName = ModelName;
            par.IsRequired = attr;
            return GetView("Components/TableCells/TextBoxCell", par);
        }

        public MvcHtmlString TextCell(string modelName,object attr)
        {
            CellParameters par = new CellParameters();
            par.ModelName = modelName;
            par.MemberName = MemberName;
            par.Attributes = ToAttributeString(attr);
            return GetView("Components/TableCells/TextCell", par);
        }

        public MvcHtmlString CalendarCell(object attr=null)
        {
            CellParameters par = new CellParameters();
            par.ModelName = ModelName;
            par.MemberName = MemberName;
            par.Attributes = ToAttributeString(attr);
            
            par.InputControl = GetInputControl(ComponentNames.CalendarTextBox);

            InputModel.PlaceHolder = GroupModel.Label;

            Validations = Helper.VCollection().AddPattern(NgOptions.DateValidationPattern);
           
            return GetView("Components/TableCells/Cell", par);
        }
    }
}
