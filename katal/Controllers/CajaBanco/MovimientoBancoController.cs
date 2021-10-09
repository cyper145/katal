using katal.Code.Helpers;
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

    public class MovimientoBancoController : BaseController
    {
        private OrdenCompraNeg userNeg;
        private RequisicionCompraNeg requisicionNeg;
        private ProveedorNeg proveedorNeg;
        private ArticuloNeg articuloNeg;
        private AreaNeg areaNeg;
        private ResponsableCmpNeg responsable;      
        // nuevos
        private CajaBancoNeg cajaBancoNeg;
        private TipoAnexoNeg anexoNeg;
        private ComprobanteNeg comprobanteNeg;

        public MovimientoBancoController()
        {
            requisicionNeg = new RequisicionCompraNeg(codEmpresa);
            userNeg = new OrdenCompraNeg(codEmpresa);
            articuloNeg = new ArticuloNeg(codEmpresa);
            proveedorNeg = new ProveedorNeg(codEmpresa);
            responsable = new ResponsableCmpNeg(codEmpresa);
            areaNeg = new AreaNeg(codEmpresa);
            cajaBancoNeg = new CajaBancoNeg(codEmpresa);
            anexoNeg = new TipoAnexoNeg(codEmpresa);
            comprobanteNeg = new ComprobanteNeg(codEmpresa);
        }
        // GET: RequisionCompra
        public ActionResult Index()
        {
            List<CMovimientoBanco> movimientoBancos = new List<CMovimientoBanco>();
            DataGeneralBanco dataGeneral = new DataGeneralBanco();
            GridViewHelper.movimientoBancosdetalles.Clear();
            if (GridViewHelper.activeData)
            {
                // movimientoBancos = cajaBancoNeg.findAllMovimientos(GridViewHelper.codigobanco, "MN");

                GridViewHelper.movimientoBancos = cajaBancoNeg.findAllMovimientos(GridViewHelper.codigobanco, GridViewHelper.monedabanco, GridViewHelper.dateTime);

                movimientoBancos = GridViewHelper.movimientoBancos;
                movimientoBancos = movimientoBancos.Where(X => X.CB_D_FECHA >= GridViewHelper.dateRangeBanco.Start && X.CB_D_FECHA <= GridViewHelper.dateRangeBanco.End).ToList();
            }
            dataGeneral.cMovimientoBancos = movimientoBancos;
            return View(dataGeneral);
        }
        public ActionResult DataRequisicionPartial()
        {
            DataGeneralBanco dataGeneral = new DataGeneralBanco();
            //Sec-Fetch-Mode
            GridViewHelper.movimientoBancosdetalles.Clear();
            List<CMovimientoBanco> movimientoBancos = new List<CMovimientoBanco>(); ;
            if (GridViewHelper.activeData)
            {
                GridViewHelper.movimientoBancos = cajaBancoNeg.findAllMovimientos(GridViewHelper.codigobanco, GridViewHelper.monedabanco, GridViewHelper.dateTime);
                movimientoBancos = GridViewHelper.movimientoBancos;
                movimientoBancos = movimientoBancos.Where(X => X.CB_D_FECHA >= GridViewHelper.dateRangeBanco.Start && X.CB_D_FECHA <= GridViewHelper.dateRangeBanco.End).ToList();
            }
            dataGeneral.cMovimientoBancos = movimientoBancos;
            return PartialView("DataRequisicionPartial", dataGeneral);
        }

        [ValidateInput(false)]
        public ActionResult RequisicionAddNewPartial(CMovimientoBanco product, FormCollection dataForm)
        {
            string CB_C_OPERA = GridViewHelper.ValidarRecuperar(dataForm["gridLookupOpciones$State"]);
            string CB_C_TPDOC = GridViewHelper.ValidarRecuperar(dataForm["gridLookupTipoDoc$State"]);
            string CB_C_ANEXO = GridViewHelper.ValidarRecuperar(dataForm["gridLookupAnexo$State"]);
            string CB_C_CONVE = GridViewHelper.ValidarRecuperar(dataForm["gridLookupMoneda$State"]);
            string CB_C_ESTAD = GridViewHelper.ValidarRecuperar(dataForm["gridLookupEstados$State"]);
            string CB_TIPMOV = GridViewHelper.ValidarRecuperar(dataForm["gridLookupTiposMovimientos$State"]);
            product.CB_C_OPERA = CB_C_OPERA;
            product.CB_C_TPDOC = CB_C_TPDOC;
            product.CB_C_ANEXO = CB_C_ANEXO;
            product.CB_C_CONVE = CB_C_CONVE;
            product.CB_C_ESTAD = CB_C_ESTAD;
            product.CB_TIPMOV = CB_TIPMOV;
            product.CB_C_SECUE = (GridViewHelper.movimientoBancos.Count + 1).ToString("0000.##");

            /*
            // obtener  codarticulo             
            var codArticulodata = dataForm["DXMVCEditorsValues"];

            string[] word = codArticulodata.Split(',');
            string dataRequisicion = codArticulodata[0] + word[11] + "," + word[14] + codArticulodata[codArticulodata.Length - 1];
            Dictionary<string, JArray> nodes = JsonConvert.DeserializeObject<Dictionary<string, JArray>>(dataRequisicion);
            JArray solicitante = nodes["gridLookupSolicitante"];
            JArray area = nodes["gridLookupArea"];
            product.CODSOLIC = solicitante.First.ToString();
            product.AREA = area.First.ToString();
            product.TIPOREQUI = "RQ";
            product.NROREQUI = requisicionNeg.nextNroDocument();

            product.detalles = GridViewHelper.detalleRequisicions;

            ModelState.Remove("FecEntrega");*/
            if (ModelState.IsValid)
                SafeExecute(() => InsertProduct(product));
            else
            {
                if (!ModelState.IsValidField("FecEntrega") && ModelState.Count == 1)
                {

                    SafeExecute(() => InsertProduct(product));
                }
                else
                {
                    ViewData["EditError"] = "Falta datos por Ingresar";
                }

            }

            return DataRequisicionPartial();
        }

        public void InsertProduct(CMovimientoBanco product)
        {
            GridViewHelper.movimientoBancos.Add(product);
        }

        [ValidateInput(false)]
        public ActionResult RequisicionUpdatePartial(CMovimientoBanco product, FormCollection dataForm)
        {


            string CB_C_OPERA = GridViewHelper.ValidarRecuperar(dataForm["gridLookupOpciones$State"]);
            string CB_C_TPDOC = GridViewHelper.ValidarRecuperar(dataForm["gridLookupTipoDoc$State"]);
            string CB_C_ANEXO = GridViewHelper.ValidarRecuperar(dataForm["gridLookupAnexo$State"]);
            string CB_C_CONVE = GridViewHelper.ValidarRecuperar(dataForm["gridLookupMoneda$State"]);
            string CB_C_ESTAD = GridViewHelper.ValidarRecuperar(dataForm["gridLookupEstados$State"]);
            string CB_TIPMOV = GridViewHelper.ValidarRecuperar(dataForm["gridLookupTiposMovimientos$State"]);

            product.CB_C_OPERA = CB_C_OPERA;
            product.CB_C_TPDOC = CB_C_TPDOC;
            product.CB_C_ANEXO = CB_C_ANEXO;
            product.CB_C_CONVE = CB_C_CONVE;
            product.CB_C_ESTAD = CB_C_ESTAD;
            product.CB_TIPMOV = CB_TIPMOV;
            product.CB_C_SECUE = (GridViewHelper.movimientoBancos.Count + 1).ToString("0000.##");
            if (ModelState.IsValid)
                SafeExecute(() => UpdateProduct(product));
            else
                ViewData["EditError"] = "KeyDownCuentaDestino";

            return DataRequisicionPartial();
        }

        public void UpdateProduct(CMovimientoBanco product)
        {           
            CMovimientoBanco detalleOrdenCompra = GridViewHelper.movimientoBancos.Find(element => element.CB_C_SECUE == product.CB_C_SECUE);
            detalleOrdenCompra = product;
        }

        [ValidateAntiForgeryToken]
        public ActionResult GridViewCustomActionPartial(string customAction, string codigo)
        {
            if (customAction == "delete")
                SafeExecute(() => DeleteProduct(codigo));
            if (customAction == "contabilizar")
            {
                if (codigo != "")
                {
                    CMovimientoBanco movimientoBanco = GridViewHelper.movimientoBancos.Find(X => X.CB_C_SECUE == codigo);

                    movimientoBanco.CB_L_CONTA = "*";
                }

            }
            if (customAction == "anular")
            {

                CMovimientoBanco movimientoBanco = GridViewHelper.movimientoBancos.Find(X => X.CB_C_SECUE == codigo);

                movimientoBanco.CB_L_ANULA = "*";
            }
            if (customAction == "imprimir")
            {

                return RedirectToAction("MovimientoBanco", "Report", new { codigo = codigo });// ver para requisiones
            }

            return DataRequisicionPartial();
        }
        public void DeleteProduct(string NROREQUI)
        {
            cajaBancoNeg.deleteMovimientoBanco(GridViewHelper.codigobanco, GridViewHelper.dateTime,   NROREQUI);
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
        public ActionResult Detail(string CB_C_SECUE = "-1")
        {
            ViewData["codigoOrden"] = CB_C_SECUE;
           
           
            if (CB_C_SECUE != "-1")
            {
                GridViewHelper.secuenciacab = CB_C_SECUE;
                GridViewHelper.movimientoBancosdetalles = cajaBancoNeg.findDetailMovimientos(CB_C_SECUE, GridViewHelper.codigobanco, GridViewHelper.monedabanco, GridViewHelper.dateTime, GridViewHelper.TipoOpcion);
                
            }
           

            return PartialView("Detail", GridViewHelper.movimientoBancosdetalles);
        }
        public ActionResult DetailRequestPartial(string CB_C_SECUE = "-1")
        {
            if (CB_C_SECUE != "-1")
            {
                GridViewHelper.secuenciacab = CB_C_SECUE;
                GridViewHelper.movimientoBancosdetalles = cajaBancoNeg.findDetailMovimientos(CB_C_SECUE, GridViewHelper.codigobanco, GridViewHelper.monedabanco, GridViewHelper.dateTime,GridViewHelper.TipoOpcion);
            }
         
            return PartialView("DetailRequestPartial", GridViewHelper.movimientoBancosdetalles);
        }
        public ActionResult DetailRequestAddNewPartial(DMovimientoBanco product, FormCollection dataForm)
        {
            string SINGSAL = "";
            if (GridViewHelper.TipoOpcion == "S")
            {
                if (product.IS == 0)
                {
                    SINGSAL = "S";
                }
                else
                {
                    SINGSAL = "I";
                }
            }
            else
            {
                if (product.IS == 1)
                {
                    SINGSAL = "S";
                }
                else
                {
                    SINGSAL = "I";
                }
            }

            product.CB_C_BANCO = GridViewHelper.codigobanco;
            product.CB_C_MES = GridViewHelper.dateTime.Month.ToString("00.##");
            product.CB_C_SECUE = GridViewHelper.dateTime.Month.ToString("00.##");
            product.CB_C_CONCE = GridViewHelper.ValidarRecuperar(dataForm["gridLookupConceptoCajaBanco$State"]);

            product.CB_C_ANEXOD = GridViewHelper.ValidarRecuperar(dataForm["gridLookupAnexoD$State"]);
            product.CB_C_TPDOCD = GridViewHelper.ValidarRecuperar(dataForm["gridLookupTipoDocD$State"]);
            product.CB_C_CENCO = GridViewHelper.ValidarRecuperar(dataForm["gridLookupCostos$State"]);
            product.CB_C_CUENT = GridViewHelper.ValidarRecuperar(dataForm["gridLookupCuenta$State"]);
            product.CB_C_DESTI = GridViewHelper.ValidarRecuperar(dataForm["gridLookupCuentaDestino$State"]);
            product.CB_C_MODO = SINGSAL;
            string moneda = GridViewHelper.ValidarRecuperar(dataForm["gridLookupTipoMoneda$State"]);

            product.monedaD = moneda;
            if (moneda == "MN")
            {
                product.CB_N_MTOMND = product.CB_N_MTOMND;

            }
            else
            {
                product.CB_N_MTOMED = product.CB_N_MTOMND;

            }

            ModelState.Remove("IS");
            ModelState.Remove("CB_N_MTOMND");
            if (ModelState.IsValid)
                SafeExecute(() => InsertProduct(product));
            else
                ViewData["EditError"] = "Porfavor Complete los campos necesarios.";
            var data = ViewData["codigoOrden"];
           
            return DetailRequestPartial(GridViewHelper.secuenciacab);
        }
        public void InsertProduct(DMovimientoBanco product)
        {
            // GridViewHelper.movimientoBancosdetalles.Add(product); 

            cajaBancoNeg.crearteDetail( GridViewHelper.secuenciacab, product,
                GridViewHelper.codigobanco, GridViewHelper.dateTime, GridViewHelper.monedabanco, GridViewHelper.tipoCambioBanco);
        }

        [ValidateInput(false)]
        public ActionResult detailRequestUpdatePartial(DMovimientoBanco product, FormCollection dataForm)
        {
            product.CB_C_BANCO = GridViewHelper.codigobanco;
            product.CB_C_MES = GridViewHelper.dateTime.Month.ToString("00.##");
            product.CB_C_SECUE = GridViewHelper.dateTime.Month.ToString("00.##");
            product.CB_C_CONCE = GridViewHelper.ValidarRecuperar(dataForm["gridLookupConceptoCajaBanco$State"]);

            product.CB_C_ANEXOD = GridViewHelper.ValidarRecuperar(dataForm["gridLookupTipoAnexoD$State"]);
            product.CB_C_TPDOCD = GridViewHelper.ValidarRecuperar(dataForm["gridLookupTipoDocD$State"]);
            product.CB_C_CENCO = GridViewHelper.ValidarRecuperar(dataForm["gridLookupCostos$State"]);
            product.CB_C_CUENT = GridViewHelper.ValidarRecuperar(dataForm["gridLookupCuenta$State"]);
            product.CB_C_DESTI = GridViewHelper.ValidarRecuperar(dataForm["gridLookupCuentaDestino$State"]);
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
        public void UpdateProduct(DMovimientoBanco product)
        {

            DMovimientoBanco detalleOrdenCompra = GridViewHelper.movimientoBancosdetalles.Find(element => element.CB_C_SECDE == product.CB_C_SECDE);
            detalleOrdenCompra = product;
            // crear la logica para agregar un producto
        }
        [ValidateInput(false)]
        public ActionResult DetailRequestDeletePartial(string CB_C_SECDE = "")
        {
            if (CB_C_SECDE != "")
                SafeExecute(() => Delete(CB_C_SECDE));
            var data = ViewData["codigoOrden"];
            if (data == null)
            {
                return DetailRequestPartial("-1");
            }
            return DetailRequestPartial(ViewData["codigoOrden"].ToString());
        }
        public void Delete(string product)
        {
            GridViewHelper.movimientoBancosdetalles.Remove(GridViewHelper.movimientoBancosdetalles.Find(element => element.CB_C_SECDE == product));
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
        public ActionResult MultiSelectPartial(string CurrentCategory = "")
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
                GridViewHelper.dateRangeBanco.End = DateTime.Parse(Request.Params["End"]);
                GridViewHelper.dateRangeBanco.Start = DateTime.Parse(Request.Params["Start"]);
            }

            return RedirectToAction("index");
        }

        #region ===== cambios ===
        public ActionResult MultiSelectBancos(string CB_C_CODIG = "-1")
        {
            ViewData["Bancos"] = cajaBancoNeg.findAll();
            if (CB_C_CODIG == "-1")
                CB_C_CODIG = "";
            return PartialView("MultiSelectBancos", new CajaBanco() { CB_C_CODIG = CB_C_CODIG });

        }
        [HttpPost]
        public ActionResult parametros(FormCollection data)
        {
            if (Request.Params["Submit2"] == null)
                ModelState.Clear();
            else
            {
                if (data != null)
                {
                    string dar = data["gridLookupBancos$State"];
                    string ver = HttpUtility.HtmlDecode(dar);
                    if (ver != null)
                    {
                        Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                        // Array codigo = nodes["selectedKeyValues"] ;
                        if (nodes.selectedKeyValues != null)
                        {
                            GridViewHelper.codigobanco = nodes.selectedKeyValues[0];
                            CajaBanco cajaBanco = cajaBancoNeg.findBanco(GridViewHelper.codigobanco);
                            GridViewHelper.monedabanco = cajaBanco.CB_C_MONED;
                            GridViewHelper.activeData = true;
                            GridViewHelper.codigoContabilidad = cajaBanco.CB_C_CUENT;
                            GridViewHelper.CB_C_BANCO = cajaBanco.CB_C_BANCO;
                           
                            GridViewHelper.dateTime = DateTime.Parse(Request.Params["Proceso"]);

                            DateTime date = GridViewHelper.dateTime;
                            int nrodias = DateTime.DaysInMonth(date.Year, date.Month);
                            GridViewHelper.dateRangeBanco.Start = new DateTime(date.Year, date.Month, 1);
                            GridViewHelper.dateRangeBanco.End = new DateTime(date.Year, date.Month, nrodias);


                        }
                    }

                }


            }

            return RedirectToAction("index");
        }
        //operacion
        public ActionResult MultiSelectOpciones(FormCollection data, string CB_C_CODIG = "-1")
        {
            ViewData["tipoOpciones"] = cajaBancoNeg.findAllTipoOpciones(GridViewHelper.TipoOpcion);


            if (data != null)
            {
                string dar = data["gridLookupOpciones$State"];
                string ver = HttpUtility.HtmlDecode(dar);
                if (ver != null)
                {
                    Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                    // Array codigo = nodes["selectedKeyValues"] ;
                    if (nodes.selectedKeyValues != null)
                    {
                        string codigoOperacion = nodes.selectedKeyValues[0];
                        CajaBanco cajaBanco = cajaBancoNeg.findBanco(GridViewHelper.codigobanco);
                        GridViewHelper.operacionbanco = codigoOperacion;               
                    }
                }

            }
            if (CB_C_CODIG == "-1")
                CB_C_CODIG = "";
            else
                GridViewHelper.operacionbanco = CB_C_CODIG;
            return PartialView("MultiSelectOpciones", new TipoOpcionCajaBanco() { CB_C_CODIG = CB_C_CODIG });

        }
        // determinar si es un ingreso o egreso
        public JsonResult changeTipo(string tipo)
        {
            GridViewHelper.TipoOpcion = tipo;

            ViewData["tipoOpciones"] = cajaBancoNeg.findAllTipoOpciones(GridViewHelper.TipoOpcion);
            return Json(new { respuesta = "" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MultiSelectTipoDoc(string TIPDOC_CODIGO = "-1")
        {
            ViewData["TipoDoc"] = anexoNeg.findAllTipoDocumento();
            if (TIPDOC_CODIGO == "-1")
                TIPDOC_CODIGO = "";
            return PartialView("MultiSelectTipoDoc", new TipoDocumento() { TIPDOC_CODIGO = TIPDOC_CODIGO });

        }
        public ActionResult MultiSelectTipoAnexo(FormCollection data, string TIPOANEX_CODIGO = "-1")
        {

            ViewData["TipoAnexo"] = anexoNeg.findAll();
            if (data != null)
            {
                string dar = data["gridLookupTipoAnexo$State"];
                string ver = HttpUtility.HtmlDecode(dar);
                if (ver != null)
                {
                    Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                    // Array codigo = nodes["selectedKeyValues"] ;
                    if (nodes.selectedKeyValues != null)
                    {
                        string tipoAnexo = nodes.selectedKeyValues[0];

                        GridViewHelper.TipoAnexoBanco = tipoAnexo;

                        ViewData["Anexo"] = anexoNeg.findAllAnexo(GridViewHelper.TipoAnexoBanco);
                    }
                }
                else
                {
                    GridViewHelper.TipoAnexoBanco = "";
                }

            }
            if (TIPOANEX_CODIGO == "-1")
                TIPOANEX_CODIGO = "-1";
            else
            {
                Anexo ANEXO = anexoNeg.findAnexo(TIPOANEX_CODIGO);
                TIPOANEX_CODIGO = ANEXO.TIPOANEX_CODIGO;
                GridViewHelper.TipoAnexoBanco = TIPOANEX_CODIGO;
            }
            return PartialView("MultiSelectTipoAnexo", new TipoAnexo() { TIPOANEX_CODIGO = TIPOANEX_CODIGO });

        }

        public ActionResult MultiSelectAnexo(string ANEX_CODIGO = "-1")
        {
            if (GridViewHelper.TipoAnexoBanco == "" && ANEX_CODIGO =="-1")
            {
                ViewData["Anexo"] = new List<Anexo>();
            }
            else
            {
                ViewData["Anexo"] = anexoNeg.findAllAnexo(GridViewHelper.TipoAnexoBanco);
            }


            if (ANEX_CODIGO == "-1")
                ANEX_CODIGO = "-1";
            return PartialView("MultiSelectAnexo", new Anexo() { ANEX_CODIGO = ANEX_CODIGO });

        }



        public ActionResult MultiSelectMoneda(FormCollection data, string COVMON_CODIGO = "-1")
        {
            
            List<Moneda> monedas= comprobanteNeg.findAllMonedas();
            ViewData["moneda"] = monedas;
            if (data != null)
            {
                string dar = data["gridLookupMoneda$State"];
                string ver = HttpUtility.HtmlDecode(dar);
                if (ver != null)
                {
                    Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);                  
                    if (nodes.selectedKeyValues != null)
                    {
                        string codigo= nodes.selectedKeyValues[0];
                        GridViewHelper.tipoCambioBanco = codigo;               
                    }
                }

            }

            if (COVMON_CODIGO == "-1")
                COVMON_CODIGO = "-1";
            else
            {
                GridViewHelper.tipoCambioBanco = COVMON_CODIGO;
            }
            return PartialView("MultiSelectMoneda", new Moneda() { COVMON_CODIGO = COVMON_CODIGO });

        }

        public ActionResult MultiSelectEstadosOperaciones(string CB_C_CODIG = "-1")
        {
            ViewData["Estados"] = cajaBancoNeg.findAllTipoEstadosOperaciones("E");
            if (CB_C_CODIG == "-1")
                CB_C_CODIG = "-1";
            return PartialView("MultiSelectEstadosOperaciones", new TipoEstadoOperacion() { CB_C_CODIG = CB_C_CODIG });

        }
        public ActionResult MultiSelectTiposMovimientos(string CB_C_CODIG = "-1")
        {
            ViewData["tiposMovimientos"] = cajaBancoNeg.findAllTipoMovimientos(GridViewHelper.TipoOpcion);
            if (CB_C_CODIG == "-1")
                CB_C_CODIG = "-1";
            return PartialView("MultiSelectTiposMovimientos", new TipoMovimientos() { CB_C_CODIG = CB_C_CODIG });

        }
        public ActionResult MultiSelectMedioPago(string CODIGO = "-1")
        {
            ViewData["MedioPago"] = cajaBancoNeg.findAllMedioPago();
            if (CODIGO == "-1")
                CODIGO = "-1";
            return PartialView("MultiSelectMedioPago", new MedioPago() { CODIGO = CODIGO });

        }
        // cambio de  operacion
        public JsonResult changeOperacion(string codigoOperacion)
        {

            TipoOpcionCajaBanco operacion = cajaBancoNeg.findTipoOpciones(GridViewHelper.TipoOpcion, codigoOperacion);
            string cadena = cajaBancoNeg.Busca_Gen("OPLABCOCOB");
            bool activar = true;
            if (GridViewHelper.TipoOpcion == "I" && cadena == codigoOperacion)
            {
                activar = false;
            }

            return Json(new { respuesta = activar }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Initsecuencia()
        {

            string secuencia = cajaBancoNeg.Genera_Secuencia(GridViewHelper.codigobanco, GridViewHelper.dateTime);
            return Json(new { respuesta = secuencia }, JsonRequestBehavior.AllowGet);
        }
        // detalles de Dmov
        public JsonResult initSecuenciaDetalle(string secuencia)
        {

            string secuenciaDetalle = cajaBancoNeg.Genera_Secuencia_detalle(GridViewHelper.codigobanco, GridViewHelper.dateTime, secuencia);
            return Json(new { respuesta = secuenciaDetalle }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MultiSelectConceptoCajaBanco(FormCollection data, string CB_C_CODIG = "-1")
        {
            string opcion = GridViewHelper.TipoOpcion;

            List<ConceptoCajaBanco>  list= cajaBancoNeg.findAllConceptoCajaBanco("B", opcion,GridViewHelper.operacionbanco, GridViewHelper.salidaentrada);
            ViewData["conceptos"] = list;

            if (data != null)
            {
                string dar = data["gridLookupConceptoCajaBanco$State"];
                string ver = HttpUtility.HtmlDecode(dar);
                if (ver != null)
                {
                    Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                    // Array codigo = nodes["selectedKeyValues"] ;
                    if (nodes.selectedKeyValues != null)
                    {
                        GridViewHelper.conceptoCajaBanco = nodes.selectedKeyValues[0];
                    }
                }

            }

            if (CB_C_CODIG == "-1")
                CB_C_CODIG = "";
            return PartialView("MultiSelectConceptoCajaBanco", new ConceptoCajaBanco() { CB_C_CODIG = CB_C_CODIG });
        }

        public ActionResult MultiSelectTipoAnexoD(FormCollection data, string TIPOANEX_CODIGO = "-1")
        {

            ViewData["TipoAnexoD"] = anexoNeg.findAll();
            if (data != null)
            {
                string dar = data["gridLookupTipoAnexoD$State"];
                string ver = HttpUtility.HtmlDecode(dar);
                if (ver != null)
                {
                    Trans nodes = JsonConvert.DeserializeObject<Trans>(ver);
                    // Array codigo = nodes["selectedKeyValues"] ;
                    if (nodes.selectedKeyValues != null)
                    {
                        string tipoAnexo = nodes.selectedKeyValues[0];

                        GridViewHelper.TipoAnexoBancoDetalle = tipoAnexo;

                        ViewData["AnexoD"] = anexoNeg.findAllAnexo(GridViewHelper.TipoAnexoBanco);
                    }
                }
                else
                {
                    GridViewHelper.TipoAnexoBancoDetalle = "";
                }

            }
            if (TIPOANEX_CODIGO == "-1")
                TIPOANEX_CODIGO = "-1";
            else
            {
                Anexo ANEXO = anexoNeg.findAnexo(TIPOANEX_CODIGO);
                TIPOANEX_CODIGO = ANEXO.TIPOANEX_CODIGO;
                GridViewHelper.TipoAnexoBanco = TIPOANEX_CODIGO;
            }
            return PartialView("MultiSelectTipoAnexoD", new TipoAnexo() { TIPOANEX_CODIGO = TIPOANEX_CODIGO });

        }

        public ActionResult MultiSelectAnexoD(string ANEX_CODIGO = "-1")
        {
            if (GridViewHelper.TipoAnexoBancoDetalle == "")
            {
                ViewData["AnexoD"] = new List<Anexo>();
            }
            else
            {
                ViewData["AnexoD"] = anexoNeg.findAllAnexo(GridViewHelper.TipoAnexoBanco);
            }


            if (ANEX_CODIGO == "-1")
                ANEX_CODIGO = "-1";
            return PartialView("MultiSelectAnexoD", new Anexo() { ANEX_CODIGO = ANEX_CODIGO });

        }
        public ActionResult MultiSelectTipoDocD(string TIPDOC_CODIGO = "-1")
        {
            ViewData["TipoDocD"] = anexoNeg.findAllTipoDocumento();
            if (TIPDOC_CODIGO == "-1")
                TIPDOC_CODIGO = "";
            return PartialView("MultiSelectTipoDocD", new TipoDocumento() { TIPDOC_CODIGO = TIPDOC_CODIGO });

        }
        public ActionResult MultiSelectTipoMoneda(string TIPOMON_CODIGO = "-1")
        {
            ViewData["tipoMoneda"] = cajaBancoNeg.tipoMonedas();
            if (TIPOMON_CODIGO == "-1")
                TIPOMON_CODIGO = "";
            return PartialView("MultiSelectTipoMoneda", new TipoMoneda() { TIPOMON_CODIGO = TIPOMON_CODIGO });

        }
        public ActionResult MultiSelectCuenta(string PLANCTA_CODIGO = "-1", FormCollection dataR = null)
        {
            ViewData["cuenta"] = comprobanteNeg.findAllCuentasNacionales(GridViewHelper.NivelCOntable);

            if (PLANCTA_CODIGO == "-1")
                PLANCTA_CODIGO = "";
            return PartialView("MultiSelectCuenta", new PlanCuentaNacional() { PLANCTA_CODIGO = PLANCTA_CODIGO });

        }
        public ActionResult MultiSelectCuentaDestino(string PLANCTA_CODIGO = "-1", FormCollection dataR = null)
        {
            ViewData["cuentaDestino"] = comprobanteNeg.findAllCuentasNacionales(GridViewHelper.NivelCOntable);

            if (PLANCTA_CODIGO == "-1")
                PLANCTA_CODIGO = "";
            return PartialView("MultiSelectCuentaDestino", new PlanCuentaNacional() { PLANCTA_CODIGO = PLANCTA_CODIGO });

        }

        public JsonResult grabar(string data)
        {
            CMovimientoBanco nodes = JsonConvert.DeserializeObject<CMovimientoBanco>(data);
            nodes.CB_C_BANCO = GridViewHelper.codigobanco;
            nodes.CB_C_CONTA = GridViewHelper.codigoContabilidad;
            nodes.CB_C_MES = GridViewHelper.dateTime.Month.ToString("00.##");
            bool secuencia = false;

            cajaBancoNeg.create(nodes, nodes.CB_C_BANCO, GridViewHelper.dateTime, GridViewHelper.monedabanco);
            secuencia = true;

            return Json(new { respuesta = secuencia }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult OnChangeConcepto()
        {
            string codigo = GridViewHelper.conceptoCajaBanco;
            string opcion = GridViewHelper.TipoOpcion;
            List<ConceptoCajaBanco>  conceptoCajaBancos= cajaBancoNeg.findAllConceptoCajaBanco("B", opcion, GridViewHelper.operacionbanco, GridViewHelper.salidaentrada);
            ConceptoCajaBanco d = conceptoCajaBancos.Find(X => X.CB_C_CODIG == codigo);
            string cuenta = "";
            if (d != null)
            {
                cuenta = d.CB_C_CUENT;
            }        
            return Json(new { respuesta = cuenta }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult OnChangeMoneda()
        {
            bool realony = true;
            string tipocambio = GridViewHelper.tipoCambioBanco;
            if (tipocambio == "ESP")
            {
                realony = false;
            }
              return Json(new { respuesta = realony }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult initTipoMoneda()
        {
            string tipoMoneda = "";
            tipoMoneda = GridViewHelper.monedabanco;
          
            return Json(new { respuesta = tipoMoneda }, JsonRequestBehavior.AllowGet);
        }
        

        public JsonResult combrobarPlanilla(string codigoOperacion)
        {

            FlagForForm flagForForm = new FlagForForm();
            string cadena = cajaBancoNeg.Busca_Gen("OPLABCOCOB");
            string cadena2 = cajaBancoNeg.Busca_Gen("OPERPLACON");
            string cadena3 = cajaBancoNeg.Busca_Gen("OPLABCOPAG");
            List<string> Opagar;

            if (GridViewHelper.TipoOpcion == "I" && cadena == codigoOperacion)
            {
                flagForForm.frmLisPlaniCobranza = true;
            }
            if (GridViewHelper.TipoOpcion == "I" && cadena2 == codigoOperacion)
            {
                if (cajaBancoNeg.exiteFactura(GridViewHelper.dateTime))
                {
                    flagForForm.frmselecfacont = true;
                }
            }
#pragma warning disable CS0219 // La variable 'SW' está asignada pero su valor nunca se usa
            bool SW = false;
#pragma warning restore CS0219 // La variable 'SW' está asignada pero su valor nunca se usa
            if (GridViewHelper.TipoOpcion == "S")
            {
                Opagar = cadena3.Split(';').ToList();
                Opagar.ForEach(X =>
                {
                    string valor = X;
                    if (valor == codigoOperacion)
                    {
                        SW = true;
                    }
                });
                bool darle = Boolean.Parse(cajaBancoNeg.verDataProgramacion(GridViewHelper.dateTime));
                if (darle)
                {
                    flagForForm.frmCtasxPagarPrueba1 = true;
                }
                else
                {
                    flagForForm.frmCtasxPagarPrueba2 = true;
                }
            }


            return Json(new { respuesta = flagForForm }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult cambiarES()
        {
            bool salidaentrada = GridViewHelper.salidaentrada= !GridViewHelper.salidaentrada;
           
            return Json(new { respuesta = salidaentrada }, JsonRequestBehavior.AllowGet);
        }
        

        #endregion

    }



}