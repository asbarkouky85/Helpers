using CodeShellCore.Web;
using Microsoft.Extensions.Configuration;

namespace $safeprojectname$
{
    public class Startup : ShellStartup<ConfigShell>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
