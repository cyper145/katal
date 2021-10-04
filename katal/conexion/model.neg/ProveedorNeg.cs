using katal.conexion.model.dao;
using katal.conexion.model.entity;
using System.Collections.Generic;

namespace katal.conexion.model.neg
{
    public class ProveedorNeg
    {
        private ProveedorDao objUserDao;
        public ProveedorNeg(string codEmpresa)
        {
            objUserDao = new ProveedorDao(codEmpresa);
        }
        public List<Proveedor> findAll()
        {
            return objUserDao.findAll();
        }

    }
}