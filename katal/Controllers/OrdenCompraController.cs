using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using katal.conexion.model.neg;
using katal.conexion.model.entity;
using DevExpress.Web.Mvc;
using katal.Models;
using katal.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace katal.Controllers
{
    public class OrdenCompraController : BaseController
    {
        // GET: OrdenCompra
        private OrdenCompraNeg userNeg;
        private RequisicionCompraNeg requisionNeg;
        private ProveedorNeg proveedorNeg;
        private ArticuloNeg articuloNeg;
        static OrdenCompra OrdeCurrent;
        private ResponsableCmpNeg responsable;

        public OrdenCompraController()
        {
            requisionNeg = new RequisicionCompraNeg();
            userNeg = new OrdenCompraNeg();
            articuloNeg = new ArticuloNeg();
            proveedorNeg = new ProveedorNeg();
            responsable = new ResponsableCmpNeg();
            if (OrdeCurrent == null)
                OrdeCurrent = new OrdenCompra();

        }
        public ActionResult Index()
        {
         
            ViewData["FormasPago"] = userNeg.findAllFormasPago();
            ViewData["Solicitud"] = userNeg.findAllSolicitud();
            ViewData["DocRef"] = userNeg.findAllDocRef();
            GridViewHelper.OrdenCompras = userNeg.findAll(GridViewHelper.dateRange);
            return View(GridViewHelper.OrdenCompras);
        }
        public ActionResult GridViewPartial()
        {
          
          //  Dictionary<string, Object> nodes = JsonConvert.DeserializeObject<Dictionary<string, Object>>(fecha);
                  
            return PartialView("GridViewPartial", GridViewHelper.OrdenCompras);
        }



        public ActionResult MultiSelectPartial(string CurrentCategory)
        {

        
            if (CurrentCategory == null)
                CurrentCategory = "";
            return PartialView(new Articulo() { codigo = CurrentCategory });

            
        }
        public ActionResult MultiSelectProveedor(string CurrentCategory)
        {

            ViewData["Proveedores"] = proveedorNeg.findAll();
            if (CurrentCategory == null)
                CurrentCategory = "";
            return PartialView(new Proveedor() { PRVCCODIGO = CurrentCategory });
        }

        [ValidateAntiForgeryToken]
        public ActionResult GridViewCustomActionPartial(string customAction, string codigo)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete(codigo));
            if(customAction  == "export")
            {
                return RedirectToAction( "index","Report", new {codigo=codigo });
            }
                
            return GridViewPartial();
        }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewAddNewPartial(OrdenCompra issue, FormCollection data)
        {

            var codArticulodata = data["DXMVCEditorsValues"];
            string[] word = codArticulodata.Split(',');
            string dataRequisicion = codArticulodata[0] + word[2] + "," + word[14] + ","+ word[15]+ "," + codArticulodata[codArticulodata.Length - 1];
            Dictionary<string, JArray> nodes = JsonConvert.DeserializeObject<Dictionary<string, JArray>>(dataRequisicion);
            JArray proveedor = nodes["gridLookupProveedor"];
            JArray docref = nodes["gridLookupDocRef"];
            JArray nroRef = nodes["gridLookupNroRef"];

            issue.oc_ccodpro = proveedor.First.ToString();
            issue.OC_CDOCREF = docref.First.ToString();
            issue.OC_CNRODOCREF= nroRef.First.ToString();
            var codArticulo = data["gridLookupProveedor"];
            issue.OC_CRAZSOC = codArticulo.ToString();

            issue.OC_CSITORD = "03";
            issue.EST_NOMBRE = "EMITIDA";
            issue.OC_CSOLICT = issue.RESPONSABLE_CODIGO;
            /*
            var length =   HttpUtility.HtmlDecode(data["grid"]);
            Dictionary<string, object> nodes = JsonConvert.DeserializeObject<Dictionary<string, object>>(length);
            var datad =  nodes["batchEditClientModifiedValues"].ToString();
            Dictionary<string, object> nodeinsert = JsonConvert.DeserializeObject<Dictionary<string, object>>(datad);
            var datainsert = nodeinsert["EditState"].ToString();
            Dictionary<string, object> nodeinsertreal = JsonConvert.DeserializeObject<Dictionary<string, object>>(datainsert);
            var datainsertreal = nodeinsertreal["insertedRowValues"].ToString();
            Dictionary<string, object> insertreal = JsonConvert.DeserializeObject<Dictionary<string, object>>(datainsertreal);

            List<DetalleOrdenCompra> articulos = new List<DetalleOrdenCompra>();
            foreach (var entry in insertreal)
            {
                string dataArticulo = entry.Value.ToString();

                DetalleOrdenCompra articulo = JsonConvert.DeserializeObject<DetalleOrdenCompra>(dataArticulo);
                articulos.Add(articulo);
            }
            */

            issue.detalles = GridViewHelper.detalles;

            decimal importe = 0;
            GridViewHelper.detalles.ForEach((elem) =>
            {

                importe += elem.OC_NTOTVEN;
            }
            );
            issue.OC_NVENTA= importe;
            issue.OC_NIMPORT = importe;
            return UpdateModelWithDataValidation(issue, AddNewRecord);
        }

        private void AddNewRecord(OrdenCompra issue)
        {
           // GridViewHelper.OrdenCompras.Add(issue);
            userNeg.create(issue);
            GridViewHelper.ClearDetalles();
         }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewUpdatePartial(OrdenCompra issue, HttpRequest request)
        {
            return UpdateModelWithDataValidation(issue, UpdateRecord);
        }
        private void UpdateRecord(OrdenCompra issue)
        {

        }
        private ActionResult UpdateModelWithDataValidation(OrdenCompra issue, Action<OrdenCompra> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(issue));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return GridViewPartial();
        }
        private void PerformDelete(string  codigo)
        {
            userNeg.delete(codigo);
        }
        private void PerformExport()
        {

            if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                GridViewHelper.DeleteRecords(Request.Params["SelectedRows"]);
        }

        //para el nuevo forma de agregar

        public ActionResult Toolbar(string CurrentCategory = "-1")
        {
           //string codigo = "0000000000001";
            if (CurrentCategory == null)
            {
                CurrentCategory = "-1";

            }
            ViewData["codigoOrden"] = CurrentCategory;
            OrdeCurrent = userNeg.find(CurrentCategory);
            // probando solo 
            if(OrdeCurrent==null)
                OrdeCurrent=GridViewHelper.OrdenCompras.Find(element => element.OC_CNUMORD == CurrentCategory);
            GridViewHelper.ClearDetalles();
            if (OrdeCurrent == null)
            {
                ViewData["codigoOrden"] = CurrentCategory;
                GridViewHelper.GetDetalles();
                return PartialView("Toolbar", GridViewHelper.detalles);
            }

            return PartialView("Toolbar", userNeg.findAllDetail(OrdeCurrent.OC_CNUMORD));
        }
        public ActionResult ToolbarPartial(string codigoOrden)
        {

            //string codigo = "0000000000001";
            ViewData["codigoOrden"] = codigoOrden;
            OrdeCurrent = userNeg.find(codigoOrden);
           
            if (OrdeCurrent == null)
            {
                GridViewHelper.GetDetalles();
                if (GridViewHelper.NROREQUI != "")
                {
                    
                    GridViewHelper.ClearDetalles();
                    cargar(requisionNeg.findAllDetail(GridViewHelper.NROREQUI));
                    GridViewHelper.NROREQUI = "";

                }
                return PartialView("ToolbarPartial", GridViewHelper.detalles);
            }

           GridViewHelper.detalles = userNeg.findAllDetail(OrdeCurrent.OC_CNUMORD);
            return PartialView("ToolbarPartial", GridViewHelper.detalles);
        }

        public void cargar(List<DetalleRequisicion> detalles)
        {
            detalles.ForEach(element =>
            {
                DetalleOrdenCompra temp = new DetalleOrdenCompra();
                temp.oc_ccodigo = element.codpro;
                temp.OC_NCANTID = element.CANTID;
                temp.OC_CDESREF = element.DESCPRO;
                temp.OC_CUNIDAD = element.UNIPRO;
                temp.OC_GLOSA = element.ESPTECNICA;

                GridViewHelper.detalles.Add(temp);
            });
        }

        [ValidateInput(false)]
        public ActionResult ToolbarAddNewPartial(DetalleOrdenCompra product, FormCollection dataForm)
        {
            // obtener  codarticulo             
            var codArticulodata = dataForm["DXMVCEditorsValues"];
            Dictionary<string, object> nodes = JsonConvert.DeserializeObject<Dictionary<string, object>>(codArticulodata);
            var codArticulo = nodes["gridLookup"];

            JArray array =(JArray) codArticulo ;
            
            var description = dataForm["gridLookup"];
            product.oc_ccodigo = array.First.ToString();
            product.OC_CDESREF = description.ToString();
            if (ModelState.IsValid)
                SafeExecute(() => InsertProduct(product));
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var data=ViewData["codigoOrden"];
            if (data == null)
            {
                return ToolbarPartial("-1");
            }
            return ToolbarPartial(ViewData["codigoOrden"].ToString());
        }

        public void InsertProduct(DetalleOrdenCompra product)
        {
            GridViewHelper.detalles.Add(product);
        }



        [ValidateInput(false)]
        public ActionResult ToolbarUpdatePartial(DetalleOrdenCompra product, FormCollection dataForm)
        {
            var codArticulodata = dataForm["DXMVCEditorsValues"];
            Dictionary<string, object> nodes = JsonConvert.DeserializeObject<Dictionary<string, object>>(codArticulodata);
            var codArticulo = nodes["gridLookup"];

            JArray array = (JArray)codArticulo;
            var description = dataForm["gridLookup"];
            product.oc_ccodigo = array.First.ToString();
            product.OC_CDESREF = description.ToString();
            //Request.Params
            if (ModelState.IsValid)
                SafeExecute(() => UpdateProduct(product));
            else
                ViewData["EditError"] = "Please, correct all errors.";

            var data = ViewData["codigoOrden"];
            if (data == null)
            {
                return ToolbarPartial("-1");
            }

            return ToolbarPartial(ViewData["codigoOrden"].ToString());
        }

        public void UpdateProduct(DetalleOrdenCompra product)
        {

            DetalleOrdenCompra detalleOrdenCompra= GridViewHelper.detalles.Find(element=> element.oc_ccodigo== product.oc_ccodigo);
            detalleOrdenCompra.OC_GLOSA = product.OC_GLOSA;
            detalleOrdenCompra.OC_NCANTID = product.OC_NCANTID;
            detalleOrdenCompra.OC_NPREUNI= product.OC_NPREUNI;
            detalleOrdenCompra.OC_NDSCPOR= product.OC_NDSCPOR;
            detalleOrdenCompra.OC_NTOTVEN= product.OC_NTOTVEN;

        }

        [ValidateInput(false)]
        public ActionResult ToolbarDeletePartial(string oc_ccodigo = "")
        {
            if (oc_ccodigo != "")
                SafeExecute(() => DeleteProduct(oc_ccodigo));
            var data = ViewData["codigoOrden"];
            if (data == null)
            {
                return ToolbarPartial("-1");
            }
            return ToolbarPartial(ViewData["codigoOrden"].ToString());
        }

        public void DeleteProduct(string product)
        {
            GridViewHelper.detalles.Remove( GridViewHelper.detalles.Find(element => element.oc_ccodigo == product));
            // crear la logica para agregar un producto
        }


        // parte 
        public ActionResult MultiSelectDocRef(string OC_CDOCREF = "-1", FormCollection dataR=null)
        {
            if (dataR != null)
            {
                string v = dataR["gridLookupDocRef"];
                GridViewHelper.OC_CDOCREF = v;
            }
          
            ViewData["DocRef"] = userNeg.findAllDocRef();
            if (OC_CDOCREF != "-1")
                OC_CDOCREF = "";
            return PartialView("MultiSelectDocRef", new  NumDocCompras () { CTNCODIGO = OC_CDOCREF });
            
   
        }
        public ActionResult MultiSelectNroRef(string  NROREQUI  = "-1", FormCollection dataR = null)
        {
            if (dataR != null)
            {
                string v = dataR["gridLookupNroRef"];
                if (v != null && v!="")
                {
                    GridViewHelper.NROREQUI = v;                    
                }
               
            }

            // VER QUE EENTO PUEDE VER ESTE DETALLE POR EL MOMENTO SOLO SE NECESITA RQ
            if (GridViewHelper.OC_CDOCREF== "RQ") {
                ViewData["NroRef"] = requisionNeg.findAllPendientes();
            }
            ViewData["NroRef"] = requisionNeg.findAllPendientes();
            if (NROREQUI != "-1")
                NROREQUI = "";
            return PartialView("MultiSelectNroRef", new RequisicionCompra() { NROREQUI = NROREQUI });

        }

     
        public ActionResult MultiSelectResponsable(string oc_csolict = "-1", FormCollection dataR = null)
        {
            if (dataR != null)
            {
                string v = dataR["gridLookupDocRef"];
               // GridViewHelper.OC_SOLICITA = v;
            }

            ViewData["responsable"] = responsable.findAll();
            if (oc_csolict == "-1")
                oc_csolict = "";
            return PartialView("MultiSelectResponsable", new ResponsableCompra() { RESPONSABLE_CODIGO = oc_csolict });

        }
        
        public JsonResult nroDera()
        {

            return Json(new { document = userNeg.nextNroDocument() }, JsonRequestBehavior.AllowGet);
        }

        // controlor de filtro
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
                GridViewHelper.dateRange.End =DateTime.Parse(  Request.Params["End"]);
                GridViewHelper.dateRange.Start= DateTime.Parse(Request.Params["Start"]);
            }
               
            return RedirectToAction("index");
        }
    }
}