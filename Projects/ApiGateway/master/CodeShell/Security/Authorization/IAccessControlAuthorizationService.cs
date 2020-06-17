using CodeShell.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Security.Authorization
{
    public interface IAccessControlAuthorizationService : IAuthorizationService
    {
        bool IsAuthorized(string claim, string action);
        
    }
}
