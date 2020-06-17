using CodeShell.Business;
using CodeShell.Data.Helpers;
using CodeShell.Services;
using CodeShell.Web.Filters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace CodeShell.Web.Controllers
{
    [ApiExceptionFilter]
    [ApiAuthorize]
    public class BaseApiController : ApiController
    {
        Dictionary<Type, ServiceBase> services;
        

        public BaseApiController()
        {
            
        }

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
            Request.RequestUri.TryReadQueryAs(out obj);
            return obj;
        }

        protected GetOptions<T> ReadOptionsFor<T>() where T: class
        {
            LoadOpts opts= Activator.CreateInstance<LoadOpts>();
            if (Request.RequestUri.TryReadQueryAs(out opts))
                return opts.GetOptionsFor<T>();
            else
                return new GetOptions<T>();
        }

        
    }
}
