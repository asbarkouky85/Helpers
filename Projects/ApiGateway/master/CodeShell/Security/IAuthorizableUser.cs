using CodeShell.Data;
using CodeShell.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Security
{
    public interface IAuthorizableUser : IUser,IModel
    {
        Dictionary<string, Permission> Permissions { get; }
    }
}
