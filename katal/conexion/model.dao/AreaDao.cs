using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using katal.Model;
using katal.conexion.model.entity;

namespace katal.conexion.model.dao
{
    public class AreaDao : Obligatorio
    {
       


        public AreaDao(string  codEmpresa):base(codEmpresa)
        {                     
           objConexion = Conexion.saberEstado();
            ///014BDCOMUN
        }
        public void create(Area obj)
        {
            
            string create = "insert into AREA (AREA_CODIGO,AREA_DESCRIPCION)values('" + obj.AREA_CODIGO + "','" + obj.AREA_DESCRIPCION + "')";
            try
            {
                comando = new SqlCommand(conexionComun( create), objConexion.getCon());
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

        public void delete(Area obj)
        {
            string delete = "delete from AREA where AREA_CODIGO='" + obj.AREA_CODIGO + "'";
            try
            {
                comando = new SqlCommand(conexionComun(delete), objConexion.getCon());
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

        public void delete(string AREA_CODIGO)
        {
            string delete = "delete from AREA where AREA_CODIGO='" + AREA_CODIGO + "'";
            try
            {
                comando = new SqlCommand(conexionComun(delete), objConexion.getCon());
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


        public bool find(Area obj)
        {
            bool hayRegistros=false;
            string find = "select*from AREA where AREA_CODIGO='" + obj.AREA_CODIGO + "' ";
            try
            {
                comando = new SqlCommand(conexionComun(find), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {
                   
                    obj.AREA_CODIGO = read[0].ToString();
                    obj.AREA_DESCRIPCION = read[1].ToString();
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

        public Area find(string codigoArea)
        {
            bool hayRegistros;
            string find = "select*from AREA where AREA_CODIGO='" + codigoArea + "' ";
            Area area = new Area();
            try
            {
                comando = new SqlCommand(conexionComun(find), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {
                    area.AREA_CODIGO = read[0].ToString();
                    area.AREA_DESCRIPCION = read[1].ToString();
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
        public List<Area> findAll()
        {

            List<Area> listAreas = new List<Area>();

            string findAll = "SELECT * FROM AREA";
            try
            {
                comando = new SqlCommand(conexionComun(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Area area = new Area();
                    area.AREA_CODIGO = read[0].ToString();
                    area.AREA_DESCRIPCION = read[1].ToString();

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

        public void update(Area obj)
        {
            string update = "update  AREA set AREA_DESCRIPCION='" + obj.AREA_DESCRIPCION + "' where AREA_CODIGO='" + obj.AREA_CODIGO + "'";
            try
            {
                comando = new SqlCommand(conexionComun(update), objConexion.getCon());
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