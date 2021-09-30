﻿using Newtonsoft.Json;
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
   
    public class MovimientoBancoController : BaseController
    {
        private OrdenCompraNeg userNeg;
        private RequisicionCompraNeg requisicionNeg;
        private ProveedorNeg proveedorNeg;
        private ArticuloNeg articuloNeg;
        private AreaNeg areaNeg;
        private ResponsableCmpNeg responsable;
        private RequisicionCompra requisicionCompra;
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

            GridViewHelper.movimientoBancosdetalles.Clear();
            if (GridViewHelper.activeData)
            {
                // movimientoBancos = cajaBancoNeg.findAllMovimientos(GridViewHelper.codigobanco, "MN");
                if (GridViewHelper.movimientoBancos.Count == 0)
                {
                    GridViewHelper.movimientoBancos = cajaBancoNeg.findAllMovimientos(GridViewHelper.codigobanco, GridViewHelper.monedabanco);
                }
                movimientoBancos = GridViewHelper.movimientoBancos;
                movimientoBancos = movimientoBancos.Where(X => X.CB_D_FECCA >= GridViewHelper.dateRangeBanco.Start && X.CB_D_FECCA <= GridViewHelper.dateRangeBanco.End).ToList();
            }
           
            return View(movimientoBancos);
        }
        public ActionResult DataRequisicionPartial()
        {
            //Sec-Fetch-Mode
            GridViewHelper.movimientoBancosdetalles.Clear();
            List<CMovimientoBanco> movimientoBancos = new List<CMovimientoBanco>(); ;
            if (GridViewHelper.activeData)
            {
                if (GridViewHelper.movimientoBancos.Count == 0)
                {
                    GridViewHelper.movimientoBancos = cajaBancoNeg.findAllMovimientos(GridViewHelper.codigobanco, GridViewHelper.monedabanco);
                }
                movimientoBancos = GridViewHelper.movimientoBancos;
                movimientoBancos = movimientoBancos.Where(X => X.CB_D_FECCA >= GridViewHelper.dateRangeBanco.Start && X.CB_D_FECCA <= GridViewHelper.dateRangeBanco.End).ToList();
            
            }
            return PartialView("DataRequisicionPartial", movimientoBancos);
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
            product.CB_C_SECUE =  ( GridViewHelper.movimientoBancos.Count+1).ToString("0000.##");

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
            GridViewHelper.movimientoBancos.Add(product)  ;
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

        public void UpdateProduct(MovimientoBanco product)
        {
          //  requisicionNeg.update(product);

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
                    CMovimientoBanco movimientoBanco=  GridViewHelper.movimientoBancos.Find(X => X.CB_C_SECUE == codigo);

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
                return PartialView("Detail", GridViewHelper.movimientoBancosdetalles);
            }
            GridViewHelper.detalleRequisicions = requisicionNeg.findAllDetail(requisicionCompra.NROREQUI);
           // return PartialView("Detail", GridViewHelper.detalleRequisicions);
            return PartialView("Detail", GridViewHelper.movimientoBancosdetalles);
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
                return PartialView("DetailRequestPartial", GridViewHelper.movimientoBancosdetalles);
            }
            GridViewHelper.detalleRequisicions = requisicionNeg.findAllDetail(requisicionCompra.NROREQUI);
            return PartialView("DetailRequestPartial", GridViewHelper.movimientoBancosdetalles);
        }
        public ActionResult DetailRequestAddNewPartial(DMovimientoBanco product , FormCollection dataForm)
        {
            // product.CENCOST = arraycosto.First.ToString();

            product.CB_C_BANCO = GridViewHelper.codigobanco;
            product.CB_C_MES = GridViewHelper.dateTime.Month.ToString("00.##");
            product.CB_C_SECUE = GridViewHelper.dateTime.Month.ToString("00.##");
            product.CB_C_CONCE = GridViewHelper.ValidarRecuperar(dataForm["gridLookupConceptoCajaBanco$State"]);
         
            product.CB_C_ANEXO= GridViewHelper.ValidarRecuperar(dataForm["gridLookupTipoAnexoD$State"]);
            product.CB_C_TPDOC = GridViewHelper.ValidarRecuperar(dataForm["gridLookupTipoDocD$State"]);
            product.CB_C_CENCO = GridViewHelper.ValidarRecuperar(dataForm["gridLookupCostos$State"]);
            product.CB_C_CUENT = GridViewHelper.ValidarRecuperar(dataForm["gridLookupCuenta$State"]);
            product.CB_C_DESTI = GridViewHelper.ValidarRecuperar(dataForm["gridLookupCuentaDestino$State"]);
           
            string moneda= GridViewHelper.ValidarRecuperar(dataForm["gridLookupTipoMoneda$State"]);
            if (moneda=="MN")
            {
                product.CB_N_MTOMND = product.CB_N_MTOMND;

            }
            else
            {
                product.CB_N_MTOMED = product.CB_N_MTOMND;

            }




            if (ModelState.IsValid)
                SafeExecute(() => InsertProduct(product));
            else
                ViewData["EditError"] = "Please, correct all errors.";
            var data = ViewData["codigoOrden"];
            if (data == null)
            {
                return DetailRequestPartial("-1");
            }
            return DetailRequestPartial("-1");
        }
        public void InsertProduct(DMovimientoBanco product)
        {
            GridViewHelper.movimientoBancosdetalles.Add(product);
        }

        [ValidateInput(false)]
        public ActionResult detailRequestUpdatePartial(CMovimientoBanco product, FormCollection dataForm)
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
        public void UpdateProduct(CMovimientoBanco product)
        {

            CMovimientoBanco detalleOrdenCompra = GridViewHelper.movimientoBancos.Find(element => element.CB_C_SECUE == product.CB_C_SECUE);
           
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
                            DateTime date = DateTime.Now;
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
        public ActionResult MultiSelectOpciones( FormCollection data,string CB_C_CODIG = "-1")
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
                        GridViewHelper.monedabanco = cajaBanco.CB_C_MONED;
                        GridViewHelper.activeData = true;
                        DateTime date = DateTime.Now;
                        int nrodias = DateTime.DaysInMonth(date.Year, date.Month);
                        GridViewHelper.dateRangeBanco.Start = new DateTime(date.Year, date.Month, 1);
                        GridViewHelper.dateRangeBanco.End = new DateTime(date.Year, date.Month, nrodias);

                    }
                }

            }
                if (CB_C_CODIG == "-1")
                CB_C_CODIG = "";
            return PartialView("MultiSelectOpciones", new TipoOpcionCajaBanco() { CB_C_CODIG = CB_C_CODIG });

        }
        // determinar si es un ingreso o egreso
        public JsonResult changeTipo(int tipo)
        {
            GridViewHelper.TipoOpcion= tipo;

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
        public ActionResult MultiSelectTipoAnexo(FormCollection data,string TIPOANEX_CODIGO = "-1")
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
            return PartialView("MultiSelectTipoAnexo", new TipoAnexo() { TIPOANEX_CODIGO = TIPOANEX_CODIGO });

        }

        public ActionResult MultiSelectAnexo(string ANEX_CODIGO = "-1")
        {
            if (GridViewHelper.TipoAnexoBanco=="")
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



        public ActionResult MultiSelectMoneda(string COVMON_CODIGO = "-1")
        {
             ViewData["moneda"] = comprobanteNeg.findAllMonedas();
            if (COVMON_CODIGO == "-1")
                COVMON_CODIGO = "-1";
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
           if (GridViewHelper.TipoOpcion==0 && cadena== codigoOperacion)
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
        
        public ActionResult MultiSelectConceptoCajaBanco(string CB_C_CODIG = "-1")
        {
            string opcion = GridViewHelper.TipoOpcion == 0 ? "I" : "S";

            ViewData["conceptos"] = cajaBancoNeg.findAllConceptoCajaBanco("B", opcion);
            if (CB_C_CODIG == "-1")
                CB_C_CODIG = "";
            return PartialView("MultiSelectConceptoCajaBanco", new ConceptoCajaBanco() { codigo = CB_C_CODIG });
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
       

        #endregion

    }



}