using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using katal.conexion.model.entity;
namespace katal.conexion.model.dao
{
    public class TipoAnexoDao
    {

        private Conexion objConexion;
        private SqlCommand comando;
        private string BD;
        public TipoAnexoDao(string  BD)
        {
            this.BD = BD;
            objConexion = Conexion.saberEstado();
          
        }
        public List<TipoAnexo> findAll()
        {
            List<TipoAnexo> listTipos = new List<TipoAnexo>();

            string BD = $"{this.BD}BDCONTABILIDAD"; 
            string findAll = $"SELECT TIPOANEX_CODIGO, TIPOANEX_DESCRIPCION FROM [{BD}].[dbo].[TIPO_ANEXO] ";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    TipoAnexo gasto = new TipoAnexo();
                    gasto.TIPOANEX_CODIGO = read[0].ToString();
                    gasto.TIPOANEX_DESCRIPCION = read[1].ToString();
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
        public string findAllDetail(string cuentas)
        {
         
            string conexion = Conexion.CadenaGeneral(BD, "BDCONTABILIDAD", "PLAN_CUENTA_NACIONAL");
            string codigoAnexo="";
            string findAll = $"SELECT TIPOANEX_CODIGO, TIPOANEX_DESCRIPCION FROM {conexion} WHERE PLANCTA_CODIGO='"+ cuentas + "'";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    codigoAnexo = read[0].ToString();                                      
                }
                return codigoAnexo;
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
     
        public List<Anexo> findAllAnexo()
        {
            List<Anexo> listAnexo = new List<Anexo>();


            // TIPOANEX_CODIGO = '" & _
            //txtTpoAnexo
            string BD = $"{this.BD}BDCONTABILIDAD";
            string findAll = $"SELECT ANEX_CODIGO, ANEX_DESCRIPCION, ANEX_RUC FROM [{BD}].[dbo].[ANEXO] ";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Anexo gasto = new Anexo();
                    gasto.ANEX_CODIGO = read[0].ToString();
                    gasto.ANEX_DESCRIPCION = read[1].ToString();
                    gasto.ANEX_RUC = read[2].ToString();
                    listAnexo.Add(gasto);
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
            return listAnexo;
        }

        public List<TipoDocumento> findAllTipoDocumento()
        {
            List<TipoDocumento> listAnexo = new List<TipoDocumento>();

          
            // TIPOANEX_CODIGO = '" & _
            //txtTpoAnexo
            string BD = $"{this.BD}BDCONTABILIDAD";
            string findAll = $"SELECT TIPDOC_CODIGO, TIPDOC_DESCRIPCION, tipdoc_referencia FROM [{BD}].[dbo].[TIPOS_DE_DOCUMENTOS] ";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    TipoDocumento gasto = new TipoDocumento();
                    gasto.TIPDOC_CODIGO = read[0].ToString();
                    gasto.TIPDOC_DESCRIPCION = read[1].ToString();
                    gasto.TIPDOC_REFERENCIA = read[2].ToString();
                    listAnexo.Add(gasto);
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
            return listAnexo;
        }
    }
}