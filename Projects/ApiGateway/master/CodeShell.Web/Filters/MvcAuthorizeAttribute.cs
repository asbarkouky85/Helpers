using CodeShell.Security.Authorization;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace CodeShell.Web.Filters
{
    public class MvcAuthorizeAttribute : AuthorizeAttribute
    {
        
        public string Resource { get; set; }
        public string Action { get; set; }
        public bool AllowAnonymous { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (AllowAnonymous)
            {
                return;
            }


            if (Shell.AuthorizationService is IAccessControlAuthorizationService)
            {
                IAccessControlAuthorizationService Auth = (IAccessControlAuthorizationService)Shell.AuthorizationService;

                string name = Resource ?? (string)filterContext.RequestContext.RouteData.Values["controller"];
                string action = Action ?? (string)filterContext.RequestContext.RouteData.Values["action"];


                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "UnAuthorized" }));
            }
        }

    }
}
