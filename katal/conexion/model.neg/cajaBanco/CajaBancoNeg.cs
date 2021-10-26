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

        protected int Genera_Secuencia_detalleNroI(string codigoBanco, DateTime fechaoperacion, string secuenciaCab)
        {
            return cajaBancoDao.Genera_Secuencia_detalleNro(codigoBanco, fechaoperacion, secuenciaCab, "I");
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

        public decimal  createorUpdate(CMovimientoBanco obj, string codigoBanco, DateTime dateTime, string cambioMoneda)
        {
            CMovimientoBanco objnuevo = cajaBancoDao.findMovimiento(obj.CB_C_SECUE, codigoBanco, dateTime);

            if (obj.serie == null)
            {
                obj.serie = "";
            }
            if (obj.serie.Length < 4)
            {

                obj.serie = cajaBancoDao.rellenar(obj.serie, 4, obj.serie.Length, " ", false);
            }

            if (objnuevo.CB_C_SECUE != null)
            {
              return  Update(obj, codigoBanco, dateTime, cambioMoneda);
            }
            else
            {
              return  create(obj, codigoBanco, dateTime, cambioMoneda);
            }
        }




        public decimal create(CMovimientoBanco obj, string codigoBanco, DateTime dateTime, string cambioMoneda)
        {
            decimal valortipoCambio = cajaBancoDao.tipoCambio(cambioMoneda, obj.CB_C_CONVE, obj.CB_N_CAMES, obj.CB_D_FECHA, obj.CB_D_FECHA, dateTime);
            obj.CB_N_TIPCA = valortipoCambio;
            decimal cambio = Math.Round(obj.CB_N_MTOMN * valortipoCambio, 2);

            obj.CB_N_MTOME = cajaBancoDao.ternarioG(cambioMoneda == "ME", obj.montoVisual, cambio);
            obj.CB_N_MTOMN = cajaBancoDao.ternarioG(cambioMoneda == "MN", obj.montoVisual, cambio);
            cajaBancoDao.create(obj, codigoBanco, dateTime);
            return valortipoCambio;
        }
        public decimal Update(CMovimientoBanco obj, string codigoBanco, DateTime dateTime, string cambioMoneda)
        {
            decimal valortipoCambio = cajaBancoDao.tipoCambio(cambioMoneda, obj.CB_C_CONVE, obj.CB_N_CAMES, obj.CB_D_FECHA, obj.CB_D_FECHA, dateTime);
            obj.CB_N_TIPCA = valortipoCambio;
            decimal cambio = Math.Round(obj.CB_N_MTOMN * valortipoCambio, 2);

            obj.CB_N_MTOME = cajaBancoDao.ternarioG(cambioMoneda == "ME", obj.montoVisual, cambio);
            obj.CB_N_MTOMN = cajaBancoDao.ternarioG(cambioMoneda == "MN", obj.montoVisual, cambio);
            cajaBancoDao.Update(obj, codigoBanco, dateTime);
            return valortipoCambio;
        }
        public void crearteDetail(string CB_C_SECUE, DMovimientoBanco obj, string codigoBanco, DateTime dateTime, string moneda,string cambioMoneda, string CB_C_BANCO)
        {

            CMovimientoBanco objnuevo = cajaBancoDao.findMovimiento(CB_C_SECUE, codigoBanco, dateTime); 
            decimal valortipoCambio = cajaBancoDao.tipoCambio(cambioMoneda, objnuevo.CB_C_CONVE, objnuevo.CB_N_CAMES, objnuevo.CB_D_FECHA, objnuevo.CB_D_FECHA, dateTime);
           
            // eliminar cartera //  tipodoc, numaux, cliente(anexo)
            if (obj.CB_C_MODO == "I")
            {
                if (obj.serieD.Length < 4)
                {

                    obj.serieD = cajaBancoDao.rellenar(obj.serieD, 4, obj.serieD.Length, " ", false);
                }
                string num_aux = obj.serieD + obj.CB_C_DOCUMD;             
                cajaBancoDao.createCartera(obj, codigoBanco, dateTime, CB_C_BANCO, moneda, valortipoCambio);
            }
            else
            {
                cajaBancoDao.AccionSalida(dateTime, obj, valortipoCambio, 1);
            }
            cajaBancoDao.crearteDetail(objnuevo,  obj,  codigoBanco,  dateTime,  moneda, valortipoCambio);
        }
        public void crearteDetailXplanilla(List<PlantillaDetalle> plantillaDetalles,string CB_C_SECUE, string codigoBanco, DateTime dateTime, string moneda, string REFER)
        {

            CMovimientoBanco objnuevo = cajaBancoDao.findMovimiento(CB_C_SECUE, codigoBanco, dateTime);
            decimal valortipoCambio = cajaBancoDao.tipoCambio(moneda, objnuevo.CB_C_CONVE, objnuevo.CB_N_CAMES, objnuevo.CB_D_FECHA, objnuevo.CB_D_FECHA, dateTime);

            int sec = cajaBancoDao.Genera_Secuencia_detalleNro(codigoBanco, dateTime, CB_C_SECUE, "I"); 

            string doc = "";
            string Clien = "";
            string NUMDOC = "";
            string AnexoCliente = cajaBancoDao.ConceptosGenerales("ANEXOCLIE"); ;
            plantillaDetalles.ForEach(X => {

                if(doc!= X.TpoDoc)
                {
                    doc = X.TpoDoc;
                    Clien = X.Cliente;
                    NUMDOC = X.Documento;
                }
            });
                plantillaDetalles.ForEach(X => {
                ParametrosCuentas parametrosCuentas = cajaBancoDao.findParametrosCuentas(X.TpoDoc);
                Cartera cartera= cajaBancoDao.findCartera(X.Cliente, X.TpoDoc,X.Documento );
                    string tipoMonC = "";
                    string cuenta = "";
                    if (cartera.CDOTIPMON != null)
                    {
                        tipoMonC = cartera.CDOTIPMON;
                    }
                    else
                    {
                        tipoMonC = X.Moneda;
                    }
                    if (tipoMonC == "MN")
                    {
                        cuenta = parametrosCuentas.CTA_SOLES;
                    }
                    else
                    {
                        cuenta = parametrosCuentas.CTA_DOLA;
                    }
                cajaBancoDao.crearteDetailxPlantilla(X, sec++, CB_C_SECUE, codigoBanco, dateTime, AnexoCliente, cartera.CDOFECDOC, REFER, cuenta, tipoMonC, valortipoCambio);
                    cajaBancoDao.UpdatePlantconDet(X.DetKey.ToString(),1); ;


                });
            

        }

        public void crearteDetailXplanillaCuenta(List<PlantillacuentaxPagar> plantillaDetalles, string CB_C_SECUE, string codigoBanco, DateTime dateTime, string moneda, string REFER, DateTime datecuenta,string CB_C_BANCO, string CB_C_NUMCT )
        {

            CMovimientoBanco objnuevo = cajaBancoDao.findMovimiento(CB_C_SECUE, codigoBanco, dateTime);
            decimal valortipoCambio = cajaBancoDao.tipoCambio(moneda, objnuevo.CB_C_CONVE, objnuevo.CB_N_CAMES, objnuevo.CB_D_FECHA, objnuevo.CB_D_FECHA, dateTime);

            int sec = cajaBancoDao.Genera_Secuencia_detalleNro(codigoBanco, dateTime, CB_C_SECUE,"S");
            string CB_C_CODIG = cajaBancoDao.findConceptoCajaBancoCodigo("999");
            string doc = "";
            string Clien = "";
            string NUMDOC = "";
            string AnexoCliente = cajaBancoDao.ConceptosGenerales("ANEXOCLIE"); ;
           
            plantillaDetalles.ForEach(plantilla => {

                comprobanteCabCuentas cuentaCompro=cajaBancoDao.findConceptoCajaBancoCodigo(plantilla);

                if(!cajaBancoDao.isnull(cuentaCompro.CORDEN))
                {
                    cajaBancoDao.updateComprobanteCabCuenta(plantilla, cuentaCompro);
                }
                int vl_td = 0;
                if (cajaBancoDao.verdata("TIPDOC_CODIGO='" + plantilla.tpo + "'", "TIPOS_DE_DOCUMENTOS", 3, 0, "", dateTime) == "S")
                {
                    string data = cajaBancoDao.verdata("TIPDOC_CODIGO='" + plantilla.tpo + "'", "TIPOS_DE_DOCUMENTOS", 3, 1, "TIPDOC_RESTA", dateTime);
                    vl_td = cajaBancoDao.ternarioG(Boolean.Parse(data) == false, 0, 1);
                }
                string codcont = cajaBancoDao.findConceptoCajaBancoCuenta(plantilla, cuentaCompro);
                string centrocostos = "";
                if (!cajaBancoDao.isnull(codcont))
                {
                    centrocostos = cajaBancoDao.findConceptoCajaBancoCentro(cuentaCompro, codcont);
                }

                cajaBancoDao.crearteDetailCuenta(plantilla, cuentaCompro, codcont, centrocostos, codigoBanco, dateTime, CB_C_SECUE, sec++, vl_td, valortipoCambio, REFER);

                if(cajaBancoDao.VerificarPagosDetalle(datecuenta)){
                    cajaBancoDao.UpdatePagoCb(plantilla, datecuenta);
                }
                else
                {
                    cajaBancoDao.crearPagoCb(plantilla, datecuenta);

                }

                cajaBancoDao.crearPagoDetalle(plantilla, objnuevo, datecuenta, moneda, valortipoCambio,dateTime, CB_C_BANCO, CB_C_NUMCT);




            });


        }



        public void UpdateMontosMbanco(DateTime dateTime, string codigobanco, string sec)
        {
            cajaBancoDao.UpdateMontosMbanco(dateTime, codigobanco, sec);
        }

        public void updateDetail(string CB_C_SECUE, DMovimientoBanco obj, string codigoBanco, DateTime dateTime, string moneda, string cambioMoneda,string  CB_C_BANCO) 
        {

            CMovimientoBanco objnuevo = cajaBancoDao.findMovimiento(CB_C_SECUE, codigoBanco, dateTime);
            decimal valortipoCambio = cajaBancoDao.tipoCambio(cambioMoneda, objnuevo.CB_C_CONVE, objnuevo.CB_N_CAMES, objnuevo.CB_D_FECCA, objnuevo.CB_D_FECCA, dateTime);

           
            if (obj.CB_C_MODO == "I")
            {
                if (obj.serieD.Length < 4)
                {

                    obj.serieD = cajaBancoDao.rellenar(obj.serieD, 4, obj.serieD.Length, " ", false);
                }
                string num_aux = obj.serieD + obj.CB_C_DOCUMD;
                if (num_aux.Trim() == "")
                {
                    cajaBancoDao.DeleteCartera(obj);
                }

                cajaBancoDao.updateCartera(obj, codigoBanco, CB_C_BANCO, moneda);
            }
            else
            {
                cajaBancoDao.AccionSalida(dateTime, obj, valortipoCambio, 1);
            }
           
            cajaBancoDao.UpdateDetail (objnuevo, obj, codigoBanco, dateTime, moneda, valortipoCambio);
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

        public CMovimientoBanco findMovimiento(string secuencia, string codigobanco, DateTime dateTime)
        {
           return cajaBancoDao.findMovimiento(secuencia, codigobanco, dateTime);
        }
       
        public void deleteMovimientoBanco(string codigobanco, DateTime dateTime, string secuencia)
        {

            cajaBancoDao.deleteMovimientoBancoDetalle(codigobanco,dateTime, secuencia);
            cajaBancoDao.deleteMovimientoBanco(codigobanco,dateTime, secuencia);
        }
        public void deleteDetailMovimientoBanco(  string codigobanco, DateTime dateTime, string secuencia,string secuenciaD, string moneda)
        {
            CMovimientoBanco objnuevo = cajaBancoDao.findMovimiento(secuencia, codigobanco, dateTime);
            DMovimientoBanco dMovimiento = cajaBancoDao.findDetailMovimiento(secuencia, secuenciaD, codigobanco, moneda, dateTime);
            // primero ver 
            if (cajaBancoDao.ConceptosGenerales("OPLABCOCOB")== objnuevo.CB_C_OPERA)
            {
                if (dMovimiento.CODDETPLA + "" != "")
                {
                    cajaBancoDao.UpdatePlantconDet(dMovimiento.CODDETPLA, 0);
                }
                
            }
            if (cajaBancoDao.ConceptosGenerales("OPERPLACON") == objnuevo.CB_C_OPERA && objnuevo.CB_C_MODO=="I" )
            {

            }
            bool SW = false;
            if(objnuevo.CB_C_MODO == "S" && cajaBancoDao.ConceptosGenerales("OPLABCOPAG")+"" !="" )
            {
                SW = true;
            }
            if(objnuevo.CB_C_MODO == "S" && SW)
            {
                if (dMovimiento.CB_L_INT)
                {
                    CuentaxPagar cuentax = cajaBancoDao.findCuentaxPagar(Conversion.Parseint( dMovimiento.CODDETPLA));
                    bool programacion = bool.Parse( cajaBancoDao.verdata("Concepto_Codigo='PROGRAMACION'", "ConceptoGral", 4, 1, "Concepto_Logico", dateTime));


                    if (programacion )
                    {
                        if(!cajaBancoDao.isnull(cuentax.N_AUTONUM))
                        {
                            cajaBancoDao.updateProgramacion1(dMovimiento, cuentax);
                            cajaBancoDao.updateProgramacion2(dMovimiento, cuentax);
                            cajaBancoDao.updateProgramacion3(dMovimiento, cuentax);
                            cajaBancoDao.updateProgramacionCAB(dMovimiento, cuentax);
                            cajaBancoDao.updateCuentaxPagar(objnuevo.CB_C_SECUE, dMovimiento, cuentax, codigobanco,dateTime);
                        }
                        

                    }
                    PagosDetalle pagosDetalle = cajaBancoDao.findPagoDetalle(dMovimiento);
                    if (!cajaBancoDao.isnull(pagosDetalle.CCODPROVE))
                    {

                        cajaBancoDao.updateProgramacionCabPago(dMovimiento, cuentax, pagosDetalle);
                    }
                    if (programacion )
                    {
                        if (!cajaBancoDao.isnull(cuentax.N_AUTONUM))
                        {
                            cajaBancoDao.updateComprobanteCabEstado5(dMovimiento, cuentax);
                            cajaBancoDao.updateComprobanteCabEstado3(dMovimiento, cuentax);
                            cajaBancoDao.updateComprobanteCabEstado1(dMovimiento, cuentax);
                        }
                    }
                    else
                    {
                       
                            string restricones = cajaBancoDao.generarCondicion(dMovimiento);
                            cajaBancoDao.updateComprobanteCabEstado5C(dMovimiento, restricones);
                            cajaBancoDao.updateComprobanteCabEstado3C(dMovimiento, restricones);
                            cajaBancoDao.updateComprobanteCabEstado1C(dMovimiento, restricones);
                            cajaBancoDao.updateComprobanteCabSaldo(dMovimiento, restricones, moneda);
                       
                    }
                    if (programacion )
                    {
                        if (!cajaBancoDao.isnull(cuentax.N_AUTONUM))
                        {
                            cajaBancoDao.updatePagosCab(dMovimiento, cuentax);
                        }

                    }
                    else
                    {
                        string tipomonm = cajaBancoDao.tipomon(dMovimiento);
                        cajaBancoDao.updatePagosCabsinprogramacion(dMovimiento, tipomonm);
                    }
                    if (programacion )
                    {
                        //eliminar 
                        cajaBancoDao.DeletePROGCANCELXCYB(dMovimiento);
                    }                  
                    cajaBancoDao.DeletePAGOSDET(dMovimiento);
                }
            }

            string tipodocu = cajaBancoDao.ConceptosGenerales("TIPDOCADE");
            tipodocu = cajaBancoDao.ternarioG(tipodocu == "", "", tipodocu);
            if (objnuevo.CB_C_TPDOC == tipodocu)
            {
                cajaBancoDao.updateCPADELANTADOOM(objnuevo, moneda, codigobanco, dateTime);
            }
            DMovimientoBanco movimientoBanco = cajaBancoDao.findDetailMovimientotipDoc(secuencia, secuenciaD, codigobanco, dateTime);
            if (cajaBancoDao.VERIFICA_PLANILLA_COB(objnuevo.CB_C_TPDOC, objnuevo.serie+objnuevo.CB_C_DOCUM, objnuevo.CB_C_ANEXO, dateTime))
            {
               
                if (!cajaBancoDao.isnull(movimientoBanco.CB_C_SECDE))
                {
                    if (movimientoBanco.CB_C_MODO == "I")
                    {
                        cajaBancoDao.DeleteCartera(movimientoBanco);
                    }
                }
            }
            
            if (cajaBancoDao.ConceptosGenerales("CGPAGOACU").Trim()!="")
            {
                if (!cajaBancoDao.isnull(movimientoBanco.CB_C_SECDE))
                {
                    if (movimientoBanco.CB_C_MODO == "S")
                    {
                        cajaBancoDao.DeleteComprobante(movimientoBanco);
                    }
                }
            }

            cajaBancoDao.deleteMovimientoBancoDetalleEspecifico(codigobanco, dateTime, secuencia, secuenciaD);
            
        }

        public List<PlantillaDetalle> AllPlanillasDetalle(string nroPlantilla)
        {
            return cajaBancoDao.AllPlanillasDetalle(nroPlantilla);
        }
        public List<PlantillacuentaxPagar> AllPlanillasCuentasxpagar(string tipoAnexo, string anexo, DateTime dateTime)
        {
            return cajaBancoDao.AllPlanillasCuentasxpagar(tipoAnexo, anexo, dateTime);
        
        }

    }
}