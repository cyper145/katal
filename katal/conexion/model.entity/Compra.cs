using System;

namespace katal.conexion.model.entity
{
    public class Compra
    {
        public string CO_C_CUENT { get; set; }
        public string CO_C_MES { get; set; }
        public string CO_C_SUBDI { get; set; }
        public string CO_C_COMPR { get; set; }
        public DateTime CO_D_FECHA { get; set; }
        public string CO_C_PROVE { get; set; }
        public string CO_C_TPDOC { get; set; }
        public string CO_C_DOCUM { get; set; }
        public string CO_D_FECDC { get; set; }
        public string CO_C_TPDRF { get; set; }
        public string CO_C_DCREF { get; set; }
        public DateTime CO_D_FECRF { get; set; }
        public string CO_C_MONED { get; set; }
        public decimal CO_N_IGV { get; set; }
        public decimal CO_N_IGVUS { get; set; }
        public decimal CO_N_TASA { get; set; }
        public decimal CO_N_MONTO { get; set; }
        public decimal CO_N_MTOUS { get; set; }
        public string CO_C_CONVE { get; set; }
        public decimal CO_N_TIPCA { get; set; }
        public decimal CO_N_CAMES { get; set; }
        public DateTime CO_D_FECCA { get; set; }
        public string CO_A_GLOSA { get; set; }
        public string CO_A_MOVIM { get; set; }
        public bool CO_L_ANULA { get; set; }
        public string CO_C_DESTI { get; set; }
        public bool CO_L_APLIC { get; set; }
        public decimal CO_N_PORCE { get; set; }
        public string CO_C_RUC { get; set; }
        public string CO_A_RAZON { get; set; }
        public decimal CO_N_VALCI { get; set; }
        public bool CO_L_REFER { get; set; }
        public string CO_NUM_RETRAC { get; set; }
        public DateTime CO_FEC_RETRAC { get; set; }
        public DateTime CO_D_FECHAVTO { get; set; }
        public decimal CO_N_UNIDA { get; set; }
        public decimal CO_N_COMPPAGO { get; set; }
        public decimal CO_N_OTROIMP { get; set; }
        public decimal CO_N_OTROIMP_ME { get; set; }
        public bool CO_L_DETRACCION { get; set; }
        public decimal CO_N_TASADETRACC { get; set; }
        public decimal CO_N_IMPORTEREF { get; set; }
        public bool CO_L_RETE { get; set; }
        public decimal NPERCEPCION { get; set; }
        public string RCO_TIPO { get; set; }
        public string RCO_SERIE { get; set; }
        public string RCO_NUMERO { get; set; }
        public string RCO_FECHA { get; set; }
        public string flg_RNTNODOMICILIADO { get; set; }

    }
}