using katal.conexion.model.dao;
using katal.conexion.model.entity;
using System.Collections.Generic;

namespace katal.conexion.model.neg
{
    public class AreaNeg
    {
        AreaDao objAreaDao;
        public AreaNeg(string codEmpresa)
        {
            objAreaDao = new AreaDao(codEmpresa);
        }
        public List<Area> findAll()
        {
            return objAreaDao.findAll();
        }
        public void create(Area objUser)
        {

            //validar Nombre Alumno estado=2
            string nombre = objUser.AREA_CODIGO;
            if (nombre == null)
            {
                return;
            }
            objAreaDao.create(objUser);
        }

        public bool find(Area objAlumno)
        {
            return objAreaDao.find(objAlumno);
        }
        public Area find(string codigo)
        {
            return objAreaDao.find(codigo);
        }

        public void update(Area objUser)
        {
            string nombre = objUser.AREA_CODIGO;
            if (nombre == null)
            {
                return;
            }
            objAreaDao.update(objUser);

        }

        public void delete(Area obj)
        {
            bool verificacion;
            Area objUserAux = new Area();
            objUserAux.AREA_CODIGO = obj.AREA_CODIGO;
            verificacion = objAreaDao.find(objUserAux);
            if (!verificacion)
            {
                return;
            }
            objAreaDao.delete(obj);
        }
        public void delete(string AREA_CODIGO)
        {
            bool verificacion;
            Area objUserAux = new Area();
            objUserAux.AREA_CODIGO = AREA_CODIGO;
            verificacion = objAreaDao.find(objUserAux);
            if (!verificacion)
            {
                return;
            }
            objAreaDao.delete(AREA_CODIGO);
        }

    }
}