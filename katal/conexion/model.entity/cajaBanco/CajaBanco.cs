using System;
using System.Collections.Generic;

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

        public string CB_C_Secue { get; set; }
        public string Opera { get; set; }
        public string docu { get; set; }
        public decimal MONTO { get; set; }
        public string Conta { get; set; }
        public string Anula { get; set; }
        public DateTime CB_D_Fecha { get; set; }
        public string CB_C_AnexoV { get; set; }
        public string CB_C_CONTAV { get; set; }
        public string CB_A_REFERV { get; set; }
        public string CB_C_NROLIV { get; set; }
        public string CB_C_BANCO { get; set; }
        public string CB_C_MES { get; set; }
        public string CB_C_SECUE { get; set; }
        public string CB_C_MODO { get; set; }
        public string CB_C_OPERA { get; set; }
        public DateTime CB_D_FECHA { get; set; }
        public string CB_C_TPDOC { get; set; }
        public string CB_C_DOCUM { get; set; }
        public string serie { get; set; }// SOLO PARA USO VISUAL
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
        public DateTime CB_D_FECCOB { get; set; }
        public string CB_TRANSBCO { get; set; }
        public string CB_TIPMOV { get; set; }
        public string CB_MEDIO { get; set; }
        public string CB_DMEDIO { get; set; }
        public string CB_USUARIO { get; set; }
        public decimal montoVisual { get; set; }
        public string TipoAnexoD { get; set; }
       


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

        // comprobante si es salida
        public string CORDEN { get; set; }

    }

    public class DMovimientoBanco
    {


        public string secd { get; set; }
        public string CB_C_Concep { get; set; }
        public string CB_C_docum { get; set; }
        public decimal montomn { get; set; }
        public decimal montome { get; set; }
        public string CB_C_BANCO { get; set; }
        public string CB_C_MES { get; set; }
        public string CB_C_SECUE { get; set; }
        public string CB_C_SECDE { get; set; }
        public string CB_C_MODO { get; set; }
        public string CB_C_CONCE { get; set; }
        public string CB_C_ANEXOD { get; set; }
        public string CB_C_TPDOCD { get; set; }
        public string CB_C_DOCUMD { get; set; }
        public DateTime CB_D_FECDC { get; set; }
        public string CB_A_REFERD { get; set; }
        public string CB_C_CUENT { get; set; }
        public string CB_C_CENCO { get; set; }
        public string CB_C_DESTI { get; set; }
        public decimal CB_N_MTOMND { get; set; }
        public decimal CB_N_MTOMED { get; set; }
        public bool CB_L_ANULA { get; set; }
        public bool CB_L_PROGR { get; set; }
        public string CODDETPLA { get; set; }
        public bool CB_L_INT { get; set; }
        
        public string CB_ACUENTA { get; set; }
        public string monedaD { get; set; }
        public string serieD { get; set; }
        public int IS { get; set; }
        
        public  string TipoAnexo { get; set; }

        public string CORDEN { get; set; }
        public string CB_C_ANEXOO { get; set; }
        public decimal monto { get; set; }



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
    public class ConceptoCajaBanco
    {

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

    public class TipoMoneda
    {
        public string TIPOMON_CODIGO { get; set; }
        public string TIPOMON_DESCRIPCION { get; set; }
        public string TIPOMON_NACIONALIDAD { get; set; }
        public string TIPOMON_SIMBOLO { get; set; }
    }


    public class FlagForForm
    {

        public bool frmLisPlaniCobranza { get; set; }
        public bool frmselecfacont { get; set; }
        public bool frmCtasxPagarPrueba1 { get; set; }
        public bool frmCtasxPagarPrueba2 { get; set; }

        public FlagForForm()
        {
            frmLisPlaniCobranza = false;
            frmselecfacont = false;
            frmCtasxPagarPrueba1 = false;
            frmCtasxPagarPrueba2 = false;
        }
    }


    public class Cobranzas
    {
        public string COCNROPLA { get; set; }
        public DateTime COCFECPLA { get; set; }
        public decimal COCTCOBMN { get; set; }
        public string COCTCOBUS { get; set; }
    }
    public class DataGeneralBanco
    {
        public List<CMovimientoBanco> cMovimientoBancos;
        public List<Cobranzas> cobranzas;
    }
    public class ParametrosCuentas
    {
        public string CTA_SOLES { get; set; }
        public string CTA_DOLA { get; set; }
    }
    public  class Cartera
    {
        public string CDOTIPDOC { get; set; }
        public string CDONRODOC { get; set; }
        public DateTime CDOFECDOC { get; set; }
        public string CDOFECVEN { get; set; }
        public string CDOTDOREF { get; set; }
        public string CDONROREF { get; set; }
        public string CDOCODVEN { get; set; }
        public string CDOIMPORTE { get; set; }
        public string CDOSALDO { get; set; }
        public string CDOTIPMON { get; set; }
        public string CDOTIPCAM { get; set; }
        public string CDODEBHAB { get; set; }
        public string CDOESTADO { get; set; }
        public string CDOFECCRE { get; set; }
        public string CDOFECACT { get; set; }
        public string CDOUSUARI { get; set; }
        public string CDOCUENTA { get; set; }
        public string CDOCOMIS { get; set; }
        public string CDOTIPFAC { get; set; }
        public string CDOFECREF { get; set; }
        public string CDOCHEDIF { get; set; }
        public string CDOSALINI { get; set; }
        public string CDFORVEN { get; set; }
        public string CDPUNVEN { get; set; }
        public string COD_BANCO { get; set; }
        public string DES_BANCO { get; set; }
        public string CDOIMPORTEPERC { get; set; }
        public string CDOTASAPERC { get; set; }
        public string CDOCTAPERC { get; set; }
        public string CDOTIPRET { get; set; }
        public string CDOTASARET { get; set; }
        public string CDOPERAUTO { get; set; }
        public string CDOORICB { get; set; }
        public string Percepcion { get; set; }
        public string Env_CNC { get; set; }
        public string idcartera { get; set; }
        public string flagverificado { get; set; }
        public string idbanco { get; set; }
        public string fechacontrol { get; set; }
       

    }
    public class Montos
    {
        public decimal SUMTOTALMN { get; set; }
        public decimal SUMTOTALME { get; set; }
    }


    public class CuentaxPagar
    {
        public string CCODMONED { get; set; }
        public string N_AUTONUM { get; set; }
        public string C_CAMESPROC { get; set; }
        public string C_CORDEN { get; set; }
    }
    public class PagosDetalle
    {
        public string CTIPPROV { get; set; }
        public string CCODPROVE { get; set; }
        public string TIPODOCU_CODIGO { get; set; }
        public string CSERIE { get; set; }
        public string CNUMERO { get; set; }      
    }


}