using CodeShell.Cryptography;
using CodeShell.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Security
{
    public abstract class TokenAuthenticationService : DefaultAuthenticationService, IAuthenticationService
    {
        protected abstract string TokenProvider { get; }
        public TokenAuthenticationService()
        {
            
        }
        public override LoginResult Login(string name, string password)
        {
            LoginResult res = base.Login(name, password);
            if (res.Success)
            {
                SetToken(res);
            }

            return res;

        }

        public override LoginResult LoginById(object id)
        {
            LoginResult res = base.LoginById(id);
            if (res.Success)
            {
                SetToken(res);
            }
            return res;
        }

        private void SetToken(LoginResult res)
        {
            TimeSpan time = Shell.AuthorizationService.SessionManager.DefaultSessionTime;
            JWTData jwt = new JWTData
            {
                UserId = res.UserData.UserId,
                Provider = TokenProvider,
                StartTime = DateTime.Now,
                ExpireTime = DateTime.Now + time
            };
            res.TokenExpiry = jwt.ExpireTime;
            res.Token = Shell.Encryptor.Encrypt(jwt.ToJson());
        }

        public static string MakeTestToken(object userId, string provider)
        {
            TimeSpan time = Shell.AuthorizationService.SessionManager.DefaultSessionTime;
            JWTData jwt = new JWTData
            {
                UserId = userId,
                Provider = provider,
                StartTime = DateTime.Now,
                ExpireTime = DateTime.Now + time
            };
            
            return Shell.Encryptor.Encrypt(jwt.ToJson());
        }

        
    }
}
