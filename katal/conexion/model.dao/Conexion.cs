
using System.Data.SqlClient;
namespace katal.conexion.model.dao
{
    public class Conexion
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
            //return $"data source = SERVIDOR; initial catalog = {nameBaseDatos}; user id = SOPORTE; password = SOPORTE";
            return $"Data Source=DESKTOP-RT71HEQ\\SQLEXPRESS;Initial Catalog={nameBaseDatos}; Integrated Security=True";
        }
        public static string CadenaGeneral(string codigo, string nameBaseDatosgeneral,string tabla)
        {
            string BD = $"{codigo}{nameBaseDatosgeneral}";
            return $"[{BD}].[dbo].[{tabla}] ";
        }
        public static string ConexionCadena(string codigo, string basedatos, string consulta)
        {

            return $"Use [{codigo}{basedatos}]  {consulta}";
        }
        public static string ConexionCadena(string codigo, string basedatos,int anio, string consulta)
        {

            return $"Use [{codigo}{basedatos}{anio}]  {consulta}";
        }
    }
}
