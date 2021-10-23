using katal.conexion.model.entity;
using System;
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
        //1
        public string conexionWenco(string consulta)
        {
            return Conexion.ConexionCadena("", "BDWENCO", consulta);
        }
        //2
        public string conexionComun(string consulta)
        {
            return Conexion.ConexionCadena(this.codEmpresa, "BDCOMUN", consulta);
        }
        //3
        public string conexionContabilidad(string consulta)
        {
            return Conexion.ConexionCadena(this.codEmpresa, "BDCONTABILIDAD", consulta);
        }
        //4
        public string conexionCtaPag(string consulta)
        {
            return Conexion.ConexionCadena(this.codEmpresa, "BDCTAPAG", consulta);
        }
        //5
        public string conexionBDCONT(string consulta, int anio)
        {
            return Conexion.ConexionCadena(this.codEmpresa, "BDCONT", anio, consulta);
        }
        //6
        public string conexionBDCBT(string consulta, int anio)
        {
            return Conexion.ConexionCadena(this.codEmpresa, "BDCBT", anio, consulta);
        }
        //7
        public string conexionCajaBanco(string consulta)
        {
            return Conexion.ConexionCadena(this.codEmpresa, "BDCAJABANCO", consulta);
        }

        //8
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
        public bool isnull(string dato)
        {
            bool nulo = true;
            if (dato!="" && dato != null)
            {
                nulo = false;
            }
            return nulo;
        }
        public dynamic ternarioG(bool verificacion, dynamic data1, dynamic data2)
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

        public bool hacerConsuta(string consulta)
        {
            try
            {
                comando = new SqlCommand(consulta, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    return true;
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

        public string verdata(string condicion, string tabla, int conexionType, int param, string camdev, DateTime dateTime)
        {
            string verdata = "N";
            string consulta = $"SELECT* FROM  {tabla} WHERE   {condicion}";
            try
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
                    case 5:// ctaspagar
                        consulta = conexionBDCBT(consulta, dateTime.Year);
                        break;
                    case 6:// ctaspagar
                        consulta = conexionBDCONT(consulta, dateTime.Year);
                        break;
                    case 7:// ctaspagar
                        consulta = conexionCajaBanco(consulta);
                        break;
                }

                //TODO ES PARTE SE PUED GENERALIZAR
                comando = new SqlCommand(consulta, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    if (param == 0)
                    {
                        verdata = "S";
                    }
                    else
                    {

                        string valor = read[camdev].ToString();
                        verdata = ternario(valor == "", " ", valor);
                    }

                }
                return verdata;
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


        // COMPROBANTECAB PARA CREAR NUEVO SERIE
        public string funcAutoNum(string msAnoMesProc)
        {
            string cadena = "SELECT Concepto_Logico FROM CONCEPTOGRAL WHERE Concepto_Codigo='NUMEAUTO'";
            try
            {
                comando = new SqlCommand(conexionComun(cadena), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                bool hayRegistros = read.Read();
                if (hayRegistros)
                {

                    bool codigo = Conversion.ParseBool(read[0].ToString());
                    read.Close();
                    cadena = "SELECT MAX(CORDEN) AS MAXORDEN FROM COMPROBANTECAB WHERE CAMESPROC = '" + msAnoMesProc + "'";
                    comando = new SqlCommand(conexionComun(cadena), objConexion.getCon());
                    // objConexion.getCon().Open();
                    SqlDataReader readd = comando.ExecuteReader();
                    bool hayRegistrosdd = readd.Read();
                    int nextDocumet = 0;
                    if (hayRegistrosdd)
                    {
                        string last = readd[0].ToString();
                        nextDocumet = Conversion.Parseint(last) + 1;
                    }
                    else
                    {
                        nextDocumet = 1;
                    }

                    string next = nextDocumet.ToString("00000.##");
                    /* if (codigo)
                     {
                         return next;
                     }
                     else
                     {
                         return "";
                     }*/
                    return next;
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


            return "";
        }

        public string ConceptosGenerales(string concepto)
        {
            bool hayRegistros = false;
            string data = "";
            ConceptosGenerales conceptos = new ConceptosGenerales();
            string find = $"SELECT * FROM CONCEPTOS_GENERALES WHERE CONCGRAL_CODIGO = '{concepto}'";
            try
            {
                comando = new SqlCommand(conexionContabilidad(find), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {
                    conceptos.CONCGRAL_CODIGO = read[0].ToString();
                    conceptos.CONCGRAL_DESCRIPCION = read[1].ToString();
                    conceptos.CONCGRAL_TIPO = read[2].ToString();
                    conceptos.CONCGRAL_CONTEC = read[3].ToString();
                    conceptos.CONCGRAL_CONTEN = read[4].ToString();
                    conceptos.CONCGRAL_CONTED = read[5].ToString();
                    conceptos.CONCGRAL_CONTEL = read[6].ToString();
                    switch (conceptos.CONCGRAL_TIPO)
                    {
                        case "C":
                            data = conceptos.CONCGRAL_CONTEC;
                            break;
                        case "N":
                            data = conceptos.CONCGRAL_CONTEN;
                            break;
                        case "L":
                            data = conceptos.CONCGRAL_CONTEL;
                            break;
                        default:
                            data = conceptos.CONCGRAL_CONTED;
                            break;

                    }
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
            return data;
        }

    }
}