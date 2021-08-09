using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.entity
{
    public class Comprobante
    {
        public string EMP_CODIGO { get; set; }
        public string CORDEN { get; set; }
        public string ANEX_CODIGO { get; set; }
        public string ANEX_DESCRIPCION { get; set; }
        public string TIPODOCU_CODIGO { get; set; }
        public string CSERIE { get; set; }
        public string CNUMERO { get; set; }
        public DateTime DEMISION { get; set; }
        public DateTime DVENCE { get; set; }
        public DateTime DRECEPCIO { get; set; }
        public string TIPOMON_CODIGO { get; set; }
        public decimal NIMPORTE { get; set; }
        public decimal TIPOCAMBIO_VALOR { get; set; }
        public string CNUMORDCO { get; set; }
        public string CDESCRIPC { get; set; }
        public string RESPONSABLE_CODIGO { get; set; }
        public string CESTADO { get; set; }
        public decimal NSALDO { get; set; }
        public decimal NMONTPROG { get; set; }
        public bool LCANJEADO { get; set; }
        public string CCODCONTA { get; set; }
        public string CFORMPAGO { get; set; }
        public string CSERREFER { get; set; }
        public string CNUMREFER { get; set; }
        public string CTDREFER { get; set; }
        public string SUBDIARIO_CODIGO { get; set; }
        public string CONVERSION_CODIGO { get; set; }
        public string CTIPPROV { get; set; }
        public string CCTADEST { get; set; }
        public string CCOSTO { get; set; }
        public string CNRORUC { get; set; }
        public bool CONTAB { get; set; }
        public string CTIPO { get; set; }
        public bool ESTCOMPRA { get; set; }
        public string CDESTCOMP { get; set; }
        public string COMPCON { get; set; }
        public bool CSALDINI { get; set; }
        public string CODCAJCH { get; set; }
        public decimal DIASPAGO { get; set; }
        public bool CIGVAPLIC { get; set; }
        public string CAMESPROC { get; set; }
        public string CCONCEPT { get; set; }
        public DateTime DFECREF { get; set; }
        public decimal NIGV { get; set; }
        public decimal NTASAIGV { get; set; }
        public decimal NCAMBESP { get; set; }
        public DateTime DFECCA { get; set; }
        public bool LANULA { get; set; }
        public decimal NPORCE { get; set; }
        public string CCODRUC { get; set; }
        public string CRAZON { get; set; }
        public bool LREGCO { get; set; }
        public bool LHONOR { get; set; }
        public decimal NIR4 { get; set; }
        public decimal NIES { get; set; }
        public decimal NTOTRH { get; set; }
        public decimal NRENTA2DA { get; set; }
        public decimal NISC { get; set; }
        public string BANCO_CODIGO { get; set; }
        public string CAGENCIA { get; set; }
        public string Cnltbco { get; set; }
        public decimal NVALCIF { get; set; }
        public decimal NBASEIMP { get; set; }
        public decimal NINAFECTO { get; set; }
        public DateTime DCONTAB { get; set; }
        public decimal NTASAPERCEPCION { get; set; }
        public decimal NPERCEPCION { get; set; }
        public decimal NTASARETENCION { get; set; }
        public decimal NRETENCION { get; set; }
        public string CO_L_RETE { get; set; }
        public string CODDETRACC { get; set; }
        public string NUMRETRAC { get; set; }
        public DateTime FECRETRAC { get; set; }
        public bool LPASOIMP { get; set; }
        public bool LDETRACCION { get; set; }
        public decimal NTASADETRACCION { get; set; }
        public decimal DETRACCION { get; set; }
        public string COD_SERVDETRACC { get; set; }
        public string COD_TIPOOPERACION { get; set; }
        public decimal NIMPORTEREF { get; set; }
        public string RCO_TIPO { get; set; }
        public string RCO_SERIE { get; set; }
        public string RCO_NUMERO { get; set; }
        public DateTime RCO_FECHA { get; set; }
        public DateTime DREGISTRO { get; set; }
        public string USERREG { get; set; }
        public int flg_RNTNODOMICILIADO { get; set; }
        public string CAOCOMPRA { get; set; }
        // para el codigo
        public string codigo { get; set; }
        public string documento { get; set; }
        public decimal TIPOCAMBIO_VALOR2 { get; set; }


        // para la tabla

        public string descriptionGasto { get; set; }
        public string descriptiontipoAnexo { get; set; }
        public string descriptiontipoDocumento { get; set; }


        //
        public decimal MontoPagar { get; set; }
        public string DDetraccion { get; set; }
        public decimal DTasa { get; set; }
        public string DDocumento { get; set; }
        public DateTime DFecha { get; set; }
        public string DtipoServicio { get; set; }
        public string tipoOperacion { get; set; }
        public decimal gasto1 { get; set; }
        public decimal gasto2 { get; set; }

        public  ContableDet ContableDet { get; set; }

        //public 
    }


    public class ComprobanteDetalle
    {
        public string EMP_CODIGO { get; set; }
        public string CORDEN { get; set; }
        public string CCODPROVE { get; set; }
        public string TIPODOCU_CODIGO { get; set; }
        public string CSERIE { get; set; }
        public string CNUMERO { get; set; }
        public string CNROITEM { get; set; }
        public decimal NVALOR { get; set; }
        public string CCODCONTA { get; set; }
        public string cencost_codigo { get; set; }
        public string codmaquina { get; set; }
        public string CANTIDAD { get; set; }
        public string CCTADEST { get; set; }
        public string CTIPPROV { get; set; }
        public string SUBDIARIO_CODIGO { get; set; }
        public string CFACTOR { get; set; }
        public string CTIPO { get; set; }
        public string CCONCEPT { get; set; }
        public string CGLOSA { get; set; }
        public string CAMESPROC { get; set; }
        public string COMPCON { get; set; }
        public string ANEX_CODIGO { get; set; }
        public string CANEXO { get; set; }
        public string ORDENFAB { get; set; }
      
    }

    public class Detraccion
    {
        public string DDetraccion { get; set; }
        public string DTasa { get; set; }
        public string DDocumento { get; set; }
        public string DFecha { get; set; }
        public string DtipoServicio { get; set; }
        public string tipoOperacion { get; set; }
    }


    public class ContableDet
    {
        public string CNROITEM { get; set; }
        public decimal NVALOR { get; set; }
        public string CCODCONTA { get; set; }
        public string CTIPO { get; set; }
        public string CCOSTO { get; set; }
        public string CCTADEST { get; set; }
        public string CCODSUBDI { get; set; }
        public string CGLOSA { get; set; }
        public string CTIPDOC { get; set; }
        public string CSERDOC { get; set; }
        public string CNUMDOC { get; set; }
        public string CANEXO { get; set; }
        public string CCODANEXO { get; set; }
        public string NUMRETRAC { get; set; }
        public DateTime FECRETRAC { get; set; }
        public string ORDFAB { get; set; }

        public string campo1 { get; set; }
        public string campo2 { get; set; }
        
        // CAMPOS PARA EL TABLA REAL
        
        public string CDESTCOMP { get; set; }
        public string codmaquina { get; set; }
        public int cantidad { get; set; }



    }
}