using CodeShell.Data;
using CodeShell.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Business.RoleBasedAuthorization
{
    public interface IProtectedPage : IModel
    {
        string PageIndex { get; set; }
    }

    public interface IAuthorizationRole : IModel
    {
        string Name { get; set; }


    }

    public interface IRoleBasedAuthorizableUser : IAuthorizableUser
    {
        IEnumerable<IAuthorizationRole> Roles { get; }
    }
}
