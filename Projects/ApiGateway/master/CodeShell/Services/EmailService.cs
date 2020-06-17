using CodeShell.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Services
{
    public abstract class EmailService : ServiceBase
    {
        public abstract SmtpConfig Config { get; }

        public  void SendEmail(string To, string Subject, string MsgBody)
        {
            string fromEmail = Config.SmtpUser;

            SmtpClient SmtpServer = new SmtpClient(Config.SmtpHost);

            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            SmtpServer.Credentials = new NetworkCredential(fromEmail, Config.SmtpPassword);
            SmtpServer.Port = Config.SmtpPort;
            SmtpServer.EnableSsl = true;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(To);
            mail.Subject = Subject;
            mail.Body = MsgBody;
            SmtpServer.Send(mail);
        }
    }
}
 