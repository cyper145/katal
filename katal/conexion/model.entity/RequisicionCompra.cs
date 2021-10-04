using System;
using System.Collections.Generic;
namespace katal.conexion.model.entity
{
    public class RequisicionCompra
    {
        public string NROREQUI { get; set; }
        public string CODSOLIC { get; set; }
        public DateTime FECREQUI { get; set; }
        public string GLOSA { get; set; }
        public string AREA { get; set; }
        public string ESTREQUI { get; set; }
        public string TIPOREQUI { get; set; }
        public int prioridad { get; set; }
        public DateTime FecEntrega { get; set; }
        public int flgCerrado { get; set; }
        public int IndAutorizado { get; set; }
        public string UsrAutoriza { get; set; }
        public string comrechazo { get; set; }

        public List<DetalleRequisicion> detalles { get; set; }
    }

    public class DetalleRequisicion
    {
        public string NROREQUI { get; set; }
        public string codpro { get; set; }
        public string DESCPRO { get; set; }
        public string UNIPRO { get; set; }
        public decimal CANTID { get; set; }
        public string ESTREQUI { get; set; }
        public DateTime FECREQUE { get; set; }
        public int REQITEM { get; set; }
        public decimal SALDO { get; set; }
        public string CENCOST { get; set; }
        public string GLOSA { get; set; }
        public string REMAQ { get; set; }
        public string TIPOREQUI { get; set; }
        public string ESPTECNICA { get; set; }
        public string stock { get; set; }
    }
}