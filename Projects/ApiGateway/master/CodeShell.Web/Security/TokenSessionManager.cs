using CodeShell.Cryptography;
using CodeShell.Security;
using CodeShell.Security.Authentication;
using CodeShell.Security.Sessions;
using CodeShell.Tracer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CodeShell.Web.Security
{
    public class TokenSessionManager : SessionManagerBase, ISessionManager
    {
        public TimeSpan DefaultSessionTime { get { return new TimeSpan(24, 0, 0); } }
        
        public TokenSessionManager()
        {

        }

        public void AuthorizationRequest()
        {
            HttpContext.Current.User = null;
            if (HttpContext.Current.Request.Headers.AllKeys.Contains("auth-token"))
            {
                //Logger.WriteLine("token recieved on Portal.Web " + HttpContext.Current.Request.Headers["auth-token"]);
                string head = HttpContext.Current.Request.Headers["auth-token"];

                string data = Shell.Encryptor.Decrypt(head);
                if (data != null)
                {
                    //Logger.WriteLine("token Successfully decrypted : " + data);
                    JWTData jwt = data.FromJson<JWTData>();
                    //Logger.WriteLine("token successfully parsed");
                    if (jwt != null && jwt.IsValid(HttpContext.Current.Request.GetHostName()))
                    {
                        HttpContext.Current.User = new DefaultPrincipal(jwt.UserId.ToString());
                    }
                }
            }

        }
        
        public override object GetCurrentUserId()
        {
            int id;

            if (HttpContext.Current.User != null && int.TryParse(HttpContext.Current.User.Identity.Name, out id))
                return id;

            return 0;
        }

        public bool IsLoggedIn()
        {
            return !GetCurrentUserId().Equals(0);
        }

        public void StartSession(IUser user, TimeSpan? length = null) { }

        public void EndSession()
        {
            base.ResetUserCache(GetCurrentUserId());

        }
    }
}
