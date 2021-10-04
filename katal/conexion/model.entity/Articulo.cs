using System.ComponentModel.DataAnnotations;

namespace katal.conexion.model.entity
{
    public class Articulo
    {
        [Required]
        [Display(Name = "Codigo ")]
        public string codigo { get; set; }
        [Required]
        [Display(Name = "Codigo 2")]
        public string codigo2 { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string description { get; set; }
        [Required]
        [Display(Name = "Descripcion 2")]
        public string description2 { get; set; }
        [Required]
        [Display(Name = "Unidad")]
        public string unidad { get; set; }
        [Required]
        [Display(Name = "Referencia")]
        public string referencia { get; set; }
        [Required]
        [Display(Name = "Familia")]
        public string familia { get; set; }
        [Required]
        [Display(Name = "Modelo")]
        public string modelo { get; set; }
        [Required]
        [Display(Name = "Grupo")]
        public string grupo { get; set; }
        [Required]
        [Display(Name = "Peso")]
        public decimal peso { get; set; }
        [Required]
        [Display(Name = "tipo")]
        public string tipo { get; set; }
        [Required]
        [Display(Name = "usuario")]
        public string user { get; set; }
        [Required]
        [Display(Name = "estado")]
        public string estado { get; set; }
        [Required]
        [Display(Name = "fecha")]
        public string fecha { get; set; }
        [Required]
        [Display(Name = "serie")]
        public string serie { get; set; }
        [Required]
        [Display(Name = "lote")]
        public string lote { get; set; }
        public string codmon { get; set; }
        public string igvpor { get; set; }
        public string flagigv { get; set; }
        public string marca { get; set; }
        public string color { get; set; }
        public string talla { get; set; }
        public string foto { get; set; }
        public string comenta { get; set; }
        public string stock { get; set; }
        public int clasificacion { get; set; }
        public string cuenta { get; set; }


    }
}