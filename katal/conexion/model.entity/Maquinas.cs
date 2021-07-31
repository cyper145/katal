using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.entity
{
    public class Maquinas
    {
        public string idmaquina { get; set; }
        public string maq_codigo { get; set; }
        public string maq_descripcion { get; set; }
        public string ind_pasos { get; set; }
        public string noproduccion { get; set; }
        public string idproceso { get; set; }
        public string flagmantenimiento { get; set; }
    }
}