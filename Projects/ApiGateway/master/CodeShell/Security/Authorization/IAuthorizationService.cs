using CodeShell.Security.Authentication;
using CodeShell.Security.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Security.Authorization
{
    public interface IAuthorizationService
    {
        IAuthenticationService AuthenticationService { get; }
        ISessionManager SessionManager { get; }
        LoginResult Login(LoginModel mod);

        void LogOut();
    }
}
