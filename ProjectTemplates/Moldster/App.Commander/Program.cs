using CodeShellCore.Cli;
using $safeprojectname$.Cli;
using System;

namespace $safeprojectname$
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleShell.Start<MainController>(new CommanderShell());
        }
    }
}
