using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Monitor.UI
{
    public class Startup : CodeShellCore.Web.ShellStartup<MonitorShell>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
