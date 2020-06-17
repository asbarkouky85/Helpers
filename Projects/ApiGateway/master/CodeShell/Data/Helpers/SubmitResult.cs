using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeShell.Data.Helpers
{
    public class SubmitResult
    {
        private Dictionary<string, object> _data;
        public Dictionary<string, object> Data
        {
            get
            {
                if (_data == null)
                    _data = new Dictionary<string, object>();
                return _data;
            }
        }
        public int Code { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string[] StackTrace { get; set; }
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
            ExceptionMessage = GetMessage(e);
            if (e.StackTrace != null)
                StackTrace = e.StackTrace.Split('\r', '\n').Where(d => d.Length > 0).ToArray();
            else
                StackTrace = new string[] { };
        }

        private string GetMessage(Exception ex)
        {
            string message = ex.Message;
            if (ex.InnerException != null)
                message += "\n" + GetMessage(ex.InnerException);
            return message;
        }

        public SubmitResult()
        {
            Code = 0;
            AffectedRows = 0;
            Message = "No Changes";
        }

        public SubmitResult(int code, string message = "No Changes")
        {
            Code = code;
            Message = message;
        }

        public static SubmitResult FromChangeResult(ChangeResult res)
        {
            SubmitResult ret = new SubmitResult();
            if (!res.Success)
                ret.Code = 1;

            ret.Message = res.Message;
            ret.AffectedRows = res.AffectedRows;
            ret.EntityId = res.EntityId;
            return ret;
        }
    }
}
