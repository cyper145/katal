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
        public ComprobanteNeg()
        {
            string bd = "014";// modificar
            objComprobanteDao = new ComprobanteDao(bd);
            anexoDao = new TipoAnexoDao(bd);
        }
        public List<Comprobante> findAll()
        {
            return objComprobanteDao.findAll();
        }
        public List<Gasto> findAllGastos(){
            return objComprobanteDao.findAllGastos();
        }
        public gastoTipoAnexo  cargarChangeTipoGasto(string codigo)
        {
            gastoTipoAnexo gastoTipoAnexo = new gastoTipoAnexo();
            Gasto gasto=  objComprobanteDao.findAllGastosDetail(codigo);
            string codigoAnexo = anexoDao.findAllDetail(gasto.Gastos_CuentaCon);
            gastoTipoAnexo.gasto = gasto;
            gastoTipoAnexo.codigoTipoAnexo = codigoAnexo;
            return gastoTipoAnexo;
        }

    }
}