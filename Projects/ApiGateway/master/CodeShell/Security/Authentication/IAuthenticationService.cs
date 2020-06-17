namespace CodeShell.Security.Authentication
{
    public interface IAuthenticationService
    {
        LoginResult Login(string name, string password);
        bool Check(string name, string password);
        LoginResult LoginById(object id);
    }
}
