﻿using System;
using System.Web.Mvc;

namespace katal.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index(String codigo)
        {
            Reports.ReportOrdenCompra report = new Reports.ReportOrdenCompra();
            report.Parameters["parameter1"].Value = codigo;
            report.RequestParameters = false;
            return View(new DevExpress.XtraReports.Web.CachedReportSourceWeb(report));
        }
        public ActionResult Requerimiento(String codigo)
        {
            Reports.Requisicion report = new Reports.Requisicion();
            report.Parameters["parameter0"].Value = codigo;
            report.Parameters["parameter1"].Value = codigo;
            report.RequestParameters = false;
            return View(new DevExpress.XtraReports.Web.CachedReportSourceWeb(report));
        }

        public ActionResult MovimientoBanco(String codigo)
        {
            Reports.MOVIMIENTO report = new Reports.MOVIMIENTO();
            report.Parameters["SECUENCIA01"].Value = codigo;

            report.RequestParameters = false;
            return View(new DevExpress.XtraReports.Web.CachedReportSourceWeb(report));
        }

        public ActionResult OrdenesCompraProveedor(String codigo, DateTime star, DateTime end, string estado1, string estado2, string PROV1, string PROV2)
        {
            Reports.Report5 report = new Reports.Report5();


            // report.Parameters["empre"].Value = codigo;
            report.Parameters["ESTADO1"].Value = estado1;
            report.Parameters["ESTADO2"].Value = estado2;
            report.Parameters["PROV1"].Value = PROV1;
            report.Parameters["PROV2"].Value = PROV2;
            report.Parameters["star"].Value = star;
            report.Parameters["end"].Value = end;

            report.RequestParameters = false;
            return View(new DevExpress.XtraReports.Web.CachedReportSourceWeb(report));
        }


    }
}