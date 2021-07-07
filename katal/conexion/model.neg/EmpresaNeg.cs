using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using katal.conexion.model.dao;
using katal.conexion.model.entity;

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
    }
}