using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Helpers;
using katal.conexion.model.entity;
using katal.Model;

namespace katal.conexion.model.dao
{
    public class UserDao : Obligatorio<User>
    {
        private Conexion objConexion;
        private SqlCommand comando;

        public UserDao()
        {
            objConexion = Conexion.saberEstado();
        }

        public void create(User obj)
        {

            string creat = obj.created_at.ToString("MM/dd/yyyy HH:mm:ss");
            string updte = obj.update_at.ToString("MM/dd/yyyy HH:mm:ss");
            string last = obj.lastlogin.ToString("MM/dd/yyyy HH:mm:ss");

            string create = "insert into dk_users(id,username,password,email, photo,created_at,updated_at,isactive,last_login,rol_id)values('" + obj.id + "','" + obj.username + "','" + obj.password + "','" + obj.email + "','" + obj.photo + "','" + creat + "','" + updte + "','" + obj.isactive + "','" + last + "','" + obj.rol_id + "')";
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

        public void delete(User obj)
        {
            string delete = "delete from dk_users where id='" + obj.id + "'";
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

        public bool find(User user)
        {
            bool hayRegistros;
            string find = "select*from dk_users where id='" + user.id + "' ";
            try
            {
                comando = new SqlCommand(find, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {
                    user.id = Convert.ToInt32(read[0].ToString());
                    user.username = read[1].ToString();
                    user.password = read[2].ToString();
                    user.email = read[3].ToString();
                    user.photo = read[4].ToString();
                    user.created_at = DateTime.Parse(read[5].ToString());
                    user.update_at = DateTime.Parse(read[6].ToString());
                    user.isactive = Convert.ToInt32(read[8].ToString());
                    user.lastlogin = DateTime.Parse(read[9].ToString());
                    user.estado = 99;
                }
                else
                {
                    user.estado = 1;
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



        public User find(int id)
        {
            bool hayRegistros;
            string find = "select*from dk_users where id='" + id + "' ";
            User user = new User();
            try
            {
                comando = new SqlCommand(find, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {
                    user.id = Convert.ToInt32(read[0].ToString());
                    user.username = read[1].ToString();
                    user.password = read[2].ToString();
                    user.email = read[3].ToString();
                    user.photo = read[4].ToString();
                    user.created_at = DateTime.Parse(read[5].ToString());
                    user.update_at = DateTime.Parse(read[6].ToString());
                    user.isactive = Convert.ToInt32(read[8].ToString());
                    user.lastlogin = DateTime.Parse(read[9].ToString());
                    user.estado = 99;
                    return user;
                }
                else
                {
                    user.estado = 1;
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

        public List<User> findAll()
        {
            List<User> listUsers = new List<User>();
            string findAll = consultaLogin();
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    int id = Convert.ToInt32(read[0].ToString());
                    User userTemp = listUsers.Find(X => X.id == id);
                    if (listUsers.Find(X=>X.id == Convert.ToInt32(read[0].ToString())) == null)
                    {
                        User user = new User();
                        user.id = id;
                        user.username = read[1].ToString();
                        user.password = read[2].ToString();
                        user.email = read[3].ToString();
                        user.photo = read[4].ToString();
                        user.created_at = DateTime.Parse(read[5].ToString());
                        user.update_at = DateTime.Parse(read[6].ToString());
                        user.isactive = Convert.ToInt32(read[8].ToString());
                        user.lastlogin = DateTime.Parse(read[9].ToString());
                        var dar = read[10].ToString();
                        user.rol_id = dar == "" ? -1 : Convert.ToInt32(dar);
                        user.rol = read[11].ToString();
                        user.empresa = read[12].ToString();
                        listUsers.Add(user);
                    }
                    else
                    {
                        userTemp.rol +=" - " +read[11].ToString();
                        userTemp.empresa +=" - "+ read[12].ToString();
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
            return listUsers;
        }
        public void update(User obj)
        {
            string updte = obj.update_at.ToString("MM/dd/yyyy HH:mm:ss");
            string update = "update  dk_users set username='" + obj.username + "',password='" + obj.password + "',email='" + obj.email + "',isactive='" + obj.isactive + "',updated_at='" + updte + "' where id='" + obj.id + "'";
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

        public User login(string name, string password, string codEmpresa)
        {
            bool hayRegistros;
            string findUser =consultaLogin(name, password, codEmpresa) ;
            User user = new User();
            try
            {
                comando = new SqlCommand(findUser, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {
                    user.id = Convert.ToInt32(read[0].ToString());
                    user.username = read[1].ToString();
                    user.password = read[2].ToString();
                    user.email = read[3].ToString();
                    user.photo = read[4].ToString();
                    user.created_at = DateTime.Parse(read[5].ToString());
                    user.update_at = DateTime.Parse(read[6].ToString());
                    user.isactive = Convert.ToInt32(read[8].ToString());
                    user.lastlogin = DateTime.Parse(read[9].ToString());
                    var dar = read[10].ToString();
                    user.rol_id = dar == "" ? -1 : Convert.ToInt32(dar);// detalle cuando aya varios roles cual escoger
                    user.rol = read[11].ToString();
                    user.empresa = read[12].ToString();
                    user.estado = 99;
                    string passwordd = Crypto.HashPassword(password);
                    bool darff = Crypto.VerifyHashedPassword(user.password, password);
                    bool dar2 = Crypto.VerifyHashedPassword(passwordd, password);
                    if (Crypto.VerifyHashedPassword(user.password, password))
                    {
                        return user;
                    }
                    else
                    {
                        user.estado = 1;
                        return null;
                    }
                }
                else
                {
                    user.estado = 1;
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
      
        
        private string consultaLogin(string name, string password, string codEmpresa)
        {
            string  consulta = "select U.*, r.name, e.EMP_RAZON_NOMBRE  from dk_users as U inner join dk_empresa_rol_user eu on U.id=eu.user_id inner join EMPRESA e on eu.cod_empres = e.EMP_CODIGO inner join dk_roles r on eu.rol_id = r.id where U.username='" + name + "'" + "AND"+ " e.EMP_CODIGO = '" + codEmpresa + "'" ;
            return consulta;
        }
        private string consultaLogin()
        {
            string consulta = "select U.*, r.name, e.EMP_RAZON_NOMBRE  from dk_users as U left join dk_empresa_rol_user eu on U.id=eu.user_id left join EMPRESA e on eu.cod_empres = e.EMP_CODIGO left join dk_roles r on eu.rol_id = r.id ";
            return consulta;
        }
        public static bool tienePrograma(int user_id, string programa)
        {
            Conexion objConexion;
            SqlCommand comando;
            objConexion = Conexion.saberEstado();
            string permisos = "select*from dk_users ";
            try
            {
                comando = new SqlCommand(permisos, objConexion.getCon());
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

            return true;
        }
        public static bool tieneModulo(int user_id, string programa)
        {
            Conexion objConexion;
            SqlCommand comando;
            objConexion = Conexion.saberEstado();
            string permisos = "select*from dk_users ";
            try
            {
                comando = new SqlCommand(permisos, objConexion.getCon());
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

            return true;
        }


    }
}
