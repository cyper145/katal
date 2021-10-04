using System.Collections.Generic;

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
        public bool opcion { get; set; }
        public bool especial { get; set; }

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
    public class RespuestaCDataComprobante
    {
        public string NROITEM { get; set; }
        public string GLOSA { get; set; }

    }
    public class RespuestaPlan
    {
        public string xcanexo { get; set; }
        public bool activeCenco { get; set; }
        public bool activeOrb { get; set; }
        public bool activeCtaDest { get; set; }
        public bool activeAnexo { get; set; }
        public decimal xnvalor { get; set; }
    }
}