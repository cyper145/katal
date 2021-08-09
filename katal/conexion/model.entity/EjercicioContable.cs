using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.entity
{
    public class EjercicioContable
    {
        public string EJERCICIO_CONTABLE { get; set; }
        public string EJECONT_ANO { get; set; }
        public string EJECONT_CIERRE { get; set; }
        public DateTime EJECONT_INICIO { get; set; }
        public DateTime EJECONT_FINAL { get; set; }

    }
}