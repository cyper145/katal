using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using katal.conexion.model.entity;

namespace katal.conexion.model.dao
{
    public class EmpresaDao : Obligatorio
    {
        private Conexion objConexion;
        private SqlCommand comando;

        public EmpresaDao()
        {
            objConexion = Conexion.saberEstado();
        }

        public List<Empresa> findAll()
        {
            List<Empresa> listUsers = new List<Empresa>();

            string findAll = "select*from EMPRESA";
            try
            {
                comando = new SqlCommand( conexionWenco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Empresa user = new Empresa();              
                    user.codigoEmpresa = read[0].ToString();
                    user.RazonSocial = read[2].ToString();

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

        public Empresa findContable(string codigoEmpresa)
        {

           
            string findAll = $"select EMP_NIVEL from EMPRESA where  EMP_CODIGO ='{codigoEmpresa} '"  ;
            Empresa user = new Empresa();
            try
            {
                comando = new SqlCommand(conexionWenco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    
                    user.EMP_NIVEL = read[0].ToString();
                    
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
            return user;
        }

    }
}