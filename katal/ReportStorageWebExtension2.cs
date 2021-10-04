using System.Collections.Generic;

namespace katal
{
    public class ReportStorageWebExtension2 : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        public override bool CanSetData(string url)
        {
            return base.CanSetData(url);
        }
        public override bool IsValidUrl(string url)
        {
            return base.IsValidUrl(url);
        }

        public override byte[] GetData(string url)
        {
            return base.GetData(url);
        }

        public override Dictionary<string, string> GetUrls()
        {
            return base.GetUrls();
        }

        public override void SetData(DevExpress.XtraReports.UI.XtraReport report, string url)
        {
            base.SetData(report, url);
        }

        public override string SetNewData(DevExpress.XtraReports.UI.XtraReport report, string defaultUrl)
        {
            return base.SetNewData(report, defaultUrl);
        }
    }
}
