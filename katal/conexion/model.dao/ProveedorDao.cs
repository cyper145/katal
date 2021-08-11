using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using katal.conexion.model.entity;
using katal.Model;

namespace katal.conexion.model.dao
{
    public class ProveedorDao : Obligatorio
    {


        private Conexion objConexion;
        private SqlCommand comando;

        public ProveedorDao(string codEmpresa):base(codEmpresa)
        {
          
             objConexion = Conexion.saberEstado();
           
        }
        public void create(Proveedor obj)
        {
            throw new NotImplementedException();
        }

        public void delete(Proveedor obj)
        {
            throw new NotImplementedException();
        }

        public bool find(Proveedor obj)
        {
            throw new NotImplementedException();
        }

        public List<Proveedor> findAll()
        {
            List<Proveedor> listArticulos = new List<Proveedor>();
            string findAll = "SELECT prvccodigo,prvcnombre,prvcdirecc,prvctelef1,PRVCRUC FROM maeprov ";
            try
            {
                comando = new SqlCommand(conexionComun( findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Proveedor proveedor = new Proveedor();
                    proveedor.PRVCCODIGO = read[0].ToString();
                    proveedor.PRVCNOMBRE = read[1].ToString();
                    proveedor.PRVCDIRECC = read[2].ToString();
                    proveedor.PRVCTELEF1 = read[3].ToString(); 
                    proveedor.PRVCRUC= read[4].ToString();
                    listArticulos.Add(proveedor);
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
            return listArticulos;
        }

        public void update(Proveedor obj)
        {
            throw new NotImplementedException();
        }
    }
}