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


        public ComprobanteController()
        {
            responsable = new ResponsableCmpNeg(codEmpresa);
            comprobanteNeg = new ComprobanteNeg(codEmpresa);
            empresaNeg = new EmpresaNeg();
            destinoNeg = new DestinoNeg();
            requisicionNeg = new RequisicionCompraNeg(codEmpresa);

            tipoAnexoNeg = new TipoAnexoNeg(codEmpresa);
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
            GridViewHelper.contableDets = comprobanteNeg.findallContableDet();
            calcularDebeHaber();
            return View(GridViewHelper.contableDets);
        }
        private void calcularDebeHaber()
        {
            decimal haber = 0;
            decimal debe = 0;
            GridViewHelper.contableDets.ForEach(elem => {
                haber += decimal.Parse( elem.campo2);
                debe += decimal.Parse( elem.campo1);
            });
            GridViewHelper.TDebe = debe;
            GridViewHelper.THaber = haber;
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
            GridViewHelper.contableDets = comprobanteNeg.findallContableDet();
            return PartialView("ContaGridViewPartial", GridViewHelper.contableDets);
        }

        public ActionResult GridViewAddComprobante(ContableDet issue, FormCollection data)
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
            issue.CCODCONTA = ValidarRecuperar(PlanCuenta);
            issue.CCOSTO = ValidarRecuperar(Costos);
            issue.ORDFAB = ValidarRecuperar(OrdenFabricacion);
            issue.CCTADEST = ValidarRecuperar(DestinoConta);
            issue.CCODANEXO = ValidarRecuperar(AnexoConta);
            issue.CDESTCOMP = ValidarRecuperar(DestinoConta);

            //gridLookupResponsable$State        
            //gridLookupTipoDocRef$State
            // issue.CCONCEPT = ValidarRecuperar(concepto);
            Comprobante comp = comprobanteNeg.findAllConta(GridViewHelper.COMP_CORDEN, GridViewHelper.COMP_TIPODOCU_CODIGO, GridViewHelper.COMP_CSERIE, GridViewHelper.COMP_CNUMERO);
            comp.ContableDet = issue;
            return UpdateModelWithDataValidation(comp, AddNewRecordContable);
        }

        private void AddNewRecordContable(Comprobante issue)
        {
            comprobanteNeg.create(issue, GridViewHelper.NivelCOntable);
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
            if (customAction == "transferir")
            {
                PerformTransfer();
            }

            return GridViewPartial();
        }


        private void PerformTransfer()
        {
            Comprobante comp = comprobanteNeg.findAllConta(GridViewHelper.COMP_CORDEN, GridViewHelper.COMP_TIPODOCU_CODIGO, GridViewHelper.COMP_CSERIE, GridViewHelper.COMP_CNUMERO);
            comprobanteNeg.Tranferir(comp, GridViewHelper.NivelCOntable);
            //userNeg.delete(codigo);
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
            string codConversion = data["gridLookupMoneda$State"];

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
            issue.LDETRACCION = GridViewHelper.comprobante.DDetraccion == "0" || GridViewHelper.comprobante.DDetraccion == null ? false : true;
            issue.NUMRETRAC = GridViewHelper.comprobante.DDocumento;
            issue.NTASADETRACCION = GridViewHelper.comprobante.DTasa;
            issue.FECRETRAC = GridViewHelper.comprobante.DFecha;
            issue.COD_TIPOOPERACION = GridViewHelper.comprobante.tipoOperacion;
            issue.COD_SERVDETRACC = GridViewHelper.comprobante.DtipoServicio;
            issue.CCODCONTA = GridViewHelper.comprobante.CCODCONTA;
            issue.CNRORUC = GridViewHelper.comprobante.CNRORUC;

            GridViewHelper.COMP_CORDEN = issue.CORDEN;
            GridViewHelper.COMP_TIPODOCU_CODIGO = issue.TIPODOCU_CODIGO;
            GridViewHelper.COMP_CSERIE = issue.CSERIE;
            GridViewHelper.COMP_CNUMERO = issue.CNUMERO;


            ViewData["dar"] = issue.CORDEN;
            ViewData["dar"] = issue.ANEX_CODIGO;
            ViewData["dar"] = issue.ANEX_DESCRIPCION;
            ViewData["dar"] = issue.TIPODOCU_CODIGO;
            ViewData["dar"] = issue.CSERIE;
            ViewData["dar"] = issue.DEMISION;
            ViewData["dar"] = issue.DVENCE;
            ViewData["dar"] = issue.DRECEPCIO;
            ViewData["dar"] = issue.TIPOMON_CODIGO;
            ViewData["dar"] = issue.NIMPORTE;
            ViewData["dar"] = issue.TIPOCAMBIO_VALOR;
            ViewData["dar"] = issue.CDESCRIPC;
            ViewData["dar"] = issue.RESPONSABLE_CODIGO;
            ViewData["dar"] = issue.CESTADO;
            ViewData["dar"] = issue.NSALDO;
            ViewData["dar"] = issue.CCODCONTA;
            ViewData["dar"] = issue.CFORMPAGO;
            ViewData["dar"] = issue.CSERREFER;
            ViewData["dar"] = issue.CNUMREFER;
            ViewData["dar"] = issue.CTDREFER;
            ViewData["dar"] = issue.CONVERSION_CODIGO;
            ViewData["dar"] = issue.DREGISTRO;
            ViewData["dar"] = issue.CTIPPROV;
            ViewData["dar"] = issue.CNRORUC;
            ViewData["dar"] = issue.ESTCOMPRA;
            ViewData["dar"] = issue.CDESTCOMP;
            ViewData["dar"] = issue.DIASPAGO;
            ViewData["dar"] = issue.CIGVAPLIC;
            ViewData["dar"] = issue.CCONCEPT;
            ViewData["dar"] = issue.DFECREF;
            ViewData["dar"] = issue.NTASAIGV;
            ViewData["dar"] = issue.NIGV;
            ViewData["dar"] = issue.NPORCE;
            ViewData["dar"] = issue.CCODRUC; //referencia anexo
            ViewData["dar"] = issue.LHONOR;
            ViewData["dar"] = issue.NIR4;
            ViewData["dar"] = issue.NIES;
            ViewData["dar"] = issue.NTOTRH;
            ViewData["dar"] = issue.NBASEIMP;
            ViewData["dar"] = issue.NVALCIF;
            ViewData["dar"] = issue.DCONTAB;
            ViewData["dar"] = issue.CAMESPROC;
            ViewData["dar"] = issue.CSALDINI;
            ViewData["dar"] = issue.NPERCEPCION;
            ViewData["dar"] = issue.NUMRETRAC; // tex Documento
            ViewData["dar"] = issue.FECRETRAC;
            ViewData["dar"] = issue.CNUMORDCO;
            ViewData["dar"] = issue.CO_L_RETE;// combo1
            ViewData["dar"] = issue.LDETRACCION;
            ViewData["dar"] = issue.NTASADETRACCION;
            ViewData["dar"] = issue.DETRACCION;
            ViewData["dar"] = issue.COD_SERVDETRACC;
            ViewData["dar"] = issue.COD_TIPOOPERACION;
            ViewData["dar"] = issue.NIMPORTEREF;
            ViewData["dar"] = issue.RCO_TIPO;
            ViewData["dar"] = issue.RCO_SERIE;
            ViewData["dar"] = issue.RCO_NUMERO;
            ViewData["dar"] = issue.RCO_FECHA;
            ViewData["dar"] = issue.flg_RNTNODOMICILIADO;
            ViewData["dar"] = issue.CAOCOMPRA;


            return UpdateModelWithDataValidation(issue, AddNewRecord);
        }

        private void   AddNewRecord(Comprobante issue)
        {
            //GridViewHelper.Comprobantes.Add(issue);

            GridViewHelper.respuesta=  comprobanteNeg.create(issue);
           
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
                if (issue.ContableDet == null)
                {
                    if (!GridViewHelper.respuesta)
                    {
                        ViewBag.GeneralError = "error al guardar Documento ";
                        return  PartialView("GridViewPartial");
                    }
                    Comprobante comp = comprobanteNeg.findAllConta(GridViewHelper.COMP_CORDEN, GridViewHelper.COMP_TIPODOCU_CODIGO, GridViewHelper.COMP_CSERIE, GridViewHelper.COMP_CNUMERO);
                    comprobanteNeg.inserdataTemporal(comp);
                    return RedirectToAction("contabilizar");
                }
                else
                {
                    return ContaGridViewPartial();
                }
            }
            else
                ViewBag.GeneralError = "Please, correct all errors.";

            return PartialView("GridViewPartial");
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
                    if (nodes.selectedKeyValues != null && nodes.selectedKeyValues[0]!=null)
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

        // VER CONPROBANTE
        public ActionResult MultiSelectPlanCuenta(string PLANCTA_CODIGO = "-1", FormCollection dataR = null)
        {
            ViewData["PlanCuenta"] = comprobanteNeg.findAllCuentasNacionales(GridViewHelper.NivelCOntable);
            if (dataR != null)
            {
                string dar = dataR["gridLookupPlanCuenta$State"];

                string ver = HttpUtility.HtmlDecode(dar);
                if (ver != null)
                {
                    Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                    // Array codigo = nodes["selectedKeyValues"] ;
                    if (nodes.selectedKeyValues != null)
                    {
                        GridViewHelper.PLANCTA_CODIGO = nodes.selectedKeyValues[0];
                        PLANCTA_CODIGO = GridViewHelper.PLANCTA_CODIGO;
                    }
                }

            }

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
            GridViewHelper.comprobante.DFecha = DFecha;
            return Json(new { respuesta = "" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult habilitarRetencion()
        {
            bool retencion = GridViewHelper.activarRetecion;
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
        public JsonResult ChangePlanCuenta()
        {
           
            string xco_c_conco = "";
            string xco_c_tipo = "N";
            bool xco_c_cenco = false;

            string xcanexo = "";
            bool activeCenco = false;
            bool activeOrb = false;
            bool activeCtaDest = false;
            bool activeAnexo = false;
            decimal xnvalor = 0;

            RespuestaPlan respuesta = new RespuestaPlan(); 
            PlanCuentaNacional planCuenta = comprobanteNeg.findCuentasNacionales(GridViewHelper.PLANCTA_CODIGO, GridViewHelper.NivelCOntable);
            if (planCuenta != null)
            {
                xcanexo = planCuenta.TIPOANEX_CODIGO;

                xco_c_conco = planCuenta.PLANCTA_CON_COSTO;
                xco_c_cenco =  planCuenta.PLANCTA_CENTCOST;
                if (xco_c_conco != null && xco_c_conco.Trim()!="")
                {
                    GastosIngresos gastosIngresos = comprobanteNeg.findGastoIngreso(xco_c_conco);
                    if (gastosIngresos != null)
                    {
                        xco_c_tipo = gastosIngresos.GASING_TIPO;
                    }
                }
                xnvalor = Math.Abs(GridViewHelper.THaber - GridViewHelper.TDebe);
                if (xco_c_cenco)
                {
                    activeCenco = true;
                    if (comprobanteNeg.ExisteConceptoCGORDEN())
                    {
                        if (comprobanteNeg.verdataCGORDEN())
                        {
                            activeOrb = true;
                        }
                    }
                }
                if (xco_c_cenco && xco_c_tipo== "S")
                {
                    activeCtaDest = true;
                }
                if (xcanexo.Trim()!="")
                {
                    activeAnexo = true;
                }
            }
            respuesta.activeAnexo = activeAnexo;
            respuesta.activeCenco = activeCenco;
            respuesta.activeCtaDest = activeCtaDest;
            respuesta.activeOrb = activeOrb;
            respuesta.xcanexo = xcanexo;
            respuesta.xnvalor = xnvalor;



            return Json(new { retencion = respuesta }, JsonRequestBehavior.AllowGet);
        }

    }
}