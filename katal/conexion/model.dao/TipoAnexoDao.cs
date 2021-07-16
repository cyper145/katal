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

    }
}