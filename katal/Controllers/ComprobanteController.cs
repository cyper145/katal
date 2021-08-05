using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using katal.conexion.model.neg;
using katal.conexion.model.entity;
using katal.Model;
using Newtonsoft.Json;
using DevExpress.DataAccess.Native.Json;
using System.Text.RegularExpressions;
using System.Globalization;

namespace katal.Controllers
{
    public class ComprobanteController : BaseController
    {

        private ComprobanteNeg comprobanteNeg;
        private TipoAnexoNeg tipoAnexoNeg;
        private DestinoNeg destinoNeg;
        private ResponsableCmpNeg responsable;
        private EmpresaNeg empresaNeg;
        private RequisicionCompraNeg requisicionNeg;

        private string BD;
        public ComprobanteController()
        {
            responsable = new ResponsableCmpNeg();
            comprobanteNeg = new ComprobanteNeg();
            empresaNeg = new EmpresaNeg();
            destinoNeg = new DestinoNeg();
            requisicionNeg = new RequisicionCompraNeg();
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

            GridViewHelper.activarRetecion = comprobanteNeg.habilitarRetencion();
            List<Comprobante> comp = comprobanteNeg.findAll();
            GridViewHelper.Comprobantes = comp;
            return View(GridViewHelper.Comprobantes);
        }
        public ActionResult Contabilizar()
        {
            GridViewHelper.NivelCOntable = int.Parse(empresaNeg.findContable(GridViewHelper.user.codEmpresa).EMP_NIVEL);

            List<Comprobante> comp = comprobanteNeg.findAllConta(GridViewHelper.COMP_CORDEN, GridViewHelper.COMP_TIPODOCU_CODIGO, GridViewHelper.COMP_CSERIE, GridViewHelper.COMP_CNUMERO);
            GridViewHelper.Comprobantes = comp;
            return View(GridViewHelper.Comprobantes);
        }

        public ActionResult GridViewPartial()
        {
            //determinar el tasa igv
            GridViewHelper.tasa = comprobanteNeg.tasa();
            //List<Comprobante> comp = comprobanteNeg.findAll();
            return PartialView("GridViewPartial", GridViewHelper.Comprobantes);
        }
        public ActionResult ContaGridViewPartial()
        {
            //List<Comprobante> comp = comprobanteNeg.findAll();
            return PartialView("ContaGridViewPartial", GridViewHelper.Comprobantes);
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
        [ValidateAntiForgeryToken]
        public ActionResult GridViewAddNewPartial(Comprobante issue, FormCollection data)
        {

            var codArticulodata = data["DXMVCEditorsValues"];
            var btShowModal = data["btShowModal"];

            string concepto = data["gridLookupGastos$State"];
            string tipoProveerdor = data["gridLookupTipoAnexo$State"];
            string proveedor = data["gridLookupAnexo$State"];
            string tipoDocumento = data["gridLookupTipoDoc$State"];
            string codConversion= data["gridLookupMoneda$State"];

            string codDestino = data["gridLookupDestino$State"];
            string codResponsable = data["gridLookupResponsable$State"];

            //gridLookupResponsable$State        
            //gridLookupTipoDocRef$State
            issue.CCONCEPT = ValidarRecuperar(concepto);
            issue.CTIPPROV = ValidarRecuperar(tipoProveerdor);
            issue.ANEX_CODIGO = ValidarRecuperar(proveedor);
            issue.TIPODOCU_CODIGO = ValidarRecuperar(tipoDocumento);
            issue.CONVERSION_CODIGO = ValidarRecuperar(codConversion);
            issue.CDESTCOMP = ValidarRecuperar(codDestino);
            issue.RESPONSABLE_CODIGO = ValidarRecuperar(codResponsable);


            issue.TIPOCAMBIO_VALOR = issue.TIPOCAMBIO_VALOR > issue.TIPOCAMBIO_VALOR2 ? issue.TIPOCAMBIO_VALOR : issue.TIPOCAMBIO_VALOR2;

            issue.ANEX_DESCRIPCION = data["gridLookupAnexo"];
            issue.CORDEN = comprobanteNeg.funcAutoNum();
            issue.LDETRACCION = GridViewHelper.comprobante.DDetraccion=="0" || GridViewHelper.comprobante.DDetraccion==null ?false:true;
            issue.NUMRETRAC = GridViewHelper.comprobante.DDocumento;
            issue.NTASADETRACCION = GridViewHelper.comprobante.DTasa;
            issue.FECRETRAC = GridViewHelper.comprobante.DFecha;
            issue.COD_TIPOOPERACION= GridViewHelper.comprobante.tipoOperacion;
            issue.COD_SERVDETRACC= GridViewHelper.comprobante.DtipoServicio;
            issue.CCODCONTA = GridViewHelper.comprobante.CCODCONTA;
            GridViewHelper.COMP_CORDEN = issue.CORDEN;
            GridViewHelper.COMP_TIPODOCU_CODIGO = issue.TIPODOCU_CODIGO;
            GridViewHelper.COMP_CSERIE = issue.CSERIE;
            GridViewHelper.COMP_CNUMERO = issue.CNUMERO;

            /*
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
            //GridViewHelper.Comprobantes.Add(issue);

             comprobanteNeg.create(issue);
            // GridViewHelper.ClearDetalles();
        }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewUpdatePartial(Comprobante issue, FormCollection data)
        {
            var codArticulodata = data["DXMVCEditorsValues"];

            string concepto = data["gridLookupGastos$State"];
            string tipoProveerdor = data["gridLookupTipoAnexo$State"];
            string proveedor = data["gridLookupAnexo$State"];
            string tipoDocumento = data["gridLookupTipoDoc$State"];
            string destino = data["gridLookupTipoDoc$State"];


            
            issue.CCONCEPT = ValidarRecuperar(concepto);
            issue.CTIPPROV = ValidarRecuperar(tipoProveerdor);
            issue.ANEX_CODIGO = ValidarRecuperar(proveedor);
            issue.TIPODOCU_CODIGO = ValidarRecuperar(tipoDocumento);

            issue.ANEX_DESCRIPCION = data["gridLookupAnexo"];
            GridViewHelper.COMP_CORDEN = issue.CORDEN;
            GridViewHelper.COMP_TIPODOCU_CODIGO = issue.TIPODOCU_CODIGO;
            GridViewHelper.COMP_CSERIE = issue.CSERIE;
            GridViewHelper.COMP_CNUMERO = issue.CNUMERO;

            return UpdateModelWithDataValidation(issue, UpdateRecord);
        }
        private void UpdateRecord(Comprobante issue)
        {
            GridViewHelper.Comprobantes.Add(issue);
        }
        private ActionResult UpdateModelWithDataValidation(Comprobante issue, Action<Comprobante> metodo)
        {
            ModelState.Remove("LPASOIMP");
            ModelState.Remove("ESTCOMPRA");
            ModelState.Remove("DIASPAGO");
            if (ModelState.IsValid)
                SafeExecute(() => metodo(issue));
            else
                ViewBag.GeneralError = "Please, correct all errors.";

            return RedirectToAction("contabilizar");
           // return PartialView("GridViewPartial");
        }

        public ActionResult MultiSelectGasto(string Gastos_Codigo = "-1", FormCollection dataR = null)
        {
            if (dataR != null)
            {
                string dar = dataR["gridLookupGastos$State"];
                string ver = HttpUtility.HtmlDecode(dar);
                if (ver != null)
                {
                    Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                    // Array codigo = nodes["selectedKeyValues"] ;
                    if (nodes.selectedKeyValues != null)
                    {
                        GridViewHelper.Gastos_Codigo = nodes.selectedKeyValues[0];
                        Gastos_Codigo = GridViewHelper.Gastos_Codigo;
                    }
                }

            }
            ViewData["Gastos"] = comprobanteNeg.findAllGastos();
            if (Gastos_Codigo == "-1")
                Gastos_Codigo = "";
            return PartialView("MultiSelectGasto", new Gasto() { Gastos_Codigo = Gastos_Codigo });

        }

        public ActionResult MultiSelectTipoAnexo(string TIPOANEX_CODIGO = "-1")
        {
            ViewData["TipoAnexo"] = tipoAnexoNeg.findAll();


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
        public ActionResult MultiSelectTipoDoc(string TIPDOC_CODIGO = "-1", FormCollection dataR = null)
        {
            ViewData["TipoDoc"] = tipoAnexoNeg.findAllTipoDocumento();
            if (dataR != null)
            {
                string dar = dataR["gridLookupTipoDoc$State"];
                string ver = HttpUtility.HtmlDecode(dar);
                if (ver != null)
                {
                    Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                    // Array codigo = nodes["selectedKeyValues"] ;
                    if (nodes.selectedKeyValues != null)
                    {
                        GridViewHelper.TIPDOC_CODIGO = nodes.selectedKeyValues[0];
                        TIPDOC_CODIGO = GridViewHelper.TIPDOC_CODIGO;
                    }
                }

            }
            if (TIPDOC_CODIGO == "-1")
                TIPDOC_CODIGO = "";
            return PartialView("MultiSelectTipoDoc", new TipoDocumento() { TIPDOC_CODIGO = TIPDOC_CODIGO });

        }

        public ActionResult MultiSelectDestino(string CO_C_CODIG = "-1", FormCollection dataR = null)
        {
            ViewData["Destino"] = destinoNeg.findAll();

            if (dataR != null)
            {
                string dar = dataR["gridLookupDestino$State"];
                string ver = HttpUtility.HtmlDecode(dar);
                if (ver != null)
                {
                    Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                    // Array codigo = nodes["selectedKeyValues"] ;
                    if (nodes.selectedKeyValues != null)
                    {
                        GridViewHelper.CO_C_CODIG = nodes.selectedKeyValues[0];
                        CO_C_CODIG = GridViewHelper.COVMON_CODIGO;
                    }
                }

            }
            if (CO_C_CODIG == "-1")
                CO_C_CODIG = "";
            return PartialView("MultiSelectDestino", new Destino() { CO_C_CODIG = CO_C_CODIG });

        }
        public ActionResult MultiSelectResponsable(string oc_csolict = "01")
        {
            ViewData["responsable"] = responsable.findAll();
            if (oc_csolict == "-1")
                oc_csolict = "";
            return PartialView("MultiSelectResponsable", new ResponsableCompra() { RESPONSABLE_CODIGO = oc_csolict });
        }
        public ActionResult MultiSelectMoneda(string COVMON_CODIGO = "COM", FormCollection dataR = null)
        {
            if (dataR != null)
            {
                string dar = dataR["gridLookupMoneda$State"];
                string ver = HttpUtility.HtmlDecode(dar);
                if (ver != null)
                {
                    Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                    // Array codigo = nodes["selectedKeyValues"] ;
                    if (nodes.selectedKeyValues != null)
                    {
                        GridViewHelper.COVMON_CODIGO = nodes.selectedKeyValues[0];
                        COVMON_CODIGO = GridViewHelper.COVMON_CODIGO;
                    }
                }

            }
            ViewData["moneda"] = comprobanteNeg.findAllMonedas();
            if (COVMON_CODIGO == "-1")
                COVMON_CODIGO = "";
            return PartialView("MultiSelectMoneda", new Moneda() { COVMON_CODIGO = COVMON_CODIGO });
        }

        public ActionResult MultiSelectTipoDocRef(string TIPDOC_CODIGO = "-1")
        {
            ViewData["TipoDocRef"] = tipoAnexoNeg.findAllTipoDocumento();
            if (TIPDOC_CODIGO == "-1")
                TIPDOC_CODIGO = "";
            return PartialView("MultiSelectTipoDocRef", new TipoDocumento() { TIPDOC_CODIGO = TIPDOC_CODIGO });
        }
        public JsonResult CargarTipoAnexoMoneda()
        {
            string CODIGO = GridViewHelper.Gastos_Codigo;
            gastoTipoAnexo anexo = new gastoTipoAnexo();
            if (CODIGO != "" && CODIGO != null)
            {
                anexo = comprobanteNeg.cargarChangeTipoGasto(CODIGO);
                GridViewHelper.comprobante.CCODCONTA = anexo.gasto.Gastos_CuentaCon;
                GridViewHelper.comprobante.gasto1 = anexo.gasto.Gastos_Dscto1;
                GridViewHelper.comprobante.gasto2 = anexo.gasto.Gastos_Dscto2;
                GridViewHelper.Gastos_Codigo = "";
            }
            //string Gastos_Codigo = "";
            return Json(new { tipoanexo = anexo }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CargarTipoDocumento()
        {
            string CODIGO = GridViewHelper.TIPDOC_CODIGO;
            TipoDocumento anexo = new TipoDocumento();
            if (CODIGO != "" && CODIGO != null)
            {
                anexo = tipoAnexoNeg.findTipoDocumento(CODIGO);
                
            }
            //string Gastos_Codigo = "";
            return Json(new { tipodocumento = anexo }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CargarTipoMoneda(string fecha)
        {
            string CODIGO = GridViewHelper.COVMON_CODIGO;
            TipoCambio anexo = new TipoCambio();

            Respuesta respuesta = new Respuesta();
            if (CODIGO != "" && CODIGO != null)
            {
                anexo = comprobanteNeg.findTipoCambio(fecha);
                GridViewHelper.Gastos_Codigo = "";

                switch (CODIGO)
                {
                    case "ESP":
                        respuesta.data = 0;
                        respuesta.opcion = false;
                        respuesta.especial = true;
                        break;
                    case "VTA":                     
                        respuesta.data =  anexo.TIPOCAMB_VENTA;
                        respuesta.dataEspecial = "0.000";
                        respuesta.opcion = true;
                        respuesta.especial = false;
                        break;
                    case "COM":
                        respuesta.data = anexo.TIPOCAMB_COMPRA;
                        respuesta.dataEspecial = "0.000";
                        respuesta.opcion = true;
                        respuesta.especial = false;
                        break;
                    case "FEC":
                        respuesta.opcion = true;
                        respuesta.dataEspecial = "0.000";
                        respuesta.especial = false;
                        break;

                }
            }
            //string Gastos_Codigo = "";
            return Json(new { respuesta = respuesta }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult partialPopup()
        {
            return PartialView("partialPopup");
        }
        public ActionResult MultiSelectdetraccion(string codigo = "-1", string DFecha= "", string DDocumento = "", FormCollection dataR = null)
        {
            ViewData["Detraccion"] = comprobanteNeg.findAllDetraccion(DateTime.Now.ToShortDateString());
            if (dataR.Count > 0)
            {
                string dar = dataR["DXMVCEditorsValues"];
                string tipoDetraccion = dataR["gridLookupDetraccion$State"];

                string[] word = dar.Split(',');
                string dataDetraccion = dar[0] + word[1] + "," +word[2] + "," + word[3] + dar[dar.Length - 1];
                Dictionary<string, Object> info = JsonConvert.DeserializeObject<Dictionary<string, Object>>(dataDetraccion);
                GridViewHelper.comprobante.DDetraccion = info["DDetraccion"].ToString();
                GridViewHelper.comprobante.DTasa = info["DTasa"] == null ? 0 : Decimal.Parse(info["DTasa"].ToString());
                GridViewHelper.comprobante.DDocumento = info["DDocumento"] == null?"":info["DDocumento"].ToString();

                string ver = HttpUtility.HtmlDecode(tipoDetraccion);
                if (ver != null)
                {
                    Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                    // Array codigo = nodes["selectedKeyValues"] ;
                    if (nodes.selectedKeyValues != null)
                    {
                        GridViewHelper.comprobante.DtipoServicio= nodes.selectedKeyValues[0];
                        // COVMON_CODIGO = GridViewHelper.COVMON_CODIGO;
                    }
                }

            }


            if (codigo == "-1")
                codigo = "";
            return PartialView("MultiSelectdetraccion", new ServSujDetraccion() { codigo = codigo });
        }
        public ActionResult MultiSelectTipoOperacion(string CODIGO = "-1",FormCollection dataR = null)
        {
            ViewData["TipoOperacion"] = comprobanteNeg.findAllTipoOperacion();
            if (dataR.Count > 0)
            {
                string dar = dataR["DXMVCEditorsValues"];
                string tipoDetraccion = dataR["gridLookupDetraccion$State"];

                string[] word = dar.Split(',');
                string dataDetraccion = dar[0] + word[1] + "," + word[2] + "," + word[3] + dar[dar.Length - 1];
                Dictionary<string, Object> info = JsonConvert.DeserializeObject<Dictionary<string, Object>>(dataDetraccion);
                GridViewHelper.comprobante.DDetraccion = info["DDetraccion"].ToString();
                GridViewHelper.comprobante.DTasa = info["DTasa"] == null ? 0 : Decimal.Parse( info["DTasa"].ToString());
                GridViewHelper.comprobante.DDocumento = info["DDocumento"] == null ? "" : info["DDocumento"].ToString();

                string ver = HttpUtility.HtmlDecode(tipoDetraccion);
                if (ver != null)
                {
                    Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                    // Array codigo = nodes["selectedKeyValues"] ;
                    if (nodes.selectedKeyValues != null)
                    {
                        GridViewHelper.comprobante.tipoOperacion = nodes.selectedKeyValues[0];
                        // COVMON_CODIGO = GridViewHelper.COVMON_CODIGO;
                    }
                }

            }

            if (CODIGO == "-1")
                CODIGO = "";
            return PartialView("MultiSelectTipoOperacion", new TipoOperacion() { CODIGO = CODIGO });
        }

        // VER CONPROBANTE
        public ActionResult MultiSelectPlanCuenta(string PLANCTA_CODIGO = "-1")
        {
            ViewData["PlanCuenta"] = comprobanteNeg.findAllCuentasNacionales(GridViewHelper.NivelCOntable);


            if (PLANCTA_CODIGO == "-1")
                PLANCTA_CODIGO = "";
            return PartialView("MultiSelectPlanCuenta", new PlanCuentaNacional() { PLANCTA_CODIGO = PLANCTA_CODIGO });

        }

        public ActionResult MultiSelectCentroCostos(string CENCOST_CODIGO = "-1")
        {


            ViewData["centro_costos"] = requisicionNeg.findAllCentroCostos();
            if (CENCOST_CODIGO == "-1")
                CENCOST_CODIGO = "";
            return PartialView("MultiSelectCentroCostos", new CentroCosto() { CENCOST_CODIGO = CENCOST_CODIGO });

        }
        public ActionResult MultiSelectOrdenFabricacion(string OF_COD = "-1")
        {
            ViewData["OrdenFabricacion"] = comprobanteNeg.findAllOrdenFabricacion();


            if (OF_COD == "-1")
                OF_COD = "";
            return PartialView("MultiSelectOrdenFabricacion", new OrdenFabricacion() { OF_COD = OF_COD });
        }


        // POST: /Account/SignIn
        [HttpPost]
        [AllowAnonymous]
        
        public ActionResult PopupControl(SignInViewModel model, string returnUrl)
        {
            return PartialView("GridViewPartial");
        }

        public JsonResult changeDFecha(DateTime DFecha)
        {
            GridViewHelper.comprobante.DFecha=DFecha;
            return Json(new { respuesta = "" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult habilitarRetencion()
        {
            bool retencion= GridViewHelper.activarRetecion;
            return Json(new { retencion = retencion }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult HabilitarCalculados()
        {

            string CODIGO = GridViewHelper.CO_C_CODIG;
            Destino destino = new Destino();
            RespuestaDestino respuesta = new RespuestaDestino();
            if (CODIGO != "" && CODIGO != null)
            {
                destino = destinoNeg.find(CODIGO);
                if (destino.CON_IMPSTO == "N")
                {
                    respuesta.montoIGV = 0;
                    respuesta.tasaIGV = 0;
                    respuesta.opcion = true;

                    //NTASAIGV NIGV
                }
                else
                {
                    respuesta.opcion = false;
                    respuesta.tasaIGV = GridViewHelper.tasa;
                }
            }
            return Json(new { respuesta = respuesta }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult obtenerGastos()
        {
            RespuestaGastos retencion = new RespuestaGastos();
            
            retencion.mnGasto1 = GridViewHelper.comprobante.gasto1;
            retencion.mnGasto2 = GridViewHelper.comprobante.gasto2;

            return Json(new { retencion = retencion }, JsonRequestBehavior.AllowGet);
        }
        
    }
}