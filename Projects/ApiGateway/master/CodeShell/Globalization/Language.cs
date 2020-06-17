using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Globalization
{
    public class Language
    {
        protected CultureInfo _culture;
        public Language()
        {
            _culture = Shell.DefaultCulture;
        }
        public static Language Current { get { return Shell.Injector.GetInstance<Language>(); } }

        protected void setCulture(CultureInfo inf)
        {
            _culture = inf;
        }
        public static void SetCulture(string code)
        {
            CultureInfo inf = new CultureInfo(code);
            if (inf != null)
                Current._culture = inf;
        }

        public static CultureInfo Culture { get { return Current._culture; } }

        
    }
}
