using katal.conexion.model.dao;
using katal.conexion.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.neg
{
    public class ComprobanteNeg
    {
        ComprobanteDao objComprobanteDao;
        TipoAnexoDao anexoDao;
        MonedaDao monedaDao;
        TipoOperacionDao TipoOperacion;
        public ComprobanteNeg()
        {
            string bd = "014";// modificar
            objComprobanteDao = new ComprobanteDao(bd);
            anexoDao = new TipoAnexoDao(bd);
            monedaDao = new MonedaDao(bd);
            TipoOperacion = new TipoOperacionDao(bd);
        }

        public void create(Comprobante comprobante)
        {
            objComprobanteDao.create(comprobante);
        }
        public void create(Comprobante comprobante, int nivelContable)
        {
            objComprobanteDao.create(comprobante, nivelContable);
        }
        public void Tranferir(Comprobante comprobante, int nivelContable)
        {
            objComprobanteDao.transferir(comprobante, nivelContable);
        }
        public List<Comprobante> findAll()
        {
            return objComprobanteDao.findAll();
        }
        public Comprobante findAllConta(string sCorrelativo, string TIPODOCU_CODIGO, string CSERIE, string CNUMERO)
        {
            return objComprobanteDao.findAllConta(sCorrelativo, TIPODOCU_CODIGO, CSERIE, CNUMERO);
        }
        public List<PlanCuentaNacional> findAllCuentasNacionales(int NivelContable)
        {
            return objComprobanteDao.findAllCuentasNacionales(NivelContable);
        }

        public List<OrdenFabricacion> findAllOrdenFabricacion()
        {
            return objComprobanteDao.findAllOrdenFabricacion();
        }
        public List<Maquinas> findAllMaquinas()
        {
            return objComprobanteDao.findAllMaquinas();
        }

        public List<Gasto> findAllGastos() {
            return objComprobanteDao.findAllGastos();
        }
        public gastoTipoAnexo cargarChangeTipoGasto(string codigo)
        {
            gastoTipoAnexo gastoTipoAnexo = new gastoTipoAnexo();
            Gasto gasto = objComprobanteDao.findAllGastosDetail(codigo);
            string codigoAnexo = anexoDao.findAllDetail(gasto.Gastos_CuentaCon);
            gastoTipoAnexo.gasto = gasto;
            gastoTipoAnexo.codigoTipoAnexo = codigoAnexo;
            return gastoTipoAnexo;
        }
        public  string cargarAnexoChangeTipoGasto(string codigo)
        {
            gastoTipoAnexo gastoTipoAnexo = new gastoTipoAnexo();
            Gasto gasto = objComprobanteDao.findAllGastosDetail(codigo);
            string codigoAnexo = anexoDao.findAllDetail(gasto.Gastos_CuentaCon);         
            return codigoAnexo;
        }
        public List<Moneda> findAllMonedas()
        {
            return monedaDao.findAll();
        }
        public TipoCambio findTipoCambio(string dateTime)
        {
            return monedaDao.findTipoCambio(dateTime);
        }

        public List<ServSujDetraccion> findAllDetraccion(string dateEmision)
        {
            return TipoOperacion.findAllDetraccion(dateEmision);
        }
        public List<TipoOperacion> findAllTipoOperacion()
        {
            return TipoOperacion.findAllTipoOperacion();
        }

        public decimal  tasa()
        {           
            return  Conversion.ParseDecimal(objComprobanteDao.ConceptosGenerales("TASAIGV"));
        }
        public bool habilitarRetencion()
        {
            if (objComprobanteDao.ExiteConceptos())
            {
                return objComprobanteDao.ExitedataConceptos();
            }
            else
            {
                return false;
            }
        }
        public string funcAutoNum()
        {
            DateTime date = DateTime.Now;
            string anios = date.Year.ToString("0000.##");
            string mes = date.Month.ToString("00.##");
            string msAnoMesProc = anios + mes;
            return objComprobanteDao.funcAutoNum(msAnoMesProc);
        }

        public void inserdataTemporal(Comprobante comprobante)
        {
            objComprobanteDao.insertdetalleTemporal(comprobante);
        }

        public List<ContableDet> findallContableDet()
        {
           return  objComprobanteDao.findallContableDet();
        }
    }
}