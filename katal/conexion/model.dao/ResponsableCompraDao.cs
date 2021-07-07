using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using katal.conexion.model.entity;
using katal.Model;

namespace katal.conexion.model.dao
{
    public class ResponsableCompraDao:Obligatorio<ResponsableCompra>
    {
        private Conexion objConexion;
        private SqlCommand comando;

        public ResponsableCompraDao()
        {
            ApplicationUser user = AuthHelper.GetLoggedInUserInfo();

            if (user != null)
            {
                objConexion = Conexion.saberEstado(user.codEmpresa + "BDCOMUN");
            }///014BDCOMUN
        }
        public void create(ResponsableCompra obj)
        {

            string create = "insert into Responsablecmp (RESPONSABLE_CODIGO,RESPONSABLE_NOMBRE)values('" + obj.RESPONSABLE_CODIGO + "','" + obj.RESPONSABLE_NOMBRE + "')";
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

        public void delete(ResponsableCompra obj)
        {
            string delete = "delete from Responsablecmp where RESPONSABLE_CODIGO='" + obj.RESPONSABLE_CODIGO + "'";
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

        public void delete(string RESPONSABLE_CODIGO)
        {
            string delete = "delete from Responsablecmp where RESPONSABLE_CODIGO='" + RESPONSABLE_CODIGO + "'";
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


        public bool find(ResponsableCompra obj)
        {
            bool hayRegistros = false;
            string find = "select*from Responsablecmp where RESPONSABLE_CODIGO='" + obj.RESPONSABLE_CODIGO + "' ";
            try
            {
                comando = new SqlCommand(find, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {

                    obj.RESPONSABLE_CODIGO = read[0].ToString();
                    obj.RESPONSABLE_NOMBRE = read[1].ToString();
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

        public ResponsableCompra find(string RESPONSABLE_CODIGO)
        {
            bool hayRegistros;
            string find = "select*from Responsablecmp where RESPONSABLE_CODIGO='" + RESPONSABLE_CODIGO + "' ";
            ResponsableCompra area = new ResponsableCompra();
            try
            {
                comando = new SqlCommand(find, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {
                    area.RESPONSABLE_CODIGO = read[0].ToString();
                    area.RESPONSABLE_NOMBRE = read[1].ToString();
                    return area;
                }
                else
                {

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
        public List<ResponsableCompra> findAll()
        {

            List<ResponsableCompra> listAreas = new List<ResponsableCompra>();

            string findAll = "SELECT * FROM Responsablecmp";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    ResponsableCompra area = new ResponsableCompra();
                    area.RESPONSABLE_CODIGO = read[0].ToString();
                    area.RESPONSABLE_NOMBRE = read[1].ToString();

                    listAreas.Add(area);
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
            return listAreas;
        }

        public void update(ResponsableCompra obj)
        {
            string update = "update  Responsablecmp set RESPONSABLE_NOMBRE='" + obj.RESPONSABLE_NOMBRE + "' where RESPONSABLE_CODIGO='" + obj.RESPONSABLE_CODIGO + "'";
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