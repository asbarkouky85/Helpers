﻿using CodeShellCore.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace $safeprojectname$.Controllers
{
    public class HomeController : MoldsterUIController
    {
        public override string DefaultDomain => "ClientApp";

        public override string GetDefaultTitle(string loc)
        {
            throw new NotImplementedException();
        }
    }
}
