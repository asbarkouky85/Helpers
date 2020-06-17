using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Security.Authentication
{
    
    public class ActiveDirectoryAuthenticationService : IAuthenticationService
    {

        public bool Check(string name, string password)
        {
            DomainUser user = new DomainUser(name);
            if (user.Domain == null || user.UserName == null)
                return false;
            
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, user.Domain))
            {
                return pc.ValidateCredentials(user.UserName, password);

            }
        }

        public LoginResult Login(string name, string password)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
                return new LoginResult(false, "يرجى التأكد من إدخال إسم المستخدم وكلمة المرور وإعادة المحاولة");

            string[] unameArr = name.Split('\\');

            if (unameArr.Length < 2)
                return new LoginResult(false, "الرجاء كتابه اسم النطاق \\ اسم المستخدم");

            string domain = unameArr[0];
            string userName = unameArr[1];

            IUser CurrentUser = Shell.Unit.UserRepository.GetByName(userName);
            if (CurrentUser == null)
                return new LoginResult(false, "المستخدم غير موجود أو غير مفعل");

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
            {
                if (pc.ValidateCredentials(userName, password))
                {
                    return new LoginResult(true, "مرحباً بك !", CurrentUser);
                }
                else
                    return new LoginResult(false, "تسجيل الدخول لم ينجح رجاءً تأكد من إسم المُستخدم و كلمة السر");
            }
        }

        public LoginResult LoginById(object id)
        {
            return new LoginResult(false,"Not Supported");
        }
    }
}
