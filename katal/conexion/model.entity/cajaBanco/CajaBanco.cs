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


    public class CMovimientoBanco
    {
        public string CB_C_BANCO { get; set; }
        public string CB_C_MES { get; set; }
        public string CB_C_SECUE { get; set; }
        public string CB_C_MODO { get; set; }
        public string CB_C_OPERA { get; set; }
        public DateTime CB_D_FECHA { get; set; }
        public string CB_C_TPDOC { get; set; }
        public string CB_C_DOCUM { get; set; }
        public string CB_C_ANEXO { get; set; }
        public string CB_C_CONVE { get; set; }
        public decimal CB_N_CAMES { get; set; }
        public DateTime CB_D_FECCA { get; set; }
        public decimal CB_N_TIPCA { get; set; }
        public decimal CB_N_MTOMN { get; set; }
        public decimal CB_N_MTOME { get; set; }
        public string CB_C_CONTA { get; set; }
        public string CB_C_NROLI { get; set; }
        public string CB_C_FACTU { get; set; }
        public string CB_L_CONTA { get; set; }
        public string CB_L_ANULA { get; set; }
        public string CB_A_REFER { get; set; }
        public string CB_C_ESTAD { get; set; }
        public string CB_C_ESTRET { get; set; }
        public string CB_RETLET { get; set; }
        public string CB_D_FECCOB { get; set; }
        public string CB_TRANSBCO { get; set; }
        public string CB_TIPMOV { get; set; }
        public string CB_MEDIO { get; set; }
        public string CB_DMEDIO { get; set; }
        public string CB_USUARIO { get; set; }


        /*public string CB_C_Secue   CB_C_SECUE { get; set; }
        public string Opera CB_C_OPERA   { get; set; }
        public string docu CB_C_TPDOC { get; set; }
        public decimal MONTO  CB_N_MTOMN { get; set; }
        public string Conta CB_C_CONTA  { get; set; }
        public string Anula { get; set; }
        public DateTime CB_C_Fecha  CB_D_FECCA { get; set; }
        public string CB_C_Anexo  CB_C_ANEXO{ get; set; }
        public string CB_C_CONTA { get; set; }
        public string CB_A_REFER { get; set; }
        public string CB_C_NROLI { get; set; }*/


    }


    public class DMovimientoBanco
    {
        public string CB_C_BANCO { get; set; }
        public string CB_C_MES { get; set; }
        public string CB_C_SECUE { get; set; }
        public string CB_C_SECDE { get; set; }
        public string CB_C_MODO { get; set; }
        public string CB_C_CONCE { get; set; }
        public string CB_C_ANEXO { get; set; }
        public string CB_C_TPDOC { get; set; }
        public string CB_C_DOCUM { get; set; }
        public DateTime CB_D_FECDC { get; set; }
        public string CB_A_REFER { get; set; }
        public string CB_C_CUENT { get; set; }
        public string CB_C_CENCO { get; set; }
        public string CB_C_DESTI { get; set; }
        public decimal CB_N_MTOMN { get; set; }
        public decimal CB_N_MTOME { get; set; }
        public bool CB_L_ANULA { get; set; }
        public decimal CB_L_PROGR { get; set; }
        public string CODDETPLA { get; set; }
        public string CB_ACUENTA { get; set; }
        public string CB_L_ESTADO { get; set; }
        public decimal CB_N_RESTANTE { get; set; }
    }

    public class TemporalGC
    {
        public string secuencia { get; set; }
        public string ANEXO { get; set; }
        public string DOC { get; set; }
        public string fecha { get; set; }
        public string cjavco { get; set; }
        public string concepto { get; set; }
        public string documento { get; set; }
        public string tc { get; set; }
        public string tipmon { get; set; }
        public string importe { get; set; }
        
    }
    public class ConceptoCajaBanco{

        public string CB_C_TIPO { get; set; }
        public string CB_C_MODO { get; set; }
        public string CB_C_CODIG { get; set; }
        public string CB_A_DESCR { get; set; }
        public string CB_L_TRANS { get; set; }
        public string CB_C_CUENT { get; set; }
        public string CB_C_INTER { get; set; }
        public string CB_C_OPERA { get; set; }
        public string CB_C_LINEA { get; set; }
        public string codigo { get; set; }
       
    }
    

}