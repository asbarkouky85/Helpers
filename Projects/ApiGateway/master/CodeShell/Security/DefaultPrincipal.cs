using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Security
{
    public class DefaultPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public string Name { get; set; }

        public DefaultPrincipal(string name)
        {
            Name = name;
            Identity = new DefaultIdentity { Name = name };
        }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}
