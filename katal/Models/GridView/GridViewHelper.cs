using DevExpress.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using katal.conexion.model.entity;
using katal.conexion.model.neg;
using System.Web;
using Newtonsoft.Json;
using System;

namespace katal.Model
{
    public static class GridViewHelper
    {


        public  static ApplicationUser user = new ApplicationUser();// solucion temporal
        public  static List<DetalleOrdenCompra> detalles = new List<DetalleOrdenCompra>();

        public static List<OrdenCompra> OrdenCompras = new List<OrdenCompra>();
        public static List<Comprobante> Comprobantes= new List<Comprobante>();
        public static List<ComprobanteDetraccion> ComprobantesD= new List<ComprobanteDetraccion>();
        public static List<ContableDet> contableDets= new List<ContableDet>();

        public static List<DetalleRequisicion> detalleRequisicions = new List<DetalleRequisicion>();
        public static List<RequisicionCompra> requisicionCompras = new List<RequisicionCompra>();
        public static string OC_CDOCREF ="";
        public static string NROREQUI = "";
        public static string Gastos_Codigo = "";
        public static string TIPDOC_CODIGO = "";
        public static string COVMON_CODIGO = "";
        public static string PLANCTA_CODIGO = "";
        
        public static string RUC = "";

        public static string CO_C_CODIG = "";
        public static string BD = "";
        public static string COMP_CORDEN = "";
        public static string COMP_TIPODOCU_CODIGO = "";
        public static string COMP_CSERIE = "";
        public static string COMP_CNUMERO = "";
        public static int NivelCOntable = 0;
        public static decimal tasa = 0;
        public static decimal THaber = 0;
        public static decimal TDebe = 0;
      
        public static bool activarRetecion = false;
        public static bool respuesta = false;
        public static bool wlConten = true;// activar desde comprobante
       
        public static Comprobante comprobante = new Comprobante();
        // caja banco
        public static bool activeData=false;
        public static bool activeDataConsulta=false;
        public static bool activeBancoAnexo=true;
        public static bool activeBancoTipoAnexo= true;
        public static string codigobanco = "";
        public static string codigoContabilidad = "";
        public static string monedabanco = "";
        public static int TipoOpcion = 0;
        public static string TipoAnexoBanco = "";
        public static string TipoAnexoBancoDetalle = "";

        public static DateRangePickerModel dateRange = new DateRangePickerModel();
        public static DateRangePickerModel dateRangeBanco = new DateRangePickerModel();
        public static DateTime dateTime = DateTime.Now;
        
        // SOLO 
        public static List<CMovimientoBanco> movimientoBancos= new List<CMovimientoBanco>();
        public static List<DMovimientoBanco> movimientoBancosdetalles = new List<DMovimientoBanco>();
        //detallemovbaco

        public static string conceptoCajaBanco = "";//codigo

        

        public static void GetDetalles()
        {
            if (detalles == null)
            {
                detalles = new List<DetalleOrdenCompra>();
            }

        }
        public static void ClearDetalles()
        {
           
                detalles = new List<DetalleOrdenCompra>();    
        }
        public static void GetOrdenCompras()
        {
            if (OrdenCompras == null)
            {
                OrdenCompras = new List<OrdenCompra>();
            }
        }

        public static void GetDetallesRequision()
        {
            if (detalleRequisicions == null)
            {
                detalleRequisicions = new List<DetalleRequisicion>();
            }

        }
        public static void ClearDetallesRequision()
        {

            detalleRequisicions = new List<DetalleRequisicion>();
        }
        public static void GetRequisionComprasObjetc()
        {
            if (requisicionCompras == null)
            {
                requisicionCompras = new List<RequisicionCompra>();
            }
        }

        public static List<RequisicionCompra> GetRequisionCompras()
        {
            RequisicionCompraNeg userNeg = new RequisicionCompraNeg(user.codEmpresa);
            return userNeg.findAll();
        }
        public static List<CentroCosto> GetCentroCostos()
        {
            RequisicionCompraNeg userNeg = new RequisicionCompraNeg(user.codEmpresa);
            return userNeg.findAllCentroCostos();
        }
        public static List<Issue> GetIssues()
        {
            UserNeg userNeg = new UserNeg();
            return DataProvider.GetIssues();
        }

        public static List<User> getUsers()
        {
            UserNeg userNeg = new UserNeg();
            return userNeg.findAll();
        }

        public static List<Empresa> getEmpresas()
        {
            EmpresaNeg userNeg = new EmpresaNeg();
            return userNeg.findAll();
        }
        public static List<Proveedor> getProveedor()
        {
            ProveedorNeg userNeg = new ProveedorNeg(user.codEmpresa);
            return userNeg.findAll();
        }

        public static List<Articulo> getArticulos()
        {
            ArticuloNeg  userNeg = new ArticuloNeg(user.codEmpresa);
            return userNeg.findAll();
        }
        public static List<Solicitud> GetSolitud()
        {
            OrdenCompraNeg userNeg = new OrdenCompraNeg(user.codEmpresa);
            return userNeg.findAllSolicitud();
        }
        public static List<FormaPago> GetFormaPago()
        {
            OrdenCompraNeg userNeg = new OrdenCompraNeg(user.codEmpresa);
            return userNeg.findAllFormasPago();
        }

        public static List<Contact> GetCustomers()
        {
            return DataProvider.GetContacts();
        }
        public static GridViewModel GetGridViewModel()
        {
            return new GridViewModel();
        }
        public static void AddNewRecord(User issue)
        {
            UserNeg userNeg = new UserNeg();
            userNeg.create(issue);
        }

        public static void UpdateRecord(User issue)
        {
            UserNeg userNeg = new UserNeg();
            userNeg.update(issue);
        }

        public static void DeleteRecords(string selectedRowIds)
        {
            UserNeg userNeg = new UserNeg();
            List<int> selectedIds = selectedRowIds.Split(',').ToList().ConvertAll(id => int.Parse(id));
            userNeg.DeleteUser(selectedIds);
        }

        public static string ValidarRecuperar(string data)
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

    }
}