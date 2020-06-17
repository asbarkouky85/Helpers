using CodeShell.Security.Authorization;
using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace CodeShell.Web.Filters
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        public override bool AllowMultiple { get { return false; } }
        public string Resource { get; set; }
        public string Action { get; set; }
        public bool AllowAnonymous { get; set; }
        
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (Shell.AuthorizationService==null || AllowAnonymous)
            {
                return true;
            }


            if (Shell.AuthorizationService is IAccessControlAuthorizationService)
            {
                IAccessControlAuthorizationService Auth = (IAccessControlAuthorizationService)Shell.AuthorizationService;
                
                string name = Resource ?? (string)actionContext.RequestContext.RouteData.Values["controller"];
                string action = Action ?? (string)actionContext.RequestContext.RouteData.Values["action"];

                return Auth.IsAuthorized(name, action);
            }

            return true;

        }


    }
}
