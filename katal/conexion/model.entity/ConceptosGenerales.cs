using System;

namespace katal.conexion.model.entity
{
    public class ConceptosGenerales
    {
        public string CONCGRAL_CODIGO { get; set; }
        public string CONCGRAL_DESCRIPCION { get; set; }
        public string CONCGRAL_TIPO { get; set; }
        public string CONCGRAL_CONTEC { get; set; }
        public string CONCGRAL_CONTEN { get; set; }
        public string CONCGRAL_CONTED { get; set; }
        public string CONCGRAL_CONTEL { get; set; }

    }

    public class ConceptoGral
    {
        public string Concepto_Codigo { get; set; }
        public string Concepto_Descripcion { get; set; }
        public string Concepto_Tipo { get; set; }
        public string Concepto_Caracter { get; set; }
        public Decimal Concepto_Numerico { get; set; }
        public DateTime Concepto_Fecha { get; set; }
        public bool Concepto_Logico { get; set; }
    }
}