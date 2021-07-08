using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using katal.conexion.model.entity;
namespace katal.conexion.model.dao
{
    class RoleDao : Obligatorio<Role>
    {

        private Conexion objConexion;
        private SqlCommand comando;

        public RoleDao()
        {
            
            // solo en este caso
            objConexion = Conexion.saberEstado("rol");
        }
        public void create(Role obj)
        {

            string create = "insert into dk_roles(id,name,description)values('" + obj.id + "','" + obj.name + "','" + obj.description + "')";
            try
            {
                comando = new SqlCommand(create, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        public void delete(Role obj)
        {
            string delete = "delete from dk_roles where id='" + obj.id + "'";
            try
            {
                comando = new SqlCommand(delete, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        public bool find(Role obj)
        {
            bool hayRegistros;
            string find = "select*from dk_roles where id='" + obj.id + "' ";
            try
            {
                comando = new SqlCommand(find, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {
                    obj.id = Convert.ToInt32(read[0].ToString());
                    obj.name = read[1].ToString();
                    obj.description = read[2].ToString();
                }
                else
                {
                    obj.estado = 1;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return hayRegistros;
        }
        public Role find(int id)
        {
            bool hayRegistros;
            string find = "select*from dk_roles where id='" + id + "' ";
            Role role = new Role();
            try
            {
                comando = new SqlCommand(find, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {
                    role.id = Convert.ToInt32(read[0].ToString());
                    role.name = read[1].ToString();
                    role.description = read[2].ToString();
                    return role;
                }
                else
                {
                    role.estado = 1;
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            
        }
        public List<Role> findAll()
        {
            List<Role> listUsers = new List<Role>();

            string findAll = "select*from dk_roles";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Role user = new Role();
                    user.id = Convert.ToInt32(read[0].ToString());
                    user.name = read[1].ToString();
                    user.description = read[2].ToString();

                    listUsers.Add(user);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return listUsers;
        }
        public void update(Role obj)
        {

            string update = "update  dk_roles set name='" + obj.name + "',description='" + obj.description + "' where id='" + obj.id + "'";
            try
            {
                comando = new SqlCommand(update, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }


        public List<ModuloAsignacion> listAsignation(int role_id)
        {
            List<ModuloAsignacion> listModuloAsignacion = new List<ModuloAsignacion>();
            string findAll = "SELECT m.*,p.* , o.* FROM  dk_modulos m INNER JOIN dk_programas p ON m.id = p.module_id INNER JOIN dk_operacion_programa op ON op.programa_id = p.id INNER JOIN dk_operaciones o ON o.id = op.operacion_id ";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                ModuloAsignacion current_module;
                ProgramaAsignacion current_programa;
                OperacionAsignacion current_operacion;
                while (read.Read())
                {

                    string name = read[1].ToString();
                    current_module = listModuloAsignacion.Find(eleme => eleme.name == name);// busca el nombre
                    if (current_module == null)
                    {
                        current_module = new ModuloAsignacion(Convert.ToInt32(read[0].ToString()), name, read[3].ToString());
                        current_programa = new ProgramaAsignacion(Convert.ToInt32(read[4].ToString()), read[5].ToString(), read[6].ToString());
                        current_operacion = new OperacionAsignacion(Convert.ToInt32(read[8].ToString()), read[9].ToString(), read[10].ToString());
                        current_programa.operacions.Add(current_operacion);
                        current_module.programas.Add(current_programa);
                        listModuloAsignacion.Add(current_module);
                    }
                    else
                    {
                        current_programa = current_module.programas.Find(x => x.id == Convert.ToInt32(read[4].ToString()));
                        if (current_programa == null)
                        {
                            current_programa = new ProgramaAsignacion(Convert.ToInt32(read[4].ToString()), read[5].ToString(), read[6].ToString());
                            current_operacion = new OperacionAsignacion(Convert.ToInt32(read[8].ToString()), read[9].ToString(), read[10].ToString());
                            current_programa.operacions.Add(current_operacion);
                            current_module.programas.Add(current_programa);
                        }
                        else
                        {
                            current_operacion = new OperacionAsignacion(Convert.ToInt32(read[8].ToString()), read[9].ToString(), read[10].ToString());
                            current_programa.operacions.Add(current_operacion);
                        }

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return listModuloAsignacion;
        }

        public string listFunciones(int role_id)
        {
            bool hayRegistros;
            string funciones = "";
            string find = "SELECT r.asignaciones FROM  dk_roles r " + "where id='" + role_id + "'";
            try
            {
                comando = new SqlCommand(find, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();

                if (hayRegistros)
                {
                    funciones = read[0].ToString();

                    return funciones;
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return funciones;
        }

        public void updateFunctiones(Role obj)
        {

            string update = "update  dk_roles set asignaciones='" + obj.funciones + "' where id='" + obj.id + "'";
            try
            {
                comando = new SqlCommand(update, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

    }

}