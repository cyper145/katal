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
}