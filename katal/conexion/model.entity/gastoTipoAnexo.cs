﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.entity
{
    public class gastoTipoAnexo
    {
        public  Gasto gasto { set; get; } // para cambiar el comprobante
        public string codigoTipoAnexo { set; get; }// 
    }
}