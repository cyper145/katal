namespace katal.conexion.model.entity
{
    public class Gasto
    {
        public string Gastos_Codigo { get; set; }
        public string Gastos_Descripcion { get; set; }
        public string Gastos_Moneda { get; set; }
        public bool Gastos_Honorario { get; set; }
        public string AREA_CODIGO { get; set; }
        public string Gastos_CuentaCon { get; set; }
        public decimal Gastos_Dscto1 { get; set; }
        public decimal Gastos_Dscto2 { get; set; }
        public string Gastos_Cta1 { get; set; }
        public string Gastos_Cta2 { get; set; }

    }
    public class GastosIngresos
    {
        public string GASING_CODIGO { get; set; }
        public string GASING_DESCRIPCION { get; set; }
        public string GASING_TIPO { get; set; }
    }
}