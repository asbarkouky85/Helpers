using CodeShell;
using CodeShell.Data;
using CodeShell.Security.Authorization;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Gateway.Test.Api
{
    public class TestShell : Shell
    {
        protected override CultureInfo defaultCulture => new CultureInfo("en");

        protected override string appRoot => HttpContext.Current.Server.MapPath("~");

        protected override string urlRoot => "~";

        protected override AuthorizationService authorizationService => null;
        Container _inj = new Container();

        public TestShell()
        {

        }
        protected override object getConfig(string key)
        {
            return Properties.Settings.Default[key];
        }

        protected override Container getInjectorContainer()
        {
            return _inj;
        }

        protected override IUnitOfWork getNewUnit()
        {
            return null;
        }
    }
}