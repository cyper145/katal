using System;
using System.Collections.Generic;
using System.Globalization;

namespace katal.conexion.model.dao
{
    public class  Obligatorio
    {
       string codEmpresa;
       public Obligatorio(string codEmpresa="")
        {
            this.codEmpresa = codEmpresa;
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
        public string rellenar(string dato, int lmax, int lreal, string relleno, bool giro)
        {
            string lleno = "";
            for (int f = 1; f <= lmax-lreal; f++)
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
       
    }
}