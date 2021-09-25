using katal.conexion.model.dao;
using katal.conexion.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace katal.conexion.model.neg
{
    public class CajaBancoNeg
    {

        CajaBancoDao cajaBancoDao;
        public CajaBancoNeg(string codEmpresa)
        {
            cajaBancoDao = new CajaBancoDao(codEmpresa);
        }
        public List<CajaBanco> findAll()
        {
            return cajaBancoDao.findAll();
        }
        public List<CMovimientoBanco> findAllMovimientos(string banco, string moneda)
        {
            return cajaBancoDao.findAllMovimientos(banco, moneda);
        }

        public List<TipoOpcionCajaBanco> findAllTipoOpciones( int tipo)
        {
            string tipoingresosalida="I";
            if (tipo == 1)
            {
                tipoingresosalida = "S";
            }
            return cajaBancoDao.findAllTipoOpciones(tipoingresosalida);
        }
        public List<TipoEstadoOperacion> findAllTipoEstadosOperaciones(string tipo)
        {

            return cajaBancoDao.findAllTipoEstadosOperaciones(tipo);
        }
        public List<TipoMovimientos> findAllTipoMovimientos(int tipo)
        {
            string tipoIngresosalida = "I";
            if (tipo == 1)
            {
                tipoIngresosalida = "S";
            }
            return cajaBancoDao.findAllTipoMovimientos(tipoIngresosalida);
        }
        public List<MedioPago> findAllMedioPago()
        {
            return cajaBancoDao.findAllMedioPago();
        }


    }
}