using katal.conexion.model.dao;
using katal.conexion.model.entity;
using System.Collections.Generic;

namespace katal.conexion.model.neg
{
    public class EstadoDocNeg
    {

        EstadoDocDao objAreaDao;
        public EstadoDocNeg(string codEmpresa)
        {
            objAreaDao = new EstadoDocDao(codEmpresa);
        }

        public List<EstadoDoc> findAll()
        {
            return objAreaDao.findAll();
        }
    }
}