using CodeShellCore.Web.Moldster;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace $safeprojectname$
{
    public class UIShell : MoldsterShell
    {
        protected override bool useLocalization => false;

        protected override CultureInfo defaultCulture => new CultureInfo("en");
        public UIShell(IConfiguration config) : base(config)
        {
        }


    }
}
