using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeShell.Data;
using CodeShell.Services;

namespace CodeShell.Security.Authentication
{
    public class DefaultAuthenticationService : ServiceBase, IAuthenticationService
    {
        
        public virtual bool Check(string name, string password)
        {
            IUser user = UnitOfWork.UserRepository.GetByCredentials(name, password);
            return user != null;
        }

        public virtual LoginResult Login(string name, string password)
        {
            IUser usr = UnitOfWork.UserRepository.GetByCredentials(name, password);
            string message = usr == null ? "اسم المستخدم او كلمه المرور غير صحيحه" : "مرحبا";
            return new LoginResult(usr != null, message, usr);
        }

        public virtual LoginResult LoginById(object id)
        {
            IUser usr = UnitOfWork.UserRepository.GetByUserId(id);
            string message = usr == null ? "اسم المستخدم او كلمه المرور غير صحيحه" : "مرحبا";
            return new LoginResult(usr != null, message, usr);
        }
    }
}
