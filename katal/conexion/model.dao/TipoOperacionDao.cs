using katal.conexion.model.entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace katal.conexion.model.dao
{
    public class TipoOperacionDao
    {

        private Conexion objConexion;
        private SqlCommand comando;
        private string BD;
        public TipoOperacionDao(string BD)
        {
            this.BD = BD;
            objConexion = Conexion.saberEstado();
        }
        public List<ServSujDetraccion> findAllDetraccion( string  dateEmision)
        {
            List<ServSujDetraccion> listTipos = new List<ServSujDetraccion>();

            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "Tab_ServSujDetracc");
            string findAll = $"SELECT codigo, servicio  from {conexion}  Where vigencia is null or vigencia>='{dateEmision}'";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
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

            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "Tab_TipoOperacion");
            string findAll = $"SELECT codigo, tipo_operacion FROM  {conexion}";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
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