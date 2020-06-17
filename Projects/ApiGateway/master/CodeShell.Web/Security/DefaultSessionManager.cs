using CodeShell.Security;
using CodeShell.Security.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace CodeShell.Web.Security
{
    public class DefaultSessionManager : SessionManagerBase, ISessionManager
    {

        public TimeSpan DefaultSessionTime
        {
            get { return new TimeSpan(12, 0, 0); }
        }

        public void EndSession()
        {
            object ob = GetCurrentUserId();
            if (ob != null && UsersCache.ContainsKey(ob))
                UsersCache.Remove(ob);
            HttpContext.Current.Session.Remove("UserId");

        }

        public override object GetCurrentUserId()
        {
            return HttpContext.Current.Session["UserId"];
        }



        public void StartSession(IUser user, TimeSpan? length = null)
        {
            HttpContext.Current.Session["UserId"] = user.UserId;
            UsersCache[user.UserId] = user;
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, HttpContext.Current.Session.SessionID);
            cookie.Domain = "/";
            cookie.Expires = DateTime.Now + (length.HasValue ? length.Value : DefaultSessionTime);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        public bool IsLoggedIn()
        {
            return HttpContext.Current.Session["UserId"] != null;
        }

        public void AuthorizationRequest()
        {
           
        }
    }
}
