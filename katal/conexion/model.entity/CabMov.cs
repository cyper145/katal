using System;

namespace katal.conexion.model.entity
{
    public class CabMov
    {
        public string SUBDIAR_CODIGO { get; set; }
        public string CMOV_C_COMPR { get; set; }
        public DateTime CMOV_FECHA { get; set; }
        public string CMOV_GLOSA { get; set; }
        public string CMOV_MONED { get; set; }
        public string CMOV_CONVE { get; set; }
        public string CMOV_CAMES { get; set; }
        public string CMOV_FECCA { get; set; }
        public string CMOV_TIPCA { get; set; }
        public string CMOV_DEBE { get; set; }
        public string CMOV_HABER { get; set; }
        public string CMOV_DEBUS { get; set; }
        public string CMOV_HABUS { get; set; }
        public bool CMOV_AUTOM { get; set; }
        public bool CMOV_COSTO { get; set; }
        public bool CMOV_CHEQU { get; set; }
        public bool CMOV_L_COMPR { get; set; }
        public bool CMOV_VENTA { get; set; }
        public bool FECH_VCTO { get; set; }
        public bool CMOV_CAJAB { get; set; }
        public string CMOV_MEDIO { get; set; }
        public string CMOV_DMEDIO { get; set; }
    }
}