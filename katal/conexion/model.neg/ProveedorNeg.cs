﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using katal.conexion.model.dao;
using katal.conexion.model.entity;

namespace katal.conexion.model.neg
{
    public class ProveedorNeg
    {
        private ProveedorDao objUserDao;
        public ProveedorNeg()
        {
            objUserDao = new ProveedorDao();
        }
        public List<Proveedor> findAll()
        {
            return objUserDao.findAll();
        }
        
    }
}