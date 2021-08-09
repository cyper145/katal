using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.entity
{
    public class Cabecera
    {
        public string CO_C_SUBDI { get; set; }
        public string CO_C_COMPR { get; set; }
        public string CO_D_FECHA { get; set; }
        public string CO_A_GLOSA { get; set; }
        public string CO_C_MONED { get; set; }
        public string CO_C_CONVE { get; set; }
        public string CO_D_FECCA { get; set; }
        public string CO_N_DEBE { get; set; }
        public string co_n_haber { get; set; }
        public string co_n_debus { get; set; }
        public string co_n_habus { get; set; }
        public string CO_N_TIPCA { get; set; }
        public string CO_N_CAMES { get; set; }
        public string CO_L_COMPR { get; set; }
        public string FECH_VEN { get; set; }
        
    }

    public class Detalle
    {
        public string CO_C_SUBDI { get; set; }
        public string CO_C_COMPR { get; set; }
        public string CO_D_FECHA { get; set; }
        public string CO_C_CUENT { get; set; }
        public string CO_D_FECDC { get; set; }
        public string CO_C_CENCO { get; set; }
        public string CO_A_GLOSA { get; set; }
        public string CO_C_DESTI { get; set; }
        public string CO_C_DOCUM { get; set; }
        public string CO_C_ANEXO { get; set; }
        public string CO_N_DEBE { get; set; }
        public string co_n_debus { get; set; }
        public string co_n_haber { get; set; }
        public string co_n_habus { get; set; }
        public string CO_C_SECUE { get; set; }
        public string NUMRETRAC { get; set; }
        public string FECHAVENC { get; set; }
        public string ORDFAB { get; set; }

    }
}