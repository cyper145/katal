using katal.conexion.model.dao;
using katal.conexion.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.neg
{
    public class TipoAnexoNeg
    {
        TipoAnexoDao objAreaDao;
        public TipoAnexoNeg(string BD)
        {
            objAreaDao = new TipoAnexoDao(BD);
        }
        public List<TipoAnexo> findAll()
        {
            return objAreaDao.findAll();
        }
        public List<Anexo> findAllAnexo()
        {
            return objAreaDao.findAllAnexo();
        }
        public List<TipoDocumento> findAllTipoDocumento()
        {
            return objAreaDao.findAllTipoDocumento();
        }
    }
}