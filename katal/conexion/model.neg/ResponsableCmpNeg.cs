using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using katal.conexion.model.dao;
using katal.conexion.model.entity;

namespace katal.conexion.model.neg
{
    public class ResponsableCmpNeg
    {
        ResponsableCompraDao objResponsableDao;
        public ResponsableCmpNeg()
        {
            objResponsableDao = new ResponsableCompraDao();
        }
        public List<ResponsableCompra> findAll()
        {
            return objResponsableDao.findAll();
        }
        public void create(ResponsableCompra objUser)
        {

            //validar Nombre Alumno estado=2
            string nombre = objUser.RESPONSABLE_CODIGO;
            if (nombre == null)
            {
                return;
            }
            objResponsableDao.create(objUser);
        }

        public bool find(ResponsableCompra objAlumno)
        {
            return objResponsableDao.find(objAlumno);
        }
        public ResponsableCompra find(string codigo)
        {
            return objResponsableDao.find(codigo);
        }

        public void update(ResponsableCompra objUser)
        {
            string nombre = objUser.RESPONSABLE_CODIGO;
            if (nombre == null)
            {
                return;
            }
            objResponsableDao.update(objUser);

        }

        public void delete(ResponsableCompra obj)
        {
            bool verificacion;
            ResponsableCompra objUserAux = new ResponsableCompra();
            objUserAux.RESPONSABLE_CODIGO = obj.RESPONSABLE_CODIGO;
            verificacion = objResponsableDao.find(objUserAux);
            if (!verificacion)
            {
                return;
            }
            objResponsableDao.delete(obj);
        }
        public void delete(string RESPONSABLE_CODIGO)
        {
            bool verificacion;
            ResponsableCompra objUserAux = new ResponsableCompra();
            objUserAux.RESPONSABLE_CODIGO = RESPONSABLE_CODIGO;
            verificacion = objResponsableDao.find(objUserAux);
            if (!verificacion)
            {
                return;
            }
            objResponsableDao.delete(RESPONSABLE_CODIGO);
        }
    }
}