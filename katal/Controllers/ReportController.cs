using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace katal.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index(String codigo)
        {
            Reports.ReportOrdenCompra report = new Reports.ReportOrdenCompra();
            report.Parameters["parameter1"].Value =codigo;
            report.RequestParameters = false;
            return View(new DevExpress.XtraReports.Web.CachedReportSourceWeb(report));
        }
    }
}