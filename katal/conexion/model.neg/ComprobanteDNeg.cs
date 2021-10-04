using katal.Code;
using katal.conexion.model.dao;
using katal.conexion.model.entity;
using System.Collections.Generic;

namespace katal.conexion.model.neg
{
    public class ComprobanteDNeg
    {
        ComprobanteDetraccionDao objComprobanteDao;

        public ComprobanteDNeg(string codEmpresa)
        {

            objComprobanteDao = new ComprobanteDetraccionDao(codEmpresa);

        }

        public List<ComprobanteDetraccion> findAll(DateRangePickerModel dateRange)
        {
            return objComprobanteDao.findAll(dateRange);
        }
        public void updateDetail(List<ComprobanteDetraccion> obj)
        {
            ManejoArchivo manejoArchivo = new ManejoArchivo();

            manejoArchivo.GenerarTXT("dat");


            objComprobanteDao.updateDetail(obj);

        }
    }
}