using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Data.Helpers
{
    public class ChangeResult 
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object EntityId { get; set; }
        public int AffectedRows { get; set; }

        private Exception ex;
        public Exception GetException()
        {
            return ex;
        }

        public void SetException(Exception e)
        {
            ex = e;
        }

        public ChangeResult()
        {
            Success = true;
            AffectedRows = 0;
            Message = "No Changes";
        }
    }
}
