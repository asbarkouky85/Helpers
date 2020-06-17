using CodeShellCore.Services;
using CodeShellCore.Services.Email;
using CodeShellCore.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Monitor.UI
{
    public class MonitorShell : WebShell
    {

        public MonitorShell(IConfiguration config) : base(config)
        {
        }

        protected override bool useLocalization => false;

        protected override CultureInfo defaultCulture => new CultureInfo("en");

        public override void RegisterServices(IServiceCollection coll)
        {
            base.RegisterServices(coll);
            coll.AddTransient<EmailService>();
            coll.AddTransient<WriterService>();
            var s = JsonConvert.SerializeObject(new FailureMessage
            {
                AppName = "[[LIVENESS]]",
                Description = "[[DESCRIPTIONS]]",
                Failure = "[[FAILURE]]"
            });
            var conf = getConfig("NotificationUrl");

            coll.AddHealthChecksUI("healthchecksdb", d => d.AddWebhookNotification("hii", conf.Value, s));
        }

        public override void ConfigureHttp(IApplicationBuilder app, IHostingEnvironment env)
        {
            base.ConfigureHttp(app, env);
            app.UseHealthChecksUI(config =>
            {
                config.UIPath = "/hc-ui";
            });

        }

        protected override void OnReady()
        {
            base.OnReady();
            
        }
    }
}
