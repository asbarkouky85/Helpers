using CodeShell.Business;
using CodeShell.Files;
using CodeShell.Services;
using CodeShell.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CodeShell.Web.Services
{
    public class FileService : ServiceBase
    {
        private static string _tmpRoot = null;
        protected virtual string TmpRoot
        {
            get
            {
                if (_tmpRoot == null)
                {
                    _tmpRoot = Path.Combine(Shell.AppRootPath, "Tmp");
                    if (!Directory.Exists(_tmpRoot))
                        Directory.CreateDirectory(_tmpRoot);
                }
                return _tmpRoot;
            }
        }

        

        public List<TmpFileData> Upload(Dictionary<string, HttpPostedFileBase> files)
        {
            List<TmpFileData> lst = new List<TmpFileData>();

            foreach (var d in files)
            {
                string name = d.Key;
                string path = Path.Combine(TmpRoot, name);

                using (MemoryStream str = new MemoryStream())
                {
                    d.Value.InputStream.CopyTo(str);
                    byte[] byts = str.ToArray();
                    if (byts.Length > 0)
                        File.WriteAllBytes(path, byts);

                    lst.Add(new TmpFileData
                    {
                        Url = "Tmp/" + name,
                        Size = byts.Length,
                        TmpPath = path,
                        UploadId = name,
                        Name = d.Value.FileName
                    });
                }
            }
            return lst;
        }

        public async Task<List<TmpFileData>> Upload(MultipartMemoryStreamProvider arr)
        {
            List<TmpFileData> lst = new List<TmpFileData>();
            foreach (HttpContent cont in arr.Contents)
            {
                string name = cont.Headers.ContentDisposition.Name.Replace("\"", "");
                string path = Path.Combine(TmpRoot, name);

                byte[] byts = await cont.ReadAsByteArrayAsync();
                if (byts.Length > 0)
                    File.WriteAllBytes(path, byts);

                lst.Add(new TmpFileData
                {
                    Url = "Tmp/" + name,
                    Size = byts.Length,
                    TmpPath = path,
                    UploadId = name,
                    Name = cont.Headers.ContentDisposition.FileName
                });
            }
            return lst;
        }
    }
}
