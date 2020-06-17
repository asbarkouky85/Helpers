using CodeShell.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using System.Configuration;
using System.Globalization;
using CodeShell.Security.Authentication;
using CodeShell.Security.Sessions;
using CodeShell.Security.Authorization;
using System.Reflection;
using CodeShell.Security;
using CodeShell.Tracer;
using System.IO;
using CodeShell.Cryptography;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using CodeShell.Data.Helpers;

namespace CodeShell
{
    public abstract class Shell
    {
        private static Shell _instance;
        private static Encryptor _encryptor;

        public static void Start(Shell cont)
        {
            _instance = cont;
            string path = Path.Combine(AppRootPath, "Logs");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            Logger.Set(ProjectAssembly.GetName().Name, path);

            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            ServicePointManager.Expect100Continue = true;
            

        }

        public static IUnitOfWork Unit { get { return Injector.GetInstance<IUnitOfWork>(); } }
        public static UtilityUnitContainer UtilityUnits { get { return Injector.GetInstance<UtilityUnitContainer>(); } }
        public static IUser User { get { return AuthorizationService.SessionManager?.GetUserData(); } }
        public static Assembly ProjectAssembly { get { return _instance.GetType().Assembly; } }
        public static string LocalizationAssembly
        {
            get { return _instance.localizationAssembly ?? ProjectAssembly.GetName().Name; }
        }
        // Abdelrahman 11/6/2018
        public static object GetConfig(string key)
        {
            object val = _instance.getConfig(key);
            if (val == null)
                throw new Exception("Config '" + key + "' is required to be present in the config file");
            
            return val;
        }

        public static T GetConfigAs<T>(string key)
        {
            return (T)GetConfig(key);
        }
        // Abdelrahman 11/6/2018

        public static Encryptor Encryptor
        {
            get
            {
                if (_encryptor == null)
                {
                    string key = GetConfigAs<string>("AuthenticationKey");
                    _encryptor = new Encryptor(key);
                }
                return _encryptor;
            }
        }

        public static CultureInfo DefaultCulture { get { return _instance.defaultCulture; } }
        public static AuthorizationService AuthorizationService { get { return _instance.authorizationService; } }
        public static string ReportsRoot { get { return _instance.reportsRoot; } }
        public static string AppRootPath { get { return _instance.appRoot; } }
        public static string AppRootUrl { get { return _instance.urlRoot; } }
        public static Container Injector { get { return _instance.getInjectorContainer(); } }
        public static IUnitOfWork GetFreshUnit() { return _instance.getNewUnit(); }


        protected abstract CultureInfo defaultCulture { get; }

        protected abstract string appRoot { get; }
        protected abstract string urlRoot { get; }
        protected abstract object getConfig(string key);    // Abdelrahman 11/6/2018
        protected abstract AuthorizationService authorizationService { get; }
        protected abstract IUnitOfWork getNewUnit();
        protected abstract Container getInjectorContainer();

        protected virtual string localizationAssembly { get { return null; } }
        protected virtual string reportsRoot { get { return Path.Combine(appRoot, "Reports"); } }
    }
}
