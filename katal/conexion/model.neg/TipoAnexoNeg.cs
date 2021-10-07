using katal.conexion.model.dao;
using katal.conexion.model.entity;
using System.Collections.Generic;

namespace katal.conexion.model.neg
{
    public class TipoAnexoNeg
    {
        TipoAnexoDao objAreaDao;
        public TipoAnexoNeg(string codEmpresa)
        {
            objAreaDao = new TipoAnexoDao(codEmpresa);
        }
        public List<TipoAnexo> findAll()
        {
            return objAreaDao.findAll();
        }
        public List<Anexo> findAllAnexo(string codigo = "")
        {
            return objAreaDao.findAllAnexo(codigo);
        }
        public Anexo findAnexo(string codigo)
        {
            return objAreaDao.findAnexo(codigo);
        }
        public List<TipoDocumento> findAllTipoDocumento()
        {
            return objAreaDao.findAllTipoDocumento();
        }
        public TipoDocumento findTipoDocumento(string codigo)
        {
            return objAreaDao.findTipoDocumento(codigo);
        }
    }
}