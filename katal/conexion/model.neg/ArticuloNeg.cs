using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using katal.conexion.model.dao;
using katal.conexion.model.entity;
using katal.Model;

namespace katal.conexion.model.neg
{
    public class ArticuloNeg
    {

        private ArticuloDao objUserDao;
        public ArticuloNeg(string codEmpresa)
        {
            objUserDao = new ArticuloDao(codEmpresa);
        }
        public List<Articulo> findAll()
        {
            return objUserDao.findAll();
        }
        public void create(User objUser)
        {
            /*
            bool verificacion;

            //validar Nombre Alumno estado=2
            string nombre = objUser.username;
            if (nombre == null)
            {
                objUser.estado = 20;
                return;
            }
            else
            {
                nombre = objUser.username.Trim();
                verificacion = nombre.Length > 0 && nombre.Length <= 50;
                if (!verificacion)
                {
                    objUser.estado = 2;
                    return;
                }
            }
            objUser.id = int.Parse(AuthHelper.generateID());
            objUser.password = Crypto.HashPassword(objUser.password);
            //validar que no exista idAlumno repetido estado=5
            User objUserAux = new User();

            objUserAux.id = objUser.id;

            verificacion = !objUserDao.find(objUserAux);
            if (!verificacion)
            {
                objUser.estado = 5;
                return;
            }
            objUser.estado = 99;
            objUser.created_at = DateTime.Now;
            objUser.update_at = DateTime.Now;
            objUser.lastlogin = DateTime.Now;

            objUserDao.create(objUser);
            */
        }

        public bool find(User objAlumno)
        {
            //return objUserDao.find(objAlumno);
            return true;
        }
        public void find(int id)
        {
            //return objUserDao.find(id);
        }

        public void update(User objUser)
        {
            /*
            bool verificacion;

            //validar Nombre Alumno estado=2
            string nombre = objUser.username;
            if (nombre == null)
            {
                objUser.estado = 20;
                return;
            }
            else
            {
                nombre = objUser.username.Trim();
                verificacion = nombre.Length > 0 && nombre.Length <= 50;
                if (!verificacion)
                {
                    objUser.estado = 2;
                    return;
                }
            }
            objUser.password = Crypto.HashPassword(objUser.password);
            objUser.update_at = DateTime.Now;
            objUser.estado = 99;
            objUserDao.update(objUser);
            */
        }

        public void delete(User obj)
        {
            /*
            bool verificacion;
            User objUserAux = new User();
            objUserAux.id = obj.id;
            verificacion = objUserDao.find(objUserAux);
            if (!verificacion)
            {
                obj.estado = 33;
                return;
            }
            obj.estado = 99;
            objUserDao.delete(obj);
            */
        }
        public void delete(int id)
        {
            /*
            bool verificacion;
            User objUserAux = new User();
            objUserAux.id = obj.id;
            verificacion = objUserDao.find(objUserAux);
            if (!verificacion)
            {
                obj.estado = 33;
                return;
            }
            obj.estado = 99;
            objUserDao.delete(obj);
            */
        }

        public void DeleteUser(List<int> ids)
        {
            /*

            ids.ForEach(elemet =>
            {
                delete(find(elemet));
            });
            */
        }



    }
}