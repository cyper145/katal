namespace katal.conexion.model.entity
{
    public class TipoAnexo
    {
        public string TIPOANEX_CODIGO { get; set; }
        public string TIPOANEX_DESCRIPCION { get; set; }
    }
    public class Anexo
    {
        public string ANEX_CODIGO { get; set; }
        public string TIPOANEX_CODIGO { get; set; }
        public string ANEX_RUC { get; set; }
        public string ANEX_DESCRIPCION { get; set; }
        public string ANEX_REFERENCIA { get; set; }
        public string ANEX_DIRECCION { get; set; }
        public string ANEX_TELEFONO { get; set; }
        public string ANEX_REPRESENTANTE { get; set; }
        public string ANEX_GIRO { get; set; }
        public string NRETENCION { get; set; }
        public string ANEX_NOMBRE { get; set; }
        public string ANEX_APE_PAT { get; set; }
        public string ANEX_APE_MAT { get; set; }
        public string TIPOPERSONA { get; set; }
        public string TIPODOCUMENTO { get; set; }
        public string DOCUMENTOIDENTIDAD { get; set; }
        public string ANE_DETRACCION { get; set; }
        public string ANE_TASA_DETRACC { get; set; }
        public string CTA_DETRACC_BCO { get; set; }
    }
    public class TipoDocumento
    {
        public string TIPDOC_CODIGO { get; set; }
        public string TIPDOC_DESCRIPCION { get; set; }
        public string TIPDOC_SUNAT { get; set; }
        public bool TIPDOC_RESTA { get; set; }
        public bool TIPDOC_REFERENCIA { get; set; }
        public string TIPDOC_FILE { get; set; }
        public string TIPDOC_FECHAVTO { get; set; }
        public string TIPDOC_REGCOMP { get; set; }

    }
}