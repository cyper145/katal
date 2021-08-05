using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.entity
{
    public class Trans
    {
        public List<string> selectedKeyValues { get; set; }

    }

    public class Respuesta
    {
        public decimal data { get; set; }
        public string dataEspecial { get; set; }
        public bool opcion  { get; set; }
        public bool especial  { get; set; }

    }
    public class RespuestaDestino
    {
        public decimal tasaIGV { get; set; }
        public decimal montoIGV { get; set; }
        public bool opcion { get; set; }
        public bool especial { get; set; }

        
    }
    public class RespuestaGastos
    {
        public decimal mnGasto1 { get; set; }
        public decimal mnGasto2 { get; set; }
    }

}