using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Razor.Angular
{
    public class NgOptions
    {
        private static NgOptions _instance;
        private string _modelName;
        private string _calendarCssClass;
        private string _validationMessageContainer;
        private string _dateValidationPattern;
        private static NgOptions Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NgOptions();
                return _instance;
            }
        }
        public static string ModelName
        {
            get { return Instance._modelName; }
            set { Instance._modelName = value; }
        }

        public static string CalendarCssClass
        {
            get { return Instance._calendarCssClass; }
            set { Instance._calendarCssClass = value; }
        }

        public static string ValidationMessageContainer
        {
            get { return Instance._validationMessageContainer; }
            set { Instance._validationMessageContainer = value; }
        }

        public static string DateValidationPattern
        {
            get { return Instance._dateValidationPattern; }
            set { Instance._dateValidationPattern = value; }
        }

        private NgOptions()
        {
            _modelName = "model";
            _calendarCssClass = "date-picky";
            _validationMessageContainer = "<small ng-show='{0}.{1}.$error.{2}' class='form-text text-danger'>{3}</small>";
            _dateValidationPattern = "^([0-2][0-9]|3[0-1])[/]([0-1][0-9])[/]([1-9][0-9]{3})$";
        }
    }
}
