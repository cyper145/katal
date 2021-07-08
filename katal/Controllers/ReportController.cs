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
            Reports.ReportOrden report = new Reports.ReportOrden();
            report.Parameters["parameter0"].Value =codigo;
            report.RequestParameters = false;
            return View(report);
        }
    }
}