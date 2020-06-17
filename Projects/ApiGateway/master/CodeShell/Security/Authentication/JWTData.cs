using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Security.Authentication
{
    public class JWTData
    {
        public object UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public string Provider { get; set; }

        public bool IsValid(string provider)
        {
            return ExpireTime > DateTime.Now && Provider == provider;
        }
    }
}
