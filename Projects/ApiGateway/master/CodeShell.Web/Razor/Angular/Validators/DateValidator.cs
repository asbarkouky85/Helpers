using CodeShell.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Razor.Angular.Validators
{
    public class DateValidator : Validator
    {
        CalendarTypes Type;
        
        string Pattern;
        DateRange Range;
        public DateValidator(string pattern, CalendarTypes type)
        {
            Type = type;
            Pattern = pattern;
        }

        public DateValidator(string pattern, DateRange range)
        {
            Type = CalendarTypes.Custom;
            Pattern = pattern;
            Range = range;
        }
        public override string Attribute {
            get
            {
                string attrs="ng-pattern='/"+Pattern+"/'";

                switch (Type)
                {
                    case CalendarTypes.PastDate:
                        attrs += " data-range='past'";
                        break;
                    case CalendarTypes.Custom:
                        attrs += " data-range='"+Range.ToString()+"'";
                        break;
                }
                return attrs;
            }
        }

        public override string ValidationMessage
        {
            get
            {
                List<string> lst = new List<string>();
                string lab = GetLabel(ModelType, PropertyName);
                lst.Add(MakeMessage("pattern", Strings.Message(MessageIds.invalid_field, lab)));

                switch (Type)
                {
                    
                    case CalendarTypes.PastDate:
                        lst.Add(MakeMessage("date_validation", Strings.Message(MessageIds.past_date_only, lab)));
                        break;
                    case CalendarTypes.Custom:
                        if (!string.IsNullOrEmpty(Range.StartDate) && !string.IsNullOrEmpty(Range.EndDate))
                        {
                            lst.Add(MakeMessage("date_validation", Strings.Message(MessageIds.invalid_min, lab, Range.StartDate, Range.EndDate)));
                        }
                        else if(!string.IsNullOrEmpty(Range.StartDate))
                        {
                            lst.Add(MakeMessage("date_validation", Strings.Message(MessageIds.invalid_min, lab, Range.StartDate)));
                        }
                        else if (!string.IsNullOrEmpty(Range.EndDate))
                        {
                            lst.Add(MakeMessage("date_validation", Strings.Message(MessageIds.invalid_max, lab, Range.EndDate)));
                        }
                        break;
                }

                return string.Join("" , lst);
            }
        }
    }
}
