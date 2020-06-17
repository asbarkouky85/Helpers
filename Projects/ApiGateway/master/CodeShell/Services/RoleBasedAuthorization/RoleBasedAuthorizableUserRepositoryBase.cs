using CodeShell.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Business.RoleBasedAuthorization
{
    public class RoleBasedAuthorizableUserRepositoryBase<T> : Repository<T> where T : class, IRoleBasedAuthorizableUser
    {
        public RoleBasedAuthorizableUserRepositoryBase(DbContext con) : base(con) { }


    }
}
