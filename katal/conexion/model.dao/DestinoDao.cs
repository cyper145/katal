using katal.conexion.model.entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace katal.conexion.model.dao
{
    public class DestinoDao
    {
        private Conexion objConexion;
        private SqlCommand comando;
        public DestinoDao()
        {
           
           objConexion = Conexion.saberEstado();
          
        }
        public List<Destino> findAll()
        {
            List<Destino> listUsers = new List<Destino>();

            string findAll = "select CO_C_CODIG, CO_A_DESCR, CON_IMPSTO FROM [BDWENCO].[dbo].[DESTINO_COMVEN]";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Destino user = new Destino();
                    user.CO_C_CODIG = read[0].ToString();
                    user.CO_A_DESCR = read[1].ToString();
                    user.CON_IMPSTO = read[2].ToString();

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

        public Destino find(string codigo)
        {
            Destino user = new Destino();
            string findAll = "SELECT CO_C_CODIG, CO_A_DESCR, CON_IMPSTO FROM [BDWENCO].[dbo].[DESTINO_COMVEN] WHERE CO_C_TIPO = 'C' AND CO_C_CODIG = '" + codigo + "'";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {                    
                    user.CO_C_CODIG = read[0].ToString();
                    user.CO_A_DESCR = read[1].ToString();
                    user.CON_IMPSTO = read[2].ToString();                  
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