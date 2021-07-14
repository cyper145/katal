
using System.Data.SqlClient;
namespace katal.conexion.model.dao
{
    class Conexion
    {
        //singleton
        private static Conexion objConexion = null;
        private SqlConnection con;
       // private string nameBaseDatos ;
        private Conexion()
        {          
            con = new SqlConnection(this.cadenaConexion("BDWENCO"));
        }
        private Conexion(string nameBaseDatos)
        {
            con = new SqlConnection(this.cadenaConexion(nameBaseDatos));
        }

        public static Conexion saberEstado()
        {
            if (objConexion == null)
            {
                objConexion = new Conexion();
            }
            return objConexion;
        }
        public static Conexion saberEstado(string  nameBasedatos)
        {

            if (nameBasedatos == "rol")
            {
                objConexion = new Conexion();
            }
            if (objConexion == null)
            {
                objConexion = new Conexion(nameBasedatos);
            }
            return objConexion;
        }
        public SqlConnection getCon()
        {
            return con;
        }

        public void cerrarConexion()
        {
            objConexion = null;
        }

        private string cadenaConexion(string nameBaseDatos)
        {
            return $"data source = SERVIDOR; initial catalog = {nameBaseDatos}; user id = SOPORTE; password = SOPORTE";
            //return $"Data Source=DESKTOP-RDGSDMQ;Initial Catalog={nameBaseDatos}; Integrated Security=True";
        }
    }
}
