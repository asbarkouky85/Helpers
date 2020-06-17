using System.Collections.Generic;

using CodeShellCore.Cli;
using CodeShellCore.Moldster.Cli;

namespace $safeprojectname$.Cli
{
    public class MainController : ConsoleController
    {
        public override Dictionary<int, string> Functions
        {
            get
            {
                return new Dictionary<int, string>
                {
                    { 1,"Modules"},
                    { 2,"Webpack"},
                    { 3,"Localization"},
                    { 4,"Builder"},
                    { 5,"SQL"}
                };
            }
        }

        public void Modules()
        {
            ModulesConsoleController c = new ModulesConsoleController();
            c.Run();
        }

        public void Webpack()
        {
            WebpackConsoleController c = new WebpackConsoleController();
            c.Run();
        }


        public void Localization()
        {
            LocalizationConsoleController c = new LocalizationConsoleController();
            c.Run();
        }

        public void Builder()
        {
            BuilderConsoleController c = new BuilderConsoleController();
            c.Run();
        }

        public void SQL()
        {
            SqlConsoleController c = new SqlConsoleController();
            c.Run();
        }
    }
}
