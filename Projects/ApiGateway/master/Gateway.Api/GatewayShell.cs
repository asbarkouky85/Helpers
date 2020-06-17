using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using CodeShell;
using CodeShell.Data;
using CodeShell.Security.Authorization;
using SimpleInjector;

namespace Gateway.Api
{
    public class GatewayShell : Shell
    {
        protected override CultureInfo defaultCulture => new CultureInfo("ar");

        protected override string appRoot => HttpContext.Current.Server.MapPath("~");

        protected override string urlRoot => "~";
        private Container inj;
        protected override AuthorizationService authorizationService => null;

        public GatewayShell()
        {
            inj = new Container();

        }

        protected override object getConfig(string key)
        {
            return Properties.Settings.Default.Properties[key]?.DefaultValue;
        }

        protected override Container getInjectorContainer()
        {
            return inj;
        }

        protected override IUnitOfWork getNewUnit()
        {
            return null;
        }
    }
}