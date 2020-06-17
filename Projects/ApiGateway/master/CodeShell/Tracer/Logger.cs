using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;

namespace CodeShell.Tracer
{
    public class Logger
    {
        public static TracerWriter Out
        {
            get
            {
                if (_instance != null)
                    return _instance._TracerWriter;
                return null;
            }
        }

        public TracerWriter TextWriter { get { return _TracerWriter; } }

        private static Dictionary<string, FileLocation> _FileLocations;
        private static Logger _instance;
        private TracerWriter _TracerWriter;
        private int _LineCount;
        private Thread WritingThread;
        private string _FilePath;
        private string _FolderPath;
        private string App;

        private string DefaultFolder { get { return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\logs"; } }

        private static XmlSerializer Serializer { get { return new XmlSerializer(typeof(LocationCollection)); } }

        private static string LocationsFilePath { get { return Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\fileLocations"; } }

        private static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Logger("TraceLog");
                }
                return _instance;
            }
        }

        public static Dictionary<string, FileLocation> FileLocations
        {
            get
            {
                if (_FileLocations == null)
                {

                    _FileLocations = new Dictionary<string, FileLocation>();
                    LocationCollection coll = GetCollection();

                    if (coll != null)
                    {
                        foreach (FileLocation loc in coll.Locations)
                            _FileLocations[loc.AppName] = loc;

                    }
                    else
                    {
                        SaveLocations();
                    }
                }
                return _FileLocations;
            }
        }

        private TimeSpan DeleteLimit { get { return new TimeSpan(DaysToKeepFiles, 0, 0, 0); } }

        [DefaultValue(7)]
        public int DaysToKeepFiles { get; set; }

        public int LineCount { get { return _LineCount; } }

        public string FolderPath { get { return _FolderPath; } }

        public string FileName { get { return (new System.IO.FileInfo(_FilePath)).Name; } }
		
		public static void WriteLine(object ob) 
        {
            Instance._WriteLine(ob.ToString());
        }

        public static void WriteLine(string st)
        {
            Instance._WriteLine(st);
        }

        public static void WriteException(Exception e, string st = null)
        {
            Instance._WriteException(e, st);
        }

        public void WriteLogLine(string st)
        {
            _WriteLine(st);
        }

		
        public static string CombinePaths(params string[] args)
        {
            string fin = "";
            for (int i = 0; i < args.Length; i++)
            {
                string added = args[i].Replace("\\", "/");

                if (i != (args.Length - 1) && added[added.Length - 1] != '/')
                    added += "/";

                if (i != 0 && added[0] == '/')
                    added = added.Substring(1);

                fin += added;
            }
            return fin;
        }

        public void WriteLogException(Exception ex, string st = null)
        {
            _WriteException(ex, st);
        }

        private Logger(string app, string folder = null)
        {
            App = app;

            if (folder != null && folder[0] == '.') 
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                string removeDot = folder.Substring(1);
                folder = CombinePaths(path, removeDot);
            }

            if (FileLocations.ContainsKey(app))
            {
                if (folder != null && FileLocations[app].FolderPath != folder)
                {
                    FileLocations[app].FolderPath = folder;
                    FileLocations[app].FilePath = CombinePaths(folder, NewFileName());
                    SaveLocations();
                }
                _FolderPath = FileLocations[app].FolderPath;
                _FilePath = FileLocations[app].FilePath;
            }
            else
            {
                if (folder != null)
                {
                    _FolderPath = folder;
                    _FilePath = CombinePaths(_FolderPath, NewFileName());
                    FileLocations[App] = FileLocation(_FolderPath, _FilePath);
                    SaveLocations();
                }
                else
                {
                    _FolderPath = DefaultFolder;
                    _FilePath = CombinePaths(_FolderPath, NewFileName());
                }

            }
            try
            {
                if (!Directory.Exists(_FolderPath))
                    Directory.CreateDirectory(_FolderPath);
                StartFile();
            }
            catch { }

        }

        FileLocation FileLocation(string folderPath, string filePath)
        {
            FileLocation loc = new FileLocation();
            loc.AppName = App;
            loc.FolderPath = folderPath;
            loc.FilePath = filePath;
            return loc;
        }

        public static Logger Create(string AppName, string logFolder)
        {
            return new Logger(AppName, logFolder);
        }

        public static void Set(string AppName, string logFolder)
        {
            _instance = new Logger(AppName, logFolder);
        }

        private string NewFileName()
        {
            DateTime t = DateTime.Now;
            string fileName = t.Year
                + "-" + t.Month.ToString("D2")
                + "-" + t.Day.ToString("D2")
                + "_" + t.Hour.ToString("D2")
                + "-" + t.Minute.ToString("D2")
                + "-" + t.Second.ToString("D2")
                + ".log";
            return fileName;
        }

        void StartFile()
        {
            _TracerWriter = new TracerWriter(_FilePath);
            _LineCount = _TracerWriter.CountLines();

            _TracerWriter.AfterWriting += FileObj_AfterWriting;

            WritingThread = new Thread(_TracerWriter.WritingFunction);
            WritingThread.Name = "LoggerThread";
            WritingThread.IsBackground = true;
            WritingThread.Start();
        }

        void FileObj_AfterWriting(int count)
        {
            _LineCount += count;
            if (LineCount >= 4000)
            {
                _FilePath = CombinePaths(_FolderPath, NewFileName());
                _TracerWriter.ChangeFile(_FilePath);

                FileLocations[App].FilePath = _FilePath;
                SaveLocations();

                _LineCount = 0;
                Cleanup();
            }
        }

        void Cleanup()
        {
            try
            {
                foreach (string file in Directory.GetFiles(FolderPath))
                {
                    FileInfo inf = new FileInfo(file);
                    if ((DateTime.Now - inf.CreationTime) >= DeleteLimit)
                    {
                        File.Delete(file);
                    }
                }
            }
            catch { }
        }



        private static LocationCollection GetCollection()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(LocationCollection));

                if (File.Exists(LocationsFilePath))
                {
                    FileStream st = File.OpenRead(LocationsFilePath);
                    LocationCollection locs = ser.Deserialize(st) as LocationCollection;
                    st.Close();
                    return locs;
                }
            }catch{}
            return null;
        }

        static void SaveLocations()
        {
            LocationCollection locs = new LocationCollection();
            locs.Locations = (from c in _FileLocations select c.Value).ToArray<FileLocation>();
            if (locs.Locations == null)
                locs.Locations = new FileLocation[] { };

            File.WriteAllText(LocationsFilePath, "");
            FileStream str = File.OpenWrite(LocationsFilePath);
            Serializer.Serialize(str, locs);
            str.Close();
        }

        void _WriteLine(string st)
        {
            _TracerWriter.WriteLine(st);
        }

        void _WriteException(Exception e, string st = null)
        {
            if (st != null)
            {
                WriteLine(st);
            }
            WriteLine(e.Message);
            WriteLine("Exception Type : " + e.GetType().Name);
            string[] arr = e.StackTrace.Split(new char[] { '\n' });

            foreach (string str in arr)
            {
                WriteLine(str.Replace('\r', ' '));
            }

        }
    }
}
