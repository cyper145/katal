﻿using katal.conexion.model.dao;
using katal.conexion.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.neg
{
    public class ComprobanteDNeg
    {
        ComprobanteDetraccionDao objComprobanteDao;
      
        public ComprobanteDNeg(string codEmpresa)
        {

            objComprobanteDao = new ComprobanteDetraccionDao(codEmpresa);
           
        }

        public List<ComprobanteDetraccion> findAll()
        {
            return objComprobanteDao.findAll();
        }
    }
}