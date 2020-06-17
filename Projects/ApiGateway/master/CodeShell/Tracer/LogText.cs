using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CodeShell.Tracer
{
    class LogText
    {
        public string Text { get; set; }
        public TracerWriter LFile { get; set; }

        public LogText(string txt,TracerWriter fi) 
        {
            Text = txt;
            LFile = fi;
        }

        public void AppendFunction() 
        {
            LFile.Append(Text);
        }
    }
}
