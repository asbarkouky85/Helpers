using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Razor
{
    public enum ElementTypes
    {
        TextBox,
        TextBoxWithLabel,
        ComboBox,
        CheckListBox,

    }

    public enum CalendarTypes
    {
        PastDate,
        PastAndFuture,
        Custom
    }

    public static class TextBoxTypes
    {
        public const string Text = "text";
        public const string Date = "date";
        public const string Calendar = "calendar";
        public const string Number = "number";
        public const string Password = "password";
        public const string Email = "email";
    }
}
