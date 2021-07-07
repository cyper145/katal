using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.entity
{
    public class FormaPago
    {
        public string COD_FP { get; set; }
        public string DES_FP { get; set; }
        public string DIA_FP { get; set; }
        public string USUARIO { get; set; }
        public string FEC_REG { get; set; }
        public string FEC_ACT { get; set; }
        public string flg_ReqLC { get; set; }
        public string DescripEN { get; set; }
        public string FFPAGCOM { get; set; }
        public string Estado { get; set; }

    }
}