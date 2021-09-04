using katal.conexion.model.entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace katal.conexion.model.dao
{
    public class EstadoDocDao : Obligatorio
    {
        public EstadoDocDao(string codEmpresa) : base(codEmpresa)
        {
            objConexion = Conexion.saberEstado();
        }

        public List<EstadoDoc> findAll()
        {

            List<EstadoDoc> listAreas = new List<EstadoDoc>();

            string findAll = "SELECT est_codigo,est_nombre FROM estado_oc";
            try
            {
                comando = new SqlCommand(conexionComun(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    EstadoDoc area = new EstadoDoc();
                    area.est_codigo = read[0].ToString();
                    area.est_nombre = read[1].ToString();

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

    }
}