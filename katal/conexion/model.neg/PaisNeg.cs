using katal.conexion.model.dao;
using katal.conexion.model.entity;
using System.Collections.Generic;
namespace katal.conexion.model.neg
{
    public class PaisNeg
    {
        private PaisDao objUserDao;
        public PaisNeg(string codEmpresa)
        {
            objUserDao = new PaisDao(codEmpresa);
        }
        public List<Pais> findAll()
        {
            return objUserDao.findAll();
        }
    }
}