﻿using CodeShellCore.Moldster.Db.Data;
using CodeShellCore.Web.Razor.Controllers;
using CodeShellCore.Web.Razor.Services;

namespace $safeprojectname$.Controllers
{
    public class ViewsController : DbViewsControllerBase
    {
        public ViewsController(ServerViewsService service, IConfigUnit unit) : base(service, unit)
        {
        }
    }
}
