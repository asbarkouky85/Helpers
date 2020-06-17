using CodeShell.Data;
using CodeShell.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Security
{
    public interface IUserRepository : IRepository
    {
        IUser GetByName(string name);
        IUser GetByCredentials(string name, string password);
        
        IUser GetByUserId(object c);
        bool EmailExists(string Email);
      
        
    }
}
