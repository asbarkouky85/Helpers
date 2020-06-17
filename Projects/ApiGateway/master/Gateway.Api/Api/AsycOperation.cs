using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using CodeShell.Data.Helpers;
using CodeShell.Utilities;
using CodeShell.Web;
using CodeShell.Text;
using System.Collections.Generic;
using CodeShell.Files;

namespace Gateway.Api
{
    public class AsyncOperation : IAsyncResult
    {
        public string BaseUrl { get { return Properties.Settings.Default.WebApiUrl; } }
        private bool _complete;
        private object _state;
        private HttpContext _context;
        private AsyncCallback _callback;
        public bool IsCompleted { get { return _complete; } }

        public WaitHandle AsyncWaitHandle { get { return null; } }

        public object AsyncState { get { return _state; } }

        public bool CompletedSynchronously { get { return false; } }

        public AsyncOperation(AsyncCallback callback, HttpContext con, Object state)
        {
            _complete = false;
            _callback = callback;
            _context = con;
            _state = state;
        }

        public void Start()
        {
            Task.Run(() => Handle());
        }

        string _processContentType(string st)
        {
            if (st.Contains(";"))
                return st.GetBefore(";");
            return st;
        }

        protected async Task Handle()
        {
            try
            {
                HttpClient client = new HttpClient();

                string url = Utils.CombineUrl(BaseUrl, _context.Request.Url.AbsolutePath);

                client.Timeout = new TimeSpan(0, 5, 0);

                var encoding = _context.Request.ContentEncoding;
                var contentType = "application/json";

                foreach (var head in _context.Request.Headers.AllKeys)
                {
                    var elem = _context.Request.Headers[head];
                    if (head.Contains("Content"))
                    {
                        if (head.ToLower() == "content-type")
                            contentType = _processContentType(_context.Request.Headers[head]);
                    }
                    else
                    {
                        client.DefaultRequestHeaders.Add(head, elem);
                    }
                }


                if (!string.IsNullOrEmpty(_context.Request.Url.Query))
                    url += _context.Request.Url.Query;
                string data = null;
                string method = _context.Request.HttpMethod.ToLower();

                HttpResponseMessage mes = new HttpResponseMessage(HttpStatusCode.OK);
                HttpContent content = new StringContent("", Encoding.UTF8, contentType);

                if (method != "get" && method != "delete")
                {
                    if (contentType.Contains("multipart"))
                    {
                        var files = _context.Request.Files;
                        content = ConvertToMultipartContent(files);
                    }
                    else
                    {
                        using (StreamReader readStream = new StreamReader(_context.Request.InputStream, encoding))
                        {
                            data = readStream.ReadToEnd();
                        }
                        content = new StringContent(data, encoding, contentType);
                    }

                }

                switch (method)
                {
                    case "get":
                        mes = await client.GetAsync(url);
                        break;
                    case "post":
                        mes = await client.PostAsync(url, content);
                        break;
                    case "put":
                        mes = await client.PutAsync(url, content);
                        break;
                    case "delete":
                        mes = await client.DeleteAsync(url);
                        break;
                }


                var headers = mes.Content.Headers as HttpContentHeaders;
                foreach (var h in headers)
                {
                    _context.Response.Headers[h.Key] = h.Value.FirstOrDefault();
                }

                _context.Response.ContentType = headers.ContentType?.ToString();
                await mes.Content.CopyToAsync(_context.Response.OutputStream);

                _context.Response.StatusCode = (int)mes.StatusCode;
                _complete = true;
                _callback(this);
            }
            catch (Exception ex)
            {
                SubmitResult res = new SubmitResult(1, "Error");
                res.SetException(ex);
                _context.Response.Write(res.ToJson());
                _context.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                _complete = true;
                _callback(this);
            }
        }

        private MultipartFormDataContent ConvertToMultipartContent(HttpFileCollection files)
        {
            var content = new MultipartFormDataContent();


            foreach (string fil in files)
            {
                var n = files[fil];
                StreamContent cont = new StreamContent(n.InputStream);
                cont.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "\"" + fil + "\"",
                    FileName = "\"" + n.FileName + "\""
                };
                content.Add(cont, fil, n.FileName);
            }

            return content;

        }
    }
}