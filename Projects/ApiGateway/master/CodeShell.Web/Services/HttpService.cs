using CodeShell.Data.Helpers;
using CodeShell.Services;
using CodeShell.Files;
using CodeShell.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security; //Abdelrahman 7/6/2018
using System.Reflection;
using System.Security.Cryptography.X509Certificates;//Abdelrahman 7/6/2018
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

namespace CodeShell.Web.Services
{
    public abstract class HttpService : ServiceBase
    {
        HttpClient Client;

        protected abstract string BaseUrl { get; }

        private Dictionary<string, string> _headers;
        public Dictionary<string, string> Headers
        {
            get
            {
                if (_headers == null)
                    _headers = new Dictionary<string, string>();
                return _headers;
            }
        }

        public string LogToFile { get; set; }

        #region Loggers

        public string GenerateFileTolog(int serviceId)
        {

            DateTime tl = DateTime.Now;
            string dir = Path.Combine(CodeShell.Shell.AppRootPath, "RequestLogs");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string name = string.Format("Request_{0}_{1}-{2}-{3}_{4}_{5}_{6}.log",
                serviceId,
                tl.Year,
                tl.Month.ToString("D2"),
                tl.Day.ToString("D2"),
                tl.Hour.ToString("D2"),
                tl.Minute.ToString("D2"),
                tl.Second.ToString("D2"));

            return Path.Combine(dir, name);

        }
        public string GenerateFileTologPutRequest(int serviceId)
        {

            DateTime tl = DateTime.Now;
            string dir = Path.Combine(CodeShell.Shell.AppRootPath, "PutRequestLogs");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string name = string.Format("Request_{0}_{1}-{2}-{3}_{4}_{5}_{6}.log",
                serviceId,
                tl.Year,
                tl.Month.ToString("D2"),
                tl.Day.ToString("D2"),
                tl.Hour.ToString("D2"),
                tl.Minute.ToString("D2"),
                tl.Second.ToString("D2"));

            return Path.Combine(dir, name);

        }
        public virtual void AppendLog(Exception ex, string message = "Local Server Error")
        {
            if (LogToFile == null)
                return;

            List<string> lst = new List<string>();

            SubmitResult res = new SubmitResult(1, message);

            res.SetException(ex);
            lst.Add(" ");
            lst.Add("--------------------------------------------------------------------");
            lst.Add(" ");
            lst.Add(res.Message);
            lst.Add(res.ExceptionMessage);
            lst.Add(" ");
            lst.Add("--------------------------------------------------------------------");
            lst.Add(" ");

            lst.AddRange(res.StackTrace);

            File.AppendAllLines(LogToFile, lst);
        }

        public virtual void AppendLog(string url, HttpStatusCode code, string responseBody, object sent = null, int requestNo = 0, string requestActioName = "", string requestType = "",string logType="")
        {
            if (LogToFile == null)
                return;

            List<string> lst = new List<string>();
            lst.Add(" ");
            lst.Add("--------------------------------------------------------------------");
            lst.Add(" ");
            lst.Add("Request : " + url);

            if (sent != null)
            {
                lst.Add(JsonConvert.SerializeObject(sent, Formatting.Indented));
            }

            lst.Add("Response Status : " + code.ToString() + " " + ((int)code));
            lst.Add("Response Message :");
            lst.Add(responseBody);
            lst.Add(" ");
            lst.Add("--------------------------------------------------------------------");
            lst.Add(" ");

            File.AppendAllLines(LogToFile, lst);
        }

        public virtual void AppendLog(string url, IEnumerable<FileData> data, HttpStatusCode code, string responseBody)
        {
            if (LogToFile == null)
                return;

            List<string> lst = new List<string>();
            lst.Add(" ");
            lst.Add("--------------------------------------------------------------------");
            lst.Add(" ");
            lst.Add("Request : " + url);

            foreach (var up in data)
                lst.Add("File Path : " + up.FullPath);

            lst.Add("Response Status : " + code.ToString() + " " + ((int)code));
            lst.Add("Response Message :");
            lst.Add(responseBody);
            lst.Add(" ");
            lst.Add("--------------------------------------------------------------------");
            lst.Add(" ");

            File.AppendAllLines(LogToFile, lst);
        }

        public virtual void AppendException(string url, Exception ex, object sent = null)
        {
            if (LogToFile == null)
                return;

            List<string> lst = new List<string>();
            lst.Add(" ");
            lst.Add("--------------------------------------------------------------------");
            lst.Add(" ");
            lst.Add("Request : " + url);
            if (sent != null)
            {
                lst.Add(JsonConvert.SerializeObject(sent, Formatting.Indented));
            }
            lst.AddRange(GetExceptionLog(ex));
            lst.Add(" ");
            lst.Add("--------------------------------------------------------------------");
            lst.Add(" ");
            File.AppendAllLines(LogToFile, lst);
        }

        private List<string> GetExceptionLog(Exception ex)
        {
            List<string> lst = new List<string>();
            lst.Add(ex.GetType().FullName);
            lst.Add(ex.Message);
            lst.Add(ex.StackTrace);

            if (ex.InnerException != null)
            {
                lst.AddRange(GetExceptionLog(ex.InnerException));
            }
            return lst;
        }

        #endregion


        protected Uri AppendQuery(Uri uri, object obj)
        {

            PropertyInfo[] infs = obj.GetType().GetProperties();

            List<string> data = new List<string>();
            foreach (PropertyInfo inf in infs)
            {
                var val = inf.GetValue(obj);
                if (val != null)
                    data.Add(inf.Name + "=" + val.ToString());
            }
            string ur = uri.AbsoluteUri + "?" + string.Join("&", data);
            return new Uri(ur);
        }

        protected virtual void SetSecurityProtocol() { }

        protected Uri GetUri(string url, object query = null)
        {

            Uri uri = new Uri(CombineUrl(BaseUrl, url));

            if (query != null)
                uri = AppendQuery(uri, query);

            if (Headers.Count > 0)
            {
                foreach (var h in Headers)
                    Client.DefaultRequestHeaders.TryAddWithoutValidation(h.Key, h.Value);
            }
            SetSecurityProtocol();
            return uri;
        }

        string CombineUrl(params string[] parts)
        {
            string ret = "";
            for (var i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                if (i < (parts.Length - 1))
                {
                    char last = part[part.Length - 1];
                    part += (last != '/') ? "/" : "";
                }
                ret += part;
            }
            return ret;
        }

        public async Task<HttpResponseMessage> Post<T>(string url, T data, object query = null, int requestNo = 0) where T : class
        {
            Client = new HttpClient();

            Uri uri = GetUri(url, query);

            try
            {

                HttpResponseMessage mes = await Client.PostAsJsonAsync(uri, data);
                
                    string res = await mes.Content.ReadAsStringAsync();
                if (mes.IsSuccessStatusCode)
                {
                    AppendLog(uri.AbsoluteUri, mes.StatusCode, res, data, requestNo, url, "Post");
                }
                else
                {
                    AppendLog(uri.AbsoluteUri, mes.StatusCode, res, data, requestNo, url, "Post", logType: "Exception");
                }

                return new HttpResponseMessage() { StatusCode = mes.StatusCode, Content = mes.Content };
            }
            catch (Exception ex)
            {
               
                    AppendException(uri.AbsoluteUri, ex, data);
               
                return ExceptionToResponse(ex);
            }
        }



        public async Task<HttpResponseMessage> Put<T>(string url, T data, object query = null, int requestNo = 0) where T : class
        {
            Client = new HttpClient();

            Uri uri = GetUri(url, query);

            try
            {

                HttpResponseMessage mes = await Client.PutAsJsonAsync(uri, data);
                
                    string res = await mes.Content.ReadAsStringAsync();
                if (mes.IsSuccessStatusCode)
                {
                    AppendLog(uri.AbsoluteUri, mes.StatusCode, res, data, requestNo, url, "Put");
                }
                else
                {
                    AppendLog(uri.AbsoluteUri, mes.StatusCode, res, data, requestNo, url, "Put",logType: "Exception");
                }
              
                return new HttpResponseMessage() { StatusCode = mes.StatusCode, Content = mes.Content };
            }
            catch (Exception ex)
            {
                
                    AppendException(uri.AbsoluteUri, ex, data);
              
                return ExceptionToResponse(ex);
            }
        }

        public async Task<HttpResponseMessage> Get(string url, object query = null, int requestNo = 0)
        {
            Client = new HttpClient();

            Uri uri = GetUri(url, query);
            Client.Timeout = new TimeSpan(0, 15, 60);
            try
            {

                HttpResponseMessage mes = await Client.GetAsync(uri);

           
                    string res = await mes.Content.ReadAsStringAsync();
                if (mes.IsSuccessStatusCode)
                {
                    AppendLog(uri.AbsoluteUri, mes.StatusCode, res, null, requestNo, url, "Post");
                }
                else
                {
                    AppendLog(uri.AbsoluteUri, mes.StatusCode, res, null, requestNo, url, "Post", logType: "Exception");

                }
               
               
                return new HttpResponseMessage() { StatusCode = mes.StatusCode, Content = mes.Content };
            }
            catch (Exception ex)
            {
                
                    AppendException(uri.AbsoluteUri, ex);
               
                return ExceptionToResponse(ex);
            }

        }

        public async Task<FileBytes> DownloadFileAsync(string url, object query = null)
        {
            HttpResponseMessage mes = await Get(url, query);
            if (mes.IsSuccessStatusCode)
            {
                FileBytes b = new FileBytes();
                b.Bytes = await mes.Content.ReadAsByteArrayAsync();

                b.FileName = url.GetAfter("/");

                if (mes.Content.Headers.ContentDisposition != null)
                    b.FileName = mes.Content.Headers.ContentDisposition.FileName;

                if (mes.Content.Headers.ContentType != null)
                    b.MimeType = mes.Content.Headers.ContentType.MediaType;
                else
                    b.MimeType = MimeData.GetFileMimeType(b.FileName);

                return b;
            }
            else
            {
                return null;
            }
        }

        public FileBytes DownloadFile(string url, object query = null)
        {
            Task<FileBytes> obj = Task.Run(() => DownloadFileAsync(url, query));
            Task.WaitAll(obj);
            return obj.Result;
        }


        private MultipartFormDataContent ConvertToMultipartContent(IEnumerable<FileData> files)
        {
            var content = new MultipartFormDataContent();


            foreach (FileData fil in files)
            {
                byte[] byts = File.ReadAllBytes(fil.FullPath);
                MemoryStream str = new MemoryStream(byts);
                StreamContent cont = new StreamContent(str);
                cont.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "\"" + fil.FieldName + "\"",
                    FileName = "\"" + fil.FileName + "\""
                };
                content.Add(cont, fil.FieldName, fil.FileName);
            }

            return content;

        }
        public async Task<HttpResponseMessage> PostFiles(string url, IEnumerable<FileData> files, object query = null)
        {
           
            Client = new HttpClient();
            Uri uri = GetUri(url, query);

            try
            {
                var content = ConvertToMultipartContent(files);
                
                HttpResponseMessage mes = await Client.PostAsync(uri, content);
                mes.EnsureSuccessStatusCode();
                string res = await mes.Content.ReadAsStringAsync();
               
                    AppendLog(uri.AbsoluteUri, files, mes.StatusCode, res);
    
                return new HttpResponseMessage { StatusCode = mes.StatusCode, Content = mes.Content };
            }
            catch (Exception ex)
            {
               
                    AppendException(uri.AbsoluteUri, ex);
               
                return ExceptionToResponse(ex);
            }

        }

        public async Task<HttpResponseMessage> PutFiles(string url, IEnumerable<FileData> files, object query = null)
        {
            Client = new HttpClient();
            Uri uri = GetUri(url, query);

            try
            {
                var content = ConvertToMultipartContent(files);
                HttpResponseMessage mes = await Client.PutAsync(uri, content);
                mes.EnsureSuccessStatusCode();
                string res = await mes.Content.ReadAsStringAsync();
              
                    AppendLog(uri.AbsoluteUri, files, mes.StatusCode, res);
                return new HttpResponseMessage { StatusCode = mes.StatusCode, Content = mes.Content };
            }
            catch (Exception ex)
            {
               
                    AppendException(uri.AbsoluteUri, ex);
               
                return ExceptionToResponse(ex);
            }

        }

        protected HttpResponseMessage ExceptionToResponse(Exception ex)
        {
            HttpResponseMessage mes = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
            mes.Content = new StringContent(ex.Message);
            return mes;
        }
    }
}
