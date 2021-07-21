using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using katal.Code.Helpers;
using katal.conexion.model.entity;
using katal.conexion.model.neg;
using katal.Model;

namespace katal.Controllers
{
    public class RequisionCompraController : BaseController
    {
        private OrdenCompraNeg userNeg;
        private RequisicionCompraNeg requisicionNeg;
        private ProveedorNeg proveedorNeg;
        private ArticuloNeg articuloNeg;
        private AreaNeg areaNeg;
        private ResponsableCmpNeg responsable;
        private RequisicionCompra requisicionCompra;
        public RequisionCompraController()
        {
            requisicionNeg = new RequisicionCompraNeg();
            userNeg = new OrdenCompraNeg();
            articuloNeg = new ArticuloNeg();
            proveedorNeg = new ProveedorNeg();
            responsable = new ResponsableCmpNeg();
            areaNeg = new AreaNeg();


        }
        // GET: RequisionCompra
        public ActionResult Index()
        {

          
            return View(requisicionNeg.findAll(GridViewHelper.dateRange));
        }
        public ActionResult DataRequisicionPartial( )
        {
            GridViewHelper.ClearDetallesRequision();
            return PartialView("DataRequisicionPartial", requisicionNeg.findAll(GridViewHelper.dateRange));
        }

        [ValidateInput(false)]
        public ActionResult RequisicionAddNewPartial(RequisicionCompra product, FormCollection dataForm)
        {
            // obtener  codarticulo             
            var codArticulodata = dataForm["DXMVCEditorsValues"];

            string [] word = codArticulodata.Split(',');
            string dataRequisicion = codArticulodata[0]+word[11]+","+ word[14] + codArticulodata[codArticulodata.Length-1];
            Dictionary<string, JArray> nodes = JsonConvert.DeserializeObject<Dictionary<string, JArray>>(dataRequisicion);
            JArray solicitante = nodes["gridLookupSolicitante"];
            JArray area = nodes["gridLookupArea"];
            product.CODSOLIC = solicitante.First.ToString();
            product.AREA = area.First.ToString();
            product.TIPOREQUI = "RQ";
            product.NROREQUI = requisicionNeg.nextNroDocument();

            product.detalles = GridViewHelper.detalleRequisicions;

            ModelState.Remove("FecEntrega");
            if (ModelState.IsValid)
                SafeExecute(() => InsertProduct(product));
            else
            {
                if(!ModelState.IsValidField("FecEntrega") && ModelState.Count == 1)
                 {
                  
                    SafeExecute(() => InsertProduct(product));
                 }
                else
                {
                    ViewData["EditError"] = "Please, correct all errors.";
                }
             
            }
            return DataRequisicionPartial();
        }
        
        public void InsertProduct(RequisicionCompra product)
        {
            requisicionNeg.create(product);
        }

        [ValidateInput(false)]
        public ActionResult RequisicionUpdatePartial(RequisicionCompra product, FormCollection dataForm)
        {
            // obtener  codarticulo             
            var codArticulodata = dataForm["DXMVCEditorsValues"];

            string[] word = codArticulodata.Split(',');
            string dataRequisicion = codArticulodata[0] + word[13] + "," + word[16] + codArticulodata[codArticulodata.Length - 1];
            Dictionary<string, JArray> nodes = JsonConvert.DeserializeObject<Dictionary<string, JArray>>(dataRequisicion);
            JArray solicitante = nodes["gridLookupSolicitante"];
            JArray area = nodes["gridLookupArea"];
            product.CODSOLIC = solicitante.First.ToString();
            product.AREA = area.First.ToString();
            product.detalles = GridViewHelper.detalleRequisicions;
            ModelState.Remove("FecEntrega");
            if (ModelState.IsValid)
                SafeExecute(() => UpdateProduct(product));
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return DataRequisicionPartial();
        }

        public void UpdateProduct(RequisicionCompra product)
        {
            requisicionNeg.update(product);
            
        }

        [ValidateAntiForgeryToken]
        public ActionResult GridViewCustomActionPartial(string customAction, string codigo)
        {
            if (customAction == "delete")
                SafeExecute(() => DeleteProduct(codigo));
            if (customAction == "export")
            {

                return RedirectToAction("Requerimiento", "Report", new { codigo = codigo });// ver para requisiones
            }

            return DataRequisicionPartial();
        }
        public void DeleteProduct(string NROREQUI)
        {
            requisicionNeg.delete(NROREQUI);
        }

        public ActionResult ExportTo(string customExportCommand)
        {
            switch (customExportCommand)
            {
                case "CustomExportToXLS":
                case "CustomExportToXLSX":
                    return GridViewExportDemoHelper.ExportFormatsInfo[customExportCommand](
                        GridViewToolbarHelper.ExportGridSettings, GridViewHelper.GetRequisionCompras());
                default:
                    return RedirectToAction("Toolbar");
            }
        }

        // para eñ solicitante
    
        public ActionResult MultiSelectSolicitante(string TCLAVE = "-1")
        {

            var dar = Request.Params["gridLookupSolicitante"];
            ViewData["Solicitante"] = userNeg.findAllSolicitud();
            if (TCLAVE == "-1")
                TCLAVE = "";
            return PartialView("MultiSelectSolicitante", new Solicitud() { TCLAVE = TCLAVE });

        }
        public ActionResult MultiSelectArea(string AREA_CODIGO = "-1")
        {
            ViewData["Area"] = areaNeg.findAll();
            if (AREA_CODIGO == "-1")
                AREA_CODIGO = "";
            return PartialView("MultiSelectArea", new Area() { AREA_CODIGO = AREA_CODIGO });

        }

        // detalle 
        public ActionResult Detail(string NROREQUI = "-1")
        {        
            if (NROREQUI == null)
            {
                NROREQUI = "-1";

            }
            ViewData["codigoOrden"] = NROREQUI;
            requisicionCompra = requisicionNeg.find(NROREQUI);
           
            if (requisicionCompra == null)
            {
                ViewData["codigoOrden"] = NROREQUI;
                GridViewHelper.GetDetalles();
                return PartialView("Detail", GridViewHelper.detalleRequisicions);
            }
            GridViewHelper.detalleRequisicions = requisicionNeg.findAllDetail(requisicionCompra.NROREQUI);
            return PartialView("Detail", GridViewHelper.detalleRequisicions);
        }
        public ActionResult DetailRequestPartial(string NROREQUI = "-1")
        {
            if (NROREQUI == null)
            {
                NROREQUI = "-1";

            }
            ViewData["codigoOrden"] = NROREQUI;
            requisicionCompra = requisicionNeg.find(NROREQUI);

            if (requisicionCompra == null)
            {
                ViewData["codigoOrden"] = NROREQUI;
                GridViewHelper.GetDetalles();
                return PartialView("DetailRequestPartial", GridViewHelper.detalleRequisicions);
            }
            GridViewHelper.detalleRequisicions = requisicionNeg.findAllDetail(requisicionCompra.NROREQUI);
            return PartialView("DetailRequestPartial", GridViewHelper.detalleRequisicions);
        }
        public ActionResult DetailRequestAddNewPartial(DetalleRequisicion product, FormCollection dataForm)
        {
            // obtener  codarticulo             
            var codArticulodata = dataForm["DXMVCEditorsValues"];
            Dictionary<string, object> nodes = JsonConvert.DeserializeObject<Dictionary<string, object>>(codArticulodata);
            var codArticulo = nodes["gridLookup"];
            //var codCosto = nodes["gridLookupCostos"];

            JArray array = (JArray)codArticulo;
           // JArray arraycosto = (JArray)codCosto;

            var description = dataForm["gridLookup"];
            product.codpro = array.First.ToString();
            product.DESCPRO = description.ToString();
            product.FECREQUE = DateTime.Now;
            
            // product.CENCOST = arraycosto.First.ToString();
            if (ModelState.IsValid)
                SafeExecute(() => InsertProduct(product));
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var data = ViewData["codigoOrden"];
            if (data == null)
            {
                return DetailRequestPartial("-1");
            }
            return DetailRequestPartial(ViewData["codigoOrden"].ToString());
        }
        public void InsertProduct(DetalleRequisicion product)
        {
            GridViewHelper.detalleRequisicions.Add(product);
        }

        [ValidateInput(false)]
        public ActionResult detailRequestUpdatePartial(DetalleRequisicion product, FormCollection dataForm)
        {
            var codArticulodata = dataForm["DXMVCEditorsValues"];
            Dictionary<string, object> nodes = JsonConvert.DeserializeObject<Dictionary<string, object>>(codArticulodata);
            var codArticulo = nodes["gridLookup"];

            JArray array = (JArray)codArticulo;
            var description = dataForm["gridLookup"];
            product.codpro = array.First.ToString();
            product.DESCPRO = description.ToString();
            //Request.Params
            if (ModelState.IsValid)
                SafeExecute(() => UpdateProduct(product));
            else
                ViewData["EditError"] = "Please, correct all errors.";

            var data = ViewData["codigoOrden"];
            if (data == null)
            {
                return DetailRequestPartial("-1");
            }

            return DetailRequestPartial(ViewData["codigoOrden"].ToString());
        }
        public void UpdateProduct(DetalleRequisicion product)
        {

            DetalleRequisicion detalleOrdenCompra = GridViewHelper.detalleRequisicions.Find(element => element.codpro == product.codpro);
            detalleOrdenCompra.GLOSA = product.GLOSA;
            detalleOrdenCompra.CANTID = product.CANTID;          
            // crear la logica para agregar un producto
        }
        [ValidateInput(false)]
        public ActionResult DetailRequestDeletePartial(string codpro = "")
        {
            if (codpro != "")
                SafeExecute(() => Delete(codpro));
            var data = ViewData["codigoOrden"];
            if (data == null)
            {
                return DetailRequestPartial("-1");
            }
            return DetailRequestPartial(ViewData["codigoOrden"].ToString());
        }
        public void Delete(string product)
        {
            GridViewHelper.detalleRequisicions.Remove(GridViewHelper.detalleRequisicions.Find(element => element.codpro == product));
            // crear la logica para agregar un producto
        }

        // para el detalle
        public ActionResult MultiSelectCentroCostos(string CENCOST_CODIGO = "-1")
        {


            ViewData["centro_costos"] = requisicionNeg.findAllCentroCostos();
            if (CENCOST_CODIGO == "-1")
                CENCOST_CODIGO = "";
            return PartialView("MultiSelectCentroCostos", new CentroCosto() { CENCOST_CODIGO = CENCOST_CODIGO });

        }
        public ActionResult MultiSelectPartial(string CurrentCategory="")
        {
            if (CurrentCategory == null)
                CurrentCategory = "";
            return PartialView(new Articulo() { codigo = CurrentCategory });
        }
        public ActionResult DateRangePicker()
        {
            return PartialView("DateRangePicker");
        }
        [HttpPost]
        public ActionResult DateRangePicker(FormCollection data)
        {
            if (Request.Params["Submit"] == null)
                ModelState.Clear();
            else
            {
                GridViewHelper.dateRange.End = DateTime.Parse(Request.Params["End"]);
                GridViewHelper.dateRange.Start = DateTime.Parse(Request.Params["Start"]);
            }

            return RedirectToAction("index");
        }

    }
}