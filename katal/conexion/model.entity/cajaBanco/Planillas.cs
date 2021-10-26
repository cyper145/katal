using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.entity
{
    public class Planillas
    {
        public string DEPNROPLA { get; set; }
        public string DEPSECUENC { get; set; }
        public string DEPTIPDOC { get; set; }
        public string DEPNRODOC { get; set; }
        public string DEPTIPOPER { get; set; }
        public string DEPCONCEP { get; set; }
        public DateTime DEPFECCOB { get; set; }
        public decimal DEPIMPORTE { get; set; }
        public string DEPTIPMON { get; set; }
        public decimal DEPTIPCAM { get; set; }
        public DateTime DEPFECCRE { get; set; }
        public string DEPUSUARI { get; set; }
        public string DEPGLOSA { get; set; }
        public string DEPCOBRA { get; set; }
        public string DEPCODBAN { get; set; }
        public string DEPDESBAN { get; set; }
        public string DEPRFTIPDOC { get; set; }
        public string DEPRFNUMDOC { get; set; }
        public int CODDETPLA { get; set; }
        public string F_CJABCO { get; set; }
        public decimal DEPIMPORTEPERC { get; set; }
        public string DEPBCOGIR { get; set; }
        public string DEPCTABANCH { get; set; }
        public string DEPPERAUTO { get; set; }
        public string FlgPercepcion { get; set; }
        public DateTime FECPAGDOC { get; set; }
        public decimal ImpDeposito { get; set; }
        /* hasta aqui ´la parte de plan cob detalle*/
        public string TIPO { get; set; }
        public string COD_COBRANZA { get; set; }
        public string DESCRIPCION { get; set; }
        public string MONEDA { get; set; }
        public string CUENTA { get; set; }
        public bool ANEX_PROV { get; set; }
        public string BANCOS { get; set; }
        public string USUARIO { get; set; }
        public DateTime FECHA { get; set; }
        public DateTime FECACT { get; set; }
        public bool CHEQ_DIFER { get; set; }
        public bool APLIC_DOC { get; set; }
        public int TIP { get; set; }
        public string TARJCREDITO { get; set; }


  
    }

    public class PlantillaDetalle
    {
        public string Sec { get; set; }
        public string Cliente { get; set; }
        public string TpoDoc { get; set; }
        public string Documento { get; set; }
        public string NroOP { get; set; }
        public string Banco { get; set; }
        public string Moneda { get; set; }
        public decimal Importe { get; set; }
        public int DetKey { get; set; }
    }

    public class comprobanteCabCuentas
    {
        public string SERIEA { get; set; }
        public string NUMEROA { get; set; }
        public string TIPOPROVA { get; set; }
        public string ANEX_DESCRIPCION { get; set; }
        public string CCODPROVE { get; set; }
        public string CCODDOCUM { get; set; }
        public string CCODMONED { get; set; }
        public decimal NPAGAR_MN { get; set; }
        public decimal NPAGAR_US { get; set; }
        public string CCODCONTA { get; set; }
        public DateTime DEMISION { get; set; }
        public DateTime DVENCE { get; set; }
        public string CORDEN { get; set; }
        public string AUTONUM { get; set; }
        public string DESCRIPCION { get; set; }


    }

    public class PlantillacuentaxPagar
    {
        public string Sec { get; set; }
        public string tipoanexo { get; set; }
        public string anexo { get; set; }
        public string tpo { get; set; }
        public string ser { get; set; }
        public string nroDocumento { get; set; }
        public string mon { get; set; }
        public decimal Importe { get; set; }
        public decimal MontoPagar { get; set; }
        public string cuentacontable { get; set; }
        public string identificador { get; set; }
        public DateTime fechaDoc { get; set; }
        public DateTime fechaVec { get; set; }
        public string orden { get; set; }
    }

}