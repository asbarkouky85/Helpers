using CodeShell.Business;
using CodeShell.Security.Authentication;
using CodeShell.Security.Sessions;
using CodeShell.Data;
using CodeShell.Services;

namespace CodeShell.Security.Authorization
{
    public class AuthorizationService : ServiceBase, IAuthorizationService
    {
        public virtual IAuthenticationService AuthenticationService
        {
            get { return Shell.Injector.GetInstance<IAuthenticationService>(); }
        }

        public virtual ISessionManager SessionManager
        {
            get { return Shell.Injector.GetInstance<ISessionManager>(); }
        }

        public virtual LoginResult Login(LoginModel mod)
        {
            LoginResult res = AuthenticationService.Login(mod.UserName, mod.Password);
            if (res.Success)
                SessionManager.StartSession(res.UserData);
            return res;
        }
        
        public virtual void OnUserLogin(IUser user) { }

        public virtual void LogOut()
        {
            SessionManager.EndSession();
        }
    }
}
