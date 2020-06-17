using System;
using System.Web;

namespace Gateway.Api
{
    public class ApiHandler : IHttpAsyncHandler, IHttpHandler
    {
        
        public bool IsReusable { get { return true; } }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            AsyncOperation op = new AsyncOperation(cb, context, extraData);
            op.Start();
            return op;
        }
        
        public void EndProcessRequest(IAsyncResult result)
        {
            //throw new NotImplementedException();
        }

        public void ProcessRequest(HttpContext context)
        {
            //Console.WriteLine(context);
        }
    }

    
}