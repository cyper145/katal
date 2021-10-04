using katal.conexion.model.entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace katal.conexion.model.dao
{
    public class TipoOperacionDao : Obligatorio
    {

        public TipoOperacionDao()
        {
            objConexion = Conexion.saberEstado();
        }
        public List<ServSujDetraccion> findAllDetraccion(string dateEmision)
        {
            List<ServSujDetraccion> listTipos = new List<ServSujDetraccion>();


            string findAll = $"SELECT codigo, servicio  from Tab_ServSujDetracc  Where vigencia is null or vigencia >={dateFormat(dateEmision)}";
            try
            {
                comando = new SqlCommand(conexionWenco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    ServSujDetraccion gasto = new ServSujDetraccion();
                    gasto.codigo = read[0].ToString();
                    gasto.servicio = read[1].ToString();
                    listTipos.Add(gasto);
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
            return listTipos;
        }
        public List<TipoOperacion> findAllTipoOperacion()
        {
            List<TipoOperacion> listTipos = new List<TipoOperacion>();


            string findAll = "SELECT codigo, tipo_operacion FROM  Tab_TipoOperacion";
            try
            {
                comando = new SqlCommand(conexionWenco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    TipoOperacion gasto = new TipoOperacion();
                    gasto.CODIGO = read[0].ToString();
                    gasto.TIPO_OPERACION = read[1].ToString();
                    listTipos.Add(gasto);
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
            return listTipos;
        }
    }
}