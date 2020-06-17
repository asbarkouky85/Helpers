using CodeShell.Files;
using CodeShell.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CodeShell.Web.Controllers
{
    public class BaseFilesController : BaseApiController
    {
        protected FileService Service { get { return GetService<FileService>(); } }

        public async Task<IHttpActionResult> Upload()
        {
            MultipartMemoryStreamProvider prov = await Request.Content.ReadAsMultipartAsync();
            List<TmpFileData> lst = await Service.Upload(prov);
            return Ok(lst);
        }
    }
}
