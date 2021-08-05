using katal.conexion.model.dao;
using katal.conexion.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.neg
{
    public class DestinoNeg
    {
        DestinoDao objDestinoDao;
        public DestinoNeg()
        {
            objDestinoDao = new DestinoDao();
        }

        public List<Destino> findAll()
        {
            return objDestinoDao.findAll();
        }

        public Destino find(string codigo)
        {
            return objDestinoDao.find(codigo);
        }
    }
}