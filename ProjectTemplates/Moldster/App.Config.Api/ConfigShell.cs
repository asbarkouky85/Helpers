using System.Globalization;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using CodeShellCore.Web.Razor;
using CodeShellCore.Moldster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace $safeprojectname$
{
    public class ConfigShell : CodeShellCore.Web.WebShell
    {
        protected override bool useLocalization { get { return false; } }

        protected override CultureInfo defaultCulture => new CultureInfo("en");

        public ConfigShell(IConfiguration config) : base(config)
        {
        }

        public override void RegisterServices(IServiceCollection coll)
        {
            base.RegisterServices(coll);

            coll.AddMoldsterWeb(MoldsType.Db);
            coll.AddMoldsterConfigurator(MoldsType.Db);
			coll.AddMoldsterServerGeneration();
            coll.AddMoldsterRazorHelpers();

            coll.AddCodeShellEmbeddedViews();
        }

        public override void AddMvcFeatures(IMvcBuilder mvc)
        {
            base.AddMvcFeatures(mvc);
            mvc.AddMoldsterConfiguratorControllers();
        }

        public override void ConfigureHttp(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMoldsterServerGeneration();
            base.ConfigureHttp(app, env);
        }

        protected override void OnReady()
        {
            base.OnReady();
            this.ConfigureAngular2Razor();
        }
    }
}
