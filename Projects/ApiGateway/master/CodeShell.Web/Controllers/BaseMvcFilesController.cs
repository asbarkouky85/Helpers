using CodeShell.Files;
using CodeShell.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CodeShell.Web.Controllers
{
    public abstract class BaseMvcFilesController : BaseMvcController
    {
        protected FileService Service { get { return GetService<FileService>(); } }

        public ActionResult Upload()
        {
            Dictionary<string, HttpPostedFileBase> dictionary = new Dictionary<string, HttpPostedFileBase>();
            foreach (string st in Request.Files.AllKeys)
            {
                dictionary[st]= Request.Files.Get(st);
            }
            return Json(Service.Upload(dictionary));
        }
    }
}
