using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace katal.Controllers
{
    public class ReporteOrdenCompraController : Controller
    {
        // GET: ReporteOrdenCompra
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult cargarReporte(FormCollection data)
        {
            if (Request.Params["Submit"] == null)
                ModelState.Clear();
            else
            {
               // GridViewHelper.dateRange.End = DateTime.Parse(Request.Params["End"]);
                //GridViewHelper.dateRange.Start = DateTime.Parse(Request.Params["Start"]);
            }

            return RedirectToAction("index");
        }

    }
}