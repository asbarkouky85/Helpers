using System;
using System.Web;
using System.Web.Security;

using CodeShell.Security;
using CodeShell.Security.Sessions;

namespace CodeShell.Web.Security
{
    public class CookieSessionManager : SessionManagerBase, ISessionManager
    {
        public TimeSpan DefaultSessionTime
        {
            get { return new TimeSpan(12, 0, 0); }
        }

        public virtual void EndSession()
        {
            //HttpContext.Current.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
            FormsAuthentication.SignOut();
        }

        public override object GetCurrentUserId()
        {
            int id=0;

            int.TryParse(HttpContext.Current.User.Identity.Name, out id);
            return id;
        }

        public bool IsLoggedIn()
        {
            return !GetCurrentUserId().Equals(0);
        }

        public void AuthorizationRequest()
        {
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    HttpContext.Current.User = new DefaultPrincipal(authTicket.UserData);
                }
                catch { }
                
            }
        }

        public virtual void StartSession(IUser user, TimeSpan? length = null)
        {
            length = length == null ? DefaultSessionTime : length;
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
              1,
              "user",  //CompanyName
              DateTime.Now,
              DateTime.Now.Add(length.Value),  // expiry
              false,  //do not remember
              user.UserId.ToString(),//Data
              "/");

            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        
    }
}
