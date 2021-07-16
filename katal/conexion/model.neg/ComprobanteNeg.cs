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
        public ComprobanteNeg()
        {
            string bd = "[014BDCOMUN].[dbo].[COMPROBANTECAB]";// modificar
            objComprobanteDao = new ComprobanteDao(bd);
        }
        public List<Comprobante> findAll()
        {
            return objComprobanteDao.findAll();
        }
        public List<Gasto> findAllGastos(){
            return objComprobanteDao.findAllGastos();
        }
    }
}