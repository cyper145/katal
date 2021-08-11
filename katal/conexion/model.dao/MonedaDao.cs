using katal.conexion.model.entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace katal.conexion.model.dao
{
    public class MonedaDao:Obligatorio
    {

        private Conexion objConexion;
        private SqlCommand comando;
        
        public MonedaDao(string codEmpresa ):base(codEmpresa)
        {
            
            objConexion = Conexion.saberEstado();
        }
        public List<Moneda> findAll()
        {
            List<Moneda> listTipos = new List<Moneda>();

          
            
            string findAll = $"SELECT COVMON_CODIGO, COVMON_DESCRIPCION FROM CONVERSION_MONEDA ";
            try
            {
                comando = new SqlCommand(conexionContabilidad( findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Moneda gasto = new Moneda();
                    gasto.COVMON_CODIGO = read[0].ToString();
                    gasto.COVMON_DESCRIPCION = read[1].ToString();
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

        public TipoCambio findTipoCambio( string dateTime )
        {       
            
            string date = dateTime;
            string findAll = $"select TIPOCAMB_COMPRA, TIPOCAMB_EQCOMPRA, TIPOCAMB_VENTA, TIPOCAMB_EQVENTA FROM TIPO_CAMBIO WHERE TIPOCAMB_FECHA={dateFormat(date)}";
            TipoCambio gasto = new TipoCambio();
            try
            {
                comando = new SqlCommand( conexionContabilidad(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    
                    gasto.TIPOCAMB_COMPRA =Conversion.ParseDecimal(  read[0].ToString());
                    gasto.TIPOCAMB_EQCOMPRA = Conversion.ParseDecimal(read[1].ToString());
                    gasto.TIPOCAMB_VENTA = Conversion.ParseDecimal(read[2].ToString());
                    gasto.TIPOCAMB_EQVENTA = Conversion.ParseDecimal(read[3].ToString());
                   
                }
                else
                {
                    read.Close();
                    findAll = $"select top(1) TIPOCAMB_COMPRA, TIPOCAMB_EQCOMPRA, TIPOCAMB_VENTA, TIPOCAMB_EQVENTA FROM TIPO_CAMBIO ORDER BY TIPOCAMB_FECHA DESC";
                     comando = new SqlCommand(conexionContabilidad(findAll), objConexion.getCon());
                     //objConexion.getCon().Open();
                     SqlDataReader read2 = comando.ExecuteReader();
                    if (read2.Read())
                    {

                        gasto.TIPOCAMB_COMPRA = Conversion.ParseDecimal(read2[0].ToString());
                        gasto.TIPOCAMB_EQCOMPRA = Conversion.ParseDecimal(read2[1].ToString());
                        gasto.TIPOCAMB_VENTA = Conversion.ParseDecimal(read2[2].ToString());
                        gasto.TIPOCAMB_EQVENTA = Conversion.ParseDecimal(read2[3].ToString());

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

            return gasto;
        }
    }
}