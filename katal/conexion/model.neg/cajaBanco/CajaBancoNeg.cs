﻿using katal.conexion.model.dao;
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


        public List<CMovimientoBanco> findAllMovimientos(string banco, string moneda, DateTime dateTime)
        {
            return cajaBancoDao.findAllMovimientos(banco, moneda,dateTime);
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

        public string Genera_Secuencia_detalle(string codigoBanco, DateTime fechaoperacion, string secuenciaCab)
        {
            return cajaBancoDao.Genera_Secuencia_detalle(codigoBanco, fechaoperacion, secuenciaCab);
        }

        public List<TemporalGC> allTemporal()
        {
            return cajaBancoDao.allTemporal();
        }

        public List<ConceptoCajaBanco> findAllConceptoCajaBanco(string tipo, string ingresoSalida)
        {
            return cajaBancoDao.findAllConceptoCajaBanco(tipo, ingresoSalida);
        }
        public List<ConceptoCajaBanco> findAllConceptoCajaBanco(string tipo, string ingresoSalida, string operacion)
        {
            return cajaBancoDao.findAllConceptoCajaBanco(tipo, ingresoSalida, operacion);
        }

        public List<TipoMoneda> tipoMonedas()
        {
            return cajaBancoDao.tipoMonedas();
        }

        public void create(CMovimientoBanco obj, string codigoBanco, DateTime dateTime, string cambioMoneda)
        {
            decimal valortipoCambio = cajaBancoDao.tipoCambio(cambioMoneda, obj.CB_C_CONVE, obj.CB_N_CAMES, obj.CB_D_FECCA, obj.CB_D_FECCA,dateTime);
            obj.CB_N_TIPCA = valortipoCambio;
            decimal cambio = Math.Round(obj.CB_N_MTOMN * valortipoCambio, 2);
            obj.CB_N_MTOME = cajaBancoDao.ternarioG(cambioMoneda=="ME", obj.CB_N_MTOME, cambio);
            obj.CB_N_MTOMN = cajaBancoDao.ternarioG(cambioMoneda=="MN", obj.CB_N_MTOME, cambio);
            cajaBancoDao.create(obj, codigoBanco, dateTime);
        }

    }
}