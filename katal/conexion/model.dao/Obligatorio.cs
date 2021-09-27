using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

namespace katal.conexion.model.dao
{
    public class Obligatorio
    {
        string codEmpresa;
        protected Conexion objConexion;
        protected SqlCommand comando;
        public Obligatorio(string codEmpresa = "")
        {
            this.codEmpresa = codEmpresa;
        }

        public string CodEmpresa
        {
            get { return codEmpresa; }

        }
        public string conexionWenco(string consulta)
        {
            return Conexion.ConexionCadena("", "BDWENCO", consulta);
        }
        public string conexionComun(string consulta)
        {
            return Conexion.ConexionCadena(this.codEmpresa, "BDCOMUN", consulta);
        }
        public string conexionContabilidad(string consulta)
        {
            return Conexion.ConexionCadena(this.codEmpresa, "BDCONTABILIDAD", consulta);
        }
        public string conexionCtaPag(string consulta)
        {
            return Conexion.ConexionCadena(this.codEmpresa, "BDCTAPAG", consulta);
        }
        public string conexionBDCONT(string consulta, int anio)
        {
            return Conexion.ConexionCadena(this.codEmpresa, "BDCONT", anio, consulta);
        }

        public string conexionBDCBT(string consulta, int anio)
        {
            return Conexion.ConexionCadena(this.codEmpresa, "BDCBT", anio, consulta);
        }
        public string conexionCajaBanco(string consulta)
        {
            return Conexion.ConexionCadena(this.codEmpresa, "BDCAJABANCO", consulta);
        }


        public string conexionTemp(string consulta)
        {
            return Conexion.ConexionCadena("", "tempdb", consulta);
        }

        public string dateFormat(DateTime date)
        {
            DateTime dateTime = DateTime.MinValue;
            if (date == dateTime)
            {
                date = new DateTime(1900, 1, 1);
            }

            return $"CONVERT(datetime, '{date}', 103)";
        }
        public string dateFormat(string date)
        {
            return $"CONVERT(datetime, '{date}', 103)";
        }
        public string numericFormat(decimal numero)
        {
            return numero.ToString("F3", CultureInfo.InvariantCulture);
        }
        public string ternario(bool verificacion, string data1, string data2)
        {
            if (verificacion)
            {
                return data1;
            }
            else
            {
                return data2;
            }
        }
        public string rellenar(string dato, int lmax, int lreal, string relleno, bool giro)
        {
            string lleno = "";
            for (int f = 1; f <= lmax - lreal; f++)
            {
                lleno += relleno;
            }
            if (giro)
            {
                return dato.Trim() + lleno;

            }
            else
            {
                return lleno + dato.Trim();
            }
        }
        protected bool existeTabla(int conexionType, string nombreTabla)
        {

            string sCmd = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = '{nombreTabla}'";
            try
            {
                string consulta = sCmd;
                switch (conexionType)
                {
                    case 1:// wenco
                        consulta = conexionWenco(consulta);
                        break;
                    case 2:// comun
                        consulta = conexionComun(consulta);
                        break;
                    case 3:// contabilidad
                        consulta = conexionContabilidad(consulta);
                        break;
                    case 4:// ctaspagar
                        consulta = conexionCtaPag(consulta);
                        break;
                }
                comando = new SqlCommand(consulta, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    int exiteint = Conversion.Parseint(read[0].ToString());
                    return exiteint > 0;
                }
            }
            catch (Exception)
            {

                return false;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return false;
        }

        protected bool existeColumna(int conexionType, string columna, string nombreTabla)
        {

            string sCmd = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME={columna}  TABLE_NAME = '{nombreTabla}'";
            try
            {
                string consulta = sCmd;
                if (conexionType > 4)
                {
                    consulta = conexionBDCONT(consulta, conexionType);
                }
                else
                {
                    switch (conexionType)
                    {
                        case 1:// wenco
                            consulta = conexionWenco(consulta);
                            break;
                        case 2:// comun
                            consulta = conexionComun(consulta);
                            break;
                        case 3:// contabilidad
                            consulta = conexionContabilidad(consulta);
                            break;
                        case 4:// ctaspagar
                            consulta = conexionCtaPag(consulta);
                            break;


                    }
                }

                comando = new SqlCommand(consulta, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    int exiteint = Conversion.Parseint(read[0].ToString());
                    return exiteint > 0;
                }
            }
            catch (Exception)
            {

                return false;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return false;
        }

        protected string conversionCampo(int dato, string text1, string text2)
        {
            return dato == 3 ? text1 : text2;
        }

    }
}