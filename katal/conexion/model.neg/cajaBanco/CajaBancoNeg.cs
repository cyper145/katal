using katal.conexion.model.dao;
using katal.conexion.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;

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
            return cajaBancoDao.findAllMovimientos(banco, moneda, dateTime);
        }
        public List<DMovimientoBanco> findDetailMovimientos(string secuencia, string banco, string moneda, DateTime dateTime, string tipo)
        {
            return cajaBancoDao.findDetailMovimientos(secuencia, banco, moneda, dateTime, tipo);
        }
        public List<TipoOpcionCajaBanco> findAllTipoOpciones(string tipo)
        {
           
            return cajaBancoDao.findAllTipoOpciones(tipo);
        }
        public TipoOpcionCajaBanco findTipoOpciones(string tipo, string codigoOperacion)
        {
            
            return cajaBancoDao.findTipoOpciones(tipo, codigoOperacion);
        }


        public List<TipoEstadoOperacion> findAllTipoEstadosOperaciones(string tipo)
        {

            return cajaBancoDao.findAllTipoEstadosOperaciones(tipo);
        }
        public List<TipoMovimientos> findAllTipoMovimientos(string tipo)
        {
           
            return cajaBancoDao.findAllTipoMovimientos(tipo);
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

        public List<ConceptoCajaBanco> findAllConceptoCajaBanco(string tipo, string ingresoSalida,string operacion,  bool IS)
        {
            if(IS)
            {
                return cajaBancoDao.findAllConceptoCajaBanco(tipo, ingresoSalida);
            }
            else
            {
                return findAllConceptoCajaBanco(tipo, ingresoSalida, operacion);
            } 
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
            decimal valortipoCambio = cajaBancoDao.tipoCambio(cambioMoneda, obj.CB_C_CONVE, obj.CB_N_CAMES, obj.CB_D_FECCA, obj.CB_D_FECCA, dateTime);
            obj.CB_N_TIPCA = valortipoCambio;
            decimal cambio = Math.Round(obj.CB_N_MTOMN * valortipoCambio, 2);
            obj.CB_N_MTOME = cajaBancoDao.ternarioG(cambioMoneda == "ME", obj.CB_N_MTOME, cambio);
            obj.CB_N_MTOMN = cajaBancoDao.ternarioG(cambioMoneda == "MN", obj.CB_N_MTOME, cambio);
            cajaBancoDao.create(obj, codigoBanco, dateTime);
        }

        public void crearteDetail(string CB_C_SECUE, DMovimientoBanco obj, string codigoBanco, DateTime dateTime, string moneda,string cambioMoneda)
        {

            CMovimientoBanco objnuevo = cajaBancoDao.findMovimiento(CB_C_SECUE, codigoBanco, moneda, dateTime); 
            decimal valortipoCambio = cajaBancoDao.tipoCambio(cambioMoneda, objnuevo.CB_C_CONVE, objnuevo.CB_N_CAMES, objnuevo.CB_D_FECCA, objnuevo.CB_D_FECCA, dateTime);

                 
            cajaBancoDao.crearteDetail(objnuevo,  obj,  codigoBanco,  dateTime,  moneda, valortipoCambio);
        }

        public bool exiteFactura(DateTime dateTime)
        {

            string criterio = deteminarCriterio();
            string consulta = deteminarconsulta(criterio, dateTime);
            bool consul = cajaBancoDao.hacerConsuta(consulta);
            return consul;
        }
        protected string deteminarCriterio()
        {
            string cadena = Busca_Gen("TIPOPAGO");
            string criterio = "";
            List<string> subs;
            if (cadena != "")
            {
                subs = cadena.Split(';').ToList();

                subs.ForEach(X =>
                {
                    criterio += "CFFORVEN='" + X + "' OR ";
                });
                criterio = criterio.Substring(0, criterio.Length - 3);
            }
            else
            {
                criterio = "N";
            }
            return criterio;
        }
        protected string deteminarconsulta(string criterio, DateTime dateTime)
        {
            string cadena = Busca_Gen("IMPEXPCAJA");
            string consulta = "";
            if (cadena != "")
            {
                consulta = "";
            }
            else
            {
                /*
                If VGINTFAC = "CAJA" Then
                  CADSQL = "SELECT * FROM FACCAB WHERE CFFECDOC='" & Format(VGFecTrb, "DD/mm/YYYY") & "' AND (" & cCriterio & ") AND F_CJABCO=0 AND CFTD<>'NC' AND CFESTADO<>'A' ORDER BY CFTD, CFNUMSER, CFNUMDOC;"
                Else
                   CADSQL = "SELECT * FROM FACCAB WHERE CFFECDOC<='" & Format(VGFecTrb, "DD/mm/YYYY") & "' AND (" & cCriterio & ") AND F_CJABCO=0 AND CFTD<>'NC' AND CFESTADO<>'A' ORDER BY CFTD, CFNUMSER, CFNUMDOC;"
                End If
                */
                consulta = $"SELECT * FROM FACCAB WHERE CFFECDOC<={cajaBancoDao.dateFormat(dateTime)} AND ({criterio}) AND F_CJABCO=0 AND CFTD<>'NC' AND CFESTADO<>'A' ORDER BY CFTD, CFNUMSER, CFNUMDOC";

            }
            return consulta;
        }

        public string verDataProgramacion(DateTime dateTime)
        {
            string valor = cajaBancoDao.verdata("Concepto_Codigo='PROGRAMACION'", "ConceptoGral", 4, 1, "Concepto_Logico", dateTime);
            return valor;
        }
        public List<Cobranzas> AllConbranzas(DateTime dateTime)
        {
            return cajaBancoDao.AllConbranzas(dateTime);
        }
        public void deleteMovimientoBanco(string codigobanco, DateTime dateTime, string secuencia)
        {

            cajaBancoDao.deleteMovimientoBancoDetalle(codigobanco,dateTime, secuencia);
            cajaBancoDao.deleteMovimientoBanco(codigobanco,dateTime, secuencia);
        }

    }
}