using CodeShell.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gateway.Test.Api.Controllers
{
    [ApiAuthorize(AllowAnonymous = false)]
    public class FilesController : CodeShell.Web.Controllers.BaseFilesController
    {
    }
}