using CodeShellCore.Files.Logging;
using CodeShellCore.Services;
using CodeShellCore.Services.Email;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmailService emailService;
        private readonly WriterService serv;

        public HomeController(EmailService emailService, WriterService serv)
        {

            this.emailService = emailService;
            this.serv = serv;
        }
        public IActionResult Index()
        {
            return Redirect("/hc-ui");
        }

        [Route("Failure")]
        public IActionResult Failure([FromBody]FailureMessage message)
        {
            //emailService.SendEmail()
            var sendTo = CodeShellCore.Shell.GetConfigAs<string>("NotifyList");
           
            Logger.WriteLine($"Failure recorded in api {message.AppName}");
            if (sendTo != null)
            {
                string tempate = System.IO.File.ReadAllText("./EmailFormat.html");
                tempate = serv.FillStringParameters(tempate, message);
                var em = emailService.SendEmail(sendTo, $"Failure [{message.AppName}]", tempate, true, "Service Monitor");
                if (em.IsSuccess)
                {
                    Logger.WriteLine("Email sent to " + sendTo);
                }
                else
                {
                    Logger.WriteLine("Email Sending failed");
                    Logger.WriteLine(em.ExceptionMessage);
                }
            }

            return Content(Request.QueryString.Value);
        }
    }
}
