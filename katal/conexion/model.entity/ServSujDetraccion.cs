using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.entity
{
    public class ServSujDetraccion
    {
        public string codigo { get; set; }
        public string servicio { get; set; }
        public string vigencia { get; set; }      
    }
    public  class TipoOperacion
    {
        public string CODIGO { get; set; }
        public string TIPO_OPERACION { get; set; }
    }
}