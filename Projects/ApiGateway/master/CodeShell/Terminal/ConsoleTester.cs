using CodeShell.Data.Helpers;
using CodeShell.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Terminal
{
    public abstract class ConsoleTester
    {
        protected string CurrentMethod { get; set; }
        protected virtual void OnMethodSelected(string name) { }
        Dictionary<Type, ServiceBase> services;

        protected T GetService<T>() where T : ServiceBase
        {
            if (services == null)
                services = new Dictionary<Type, ServiceBase>();

            ServiceBase service;

            if (!services.TryGetValue(typeof(T), out service))
            {
                service = Activator.CreateInstance<T>();
                services[typeof(T)] = service;
            }
            return (T)service;
        }

        public abstract Dictionary<int, string> Functions { get; }
        protected void GetSelectionFromUser(string name, Dictionary<int, string> functions)
        {
            while (true)
            {
                string data = GenerateChoices(functions);
                Console.Write(string.Format(@"
Select a {0} from the List

    {1}

Enter Your Choice : ", name, data));
                int function = 0;
                string l = Console.ReadLine();
                if (!int.TryParse(l, out function) || function > Functions.Count || function < 1)
                {
                    Console.WriteLine("Invalid Choice");
                    continue;
                }

                MethodInfo info = GetType().GetMethod(Functions[function]);
                OnMethodSelected(Functions[function]);
                CurrentMethod = Functions[function];

                try
                {

                    info.Invoke(this, new object[] { });
                }
                catch (Exception ex)
                {

                    SubmitResult res = new SubmitResult(1);
                    res.SetException(ex);
                    Console.WriteLine(res.Message);
                    Console.WriteLine(res.ExceptionMessage);
                    foreach (string s in res.StackTrace)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
        }

        protected int GetIntFromUser(string message, int min = 1, int max = 0)
        {
            while (true)
            {
                Console.Write(message);
                int termId = 0;
                string l = Console.ReadLine();
                if (!int.TryParse(l, out termId) || (termId > max && max != 0) || termId < min)
                {
                    Console.WriteLine("Invalid Choice");
                    continue;
                }

                return termId;
            }

        }

        protected string GenerateChoices(Dictionary<int, string> choices)
        {
            string st = "";
            foreach (var c in choices)
            {
                st += string.Format("\t{0}. {1}\n", c.Key, c.Value);
            }
            return st;
        }

        public void Run()
        {

            GetSelectionFromUser("Function", Functions);
        }
    }
}
