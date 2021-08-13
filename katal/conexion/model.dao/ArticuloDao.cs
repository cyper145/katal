using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using katal.conexion.model.entity;
using katal.Model;

namespace katal.conexion.model.dao
{
    public class ArticuloDao : Obligatorio
    {

       


        public ArticuloDao(string codEmpresa) : base(codEmpresa)
        {        
            objConexion = Conexion.saberEstado();          
        }

        public void create(Articulo obj)
        {


            //   string create = "insert into dk_users(ACODIGO,ACODIGO2,ADESCRI,ADESCRI2, UMREFERENCIA,AFAMILIA,AMODELO,AGRUPO,APESO,ATIPO,AUSER, AESTADO,AFECHA,AFSERIE,AFLOTE,ACODMON,AIGVPOR,AFLAGIGV,AMARCA,ACOLOR,ATALLA,AFOTO,ACOMENTA,AFSTOCK,ACLASIFICACION )values('" + obj.id + "','" + obj.username + "','" + obj.password + "','" + obj.email + "','" + obj.photo + "','" + creat + "','" + updte + "','" + obj.isactive + "','" + last + "','" + obj.rol_id + "')";

            string create="";
             
            
            try
            {
                comando = new SqlCommand( conexionComun(create), objConexion.getCon());
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

        public void delete(Articulo obj)
        {
            throw new NotImplementedException();
        }

        public bool find(Articulo obj)
        {
            throw new NotImplementedException();
        }

        public List<Articulo> findAll()






        {
            List<Articulo> listArticulos = new List<Articulo>();
            string findAll = "Select ACODIGO,ADESCRI,AUNIDAD,AFSERIE,AFLOTE,ACODIGO2,AFAMILIA,AMODELO,AGRUPO,ATIPO,ACUENTA,AMARCA FROM MAEART ";
            try
            {
                comando = new SqlCommand(conexionComun(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Articulo articulo = new Articulo();
                    articulo.codigo = read[0].ToString();
                    articulo.description = read[1].ToString();
                    articulo.unidad = read[2].ToString();
                    articulo.serie = read[3].ToString();
                    articulo.lote = read[4].ToString();
                    articulo.codigo2 = read[5].ToString();
                    articulo.familia = read[6].ToString();
                    articulo.modelo = read[7].ToString();
                    articulo.grupo = read[8].ToString();
                    articulo.tipo = read[9].ToString();
                    articulo.cuenta = read[10].ToString();
                    articulo.marca = read[11].ToString();                                 
                    listArticulos.Add(articulo);
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

        public void update(Articulo obj)
        {
            throw new NotImplementedException();
        }
    }
}