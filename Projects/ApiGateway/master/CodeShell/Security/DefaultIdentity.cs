using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Security
{
    public class DefaultIdentity : IIdentity
    {
        public string AuthenticationType { get; set; }

        public bool IsAuthenticated
        {
            get
            {
                return true;
            }
        }

        public string Name { get; set; }
    }
}
