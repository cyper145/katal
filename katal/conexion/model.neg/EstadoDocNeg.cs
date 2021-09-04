using katal.conexion.model.entity;
using katal.conexion.model.dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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