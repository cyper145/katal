using Newtonsoft.Json;
using System.Collections.Generic;
using katal.conexion.model.dao;
using katal.conexion.model.entity;

namespace katal.conexion.model.neg
{
    public class RoleNeg
    {
        RoleDao objRoleDao;
        public RoleNeg()
        {
            objRoleDao = new RoleDao();
        }
        public List<Role> findAll()
        {
            return objRoleDao.findAll();
        }
        public void create(Role obj)
        {
            bool verificacion;

            //validar Nombre Alumno estado=2
            string nombre = obj.name;
            if (nombre == null)
            {
                obj.estado = 20;
                return;
            }
            else
            {
                nombre = obj.name.Trim();
                verificacion = nombre.Length > 0 && nombre.Length <= 50;
                if (!verificacion)
                {
                    obj.estado = 2;
                    return;
                }
            }
            //validar que no exista idAlumno repetido estado=5
            Role objRoleAux = new Role();
            objRoleAux.id = obj.id;

            verificacion = !objRoleDao.find(objRoleAux);
            if (!verificacion)
            {
                obj.estado = 5;
                return;
            }
            obj.estado = 99;
            objRoleDao.create(obj);
        }

        public bool find(Role obj)
        {
            return objRoleDao.find(obj);
        }
        public Role find(int id)
        {
            return objRoleDao.find(id);
        }

        public void update(Role obj)
        {
            bool verificacion;

            //validar Nombre Alumno estado=2
            string nombre = obj.name;
            if (nombre == null)
            {
                obj.estado = 20;
                return;
            }
            else
            {
                nombre = obj.name.Trim();
                verificacion = nombre.Length > 0 && nombre.Length <= 50;
                if (!verificacion)
                {
                    obj.estado = 2;
                    return;
                }
            }

            obj.estado = 99;
            objRoleDao.update(obj);

        }

        public void delete(Role obj)
        {
            bool verificacion;
            Role objRoleAux = new Role();
            objRoleAux.id = obj.id;
            verificacion = objRoleDao.find(objRoleAux);
            if (!verificacion)
            {
                obj.estado = 33;
                return;
            }
            obj.estado = 99;
            objRoleDao.delete(obj);
        }


        public void DeleteUser(List<int> ids)
        {
            ids.ForEach(elemet =>
            {
                delete(find(elemet));
            });

        }

        public List<ModuloAsignacion> lista(int role_id)
        {
            List<NodeModule> nodemodules = JsonConvert.DeserializeObject<List<NodeModule>>(objRoleDao.listFunciones(role_id));
            List<ModuloAsignacion> modules = objRoleDao.listAsignation(role_id);
            if (nodemodules != null)
            {

                for (int i = 0; i < modules.Count; i++)
                {
                    if (nodemodules[i].value == "C")
                    {
                        modules[i].assigned = 1;
                    }
                    if (nodemodules[i].value == "I")
                    {
                        modules[i].assigned = 0;
                    }
                    for (int j = 0; j < modules[i].programas.Count; j++)
                    {
                        if (nodemodules[i].programas[j].value == "C")
                        {
                            modules[i].programas[j].assigned = 1;
                        }
                        if (nodemodules[i].programas[j].value == "I")
                        {
                            modules[i].programas[j].assigned = 0;
                        }

                        for (int k = 0; k < modules[i].programas[j].operacions.Count; k++)
                        {
                            if (nodemodules[i].programas[j].operacions[k].value == "C")
                            {
                                modules[i].programas[j].operacions[k].assigned = 1;
                            }
                            if (nodemodules[i].programas[j].operacions[k].value == "I")
                            {
                                modules[i].programas[j].operacions[k].assigned = 0;
                            }
                        }
                    }
                }


            }
            return modules;
        }
        public void update(int rol_id, string functiones)
        {
            Role role = new Role(rol_id, functiones);
            objRoleDao.updateFunctiones(role);
        }

        public bool moduloAsigned(List<ModuloAsignacion> asignacions, string slug)
        {
            for (int i = 0; i < asignacions.Count; i++)
            {
                if (asignacions[i].assigned >= 0 && asignacions[i].slug == slug)
                {
                    return true;
                }
                for (int j = 0; j < asignacions[i].programas.Count; j++)
                {
                    if (asignacions[i].programas[j].assigned >= 0 && asignacions[i].programas[j].slug == slug)
                    {
                        return true;
                    }
                    for (int k = 0; k < asignacions[i].programas[j].operacions.Count; k++)
                    {
                        if (asignacions[i].programas[j].operacions[k].assigned >= 0 && asignacions[i].programas[j].operacions[k].slug == slug)
                        {
                            return true;
                        }
                    }
                }
            }


            return false;
        }
    }
}