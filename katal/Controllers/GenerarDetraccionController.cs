using katal.conexion.model.entity;
using katal.conexion.model.neg;
using katal.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace katal.Controllers
{
    public class GenerarDetraccionController : BaseController
    {

        private ComprobanteNeg comprobanteNeg;
        private TipoAnexoNeg tipoAnexoNeg;
        private DestinoNeg destinoNeg;
        private ResponsableCmpNeg responsable;
        private EmpresaNeg empresaNeg;
        private RequisicionCompraNeg requisicionNeg;
        private ComprobanteDNeg comprobanteDNeg;

        public GenerarDetraccionController()
        {
            responsable = new ResponsableCmpNeg(codEmpresa);
            comprobanteNeg = new ComprobanteNeg(codEmpresa);
            comprobanteDNeg = new ComprobanteDNeg(codEmpresa);
            empresaNeg = new EmpresaNeg();
            destinoNeg = new DestinoNeg();
            requisicionNeg = new RequisicionCompraNeg(codEmpresa);

            tipoAnexoNeg = new TipoAnexoNeg(codEmpresa);
        }// GET: Comprobante
        public ActionResult Index()
        {

            GridViewHelper.activarRetecion = comprobanteNeg.habilitarRetencion();
            List<ComprobanteDetraccion> comp = comprobanteDNeg.findAll(GridViewHelper.dateRange);
            GridViewHelper.ComprobantesD = comp;
            return View(GridViewHelper.ComprobantesD);
        }
        public ActionResult Contabilizar()
        {
            GridViewHelper.NivelCOntable = int.Parse(empresaNeg.findContable(GridViewHelper.user.codEmpresa).EMP_NIVEL);
            GridViewHelper.contableDets = comprobanteNeg.findallContableDet();
            calcularDebeHaber();
            return View(GridViewHelper.contableDets);
        }
        private void calcularDebeHaber()
        {
            decimal haber = 0;
            decimal debe = 0;
            GridViewHelper.contableDets.ForEach(elem =>
            {
                haber += decimal.Parse(elem.campo2);
                debe += decimal.Parse(elem.campo1);
            });
            GridViewHelper.TDebe = debe;
            GridViewHelper.THaber = haber;
        }
        public ActionResult GridViewPartial()
        {
            //determinar el tasa igv
            GridViewHelper.tasa = comprobanteNeg.tasa();
            //List<Comprobante> comp = comprobanteNeg.findAll();
            return PartialView("GridViewPartial", GridViewHelper.ComprobantesD);
        }
        public ActionResult ContaGridViewPartial()
        {
            //List<Comprobante> comp = comprobanteNeg.findAll();
            GridViewHelper.contableDets = comprobanteNeg.findallContableDet();
            return PartialView("ContaGridViewPartial", GridViewHelper.contableDets);
        }

        public ActionResult GridViewAddComprobante(ComprobanteDetraccion issue, FormCollection data)
        {

            var codArticulodata = data["DXMVCEditorsValues"];
            var btShowModal = data["btShowModal"];

            string concepto = data["gridLookupGastos$State"];
            string tipoProveerdor = data["gridLookupTipoAnexo$State"];
            string proveedor = data["gridLookupAnexo$State"];
            string tipoDocumento = data["gridLookupTipoDoc$State"];
            string codConversion = data["gridLookupMoneda$State"];

            string codDestino = data["gridLookupDestino$State"];
            string codResponsable = data["gridLookupResponsable$State"];

            string PlanCuenta = data["gridLookupPlanCuenta$State"];
            string Costos = data["gridLookupCostos$State"];
            string OrdenFabricacion = data["gridLookupOrdenFabricacion$State"];
            string DestinoConta = data["gridLookupDestinoConta$State"];
            string AnexoConta = data["gridLookupAnexoConta$State"];

            return UpdateModelWithDataValidation(issue, AddNewRecordContable);
        }

        private void AddNewRecordContable(ComprobanteDetraccion issue)
        {
            //comprobanteNeg.create(issue, GridViewHelper.NivelCOntable);
        }

        public JsonResult cargardata()
        {
            int CODIGO = GridViewHelper.contableDets.Count + 1;
            string next = CODIGO.ToString("00000.##");
            Comprobante comp = comprobanteNeg.findAllConta(GridViewHelper.COMP_CORDEN, GridViewHelper.COMP_TIPODOCU_CODIGO, GridViewHelper.COMP_CSERIE, GridViewHelper.COMP_CNUMERO);
            RespuestaCDataComprobante respuestaCData = new RespuestaCDataComprobante();
            respuestaCData.NROITEM = next;
            respuestaCData.GLOSA = comp.CDESCRIPC;

            //string Gastos_Codigo = "";
            return Json(new { tipoanexo = respuestaCData }, JsonRequestBehavior.AllowGet);
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
            if (customAction == "exportTxt")
            {
                PerformExportTxt();
            }

            return GridViewPartial();
        }


        private void PerformExportTxt()
        {
            List<ComprobanteDetraccion> list = GridViewHelper.ComprobantesD.Where(X => X.ImpPagar > 0).ToList();
            //userNeg.delete(codigo);       
            list.ForEach(X =>
            {

                X.restante = X.saldo - X.ImpPagar;
                if (X.restante == 0)
                {
                    X.estado = "Pagado";
                }

            });

            comprobanteDNeg.updateDetail(list);


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
        public ActionResult GridViewAddNewPartial(ComprobanteDetraccion issue, FormCollection data)
        {




            return UpdateModelWithDataValidation(issue, AddNewRecord);
        }

        private void AddNewRecord(ComprobanteDetraccion issue)
        {
            //GridViewHelper.Comprobantes.Add(issue);

            //  GridViewHelper.respuesta = comprobanteNeg.create(issue);

        }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewUpdatePartial(ComprobanteDetraccion issue, FormCollection data)
        {
            return UpdateModelWithDataValidation(issue, UpdateRecord);
        }
        private void UpdateRecord(ComprobanteDetraccion issue)
        {
            ComprobanteDetraccion comprobanteDetraccions = GridViewHelper.ComprobantesD.Find(X => X.codigo == issue.codigo);

            comprobanteDetraccions.ImpPagar = issue.ImpPagar;
        }
        private ActionResult UpdateModelWithDataValidation(ComprobanteDetraccion issue, Action<ComprobanteDetraccion> metodo)
        {
            ModelState.Remove("LPASOIMP");
            ModelState.Remove("ESTCOMPRA");
            ModelState.Remove("DIASPAGO");
            ModelState.Remove("CIGVAPLIC");
            ModelState.Remove("NIR4");
            ModelState.Remove("NIES");
            ModelState.Remove("NTOTRH");
            ModelState.Remove("NPERCEPCION");
            ModelState.Remove("RCO_FECHA");
            ModelState.Remove("flg_RNTNODOMICILIADO");
            ModelState.Remove("cantidad");

            if (ModelState.IsValid)
            {
                SafeExecute(() => metodo(issue));
            }
            else
                ViewBag.GeneralError = "Please, correct all errors.";

            return GridViewPartial();
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


        public ActionResult MultiSelectAnexo(string ANEX_CODIGO = "-1", FormCollection dataR = null)
        {

            List<Anexo> data = tipoAnexoNeg.findAllAnexo();
            ViewData["Anexo"] = data;
            if (dataR != null)
            {
                string dar = dataR["gridLookupAnexo$State"];

                string ver = HttpUtility.HtmlDecode(dar);
                if (ver != null)
                {
                    Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                    // Array codigo = nodes["selectedKeyValues"] ;
                    if (nodes.selectedKeyValues != null && nodes.selectedKeyValues[0] != null)
                    {
                        string codigoanexo = nodes.selectedKeyValues[0];
                        Anexo anexo = data.Find(X => X.ANEX_CODIGO == codigoanexo);
                        GridViewHelper.comprobante.CNRORUC = anexo.ANEX_RUC;
                    }
                }

            }
            if (ANEX_CODIGO == "-1")
                ANEX_CODIGO = "-1";
            return PartialView("MultiSelectAnexo", new Anexo() { ANEX_CODIGO = ANEX_CODIGO });

        }

        public ActionResult MultiSelectAnexoConta(string ANEX_CODIGO = "-1", FormCollection dataR = null)
        {
            List<Anexo> data = tipoAnexoNeg.findAllAnexo();
            ViewData["Anexo"] = data;
            if (ANEX_CODIGO == "-1")
                ANEX_CODIGO = "-1";
            return PartialView("MultiSelectAnexoConta", new Anexo() { ANEX_CODIGO = ANEX_CODIGO });

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
                        CO_C_CODIG = GridViewHelper.CO_C_CODIG;
                    }
                }

            }
            if (CO_C_CODIG == "-1")
                CO_C_CODIG = "";
            return PartialView("MultiSelectDestino", new Destino() { CO_C_CODIG = CO_C_CODIG });

        }



        public ActionResult MultiSelectDestinoConta(string CO_C_CODIG = "-1", FormCollection dataR = null)
        {
            ViewData["Destino"] = destinoNeg.findAll();


            if (CO_C_CODIG == "-1")
                CO_C_CODIG = "";
            return PartialView("MultiSelectDestinoConta", new Destino() { CO_C_CODIG = CO_C_CODIG });

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
            else
            {
                GridViewHelper.COVMON_CODIGO = COVMON_CODIGO;
            }
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
                        respuesta.data = anexo.TIPOCAMB_VENTA;
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
        public ActionResult MultiSelectdetraccion(string codigo = "-1", string DFecha = "", string DDocumento = "", FormCollection dataR = null)
        {
            ViewData["Detraccion"] = comprobanteNeg.findAllDetraccion(DateTime.Now.ToShortDateString());
            if (dataR.Count > 0)
            {
                string dar = dataR["DXMVCEditorsValues"];
                string tipoDetraccion = dataR["gridLookupDetraccion$State"];

                string[] word = dar.Split(',');
                string dataDetraccion = dar[0] + word[1] + "," + word[2] + "," + word[3] + dar[dar.Length - 1];
                Dictionary<string, Object> info = JsonConvert.DeserializeObject<Dictionary<string, Object>>(dataDetraccion);
                GridViewHelper.comprobante.DDetraccion = info["DDetraccion"].ToString();
                GridViewHelper.comprobante.DTasa = info["DTasa"] == null ? 0 : Decimal.Parse(info["DTasa"].ToString());
                GridViewHelper.comprobante.DDocumento = info["DDocumento"] == null ? "" : info["DDocumento"].ToString();

                string ver = HttpUtility.HtmlDecode(tipoDetraccion);
                if (ver != null)
                {
                    Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                    // Array codigo = nodes["selectedKeyValues"] ;
                    if (nodes.selectedKeyValues != null)
                    {
                        GridViewHelper.comprobante.DtipoServicio = nodes.selectedKeyValues[0];
                        // COVMON_CODIGO = GridViewHelper.COVMON_CODIGO;
                    }
                }

            }


            if (codigo == "-1")
                codigo = "";
            return PartialView("MultiSelectdetraccion", new ServSujDetraccion() { codigo = codigo });
        }
        public ActionResult MultiSelectTipoOperacion(string CODIGO = "-1", FormCollection dataR = null)
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
                GridViewHelper.comprobante.DTasa = info["DTasa"] == null ? 0 : Decimal.Parse(info["DTasa"].ToString());
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