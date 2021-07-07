using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using katal.conexion.model.dao;
using katal.conexion.model.entity;
namespace katal.conexion.model.neg
{
    public class PaisNeg
    {
        private PaisDao objUserDao;
        public PaisNeg()
        {
            objUserDao = new PaisDao();
        }
        public List<Pais> findAll()
        {
            return objUserDao.findAll();
        }
    }
}