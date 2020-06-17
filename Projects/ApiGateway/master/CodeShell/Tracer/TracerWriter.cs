using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace CodeShell.Tracer
{
    public class TracerWriter : TextWriter
    {
        private string _FilePath;
        public string FilePath {
            get 
            {
                return _FilePath;
            }
            set 
            {
                if (!File.Exists(value))
                {
                    FileStream st = File.Create(value);
                    st.Close();
                }
                _FilePath = value;
            }
        }
        public delegate void OnFishishedWriting(int count);
        public event OnFishishedWriting AfterWriting;
        
        public List<string> WritingBuffer;
        public bool Writing = false;

        string CurrentTimeString
        {
            get
            {
                DateTime now = DateTime.Now;
                return string.Format("[{0}:{1}:{2}.{3}]",
                    now.Hour.ToString("D2"),
                    now.Minute.ToString("D2"),
                    now.Second.ToString("D2"),
                    now.Millisecond.ToString("D3")
                    );
            }
        }

        public TracerWriter(string fileName)
        {
            FilePath = fileName;
            WritingBuffer = new List<string>();
        }

        public void ChangeFile(string fileName) 
        {
            FilePath = fileName;
        }

        public int CountLines() 
        {
            int lines = 0;
            lock (this)
            {
                lines=File.ReadAllLines(FilePath).Length;
            }
            return lines;
        }

        public void Append(string txt) 
        {
            lock (this)
            {
                if (Writing)
                    Monitor.Wait(this);
                
                WritingBuffer.Add(txt);
                Monitor.Pulse(this);
            }
        }

        public override void WriteLine(string value)
        {
            value = CurrentTimeString + " " + value;
            Console.WriteLine(value);
            LogText txt = new LogText(value, this);
            Thread th = new Thread(txt.AppendFunction);
            th.IsBackground = true;
            th.Start();
        }

        public void WritingFunction() 
        {
            while (true) 
            {
                lock (this)
                {
                    if (WritingBuffer.Count == 0)
                        Monitor.Wait(this);
                    
                    Writing = true;
                    Monitor.Pulse(this);
                    try
                    {
                        using (StreamWriter wt = new StreamWriter(FilePath, true))
                        {
                            foreach (string st in WritingBuffer)
                            {
                                wt.WriteLine(st);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    
                    if (AfterWriting != null)
                        AfterWriting(WritingBuffer.Count);

                    WritingBuffer = new List<string>();
                    Writing = false;
                    Monitor.Pulse(this);
                }
            }
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }
}
