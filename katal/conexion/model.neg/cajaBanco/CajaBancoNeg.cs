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
        public CajaBanco findBanco(string codigo)
        {
            return cajaBancoDao.findBanco(codigo);
        }


        public List<CMovimientoBanco> findAllMovimientos(string banco, string moneda)
        {
            return cajaBancoDao.findAllMovimientos(banco, moneda);
        }

        public List<TipoOpcionCajaBanco> findAllTipoOpciones(int tipo)
        {
            string tipoingresosalida = "I";
            if (tipo == 1)
            {
                tipoingresosalida = "S";
            }
            return cajaBancoDao.findAllTipoOpciones(tipoingresosalida);
        }
        public TipoOpcionCajaBanco findTipoOpciones(int tipo, string codigoOperacion)
        {
            string tipoingresosalida = "I";
            if (tipo == 1)
            {
                tipoingresosalida = "S";
            }
            return cajaBancoDao.findTipoOpciones(tipoingresosalida, codigoOperacion);
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
        public string numSec(TipoOpcionCajaBanco cajaBanco, string codigoBanco, DateTime mes)
        {
            string numSec = "";
            if (cajaBanco.CB_C_AUTOM == "N")
            {
                numSec = cajaBancoDao.numSec(cajaBanco.CB_C_TPDOC, codigoBanco, mes);
            }
            else
            {

            }
            return "";
        }

        public string Busca_Gen(string concepto)
        {
            return cajaBancoDao.ConceptosGenerales(concepto);
        }

        public string Genera_Secuencia(string codigoBanco, DateTime fechaoperacion)
        {
            return cajaBancoDao.Genera_Secuencia(codigoBanco, fechaoperacion);
        }
        public List<TemporalGC> allTemporal()
        {
            return cajaBancoDao.allTemporal();
        }
    }
}