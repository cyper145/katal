using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.entity
{
    public class TipoCambio
    {
        public string TIPOMON_CODIGO { get; set; }
        public string TIPOCAMB_FECHA { get; set; }
        public string TIPOCAMB_COMPRA { get; set; }
        public string TIPOCAMB_EQCOMPRA { get; set; }
        public string TIPOCAMB_VENTA { get; set; }
        public string TIPOCAMB_EQVENTA { get; set; }
    }
}