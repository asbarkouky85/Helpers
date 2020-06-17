using CodeShell.Business;
using CodeShell.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeShell.Web.Controllers
{
    public class BaseMvcController : Controller
    {
        Dictionary<Type, ServiceBase> services;

        protected T GetService<T>() where T : ServiceBase
        {
            if (services == null)
                services = new Dictionary<Type, ServiceBase>();

            if (!services.ContainsKey(typeof(T)))
                services[typeof(T)] = Activator.CreateInstance<T>();

            return (T)services[typeof(T)];
        }

        protected T ReadQueryAs<T>()
        {
            T obj = Activator.CreateInstance<T>();
            Request.Url.TryReadQueryAs(out obj);
            return obj;
        }
    }
}
