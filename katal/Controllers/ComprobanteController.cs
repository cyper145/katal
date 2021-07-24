﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using katal.conexion.model.neg;
using katal.conexion.model.entity;
using katal.Model;

namespace katal.Controllers
{
    public class ComprobanteController : BaseController
    {

        private ComprobanteNeg comprobanteNeg;
        private TipoAnexoNeg tipoAnexoNeg;
        private DestinoNeg destinoNeg;
        private ResponsableCmpNeg responsable;

        private string BD;
        public ComprobanteController()
        {
            responsable = new ResponsableCmpNeg();
            comprobanteNeg = new ComprobanteNeg();
            destinoNeg = new DestinoNeg();
            ApplicationUser user = AuthHelper.GetLoggedInUserInfo();
            if (user == null)
            {
                user = GridViewHelper.user;
            }
            this.BD = user.codEmpresa;
            tipoAnexoNeg = new TipoAnexoNeg(this.BD);
        }// GET: Comprobante
        public ActionResult Index()
        {

            List<Comprobante> comp = comprobanteNeg.findAll();
            GridViewHelper.Comprobantes = comp;
            return View(GridViewHelper.Comprobantes);
        }
        public ActionResult GridViewPartial()
        {
            //List<Comprobante> comp = comprobanteNeg.findAll();
            return PartialView("GridViewPartial", GridViewHelper.Comprobantes);
        }

        [ValidateAntiForgeryToken]
        public ActionResult GridViewCustomActionPartial(string customAction, string codigo)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete(codigo));
            if (customAction == "export")
            {
                return RedirectToAction("index", "Report", new { codigo = codigo });
            }

            return GridViewPartial();
        }
        private void PerformDelete(string codigo)
        {
            Comprobante aux = GridViewHelper.Comprobantes.Find(element => element.codigo == codigo);
            GridViewHelper.Comprobantes.Remove(aux);
            //userNeg.delete(codigo);
        }

        [ValidateAntiForgeryToken]
        public ActionResult GridViewAddNewPartial(Comprobante issue, FormCollection data)
        {


            /*
            var codArticulodata = data["DXMVCEditorsValues"];
            string[] word = codArticulodata.Split(',');
            string dataRequisicion = codArticulodata[0] + word[2] + "," + word[14] + "," + word[15] + "," + codArticulodata[codArticulodata.Length - 1];
            Dictionary<string, JArray> nodes = JsonConvert.DeserializeObject<Dictionary<string, JArray>>(dataRequisicion);
            JArray proveedor = nodes["gridLookupProveedor"];
            JArray docref = nodes["gridLookupDocRef"];
            JArray nroRef = nodes["gridLookupNroRef"];

            issue.oc_ccodpro = proveedor.First.ToString();
            issue.OC_CDOCREF = docref.First.ToString();
            issue.OC_CNRODOCREF = nroRef.First.ToString();
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
            /*
            issue.detalles = GridViewHelper.detalles;

            decimal importe = 0;
            GridViewHelper.detalles.ForEach((elem) =>
            {

                importe += elem.OC_NTOTVEN;
            }
            );
            issue.OC_NVENTA = importe;
            issue.OC_NIMPORT = importe;
            */
            return UpdateModelWithDataValidation(issue, AddNewRecord);
        }

        private void AddNewRecord(Comprobante issue)
        {
            GridViewHelper.Comprobantes.Add(issue);
           // userNeg.create(issue);
           // GridViewHelper.ClearDetalles();
        }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewUpdatePartial(Comprobante issue, HttpRequest request)
        {
            return UpdateModelWithDataValidation(issue, UpdateRecord);
        }
        private void UpdateRecord(Comprobante issue)
        {

        }
        private ActionResult UpdateModelWithDataValidation(Comprobante issue, Action<Comprobante> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(issue));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return GridViewPartial();
        }

       


       
        public ActionResult MultiSelectGasto(string Gastos_Codigo = "-1", FormCollection dataR = null)
        {
            if (dataR != null)
            {
                string v = dataR["gridLookupGastos"];
                if (v != null && v != "")
                {
                    GridViewHelper.NROREQUI = v;
                }

            }

            ViewData["Gastos"] = comprobanteNeg.findAllGastos();
            if (Gastos_Codigo == "-1")
                Gastos_Codigo = "";
            return PartialView("MultiSelectGasto", new Gasto() { Gastos_Codigo = Gastos_Codigo });

        }

        public ActionResult MultiSelectTipoAnexo(string TIPOANEX_CODIGO = "-1")
        {
            ViewData["TipoAnexo"] = tipoAnexoNeg.findAll( );
            if (TIPOANEX_CODIGO == "-1")
                TIPOANEX_CODIGO = "";
            return PartialView("MultiSelectTipoAnexo", new TipoAnexo() { TIPOANEX_CODIGO = TIPOANEX_CODIGO });

        }
        public ActionResult MultiSelectAnexo(string ANEX_CODIGO = "-1")
        {
             ViewData["Anexo"] = tipoAnexoNeg.findAllAnexo();
            if (ANEX_CODIGO == "-1")
                ANEX_CODIGO = "-1";
            return PartialView("MultiSelectAnexo", new Anexo() { ANEX_CODIGO = ANEX_CODIGO });

        }
        public ActionResult MultiSelectTipoDoc(string TIPDOC_CODIGO = "-1")
        {
            ViewData["TipoDoc"] = tipoAnexoNeg.findAllTipoDocumento();
            if (TIPDOC_CODIGO == "-1")
                TIPDOC_CODIGO = "";
            return PartialView("MultiSelectTipoDoc", new TipoDocumento() { TIPDOC_CODIGO = TIPDOC_CODIGO });

        }

        public ActionResult MultiSelectDestino(string CO_C_CODIG = "-1")
        {
            ViewData["Destino"] = destinoNeg.findAll();
            if (CO_C_CODIG == "-1")
                CO_C_CODIG = "";
            return PartialView("MultiSelectDestino", new Destino() { CO_C_CODIG = CO_C_CODIG });

        }
        public ActionResult MultiSelectResponsable(string oc_csolict = "-1")
        {
           

            ViewData["responsable"] = responsable.findAll();
            if (oc_csolict == "-1")
                oc_csolict = "";
            return PartialView("MultiSelectResponsable", new ResponsableCompra() { RESPONSABLE_CODIGO = oc_csolict });

        }

        public ActionResult MultiSelectTipoDocRef(string TIPDOC_CODIGO = "-1")
        {
            ViewData["TipoDocRef"] = tipoAnexoNeg.findAllTipoDocumento();
            if (TIPDOC_CODIGO == "-1")
                TIPDOC_CODIGO = "";
            return PartialView("MultiSelectTipoDocRef", new TipoDocumento() { TIPDOC_CODIGO = TIPDOC_CODIGO });

        }


        // evento de cambiar 
       // public JsonResult CargarTipoAnexoMoneda()
       // {
            //var  dar= Request.Params;
           // string Gastos_Codigo = "";
          //  return Json(new { document = userNeg.nextNroDocument() }, JsonRequestBehavior.AllowGet);
//}
    }
}