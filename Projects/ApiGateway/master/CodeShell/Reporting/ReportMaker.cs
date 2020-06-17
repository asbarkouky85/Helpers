using Microsoft.Reporting.WebForms;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CodeShell.Reporting
{
    public abstract class ReportMaker
    {
        public LocalReport LocalReport { get; set; }
        public DeviceInfoXML DeviceInfo { get; private set; }
        protected Dictionary<string, IEnumerable> _datasources;
        protected ReportTypes reportType;
        public Dictionary<string, IEnumerable> DataSources
        {
            get
            {
                if (_datasources == null)
                    _datasources = new Dictionary<string, IEnumerable>();
                return _datasources;
            }
        }
        public ReportMaker(string reportFileName, ReportTypes type)
        {
            LocalReport = new LocalReport();
            
            reportType = type;
            string path = Path.Combine(Shell.ReportsRoot, reportFileName + ".rdlc");
            if (!File.Exists(path))
                throw new FileNotFoundException("File Not Found", path);

            LocalReport.ReportPath = path;
            
            DeviceInfo = DeviceInfoXML.Make(type);
        }

        

        public void SetPortrait()
        {
            DeviceInfo.PageHeight = 11.69;
            DeviceInfo.PageWidth = 8.27;
        }

        public void SetLandScape()
        {
            DeviceInfo.PageWidth = 11.69;
            DeviceInfo.PageHeight = 8.27;
        }
    }
}
