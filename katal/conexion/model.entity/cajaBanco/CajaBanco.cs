using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.entity
{
    public class CajaBanco
    {
        public string CB_C_TIPO { get; set; }
        public string CB_C_CODIG { get; set; }
        public string CB_A_DESCR { get; set; }
        public string CB_C_CUENT { get; set; }
        public string CB_C_TIPCA { get; set; }
        public string AREA_CODIGO { get; set; }
        public string CB_C_BANCO { get; set; }
        public string CB_C_NUMCT { get; set; }
        public string CB_C_TIPDC { get; set; }
        public string CB_C_MONED { get; set; }
        public string CB_A_RESPO { get; set; }
        public string CB_A_DOCUM { get; set; }
        public bool CB_L_USADO { get; set; }
        public string CB_D_FECHA { get; set; }
        public bool CB_L_APERT { get; set; }
        public bool CB_L_CERRA { get; set; }
        public string CB_C_TIPCT { get; set; }
        public float CB_N_TIPCA { get; set; }
        public float CB_N_CONTA { get; set; }
        public string CB_C_USU { get; set; }
        public string CB_C_FORMATO { get; set; }
        public string CB_C_SUB { get; set; }
        public string CB_C_ESTADO { get; set; }
      

    }
    public class MovimientoBanco
    {
        public string CB_C_Secue { get; set; }
        public string Opera { get; set; }
        public string docu { get; set; }
        public decimal MONTO { get; set; }
        public string Conta { get; set; }
        public string Anula { get; set; }
        public DateTime CB_C_Fecha { get; set; }
        public string CB_C_Anexo { get; set; }
        public string CB_C_CONTA { get; set; }
        public string CB_A_REFER { get; set; }
        public string CB_C_NROLI { get; set; }
    }


    public class TipoOpcionCajaBanco 
    {
        public string CB_C_TIPO { get; set; }
        public string CB_C_MODO { get; set; }
        public string CB_C_CODIG { get; set; }
        public string CB_A_DESCR { get; set; }
        public string CB_C_TPDOC { get; set; }
        public string CB_C_AUTOM { get; set; }
        public string CB_C_FPAGO { get; set; }
      
    }

    public class TipoEstadoOperacion
    {
        public string CB_C_TIPO { get; set; }
        public string CB_C_CODIG { get; set; }
        public string CB_A_DESCR { get; set; }
    
    }
    public class TipoMovimientos
    {
        public string CB_C_TIPO { get; set; }
        public string CB_C_CODIG { get; set; }
        public string CB_A_DESCR { get; set; }

    }
    public class MedioPago
    {
        public string CODIGO { get; set; }
        public string DESCRIPCION { get; set; }
      
    }

}