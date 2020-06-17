using CodeShell.Reporting;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Web.Reporting
{
    public class WebReportMaker : ReportMaker
    {
        public WebReportMaker(string reportFileName, ReportTypes type) : base(reportFileName, type)
        {
        }

        public ReportResult GetResult()
        {
            foreach (var kvp in DataSources)
            {
                ReportDataSource src = new ReportDataSource(kvp.Key, kvp.Value);
                LocalReport.DataSources.Add(src);
            }

            ReportResult res = new ReportResult();
            res.MimeType = reportType.GetMime();
            res.Extension = reportType.GetExtension();

            res.Bytes = LocalReport.Render(reportType.GetString(), DeviceInfo.ToString());
            return res;
        }
    }
}
