using katal.conexion.model.dao;
using katal.conexion.model.entity;
using System.Collections.Generic;

namespace katal.conexion.model.neg
{
    public class EmpresaNeg
    {
        EmpresaDao objRoleDao;
        public EmpresaNeg()
        {
            objRoleDao = new EmpresaDao();
        }

        public List<Empresa> findAll()
        {
            return objRoleDao.findAll();
        }
        public Empresa findContable(string codigoEmpresa)
        {
            return objRoleDao.findContable(codigoEmpresa);
        }
    }
}