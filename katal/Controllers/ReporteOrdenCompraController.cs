using katal.conexion.model.entity;
using katal.conexion.model.neg;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;

namespace katal.Controllers
{
    public class ReporteOrdenCompraController : BaseController
    {

        private ProveedorNeg proveedorNeg;
        private EstadoDocNeg estadoDocNeg;


        public ReporteOrdenCompraController()
        {
            proveedorNeg = new ProveedorNeg(codEmpresa);
            estadoDocNeg = new EstadoDocNeg(codEmpresa);
        }
        // GET: ReporteOrdenCompra
        public ActionResult Index()
        {
            return View();
        }

        private string ValidarRecuperar(string data)
        {
            string respuesta = "";
            string ver = HttpUtility.HtmlDecode(data);
            if (ver != null)
            {
                Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                // Array codigo = nodes["selectedKeyValues"] ;
                if (nodes.selectedKeyValues != null)
                {
                    respuesta = nodes.selectedKeyValues[0];
                }
            }
            return respuesta;
        }

        [HttpPost]
        public ActionResult cargarReporte(FormCollection data)
        {

            string prov1 = ValidarRecuperar(data["gridLookupProveedor$State"]);
            string prov2 = ValidarRecuperar(data["gridLookupProveedor2$State"]);
            string esta1 = ValidarRecuperar(data["gridLookupEstadoDoc$State"]);
            string esta2 = ValidarRecuperar(data["gridLookupEstadoDoc2$State"]);


            //GridViewHelper.dateRange.End =DateTime.Parse(  Request.Params["End"]);
            //GridViewHelper.dateRange.Start = DateTime.Parse(Request.Params["Start"]);


            if (Request.Params["Submit"] == null)
                ModelState.Clear();
            else
            {
                // GridViewHelper.dateRange.End = DateTime.Parse(Request.Params["End"]);
                //GridViewHelper.dateRange.Start = DateTime.Parse(Request.Params["Start"]);
            }


            return RedirectToAction("OrdenesCompraProveedor", "Report", new { codigo = "014", star = DateTime.Parse(Request.Params["Start"]), end = DateTime.Parse(Request.Params["End"]), estado1 = esta1, estado2 = esta2, PROV1 = prov1, PROV2 = prov2, });
        }


        public ActionResult MultiSelectProveedor(string CurrentCategory)
        {
            ViewData["Proveedores"] = proveedorNeg.findAll();
            if (CurrentCategory == null)
                CurrentCategory = "-1";
            return PartialView(new Proveedor() { PRVCCODIGO = CurrentCategory });
        }
        public ActionResult MultiSelectProveedor2(string CurrentCategory)
        {
            ViewData["Proveedores"] = proveedorNeg.findAll();
            if (CurrentCategory == null)
                CurrentCategory = "-1";
            return PartialView(new Proveedor() { PRVCCODIGO = CurrentCategory });
        }
        public ActionResult MultiSelectEstadoDoc(string CurrentCategory)
        {
            ViewData["EstadoDoc"] = estadoDocNeg.findAll();
            if (CurrentCategory == null)
                CurrentCategory = "-1";
            return PartialView(new EstadoDoc() { est_codigo = CurrentCategory });
        }
        public ActionResult MultiSelectEstadoDoc2(string CurrentCategory)
        {
            ViewData["EstadoDoc"] = estadoDocNeg.findAll();
            if (CurrentCategory == null)
                CurrentCategory = "-1";
            return PartialView(new EstadoDoc() { est_codigo = CurrentCategory });
        }
    }
}