using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CodeShell.Helpers
{
    public class SW
    {
        public Stopwatch Stopwatch { get; set; }
        static int index = 0;

        private SW()
        {
            Stopwatch = new Stopwatch();
        }

        public static TimeConsumption Measure(string logName = null)
        {
            logName = (logName == null) ? (++index).ToString("D3") : logName;
            SW w = new SW();
            return new TimeConsumption(w, logName);
        }
    }

    public class TimeConsumption : IDisposable
    {
        SW Parent;
        string logName;
        public TimeConsumption(SW watch, string name)
        {
            Parent = watch;
            logName = name;
            watch.Stopwatch.Start();
        }

        public void Dispose()
        {
            Parent.Stopwatch.Stop();
            Console.WriteLine(logName + " - " + Parent.Stopwatch.Elapsed.TotalMilliseconds.ToString("F3"));
            Parent.Stopwatch.Reset();
        }
    }
}

