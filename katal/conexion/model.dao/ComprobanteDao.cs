using katal.conexion.model.entity;
using katal.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;

namespace katal.conexion.model.dao
{
    public class ComprobanteDao : Obligatorio
    {


        public ComprobanteDao(string codEmpresa) : base(codEmpresa)
        {
            objConexion = Conexion.saberEstado();
        }

        //public ComprobanteDao reporte()
        /*{
            string sp=""
            SqlCommand sql_cmnd = new SqlCommand(sp, sqlCon);
            sql_cmnd.Parameters.AddWithValue("@usuario", SqlDbType.NVarChar).Value = usuario;
            sql_cmnd.Parameters.AddWithValue("@cuenta", SqlDbType.NVarChar).Value = cuenta;
            sql_cmnd.CommandType = CommandType.StoredProcedure;
        //*}*/


        public bool create(Comprobante obj)
        {
            DateTime date = DateTime.Now;
            string sEstado = "";
            decimal nPorcen = 0;
            decimal nValCIF = 0;
            string anios = date.Year.ToString("0000.##");
            string mes = date.Month.ToString("00.##");
            string msAnoMesProc = anios + mes;
            string verificacion = verificaDoc_cxc(obj.ANEX_CODIGO, obj.TIPODOCU_CODIGO, obj.CSERIE, obj.CNUMERO);

            string cCorre = "";

            try
            {
                if (verificacion != "" && verificacion != "El Documento Esta en Cuentas x Pagar")
                {

                    string Cadenar = "UPDATE [014BDCOMUN].[dbo].COMPROBANTECAB SET CNUMORDCO='" + obj.CTDREFER.Trim() + "',";
                    Cadenar += " NUMRETRAC='" + obj.NUMRETRAC + "', CAOCOMPRA='" + obj.CAOCOMPRA + "',";
                    Cadenar += " dvence=" + obj.DVENCE + "";
                    Cadenar += ",FECRETRAC=" + obj.FECRETRAC;
                    Cadenar += " WHERE CAMESPROC='" + msAnoMesProc + "' AND CORDEN='" + obj.CORDEN + "'";
                    comando = new SqlCommand(Cadenar, objConexion.getCon());
                    objConexion.getCon().Open();
                    comando.ExecuteNonQuery();
                }

                sEstado = Estado();
                switch (obj.CDESTCOMP)
                {
                    case "002":
                        nPorcen = 100;
                        nValCIF = 0;
                        break;
                    case "005":
                        //falta capturar esta data
                        nPorcen = obj.NPORCE;
                        nValCIF = obj.NVALCIF;
                        break;
                    default:
                        nPorcen = 0;
                        nValCIF = 0;
                        break;
                }

                cCorre = funcAutoNum(msAnoMesProc);


                string CadenaD = "INSERT INTO [014BDCOMUN].[dbo].COMPROBANTECAB (";
                CadenaD += "EMP_CODIGO,CORDEN,ANEX_CODIGO,ANEX_DESCRIPCION,TIPODOCU_CODIGO,CSERIE, ";
                CadenaD += "CNUMERO, DEMISION, DVENCE, DRECEPCIO, TIPOMON_CODIGO, ";
                CadenaD += "NIMPORTE, TIPOCAMBIO_VALOR, CDESCRIPC, RESPONSABLE_CODIGO, CESTADO, ";
                CadenaD += "NSALDO, CCODCONTA, CFORMPAGO, CSERREFER, CNUMREFER, ";
                CadenaD += "CTDREFER, CONVERSION_CODIGO, DREGISTRO, CTIPPROV, CNRORUC, ";
                CadenaD += "ESTCOMPRA, CDESTCOMP, DIASPAGO, CIGVAPLIC, CCONCEPT, ";
                CadenaD += "DFECREF, NTASAIGV, NIGV, NPORCE, CCODRUC, ";
                CadenaD += "LHONOR, NIR4, NIES, NTOTRH, NBASEIMP, ";
                CadenaD += "NVALCIF, DCONTAB, CAMESPROC, CSALDINI,NPERCEPCION,NUMRETRAC,";
                CadenaD += "FECRETRAC,CNUMORDCO,";
                CadenaD += "CO_L_RETE,LDETRACCION,NTASADETRACCION, DETRACCION,COD_SERVDETRACC,COD_TIPOOPERACION,";
                CadenaD += "nImporteRef, RCO_TIPO, RCO_SERIE, RCO_NUMERO,";
                // If IsDate(txtFecDocRef) Then
                CadenaD += "RCO_FECHA,";
                CadenaD += "flg_RNTNODOMICILIADO,CAOCOMPRA) VALUES (";
                CadenaD += "'" + GridViewHelper.user.codEmpresa + "','" + cCorre + "', '" + obj.ANEX_CODIGO + "', '" + obj.ANEX_DESCRIPCION + "', '" + obj.TIPODOCU_CODIGO + "', '" + obj.CSERIE + "',";
                CadenaD += "'" + obj.CNUMERO + "', " + dateFormat(obj.DEMISION) + ", " + dateFormat(obj.DVENCE) + ", " + dateFormat(obj.DRECEPCIO) + ", '" + obj.TIPOMON_CODIGO + "',";
                //End If
                //
                if (obj.CDESTCOMP.Trim() == "007" || obj.CDESTCOMP.Trim() == "008")
                {
                    // CadenaD += "" + IIf(true, Math.Round(Val(obj.NTOTRH), 2) - Math.Round(Val(obj.NIR4), 2), Math.Round(Val(obj.NIMPORTE), 2)) + ", ";
                    CadenaD += "" + IIfData(obj.LHONOR, Math.Round(obj.NTOTRH, 2) - Math.Round(obj.NIR4, 2), Math.Round(obj.NIMPORTE, 2)) + ", ";
                }
                else
                    if (obj.CDESTCOMP.Trim() == "006")
                {
                    CadenaD += "" + IIfData(obj.LHONOR, Math.Round(obj.NTOTRH, 2) - Math.Round(obj.NIR4, 2) + Math.Round(obj.NIMPORTE, 2), Math.Round(obj.NIMPORTE, 2)) + ", ";
                }
                else
                {
                    CadenaD += "" + IIfData(obj.LHONOR, Math.Round(obj.NTOTRH, 2), Math.Round(obj.NIMPORTE, 2)) + ", ";
                }

                //   CadenaD += "" + obj.CONVERSION_CODIGO == "ESP" ? obj.TIPOCAMBIO_VALOR  : 0    + ", '" + obj.CDESCRIPC + "', '" + obj.RESPONSABLE_CODIGO + "', '" + sEstado + "', ";
                CadenaD += "" + obj.TIPOCAMBIO_VALOR.ToString("F3", CultureInfo.InvariantCulture) + ", '" + obj.CDESCRIPC + "', '" + obj.RESPONSABLE_CODIGO + "', '" + sEstado + "', ";
                //
                if (obj.CDESTCOMP.Trim() == "007" || obj.CDESTCOMP.Trim() == "008")
                {
                    // CadenaD += "" + IIf(true, Math.Round(Val(obj.NTOTRH), 2) - Math.Round(Val(obj.NIR4), 2), Math.Round(Val(obj.NIMPORTE), 2)) + ", ";
                    CadenaD += "" + IIfData(obj.LHONOR, Math.Round(obj.NTOTRH + obj.NPERCEPCION, 2) - Math.Round(obj.NIR4, 2), Math.Round(obj.NIMPORTE + obj.NPERCEPCION, 2)) + ", ";
                }
                else
                    if (obj.CDESTCOMP.Trim() == "006")
                {
                    CadenaD += "" + IIfData(obj.LHONOR, Math.Round(obj.NTOTRH + obj.NPERCEPCION, 2) - Math.Round(obj.NIR4, 2) + Math.Round(obj.NIES, 2), Math.Round(obj.NIMPORTE + obj.NPERCEPCION, 2)) + ", ";
                }
                else
                {
                    CadenaD += "" + IIfData(obj.LHONOR, Math.Round(obj.NTOTRH + obj.NPERCEPCION, 2), Math.Round(obj.NIMPORTE + obj.NPERCEPCION, 2)) + ", ";
                }
                obj.DREGISTRO = obj.DEMISION;// 
                // cuando hay honorarios
                CadenaD += "'" + obj.CCODCONTA.Trim() + "', '" + funcBlanco(obj.CFORMPAGO) + "', '" + funcBlanco(obj.CSERREFER) + "', '" + funcBlanco(obj.CNUMREFER) + "',";
                CadenaD += "'" + funcBlanco(obj.CTDREFER) + "', '" + obj.CONVERSION_CODIGO + "', " + dateFormat(obj.DREGISTRO) + ", '" + obj.CTIPPROV + "', '" + obj.CNRORUC + "', ";
                CadenaD += "" + boolToInt(obj.ESTCOMPRA) + ", '" + funcBlanco(obj.CDESTCOMP) + "', " + obj.DIASPAGO + ", '" + boolToInt(obj.CIGVAPLIC) + "', '" + obj.CCONCEPT + "',";
                CadenaD += "" + dateFormat(obj.DFECREF) + ", " + obj.NTASAIGV + ", " + obj.NIGV + ", " + nPorcen + ", '" + funcBlanco(obj.CCODRUC) + "', ";
                CadenaD += "" + 0 + ", " + obj.NIR4 + ", " + obj.NIES + ", " + obj.NTOTRH + ", " + obj.NBASEIMP + ",";
                CadenaD += "" + nValCIF + ", " + dateFormat(obj.DEMISION) + ", '" + msAnoMesProc + "', " + boolToInt(obj.CSALDINI) + "," + obj.NPERCEPCION + ",'" + obj.NUMRETRAC + "'";
                CadenaD += "," + dateFormat(obj.FECRETRAC) + "";

                CadenaD += ",'" + funcBlanco(obj.CNUMORDCO).Trim() + "','" + obj.CO_L_RETE + "',";


                if (obj.NTASAIGV == 0)
                {
                    CadenaD += "0,0,0,'','',0,";
                }
                else
                {
                    CadenaD += boolToInt(obj.LDETRACCION) + "," + obj.NTASADETRACCION + "," + (obj.NIMPORTE * obj.NTASADETRACCION / 100) + ",'" + obj.COD_SERVDETRACC + "','" + obj.COD_TIPOOPERACION + "'," + obj.NIMPORTEREF + ",";

                }
                CadenaD += "'" + obj.RCO_TIPO + "','" + obj.RCO_SERIE + "','" + obj.RCO_NUMERO + "',";
                CadenaD += "" + dateFormat(obj.RCO_FECHA) + ",";
                CadenaD += obj.flg_RNTNODOMICILIADO + ",'" + obj.CAOCOMPRA + "')";
                comando = new SqlCommand(CadenaD, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;

            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }



        }
        public void create(Comprobante comprobante, int nivelContable)
        {

            string label3 = "";
            string xcanexo = "";
            string xco_c_conco = "";
            bool xco_c_cenco = false;
            string xco_c_tipo = "";
            string xccosto = "";

            PlanCuentaNacional planCuenta = findCuentasNacionales(comprobante.ContableDet.CCODCONTA, nivelContable);
            if (planCuenta != null)
            {
                label3 = fValNull(planCuenta.PLANCTA_DESCRIPCION);
                xcanexo = fValNull(planCuenta.TIPOANEX_CODIGO);
                xco_c_cenco = planCuenta.PLANCTA_CENTCOST;
                xco_c_conco = planCuenta.PLANCTA_CON_COSTO;

                if (xco_c_conco != null)
                {
                    GastosIngresos gastoIngresos = findGastoIngreso(xco_c_conco);
                    if (gastoIngresos == null)
                    {
                        xco_c_tipo = "N";
                    }
                    else
                    {
                        xco_c_tipo = fValNull(gastoIngresos.GASING_TIPO);
                    }
                }
                else
                {
                    xco_c_tipo = "N";
                }
                xccosto = comprobante.ContableDet.CCOSTO;
            }

            string Haber = "H";
            string Debe = "D";
            string data = comprobante.ContableDet.CTIPO == "0" ? Debe : Haber;
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion} (CNROITEM,CTIPO,CCODCONTA,CANEXO,CCODANEXO,CTIPDOC,CSERDOC,CNUMDOC,CCTADEST,";
            csql += "CCOSTO,CCODSUBDI,NVALOR,CGLOSA,ORDFAB, codmaquina, cantidad) Values(";

            csql += $"'{comprobante.ContableDet.CNROITEM }',";
            csql += $"'{data}',";
            csql += $"'{comprobante.ContableDet.CCODCONTA }',";
            csql += $"'{comprobante.CTIPPROV }',";
            csql += $"'{comprobante.ANEX_CODIGO }',";
            csql += $"'{comprobante.TIPODOCU_CODIGO }',";
            csql += $"'{comprobante.CSERIE }',";
            csql += $"'{comprobante.CNUMERO }',";
            csql += $"'{comprobante.ContableDet.CCTADEST }',";
            csql += $"'{comprobante.ContableDet.CCOSTO }',";
            csql += $"'{comprobante.ContableDet.CCODSUBDI }',";
            csql += $"'{comprobante.ContableDet.NVALOR }',";
            csql += $"'{comprobante.ContableDet.CGLOSA }',";
            csql += $"'{comprobante.ContableDet.ORDFAB }',";
            csql += $"'{comprobante.ContableDet.codmaquina }',";
            csql += $"'{comprobante.ContableDet.cantidad }')";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        public void transferir(Comprobante comprobante, int nivelContable)
        {
            string xccodsubdi = "";
            string xco_a_glosa = "";
            string cCOrden = "";
            string WCCODDOCUM = "";
            int anio = 0;
            int mes = 0;
            bool wco_l_resta = false;
            decimal importe = 0;



            xccodsubdi = obtenerValer(1, "SUBDICOMPRA", "ConceptoGral", "Concepto_Codigo", false, "Concepto_Caracter");
            // falta el xccodsubdi por medio de popup

            cCOrden = comprobante.CORDEN;
            anio = comprobante.DREGISTRO.Year;
            mes = comprobante.DREGISTRO.Month;
            WCCODDOCUM = comprobante.TIPODOCU_CODIGO;

            TipoDocumento tipoDocumento = findTipoDocumento(WCCODDOCUM);

            if (tipoDocumento != null)
            {
                wco_l_resta = tipoDocumento.TIPDOC_RESTA;
            }
            deleteCabeceraTrans();
            deleteDetalleTrans();
            EjercicioContable ejercicio = findEContable(anio);
            xco_a_glosa = comprobante.CDESCRIPC;
            string BDTransacCon = "BDCONT" + anio;
            string BDMovCab = "CABMOV" + mes;
            string BDMovDet = "DETMOV" + mes;
            string mNumerodata = mNumero(anio, mes, xccodsubdi);// ver mas detalle
            /*
            if (wlConten == false)
            {
                frmTConta3.text1 = MNUMERO;
                RestauraPuntero();
                frmTConta3.Show(1);
                if (frmTConta3.Vale)
                {
                    if (frmTConta3.Valor != "")
                        MNUMERO = frmTConta3.Valor;
                }
                else
                    return;
                zco_c_numer = Format(MNUMERO, "0000");
                xco_c_numer = zco_c_numer;
            }
            else
            {
                zco_c_numer = Format(MNUMERO, "0000");
                xco_c_numer = zco_c_numer;
            };
            */
            // ver el formato de data
            CabMov cabMov = findCabMov(anio, mes, xccodsubdi, mNumerodata);
            TipoCambio tipoCambio = findTcamabio(comprobante.DEMISION);
            importe = comprobante.NIMPORTE;
            insertCabecera(comprobante, tipoCambio, xccodsubdi, mNumerodata);
            ALTERDetalleDrop();
            ALTERDetalleAdd();
            if (!exiteCampo("DETALLE", "ORDFAB"))
            {
                ALTERdetalleOrbfa();
            }
            insertDetalle(comprobante, xccodsubdi, mNumerodata);
            updateCom(comprobante, xccodsubdi);
            transfer(comprobante, anio, mes, xccodsubdi, wco_l_resta, xco_a_glosa, mNumerodata);
        }

        private void insertCabecera(Comprobante comprobante, TipoCambio tipoCambio, string codigosub, string codigo)
        {
            if (tipoCambio == null)
            {
                tipoCambio = new TipoCambio();
            }

            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "CABECERA");
            string csql = $"Insert Into {conexion}(CO_C_SUBDI,CO_C_COMPR,CO_D_FECHA,CO_A_GLOSA,CO_C_MONED,";
            csql += "CO_C_CONVE,CO_D_FECCA,CO_N_DEBE,co_n_haber,co_n_debus,co_n_habus,";
            csql += "CO_N_TIPCA,CO_N_CAMES,CO_L_COMPR,FECH_VEN) Values(";
            csql += "'" + fValNull(codigosub) + "',";
            csql += "'" + fValNull(codigo) + "',";
            csql += "" + dateFormat(comprobante.DEMISION) + ",";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'" + fValNull(comprobante.TIPOMON_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CONVERSION_CODIGO) + "',";
            csql += "" + dateFormat(comprobante.DEMISION) + ",";
            //comprobante.NIMPORTE.ToString("F3", CultureInfo.InvariantCulture)
            if (comprobante.TIPOMON_CODIGO == "MN")
            {
                csql += "" + comprobante.NIMPORTE.ToString("F3", CultureInfo.InvariantCulture) + ",";
                csql += "" + comprobante.NIMPORTE.ToString("F3", CultureInfo.InvariantCulture) + ",";
                if (comprobante.TIPOCAMBIO_VALOR != 0)
                {
                    csql += "" + (comprobante.NIMPORTE / comprobante.TIPOCAMBIO_VALOR).ToString("F3", CultureInfo.InvariantCulture) + ",";
                    csql += "" + (comprobante.NIMPORTE / comprobante.TIPOCAMBIO_VALOR).ToString("F3", CultureInfo.InvariantCulture) + ",";
                }
                else
                {
                    csql += "" + (0).ToString("F3", CultureInfo.InvariantCulture) + ",";
                    csql += "" + (0).ToString("F3", CultureInfo.InvariantCulture) + ",";
                }
                if (comprobante.CONVERSION_CODIGO == "COM")
                {
                    csql += "'" + tipoCambio.TIPOCAMB_EQCOMPRA.ToString("F3", CultureInfo.InvariantCulture) + "',";

                }
                else
                {
                    if (comprobante.CONVERSION_CODIGO == "VTA")
                    {
                        csql += "'" + tipoCambio.TIPOCAMB_EQVENTA.ToString("F3", CultureInfo.InvariantCulture) + "',";

                    }
                    else
                    {
                        csql += "'" + comprobante.TIPOCAMBIO_VALOR.ToString("F3", CultureInfo.InvariantCulture) + "',";

                    }
                }

                if (comprobante.CONVERSION_CODIGO == "ESP")
                {
                    csql += "" + comprobante.TIPOCAMBIO_VALOR.ToString("F3", CultureInfo.InvariantCulture) + ",";
                }
                else
                {
                    csql += "" + (0).ToString("F3", CultureInfo.InvariantCulture) + ",";

                }
            }
            else// ME
            {
                csql += "" + (comprobante.NIMPORTE * comprobante.TIPOCAMBIO_VALOR).ToString("F3", CultureInfo.InvariantCulture) + ",";
                csql += "" + (comprobante.NIMPORTE * comprobante.TIPOCAMBIO_VALOR).ToString("F3", CultureInfo.InvariantCulture) + ",";
                csql += "" + (comprobante.NIMPORTE).ToString("F3", CultureInfo.InvariantCulture) + ",";
                csql += "" + (comprobante.NIMPORTE).ToString("F3", CultureInfo.InvariantCulture) + ",";
                csql += "" + comprobante.TIPOCAMBIO_VALOR.ToString("F3", CultureInfo.InvariantCulture) + ",";
                if (comprobante.CONVERSION_CODIGO == "ESP")
                {
                    csql += "" + comprobante.TIPOCAMBIO_VALOR.ToString("F3", CultureInfo.InvariantCulture) + ",";
                }
                else
                {
                    csql += "" + (0).ToString("F3", CultureInfo.InvariantCulture) + ",";

                }
            }
            csql += "1," + dateFormat(comprobante.DVENCE) + ")";

            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        public void ALTERDetalleDrop()
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "DETALLE");
            string alter = $"ALTER TABLE {conexion} DROP COLUMN CO_C_SECUE";
            try
            {
                comando = new SqlCommand(alter, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        public void ALTERDetalleAdd()
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "DETALLE");
            string alter = $"ALTER TABLE {conexion}  ADD CO_C_SECUE varchar(5) NULL";
            try
            {
                comando = new SqlCommand(alter, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        public void ALTERdetalleOrbfa()
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "DETALLE");
            string delete = $"ALTER TABLE {conexion} ADD ORDFAB varchar(20) NULL";
            try
            {
                comando = new SqlCommand(delete, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        private void insertDetalle(Comprobante comprobante, string xccodsubdi, string xco_c_numer)
        {
            List<ContableDet> detalles = findContableDet();
            string item = "";
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "DETALLE");

            detalles.ForEach(element =>
            {
                // validacion de codigo de serie

                string Documento = fValNull(element.CTIPDOC) + element.CSERDOC + fValNull(element.CNUMDOC);

                string ANEXO = fValNull(element.CANEXO) + fValNull(element.CCODANEXO);

                item += $"Insert Into {conexion}(CO_C_SUBDI,CO_C_COMPR,CO_D_FECHA,CO_C_CUENT,";
                item += "CO_D_FECDC,CO_C_CENCO,CO_A_GLOSA,CO_C_DESTI,";
                item += "CO_C_DOCUM,CO_C_ANEXO,CO_N_DEBE,co_n_debus,";
                item += "co_n_haber,co_n_habus,CO_C_SECUE,NUMRETRAC,";
                item += "FECRETRAC,";
                item += "FECHAVENC,ORDFAB) Values(";

                item += "'" + fValNull(xccodsubdi) + "',";
                item += "'" + fValNull(xco_c_numer) + "',";
                item += "" + dateFormat(comprobante.DRECEPCIO) + ",";
                item += "'" + fValNull(element.CCODCONTA) + "',";
                item += "" + dateFormat(comprobante.DEMISION) + ",";
                item += "'" + fValNull(element.CCOSTO) + "',";
                item += "'" + fValNull(element.CGLOSA) + "',";
                item += "'" + fValNull(element.CCTADEST) + "',";
                item += "'" + fValNull(Documento) + "',";
                item += "'" + fValNull(ANEXO) + "',";

                if (element.CTIPO == "D")
                {
                    if (comprobante.TIPOMON_CODIGO == "MN")
                    {
                        item += "" + element.NVALOR.ToString("F3", CultureInfo.InvariantCulture) + ",";
                        if (comprobante.TIPOCAMBIO_VALOR != 0)
                        {
                            item += "" + (element.NVALOR / comprobante.TIPOCAMBIO_VALOR).ToString("F3", CultureInfo.InvariantCulture) + ",";
                        }
                        else
                        {
                            item += "" + (0).ToString("F3", CultureInfo.InvariantCulture) + ",";
                        }


                    }
                    else
                    {
                        item += "" + (element.NVALOR * comprobante.TIPOCAMBIO_VALOR).ToString("F3", CultureInfo.InvariantCulture) + ",";
                        item += "" + (element.NVALOR).ToString("F3", CultureInfo.InvariantCulture) + ",";

                    }
                    item += "" + (0).ToString("F3", CultureInfo.InvariantCulture) + ",";
                    item += "" + (0).ToString("F3", CultureInfo.InvariantCulture) + ",";
                    item += "'" + fValNull(element.CNROITEM) + "','" + fValNull(element.NUMRETRAC) + "',";
                    item += "" + dateFormat(element.FECRETRAC) + ",";
                    item += "" + dateFormat(comprobante.DVENCE) + ",'" + fValNull(element.ORDFAB) + "')";
                }

                else // haber
                {
                    item += "" + (0).ToString("F3", CultureInfo.InvariantCulture) + ",";
                    item += "" + (0).ToString("F3", CultureInfo.InvariantCulture) + ",";
                    if (comprobante.TIPOMON_CODIGO == "MN")
                    {
                        item += "" + element.NVALOR.ToString("F3", CultureInfo.InvariantCulture) + ",";
                        if (comprobante.TIPOCAMBIO_VALOR != 0)
                        {
                            item += "" + (element.NVALOR / comprobante.TIPOCAMBIO_VALOR).ToString("F3", CultureInfo.InvariantCulture) + ",";
                        }
                        else
                        {
                            item += "" + (0).ToString("F3", CultureInfo.InvariantCulture) + ",";
                        }


                    }
                    else
                    {
                        item += "" + (element.NVALOR * comprobante.TIPOCAMBIO_VALOR).ToString("F3", CultureInfo.InvariantCulture) + ",";
                        item += "" + (element.NVALOR).ToString("F3", CultureInfo.InvariantCulture) + ",";

                    }
                    item += "'" + fValNull(element.CNROITEM) + "','" + fValNull(element.NUMRETRAC) + "',";
                    item += "" + dateFormat(element.FECRETRAC) + ",";
                    item += "" + dateFormat(comprobante.DVENCE) + ",'" + fValNull(element.ORDFAB) + "')";
                }
            });
            try
            {
                comando = new SqlCommand(item, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }

        }

        private void updateCom(Comprobante comprobante, string xccodsubdi)
        {

            string conexion = Conexion.CadenaGeneral("014", "BDCOMUN", "COMPROBANTECAB");

            string csql = $"Update  {conexion} set ";
            csql += " Subdiario_Codigo='" + xccodsubdi + "' ";
            csql += " where Emp_Codigo='" + comprobante.EMP_CODIGO + "' and camesproc='" + comprobante.CAMESPROC + "' and corden='" + comprobante.CORDEN + "'";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        private void updateComCompCon(Comprobante comprobante, string numero)
        {

            string csql = $"Update COMPROBANTECAB set ";
            csql += " COMPCON='" + numero + "' ";
            csql += " where  camesproc='" + comprobante.CAMESPROC + "' and corden='" + comprobante.CORDEN + "'";
            try
            {
                comando = new SqlCommand(conexionComun(csql), objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }


        private void transfer(Comprobante comprobante, int anio, int mes, string xccodsubdi, bool wco_l_resta, string glosa, string xco_c_numer)
        {
            string BDMovCab = "CABMOV" + mes.ToString("00.##");
            string BDMovCab2 = "DETMOV" + mes.ToString("00.##");
            string conexion1 = Conexion.CadenaGeneral("014", "BDCONT" + anio, BDMovCab);
            string conexion2 = Conexion.CadenaGeneral("014", "BDCONT" + anio, BDMovCab2);
            string conexion3 = Conexion.CadenaGeneral("014", "BDCONT" + anio, "Compras");
            if (comprobante.COMPCON != null && comprobante.COMPCON.Trim() != "")
            {
                deleteBDMovCab(comprobante, conexion1);
                deleteBDMovDet(comprobante, conexion2);
                deleteCompras(comprobante, conexion3);
            }
            Cabecera cabecera = findCabecera();

            string numero = mNumeroBDMovCab(anio, mes, xccodsubdi);
            if (!GridViewHelper.wlConten)
            {
                numero = cabecera.CO_C_COMPR;
            }
            INSERTBDMovCab(comprobante, anio, BDMovCab, cabecera, numero);
            INSERTBDMovDet(comprobante, conexion1, anio, mes, xccodsubdi, numero);
            //Si este movimiento va a compras hacer : ....
            updateComCompCon(comprobante, numero);
            if (comprobante.ESTCOMPRA)
            {
                string mess = mes.ToString("00.##");
                guardarCompra(comprobante, xccodsubdi, anio, mess, numero, wco_l_resta, glosa, xco_c_numer);
            }

            //
            string conta;

            conta = xccodsubdi + xco_c_numer;

            updateComCompCon2(comprobante, conta);
            updateComEstado(comprobante);



            /* guardado para cuentas por pagar
            Set rsEmp = New ADODB.Recordset 'Verifica de que exista BDContabilidad antes de hacer el enlace
        Set rsEmp = cConeWenco.Execute("exec sp_existebasededatos '" & VGEMP_CODIGO & "BDCTAPAG'")
        If Not rsEmp.EOF Then
            Set rs = New ADODB.Recordset

            sCamesproc = Mid(MaskEdBox1, 7, 4) & Mid(MaskEdBox1, 4, 2)
            csql = "SELECT * FROM ComprobanteCab WHERE EMP_CODIGO='" & VGEMP_CODIGO & "' AND ANEX_cODIGO='" & text1(1) & "' "
            csql = csql & " AND TIPODOCU_CODIGO='" & text1(4) & "' AND CSERIE='" & text1(5) & "' AND CNUMERO='" & text1(6) & "' AND CNRORUC='" & text1(2) & "'"
            csql = csql & " AND CAMESPROC='" & sCamesproc & "' "


            rs.Open csql, CN_CTAPAG, adOpenKeyset, adLockOptimistic
            If rs.RecordCount > 0 Then
                csql = "UPDATE  COMPROBANTECAB SET CESTADO=3 WHERE EMP_CODIGO='" & VGEMP_CODIGO & "' AND ANEX_cODIGO='" & text1(1) & "' "
                csql = csql & " AND TIPODOCU_CODIGO='" & text1(4) & "' AND CSERIE='" & text1(5) & "' AND CNUMERO='" & text1(6) & "' AND CNRORUC='" & text1(2) & "'"
                csql = csql & " AND CAMESPROC='" & sCamesproc & "' "
                CN_CTAPAG.Execute csql
            End If
        End If*/
            ComprobanteDet(comprobante, conta);

        }

        private void ComprobanteDet(Comprobante comprobante, string conta)
        {
            List<ContableDet> detalles = findContableDet();
            string csql = "";


            detalles.ForEach(element =>
            {
                // validacion de codigo de serie

                csql = "Insert Into ComprobanteDet (EMP_CODIGO,CORDEN,CNROITEM,TIPODOCU_CODIGO,";
                csql += "CCODPROVE,CSERIE,CNUMERO,CTIPPROV,NVALOR,CCONCEPT,Subdiario_Codigo,";
                csql += "Cencost_Codigo,CCODCONTA,CCTADEST,CTIPO,CGLOSA,CAMESPROC,COMPCON,Anex_Codigo,CANEXO,ORDENFAB, codmaquina, cantidad) Values(";
                csql += "'" + fValNull(comprobante.EMP_CODIGO) + "',";
                csql += "'" + fValNull(comprobante.CORDEN) + "',";
                csql += "'" + fValNull(element.CNROITEM) + "',";
                csql += "'" + fValNull(element.CTIPDOC) + "',";
                csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";
                csql += "'" + fValNull(element.CSERDOC) + "',";
                csql += "'" + fValNull(element.CNUMDOC) + "',";
                csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
                csql += "" + numericFormat(element.NVALOR) + ",";
                csql += "'" + fValNull(comprobante.CCONCEPT) + "',";

                csql += "'" + fValNull(element.CCODSUBDI) + "',";
                csql += "'" + fValNull(element.CCOSTO) + "',";
                csql += "'" + fValNull(element.CCODCONTA) + "',";
                csql += "'" + fValNull(element.CCTADEST) + "',";
                csql += "'" + fValNull(element.CTIPO) + "',";
                csql += "'" + fValNull(element.CGLOSA) + "',";
                csql += "'" + fValNull(comprobante.CAMESPROC) + "',";
                csql += "'" + fValNull(conta) + "',";
                csql += "'" + fValNull(element.CCODANEXO) + "',";
                csql += "'" + fValNull(element.CANEXO) + "',";
                csql += "'" + fValNull(element.ORDFAB) + "','";
                csql += fValNull(element.codmaquina) + "',";
                csql += numericFormat(element.cantidad) + ")";

            });
            try
            {
                comando = new SqlCommand(conexionComun(csql), objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }

        }


        private void updateComCompCon2(Comprobante comprobante, string conta)
        {

            string csql = $"Update COMPROBANTECAB set ";
            csql += " COMPCON='" + conta + "' ";
            csql += " where  camesproc='" + comprobante.CAMESPROC + "' and corden='" + comprobante.CORDEN + "'";
            try
            {
                comando = new SqlCommand(conexionComun(csql), objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        private void updateComEstado(Comprobante comprobante)
        {

            string csql = $"Update COMPROBANTECAB set ";
            csql += " CESTADO='" + 3 + "' ";
            csql += " where  camesproc='" + comprobante.CAMESPROC + "' and corden='" + comprobante.CORDEN + "' AND CESTADO<>'4' AND CESTADO<>'2'";
            try
            {
                comando = new SqlCommand(conexionComun(csql), objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        private void guardarCompra(Comprobante comprobante, string xccodsubdi, int anio, string mesFormat, string numero, bool wco_l_resta, string glosa, string xco_c_numer)
        {
            string CDOCUM;
            string CDOCUM1;
            string Cadena;
            int lcadena;
            string CSERIE;
            if (comprobante.CCODRUC.Trim() != "")
            {
                CDOCUM = comprobante.CSERREFER + comprobante.CNUMREFER;
            }
            else
            {
                Cadena = comprobante.CSERIE.Trim();
                lcadena = Cadena.Length;
                CSERIE = rellenar(Cadena, 4, lcadena, " ", true);
                CDOCUM = CSERIE + comprobante.CNUMERO;
                CDOCUM1 = comprobante.CSERREFER + comprobante.CNUMREFER;
            }
            string csql = "INSERT INTO COMPRAS (CO_C_CUENT, CO_C_MES, CO_C_SUBDI, CO_C_COMPR, CO_D_FECHA, CO_C_PROVE, CO_C_TPDOC, CO_C_DOCUM, CO_L_REFER, ";
            if (comprobante.CCODRUC.Trim() != "")
            {
                csql += "CO_C_TPDRF, CO_C_DCREF, CO_D_FECDC, CO_C_MONED, CO_N_MONTO, CO_N_IGV, CO_N_MTOUS, CO_N_IGVUS, ";
            }
            else
            {
                csql += "CO_C_TPDRF, CO_C_DCREF, CO_D_FECRF, CO_D_FECDC, CO_C_MONED, CO_N_MONTO, CO_N_IGV, CO_N_MTOUS, CO_N_IGVUS, ";
            }

            csql += "CO_N_TASA, CO_C_CONVE, CO_N_TIPCA, CO_N_CAMES, CO_D_FECCA, CO_A_GLOSA, CO_A_MOVIM, CO_C_RUC, ";
            csql += "CO_A_RAZON, CO_C_DESTI, CO_N_PORCE, CO_L_APLIC, CO_N_VALCI, NPERCEPCION";

            if (existeColumna(anio, "COMPRAS", "CO_D_FECHAVTO"))
            {
                csql += ",CO_D_FECHAVTO";
            }
            if (existeColumna(anio, "COMPRAS", "CO_L_RETE"))
            {
                csql += ",CO_L_RETE";
            }

            csql += ",CO_NUM_RETRAC, CO_FEC_RETRAC,CO_L_DETRACCION,CO_N_TASADETRACC,CO_N_IMPORTEREF,RCO_TIPO,RCO_SERIE,RCO_NUMERO,";
            csql += "RCO_FECHA,";
            csql += "flg_RNTNODOMICILIADO,CO_L_ANULA) VALUES(";
            csql += "'" + fValNull(comprobante.CCODCONTA) + "', ";
            csql += "'" + fValNull(mesFormat) + "', ";
            csql += "'" + fValNull(xccodsubdi) + "', ";
            csql += "'" + numero + "', ";
            csql += "" + dateFormat(comprobante.DRECEPCIO) + ", ";

            if (comprobante.CCODRUC.Trim() != "")
            {
                csql += "'" + fValNull(comprobante.CCODRUC) + "', ";   //' TO FIELD CO_C_PROVE,
                csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',"; //'  TO FIELD  CO_C_TPDOC,

                if (fValNull(CDOCUM) != " ")
                    csql += "'" + fValNull(CDOCUM) + "', "; // TO FIELD  CO_C_DOCUM,
                else
                    csql += "' ', ";// TO FIELD CO_C_DOCUM,
                csql += "1, "; // TO FIELD CO_L_REFER,
                csql += "'" + fValNull(comprobante.CTDREFER) + "', ";
                csql += "'" + fValNull(comprobante.CSERREFER + comprobante.CNUMREFER) + "', ";
                csql += "" + dateFormat(comprobante.DEMISION) + ", ";
            }
            else
            {
                csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "', ";
                csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "', ";
                csql += "'" + fValNull(CDOCUM) + "', ";
                csql += "0,";
                csql += "' ', ";
                csql += "' ', ";
                csql += "" + dateFormat(comprobante.DFECREF) + ", " + dateFormat(comprobante.DEMISION) + ", ";
            }
            csql += "'" + fValNull(comprobante.TIPOMON_CODIGO) + "', ";
            if (wco_l_resta)
            {
                if (comprobante.TIPOMON_CODIGO == "MN")
                {
                    switch (comprobante.CDESTCOMP)
                    {
                        case "006": // Honorarios con 4ta Categ. e IES:
                            {
                                csql += "" + numericFormat(comprobante.NTOTRH - comprobante.NIR4 - comprobante.NIES * -1) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * -1) + ", ";
                                if (comprobante.TIPOCAMBIO_VALOR != 0)
                                {
                                    csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIR4 - comprobante.NIES) / (comprobante.TIPOCAMBIO_VALOR) * -1) + ", ";
                                    csql += "" + numericFormat((comprobante.NIGV / comprobante.TIPOCAMBIO_VALOR) * -1) + ", ";
                                }
                                else
                                {
                                    csql += "" + numericFormat(0) + ", ";
                                    csql += "" + numericFormat(0) + ", ";
                                }

                                break;
                            }

                        case "007": // Honorario afec. sólo por 4ta categ.

                            {
                                csql += "" + numericFormat(comprobante.NTOTRH - comprobante.NIR4 * -1) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * -1) + ", ";
                                if (comprobante.TIPOCAMBIO_VALOR != 0)
                                {
                                    csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIR4) / (comprobante.TIPOCAMBIO_VALOR) * -1) + ", ";
                                    csql += "" + numericFormat((comprobante.NIGV / comprobante.TIPOCAMBIO_VALOR) * -1) + ", ";
                                }
                                else
                                {
                                    csql += "" + numericFormat(0) + ", ";
                                    csql += "" + numericFormat(0) + ", ";
                                }

                                break;
                            }

                        case "008": // Honorario afec. sólo por IES

                            {
                                csql += "" + numericFormat(comprobante.NTOTRH - comprobante.NIES * -1) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * -1) + ", ";
                                if (comprobante.TIPOCAMBIO_VALOR != 0)
                                {
                                    csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIES) / (comprobante.TIPOCAMBIO_VALOR) * -1) + ", ";
                                    csql += "" + numericFormat((comprobante.NIGV / comprobante.TIPOCAMBIO_VALOR) * -1) + ", ";
                                }
                                else
                                {
                                    csql += "" + numericFormat(0) + ", ";
                                    csql += "" + numericFormat(0) + ", ";
                                }

                                break;
                            }

                        default:
                            {
                                csql = csql + "" + numericFormat(comprobante.NIMPORTE * -1) + ", ";
                                csql = csql + "" + numericFormat(comprobante.NIGV * -1) + ", ";
                                if (comprobante.TIPOCAMBIO_VALOR != 0)
                                {
                                    csql = csql + "" + numericFormat((comprobante.NIMPORTE / comprobante.TIPOCAMBIO_VALOR) * -1) + ", ";
                                    csql = csql + "" + numericFormat((comprobante.NIGV / comprobante.TIPOCAMBIO_VALOR) * -1) + ", ";
                                }
                                else
                                {
                                    csql += "" + numericFormat(0) + ", ";
                                    csql += "" + numericFormat(0) + ", ";
                                }

                                break;
                            }
                    }
                }
                else
                {
                    switch (comprobante.CDESTCOMP)
                    {
                        case "006":// Honorarios con 4ta Categ. e IES

                            {
                                csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIR4 - comprobante.NIES) * comprobante.TIPOCAMBIO_VALOR * -1) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * comprobante.TIPOCAMBIO_VALOR * -1) + ", ";
                                csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIR4 - comprobante.NIES) * -1) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * -1) + ", ";
                                break;
                            }

                        case "007" // Honorario afec. sólo por 4ta categ.
                 :
                            {
                                csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIR4) * comprobante.TIPOCAMBIO_VALOR * -1) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * comprobante.TIPOCAMBIO_VALOR * -1) + ", ";
                                csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIR4) * -1) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * -1) + ", ";


                                break;
                            }

                        case "008": // Honorario afec. sólo por IES
                            {
                                csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIES) * comprobante.TIPOCAMBIO_VALOR * -1) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * comprobante.TIPOCAMBIO_VALOR * -1) + ", ";
                                csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIES) * -1) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * -1) + ", ";


                                break;
                            }

                        default:
                            {
                                csql += "" + numericFormat((comprobante.NIMPORTE) * comprobante.TIPOCAMBIO_VALOR * -1) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * comprobante.TIPOCAMBIO_VALOR * -1) + ", ";
                                csql += "" + numericFormat((comprobante.NIMPORTE) * -1) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * -1) + ", ";

                                break;
                            }
                    }

                }
            }
            else
            {
                // Modif Yimmy Suyco 31/01/2002 - El Valor CIF no debe sumarse al total segun contabilidad solo se guarda en un campo aparte

                if (comprobante.TIPOMON_CODIGO == "MN")
                {
                    switch (comprobante.CDESTCOMP)
                    {
                        case "006": // Honorarios con 4ta Categ. e IES:
                            {
                                csql += "" + numericFormat(comprobante.NTOTRH - comprobante.NIR4 - comprobante.NIES) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV) + ", ";
                                if (comprobante.TIPOCAMBIO_VALOR != 0)
                                {
                                    csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIR4 - comprobante.NIES) / (comprobante.TIPOCAMBIO_VALOR)) + ", ";
                                    csql += "" + numericFormat((comprobante.NIGV / comprobante.TIPOCAMBIO_VALOR)) + ", ";
                                }
                                else
                                {
                                    csql += "" + numericFormat(0) + ", ";
                                    csql += "" + numericFormat(0) + ", ";
                                }

                                break;
                            }

                        case "007": // Honorario afec. sólo por 4ta categ.

                            {
                                csql += "" + numericFormat(comprobante.NTOTRH - comprobante.NIR4) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV) + ", ";
                                if (comprobante.TIPOCAMBIO_VALOR != 0)
                                {
                                    csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIR4) / (comprobante.TIPOCAMBIO_VALOR)) + ", ";
                                    csql += "" + numericFormat((comprobante.NIGV / comprobante.TIPOCAMBIO_VALOR)) + ", ";
                                }
                                else
                                {
                                    csql += "" + numericFormat(0) + ", ";
                                    csql += "" + numericFormat(0) + ", ";
                                }

                                break;
                            }

                        case "008": // Honorario afec. sólo por IES

                            {
                                csql += "" + numericFormat(comprobante.NTOTRH - comprobante.NIES) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV) + ", ";
                                if (comprobante.TIPOCAMBIO_VALOR != 0)
                                {
                                    csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIES) / (comprobante.TIPOCAMBIO_VALOR)) + ", ";
                                    csql += "" + numericFormat((comprobante.NIGV / comprobante.TIPOCAMBIO_VALOR)) + ", ";
                                }
                                else
                                {
                                    csql += "" + numericFormat(0) + ", ";
                                    csql += "" + numericFormat(0) + ", ";
                                }

                                break;
                            }

                        default:
                            {
                                csql = csql + "" + numericFormat(comprobante.NIMPORTE) + ", ";
                                csql = csql + "" + numericFormat(comprobante.NIGV) + ", ";
                                if (comprobante.TIPOCAMBIO_VALOR != 0)
                                {
                                    csql = csql + "" + numericFormat((comprobante.NIMPORTE / comprobante.TIPOCAMBIO_VALOR)) + ", ";
                                    csql = csql + "" + numericFormat((comprobante.NIGV / comprobante.TIPOCAMBIO_VALOR)) + ", ";
                                }
                                else
                                {
                                    csql += "" + numericFormat(0) + ", ";
                                    csql += "" + numericFormat(0) + ", ";
                                }

                                break;
                            }
                    }
                }
                else
                {
                    switch (comprobante.CDESTCOMP)
                    {
                        case "006":// Honorarios con 4ta Categ. e IES

                            {
                                csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIR4 - comprobante.NIES) * comprobante.TIPOCAMBIO_VALOR) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * comprobante.TIPOCAMBIO_VALOR) + ", ";
                                csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIR4 - comprobante.NIES)) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV) + ", ";
                                break;
                            }

                        case "007": // Honorario afec. sólo por 4ta categ.

                            {
                                csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIR4) * comprobante.TIPOCAMBIO_VALOR) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * comprobante.TIPOCAMBIO_VALOR) + ", ";
                                csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIR4)) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * -1) + ", ";


                                break;
                            }

                        case "008": // Honorario afec. sólo por IES
                            {
                                csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIES) * comprobante.TIPOCAMBIO_VALOR) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * comprobante.TIPOCAMBIO_VALOR) + ", ";
                                csql += "" + numericFormat((comprobante.NTOTRH - comprobante.NIES)) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * -1) + ", ";


                                break;
                            }

                        default:
                            {
                                csql += "" + numericFormat((comprobante.NIMPORTE) * comprobante.TIPOCAMBIO_VALOR) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV * comprobante.TIPOCAMBIO_VALOR) + ", ";
                                csql += "" + numericFormat((comprobante.NIMPORTE)) + ", ";
                                csql += "" + numericFormat(comprobante.NIGV) + ", ";

                                break;
                            }
                    }

                }
            }


            csql += "" + numericFormat(comprobante.NTASAIGV) + ", ";
            csql += "'" + fValNull(comprobante.CONVERSION_CODIGO) + "', ";

            if (comprobante.TIPOMON_CODIGO == "MN")
                csql += "" + numericFormat(1 / comprobante.TIPOCAMBIO_VALOR) + ", ";
            else
                csql += "" + numericFormat(comprobante.TIPOCAMBIO_VALOR) + ", ";
            if (comprobante.CONVERSION_CODIGO == "ESP")
                csql += "" + numericFormat(comprobante.TIPOCAMBIO_VALOR) + ", ";
            else
                csql += "" + 0 + ", ";
            csql += ternario(comprobante.CONVERSION_CODIGO == "FEC", "" + dateFormat(comprobante.DCONTAB) + ", ", "NULL, ");
            csql += "'" + fValNull(glosa) + "', ";
            csql += "'" + fValNull(glosa) + "', ";


            if (comprobante.ANEX_CODIGO == "99999999")
            {
                csql += "'" + fValNull(comprobante.CNRORUC) + "', ";
                csql += "'" + fValNull(comprobante.ANEX_DESCRIPCION) + "', ";
            }
            else
            {
                csql += "'" + fValNull("") + "', ";
                csql += "'" + fValNull("") + "', ";
            }
            csql += "'" + fValNull(comprobante.CDESTCOMP) + "', ";
            if (comprobante.NPORCE > 0)
                csql += "" + numericFormat(comprobante.NPORCE) + ", ";
            else
                csql += "" + numericFormat(0) + ", ";
            if (comprobante.CIGVAPLIC)
                csql += "1, ";
            else
                csql += "0, ";
            csql += "" + numericFormat(comprobante.NVALCIF) + "," + numericFormat(comprobante.NPERCEPCION);
            if (existeColumna(anio, "CO_D_FECHAVTO", "COMPRAS"))
                csql += "," + dateFormat(comprobante.DVENCE) + "";
            if (existeColumna(anio, "CO_L_RETE", "COMPRAS"))
                csql += "," + comprobante.CO_L_RETE + "";
            csql += ",'" + fValNull(comprobante.NUMRETRAC) + "'," + dateFormat(comprobante.FECRETRAC);
            csql += "," + ternario(comprobante.LDETRACCION, "1", "0") + "," + numericFormat(comprobante.NTASADETRACCION) + ",";
            csql += "" + numericFormat(comprobante.NIMPORTEREF) + ",";
            csql += "'" + fValNull(comprobante.RCO_TIPO) + "', ";
            csql += "'" + fValNull(comprobante.RCO_SERIE) + "', ";
            csql += "'" + fValNull(comprobante.RCO_NUMERO) + "', ";
            csql += "" + dateFormat(comprobante.RCO_FECHA) + ", ";
            csql += "" + comprobante.flg_RNTNODOMICILIADO + ",0) ";
            try
            {
                comando = new SqlCommand(conexionBDCONT(csql, anio), objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }

        }



        public void deleteBDMovCab(Comprobante comprobante, string conexion)
        {

            string data1 = comprobante.COMPCON.Substring(0, 2);
            string data2 = comprobante.COMPCON.Substring(2, 4);
            string delete = $"delete from {conexion} where SUBDIAR_CODIGO='{data1}' and CMOV_C_COMPR='{data2}'";
            try
            {
                comando = new SqlCommand(delete, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        public void deleteBDMovDet(Comprobante comprobante, string conexion)
        {

            string data1 = comprobante.COMPCON.Substring(0, 2);
            string data2 = comprobante.COMPCON.Substring(2, 4);
            string delete = $"delete from {conexion} where SUBDIAR_CODIGO='{data1}' and DMOV_C_COMPR='{data2}'";
            try
            {
                comando = new SqlCommand(delete, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        public void deleteCompras(Comprobante comprobante, string conexion)
        {

            string data1 = comprobante.COMPCON.Substring(0, 2);
            string data2 = comprobante.COMPCON.Substring(2, 4);
            string delete = $"delete from {conexion} where CO_C_SUBDI='{data1}' and CO_C_COMPR='{data2}'";
            try
            {
                comando = new SqlCommand(delete, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        public void INSERTBDMovCab(Comprobante comprobante, int anio, string mes, Cabecera cabecera, string numero)
        {

            if (cabecera != null)
            {
                string csql = $"Insert Into {mes} (SUBDIAR_CODIGO,CMOV_FECHA,CMOV_FECCA,CMOV_C_COMPR,CMOV_MONED,CMOV_CONVE,CMOV_CAMES,";
                csql += "CMOV_TIPCA,CMOV_GLOSA,CMOV_DEBE,CMOV_HABER,CMOV_DEBUS,CMOV_HABUS,CMOV_L_COMPR,FECH_VCTO) Values(";
                csql += "'" + fValNull(cabecera.CO_C_SUBDI) + "',";
                csql += "" + dateFormat(cabecera.CO_D_FECHA) + ",";
                csql += "" + dateFormat(cabecera.CO_D_FECCA) + ",";
                csql += "'" + numero + "',";
                csql += "'" + fValNull(cabecera.CO_C_MONED) + "',";
                csql += "'" + fValNull(cabecera.CO_C_CONVE) + "',";
                csql += "" + fValNull(cabecera.CO_N_CAMES) + ",";
                csql += "" + fValNull(cabecera.CO_N_TIPCA) + ",";
                csql += "'" + fValNull(cabecera.CO_A_GLOSA) + "',";

                csql += "" + numericFormat(cabecera.CO_N_DEBE) + ",";
                csql += "" + numericFormat(cabecera.co_n_haber) + ",";
                csql += "" + numericFormat(cabecera.co_n_debus) + ",";
                csql += "" + numericFormat(cabecera.co_n_habus) + ",";
                if (comprobante.ESTCOMPRA)
                {
                    csql += "1," + dateFormat(cabecera.FECH_VEN) + ")";
                }
                else
                {
                    csql += "0," + dateFormat(cabecera.FECH_VEN) + ")";
                }
                try
                {
                    comando = new SqlCommand(conexionBDCONT(csql, anio), objConexion.getCon());
                    objConexion.getCon().Open();
                    comando.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    objConexion.getCon().Close();
                    objConexion.cerrarConexion();
                }
            }
        }
        public void INSERTBDMovDet(Comprobante comprobante, string conexion, int anio, int mes, string codSub, string MNUMERO)
        {
            string mess = mes.ToString("00.##");
            string tabla = "DETMOV" + mess;
            // traer la cabecera
            List<Detalle> detalles = findDetalle();
            Cabecera cabecera = findCabecera();
            string item = "";
            if (detalles != null)
            {
                detalles.ForEach(element =>
                {
                    // validacion de codigo de serie
                    string ANEXO = verdata($"PLANCTA_CODIGO=' {element.CO_C_CUENT} '", "plan_cuenta_nacional", 1, "TIPOANEX_CODIGO");

                    item += $"Insert Into {tabla} (SUBDIAR_CODIGO,DMOV_C_COMPR,DMOV_SECUE,DMOV_FECDC,DMOV_FECHA,DMOV_FECVEN,";
                    item += "DMOV_GLOSA,DMOV_DEBE,DMOV_DEBUS,DMOV_HABER,DMOV_HABUS,DMOV_CUENT,DMOV_DOCUM,DMOV_CENCO,DMOV_ANEXO,DMOV_C_DESTI,DMOV_L_COMPR,DMOV_C_ORDEN) Values(";
                    item += "'" + fValNull(element.CO_C_SUBDI) + "',";
                    item += "'" + MNUMERO + "',";
                    item += "'" + fValNull(element.CO_C_SECUE) + "',";
                    item += " " + dateFormat(element.CO_D_FECDC) + " ,";
                    item += "" + dateFormat(element.CO_D_FECHA) + "," + dateFormat(element.FECHAVENC) + ",";
                    item += "'" + fValNull(element.CO_A_GLOSA) + "',";
                    item += "" + numericFormat(element.CO_N_DEBE) + ",";
                    item += "" + numericFormat(element.co_n_debus) + ",";
                    item += "" + numericFormat(element.co_n_haber) + ",";
                    item += "" + numericFormat(element.co_n_habus) + ",";
                    item += "'" + fValNull(element.CO_C_CUENT) + "',";
                    item += "'" + fValNull(element.CO_C_DOCUM) + "',";
                    item += "'" + fValNull(element.CO_C_CENCO) + "',";
                    if (ANEXO.Trim() != "")
                    {
                        item += "'" + fValNull(element.CO_C_ANEXO) + "',";
                    }
                    else
                    {
                        item += "' ',";
                    }
                    item += "'" + fValNull(element.CO_C_DESTI) + "',";
                    if (comprobante.ESTCOMPRA)
                    {
                        item += "1,'" + fValNull(element.ORDFAB) + "')";
                    }
                    else
                    {
                        item += "0,'" + fValNull(element.ORDFAB) + "')";
                    }


                });
                try
                {
                    comando = new SqlCommand(conexionBDCONT(item, anio), objConexion.getCon());
                    objConexion.getCon().Open();
                    comando.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    objConexion.getCon().Close();
                    objConexion.cerrarConexion();
                }
            }
        }

        private Cabecera findCabecera()
        {


            Cabecera plan = null;
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "CABECERA");
            string csql = $"SELECT * from {conexion}  ";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    plan = new Cabecera();
                    plan.CO_C_SUBDI = read[0].ToString();
                    plan.CO_C_COMPR = read[1].ToString();
                    plan.CO_D_FECHA = Conversion.ParseDateTime(read[2].ToString());
                    plan.CO_A_GLOSA = read[3].ToString();
                    plan.CO_C_MONED = read[4].ToString();
                    plan.CO_C_CONVE = read[5].ToString();
                    plan.CO_D_FECCA = Conversion.ParseDateTime(read[6].ToString());
                    plan.CO_N_DEBE = Conversion.ParseDecimal(read[7].ToString());
                    plan.co_n_haber = Conversion.ParseDecimal(read[8].ToString());
                    plan.co_n_debus = Conversion.ParseDecimal(read[9].ToString());
                    plan.co_n_habus = Conversion.ParseDecimal(read[10].ToString());
                    plan.CO_N_TIPCA = read[11].ToString();
                    plan.CO_N_CAMES = read[12].ToString();
                    plan.CO_L_COMPR = Conversion.ParseBool(read[13].ToString());
                    plan.FECH_VEN = Conversion.ParseDateTime(read[14].ToString());


                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return plan;



        }


        private List<Detalle> findDetalle()
        {

            List<Detalle> detalles = new List<Detalle>();
            string csql = $"SELECT * from detalle  ";
            try
            {
                comando = new SqlCommand(conexionWenco(csql), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Detalle plan = new Detalle();
                    plan.CO_C_SUBDI = read[0].ToString();
                    plan.CO_C_COMPR = read[1].ToString();
                    plan.CO_D_FECHA = Conversion.ParseDateTime(read[2].ToString());
                    plan.CO_C_CUENT = read[3].ToString();
                    plan.CO_D_FECDC = Conversion.ParseDateTime(read[4].ToString());
                    plan.CO_C_CENCO = read[5].ToString();
                    plan.CO_A_GLOSA = read[6].ToString();
                    plan.CO_C_DESTI = read[7].ToString();
                    plan.CO_C_DOCUM = read[8].ToString();
                    plan.CO_C_ANEXO = read[9].ToString();
                    plan.CO_N_DEBE = Conversion.ParseDecimal(read[10].ToString());
                    plan.co_n_debus = Conversion.ParseDecimal(read[11].ToString());
                    plan.co_n_haber = Conversion.ParseDecimal(read[12].ToString());
                    plan.co_n_habus = Conversion.ParseDecimal(read[13].ToString());
                    plan.NUMRETRAC = read[14].ToString();
                    plan.FECRETRAC = Conversion.ParseDateTime(read[15].ToString());
                    plan.FECHAVENC = Conversion.ParseDateTime(read[16].ToString());
                    plan.ORDFAB = read[17].ToString();
                    plan.CO_C_SECUE = read[18].ToString();
                    detalles.Add(plan);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return detalles;



        }
        // public 
        public void deleteCabeceraTrans()
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "CABECERA");
            string delete = $"delete from {conexion}";
            try
            {
                comando = new SqlCommand(delete, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        public void deleteDetalleTrans()
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "DETALLE");
            string delete = $"delete from {conexion}";
            try
            {
                comando = new SqlCommand(delete, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }



        private EjercicioContable findEContable(int anio)
        {

            EjercicioContable plan = null;
            string conexion = Conexion.CadenaGeneral("014", "BDCONTABILIDAD", "EJERCICIO_CONTABLE");
            string csql = $"SELECT * from {conexion}  WHERE  EJECONT_ANO = {anio } ";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    plan = new EjercicioContable();
                    plan.EJERCICIO_CONTABLE = read[0].ToString();
                    plan.EJECONT_ANO = read[1].ToString();
                    plan.EJECONT_CIERRE = read[2].ToString();
                    plan.EJECONT_INICIO = Conversion.ParseDateTime(read[3].ToString());
                    plan.EJECONT_FINAL = Conversion.ParseDateTime(read[4].ToString());

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return plan;


        }

        private string mNumero(int anio, int mes, string codSub)
        {

            string MNUMERO = "0001";
            int numer = 0;
            string mess = mes.ToString("00.##");
            string conexion = Conexion.CadenaGeneral("014", "BDCONT" + anio, "CABMOV" + mess);

            string csql = $"Select MAX(CMOV_C_COMPR) AS MAXCOMPR from {conexion} where SUBDIAR_CODIGO='{codSub}'";

            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {

                    numer = int.Parse(read[0].ToString()) + 1;
                    MNUMERO = numer.ToString("0000.##");
                }

            }
            catch (Exception)
            {

                throw;

            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return MNUMERO;
        }
        private string mNumeroBDMovCab(int anio, int mes, string codSub)
        {

            int numero = 0;
            string MNUMERO = "0001";
            string mess = mes.ToString("00.##");
            string conexion = Conexion.CadenaGeneral("014", "BDCONT" + anio, "CABMOV" + mess);

            string csql = $"Select MAX(CMOV_C_COMPR) AS MAXCOMPR from {conexion} where SUBDIAR_CODIGO='{codSub}'";

            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {

                    numero = int.Parse(read[0].ToString()) + 1;
                    MNUMERO = numero.ToString("0000.##");
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return MNUMERO;
        }

        private CabMov findCabMov(int anio, int mes, string codsu, string codigo)
        {

            CabMov plan = null;
            string mess = mes.ToString("00.##");
            string conexion = Conexion.CadenaGeneral("014", "BDCONT" + anio, "CABMOV" + mess);
            string csql = $"SELECT * from {conexion}  WHERE  SUBDIAR_CODIGO = '{codsu}'  and CMOV_C_COMPR='{codigo}'";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    plan = new CabMov();
                    plan.SUBDIAR_CODIGO = read[0].ToString();
                    plan.CMOV_C_COMPR = read[1].ToString();
                    plan.CMOV_FECHA = Conversion.ParseDateTime(read[2].ToString());
                    plan.CMOV_GLOSA = read[3].ToString();
                    plan.CMOV_MONED = read[4].ToString();
                    plan.CMOV_CONVE = read[5].ToString();
                    plan.CMOV_CAMES = read[6].ToString();
                    plan.CMOV_FECCA = read[7].ToString();
                    plan.CMOV_TIPCA = read[8].ToString();
                    plan.CMOV_DEBE = read[9].ToString();
                    plan.CMOV_HABER = read[10].ToString();
                    plan.CMOV_DEBUS = read[11].ToString();
                    plan.CMOV_HABUS = read[12].ToString();
                    plan.CMOV_AUTOM = Conversion.ParseBool(read[13].ToString());
                    plan.CMOV_COSTO = Conversion.ParseBool(read[14].ToString());
                    plan.CMOV_CHEQU = Conversion.ParseBool(read[15].ToString());
                    plan.CMOV_L_COMPR = Conversion.ParseBool(read[16].ToString());
                    plan.CMOV_VENTA = Conversion.ParseBool(read[17].ToString());
                    plan.FECH_VCTO = Conversion.ParseBool(read[18].ToString());
                    plan.CMOV_CAJAB = Conversion.ParseBool(read[19].ToString());
                    plan.CMOV_MEDIO = read[20].ToString();
                    plan.CMOV_DMEDIO = read[21].ToString();



                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return plan;


        }

        private TipoCambio findTcamabio(DateTime Demision)
        {

            TipoCambio plan = null;
            string conexion = Conexion.CadenaGeneral("014", "BDCONTABILIDAD", "TIPO_CAMBIO");
            string csql = $"SELECT * from {conexion}  WHERE  TIPOMON_CODIGO = 'ME' AND TIPOCAMB_FECHA ={dateFormat(Demision)} ";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    plan = new TipoCambio();
                    plan.TIPOMON_CODIGO = read[0].ToString();
                    plan.TIPOCAMB_FECHA = read[1].ToString();
                    plan.TIPOCAMB_COMPRA = Conversion.ParseDecimal(read[2].ToString());
                    plan.TIPOCAMB_EQCOMPRA = Conversion.ParseDecimal(read[3].ToString());
                    plan.TIPOCAMB_VENTA = Conversion.ParseDecimal(read[4].ToString());
                    plan.TIPOCAMB_EQVENTA = Conversion.ParseDecimal(read[5].ToString());


                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return plan;


        }
        public GastosIngresos findGastoIngreso(string codigo)
        {

            GastosIngresos plan = null;
            string csql = $"SELECT * from GASTOS_INGRESOS  WHERE  GASING_CODIGO = '{codigo }' ";
            try
            {
                comando = new SqlCommand(conexionContabilidad(csql), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    plan = new GastosIngresos();
                    plan.GASING_CODIGO = read[0].ToString();
                    plan.GASING_DESCRIPCION = read[1].ToString();
                    plan.GASING_TIPO = read[2].ToString();

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return plan;


        }

        public int boolToInt(bool val)
        {
            return val ? 1 : 0;
        }
        public string Estado()
        {
            string sEstado = "0";
            try
            {
                string CadenaD = "SELECT EDOC_OBLIGA FROM ESTADODOC WHERE EDOC_CLAVE = '1'";
                comando = new SqlCommand(conexionComun(CadenaD), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader readinterno = comando.ExecuteReader();
                if (readinterno.Read())
                {
                    bool docobliga = Conversion.ParseBool(readinterno[0].ToString());
                    if (docobliga)
                    {
                        sEstado = "0";
                    }
                    else
                    {
                        sEstado = "1";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return sEstado;
        }
        public string funcBlanco(String Valor)
        {
            if (Valor == null || Valor.Trim() == "")
            {
                return " ";
            }
            else
            {
                return Valor;
            }
        }

        public decimal IIfData(bool verificacion, decimal data1, decimal data2)
        {
            if (verificacion)
            {
                return data1;
            }
            else
            {
                return data2;
            }
        }
        public DateTime IIfDateEdit(bool verificacion, DateTime data1, DateTime data2)
        {
            if (verificacion)
            {
                return data1;
            }
            else
            {
                return data2;
            }
        }
        public string verificaDoc_cxc(string anex, string doc, string Serie, string Num)
        {


            string sql = "select ANEX_cODIGO,TIPODOCU_CODIGO,CSERIE,CNUMERO,NIMPORTE,NSALDO,LCANJEADO,NMONTPROG from [014BDCTAPAG].[dbo].[ComprobanteCab]";
            sql += " where ANEX_cODIGO='" + anex + "' and TIPODOCU_CODIGO='" + doc + "' and CSERIE='" + Serie + "' and CNUMERO='" + Num + "' ";

            try
            {
                comando = new SqlCommand(sql, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    sql = "SELECT * FROM [014BDCTAPAG].[dbo].[ProgramacionDet] WHERE CCODPROVE='" + anex + "' ";
                    sql += " AND CCODDOCUM='" + doc + "' AND CSERIE='" + Serie + "' AND CNUMERO='" + Num + "' ";
                    comando = new SqlCommand(sql, objConexion.getCon());
                    objConexion.getCon().Open();
                    SqlDataReader readinterno = comando.ExecuteReader();
                    if (readinterno.Read())
                    {
                        return "El Documento se Encuentra Programado en Cuentas x Pagar";
                    }
                    decimal saldo = Conversion.ParseDecimal(read[5].ToString());
                    decimal importe = Conversion.ParseDecimal(read[4].ToString());
                    bool canjeado = Conversion.ParseBool(read[6].ToString());
                    if (saldo == 0 || saldo < importe)
                    {
                        if (canjeado)
                        {
                            return "El Documento Esta Canjeado en Cuentas x Pagar";
                        }
                        else
                        {
                            return "El Documento Esta Canjeado en Cuentas x Pagar";

                        }


                    }

                    return "El Documento Esta en Cuentas x Pagar";
                }
                else
                {
                    return "";
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }

        }
        public void Trasferencia_CxP(string cCorre, string txtTpoDcto, string txtSerie, string txtNumero, string txNumDetraccion, bool detraacion, decimal TxtTasaDet, decimal TxTImporteRef, DateTime datedetraccion)
        {

        }

        public string funcAutoNum(string msAnoMesProc)
        {
            string cadena = "SELECT Concepto_Logico FROM CONCEPTOGRAL WHERE Concepto_Codigo='NUMEAUTO'";
            try
            {
                comando = new SqlCommand(conexionComun(cadena), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                bool hayRegistros = read.Read();
                if (hayRegistros)
                {

                    bool codigo = Conversion.ParseBool(read[0].ToString());
                    read.Close();
                    cadena = "SELECT MAX(CORDEN) AS MAXORDEN FROM COMPROBANTECAB WHERE CAMESPROC = '" + msAnoMesProc + "'";
                    comando = new SqlCommand(conexionComun(cadena), objConexion.getCon());
                    // objConexion.getCon().Open();
                    SqlDataReader readd = comando.ExecuteReader();
                    bool hayRegistrosdd = readd.Read();
                    int nextDocumet = 0;
                    if (hayRegistrosdd)
                    {
                        string last = readd[0].ToString();
                        nextDocumet = Conversion.Parseint(last) + 1;
                    }
                    else
                    {
                        nextDocumet = 1;
                    }

                    string next = nextDocumet.ToString("00000.##");
                    /* if (codigo)
                     {
                         return next;
                     }
                     else
                     {
                         return "";
                     }*/
                    return next;
                }

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }


            return "";
        }

        public void delete(Comprobante obj)
        {
            throw new NotImplementedException();
        }

        public bool find(Comprobante obj)
        {
            throw new NotImplementedException();
        }

        public List<Comprobante> findAll()
        {

            List<Comprobante> listComprobantes = new List<Comprobante>();
            DateTime date = DateTime.Now;

            string anios = date.Year.ToString("0000.##");
            // anios="2015";
            string mes = date.Month.ToString("00.##");
            string msAnoMesProc = anios + mes;
            //string findAll = $"SELECT * FROM {table} WHERE CAMESPROC='" + msAnoMesProc +"' AND CSALDINI=0";

            string conexion = Conexion.CadenaGeneral("014", "BDCOMUN", "COMPROBANTECAB");

            string findAll = $"SELECT * FROM  {conexion}  WHERE CAMESPROC='" + msAnoMesProc + "' AND CSALDINI=0";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Comprobante comp = new Comprobante();
                    comp.EMP_CODIGO = read[0].ToString();
                    comp.CORDEN = read[1].ToString();
                    comp.ANEX_CODIGO = read[2].ToString();
                    comp.ANEX_DESCRIPCION = read[3].ToString();
                    comp.TIPODOCU_CODIGO = read[4].ToString();
                    comp.CSERIE = read[5].ToString();
                    comp.CNUMERO = read[6].ToString();
                    comp.DEMISION = Conversion.ParseDateTime(read[7].ToString());
                    comp.DVENCE = Conversion.ParseDateTime(read[8].ToString());
                    comp.DRECEPCIO = Conversion.ParseDateTime(read[9].ToString());
                    comp.TIPOMON_CODIGO = read[10].ToString();
                    comp.NIMPORTE = Conversion.ParseDecimal(read[11].ToString());
                    comp.TIPOCAMBIO_VALOR = Conversion.ParseDecimal(read[12].ToString());
                    comp.CNUMORDCO = read[13].ToString();
                    comp.CDESCRIPC = read[14].ToString();
                    comp.RESPONSABLE_CODIGO = read[15].ToString();
                    comp.CESTADO = conversionCampo(Conversion.Parseint(read[16].ToString()), "CONTABILIZADO", "SIN CONTABILIZAR" +
                        "");
                    comp.NSALDO = Conversion.ParseDecimal(read[17].ToString());
                    comp.NMONTPROG = Conversion.ParseDecimal(read[18].ToString());
                    comp.LCANJEADO = Conversion.ParseBool(read[19].ToString());
                    comp.CCODCONTA = read[20].ToString();
                    comp.CFORMPAGO = read[21].ToString();
                    comp.CSERREFER = read[22].ToString();
                    comp.CNUMREFER = read[23].ToString();
                    comp.CTDREFER = read[24].ToString();
                    comp.SUBDIARIO_CODIGO = read[25].ToString();
                    comp.CONVERSION_CODIGO = read[26].ToString();
                    comp.CTIPPROV = read[27].ToString();
                    comp.CCTADEST = read[28].ToString();
                    comp.CCOSTO = read[29].ToString();
                    comp.CNRORUC = read[30].ToString();
                    comp.CONTAB = Conversion.ParseBool(read[31].ToString());
                    comp.CTIPO = read[32].ToString();
                    comp.ESTCOMPRA = Conversion.ParseBool(read[33].ToString());
                    comp.CDESTCOMP = read[34].ToString();
                    comp.COMPCON = read[35].ToString();
                    comp.CSALDINI = Conversion.ParseBool(read[36].ToString());
                    comp.CODCAJCH = read[37].ToString();
                    comp.DIASPAGO = Conversion.ParseDecimal(read[38].ToString());
                    comp.CIGVAPLIC = converionBool(read[39].ToString());
                    comp.CAMESPROC = read[40].ToString();
                    comp.CCONCEPT = read[41].ToString();
                    comp.DFECREF = Conversion.ParseDateTime(read[42].ToString());
                    comp.NIGV = Conversion.ParseDecimal(read[43].ToString());
                    comp.NTASAIGV = Conversion.ParseDecimal(read[44].ToString());
                    comp.NCAMBESP = Conversion.ParseDecimal(read[45].ToString());
                    comp.DFECCA = Conversion.ParseDateTime(read[46].ToString());
                    comp.LANULA = Conversion.ParseBool(read[47].ToString());
                    comp.NPORCE = Conversion.ParseDecimal(read[48].ToString());
                    comp.CCODRUC = read[49].ToString();
                    comp.CRAZON = read[50].ToString();
                    comp.LREGCO = Conversion.ParseBool(read[51].ToString());
                    comp.LHONOR = Conversion.ParseBool(read[52].ToString());
                    comp.NIR4 = Conversion.ParseDecimal(read[53].ToString());
                    comp.NIES = Conversion.ParseDecimal(read[54].ToString());
                    comp.NTOTRH = Conversion.ParseDecimal(read[55].ToString());
                    comp.NRENTA2DA = Conversion.ParseDecimal(read[56].ToString());
                    comp.NISC = Conversion.ParseDecimal(read[57].ToString());
                    comp.BANCO_CODIGO = read[58].ToString();
                    comp.CAGENCIA = read[59].ToString();
                    comp.Cnltbco = read[60].ToString();
                    comp.NVALCIF = Conversion.ParseDecimal(read[61].ToString());
                    comp.NBASEIMP = Conversion.ParseDecimal(read[62].ToString());
                    comp.NINAFECTO = Conversion.ParseDecimal(read[63].ToString());
                    comp.DCONTAB = Conversion.ParseDateTime(read[64].ToString());
                    comp.NTASAPERCEPCION = Conversion.ParseDecimal(read[65].ToString());
                    comp.NPERCEPCION = Conversion.ParseDecimal(read[66].ToString());
                    comp.NTASARETENCION = Conversion.ParseDecimal(read[67].ToString());
                    comp.NRETENCION = Conversion.ParseDecimal(read[68].ToString());
                    comp.CO_L_RETE = read[69].ToString();
                    comp.CODDETRACC = read[70].ToString();
                    comp.NUMRETRAC = read[71].ToString();
                    comp.FECRETRAC = Conversion.ParseDateTime(read[72].ToString());
                    comp.LPASOIMP = Conversion.ParseBool(read[73].ToString());
                    comp.LDETRACCION = Conversion.ParseBool(read[74].ToString());
                    comp.NTASADETRACCION = Conversion.ParseDecimal(read[75].ToString());
                    comp.DETRACCION = Conversion.ParseDecimal(read[76].ToString());
                    comp.COD_SERVDETRACC = read[77].ToString();
                    comp.COD_TIPOOPERACION = read[78].ToString();
                    comp.NIMPORTEREF = Conversion.ParseDecimal(read[79].ToString());
                    comp.RCO_TIPO = read[80].ToString();
                    comp.RCO_SERIE = read[81].ToString();
                    comp.RCO_NUMERO = read[82].ToString();
                    comp.RCO_FECHA = Conversion.ParseDateTime(read[83].ToString());
                    comp.DREGISTRO = Conversion.ParseDateTime(read[84].ToString());
                    comp.USERREG = read[85].ToString();
                    comp.flg_RNTNODOMICILIADO = Conversion.Parseint(read[86].ToString());
                    comp.CAOCOMPRA = read[87].ToString();

                    comp.codigo = comp.CORDEN + comp.CAMESPROC;
                    comp.documento = comp.TIPODOCU_CODIGO + ' ' + comp.CSERIE + " - " + comp.CNUMERO;
                    comp.MontoPagar = comp.NIMPORTE + comp.NPERCEPCION;
                    listComprobantes.Add(comp);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return listComprobantes;
        }
        private bool converionBool(string dale)
        {
            if (dale == "N")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private string converionBoolReci(bool dale)
        {
            if (dale)
            {
                return "S";
            }
            else
            {
                return "N";
            }
        }
        private int converionBoolint(bool dale)
        {
            if (dale)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public Comprobante findAllConta(string sCorrelativo, string TIPODOCU_CODIGO, string CSERIE, string CNUMERO)
        {
            DateTime date = DateTime.Now;
            Comprobante comp = new Comprobante();

            string anios = date.Year.ToString("0000.##");
            // anios = "2015";
            string mes = date.Month.ToString("00.##");
            string msAnoMesProc = anios + mes;
            //string findAll = $"SELECT * FROM {table} WHERE CAMESPROC='" + msAnoMesProc +"' AND CSALDINI=0";

            string conexion1 = Conexion.CadenaGeneral("014", "BDCOMUN", "COMPROBANTECAB");
            string conexion2 = Conexion.CadenaGeneral("014", "BDCOMUN", "EstadoDoc");

            string sql = $"Select A.*,EDoc_Descripcion from {conexion1}  A LEFT JOIN {conexion2} B ON ((A.CESTADO=B.EDoc_Clave  ) OR (A.CESTADO='') )  where   (CESTADO='0' OR CESTADO='1' OR CESTADO='3' OR CESTADO='4') and ";
            sql += "CAMESPROC='" + msAnoMesProc + "' AND A.CORDEN='" + sCorrelativo + "' AND TIPODOCU_CODIGO='" + TIPODOCU_CODIGO + "' AND CSERIE='" + CSERIE + "' AND   CNUMERO='" + CNUMERO + "'  order by COrden";
            try
            {
                comando = new SqlCommand(sql, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {

                    comp.EMP_CODIGO = read[0].ToString();
                    comp.CORDEN = read[1].ToString();
                    comp.ANEX_CODIGO = read[2].ToString();
                    comp.ANEX_DESCRIPCION = read[3].ToString();
                    comp.TIPODOCU_CODIGO = read[4].ToString();
                    comp.CSERIE = read[5].ToString();
                    comp.CNUMERO = read[6].ToString();
                    comp.DEMISION = Conversion.ParseDateTime(read[7].ToString());
                    comp.DVENCE = Conversion.ParseDateTime(read[8].ToString());
                    comp.DRECEPCIO = Conversion.ParseDateTime(read[9].ToString());
                    comp.TIPOMON_CODIGO = read[10].ToString();
                    comp.NIMPORTE = Conversion.ParseDecimal(read[11].ToString());
                    comp.TIPOCAMBIO_VALOR = Conversion.ParseDecimal(read[12].ToString());
                    comp.CNUMORDCO = read[13].ToString();
                    comp.CDESCRIPC = read[14].ToString();
                    comp.RESPONSABLE_CODIGO = read[15].ToString();
                    comp.CESTADO = conversionCampo(Conversion.Parseint(read[16].ToString()), "CONTABILIZADO", "SIN CONTABILIZAR" +
                        "");

                    comp.NSALDO = Conversion.ParseDecimal(read[17].ToString());
                    comp.NMONTPROG = Conversion.ParseDecimal(read[18].ToString());
                    comp.LCANJEADO = Conversion.ParseBool(read[19].ToString());
                    comp.CCODCONTA = read[20].ToString();
                    comp.CFORMPAGO = read[21].ToString();
                    comp.CSERREFER = read[22].ToString();
                    comp.CNUMREFER = read[23].ToString();
                    comp.CTDREFER = read[24].ToString();
                    comp.SUBDIARIO_CODIGO = read[25].ToString();
                    comp.CONVERSION_CODIGO = read[26].ToString();
                    comp.CTIPPROV = read[27].ToString();
                    comp.CCTADEST = read[28].ToString();
                    comp.CCOSTO = read[29].ToString();
                    comp.CNRORUC = read[30].ToString();
                    comp.CONTAB = Conversion.ParseBool(read[31].ToString());
                    comp.CTIPO = read[32].ToString();
                    comp.ESTCOMPRA = Conversion.ParseBool(read[33].ToString());
                    comp.CDESTCOMP = read[34].ToString();
                    comp.COMPCON = read[35].ToString();
                    comp.CSALDINI = Conversion.ParseBool(read[36].ToString());
                    comp.CODCAJCH = read[37].ToString();
                    comp.DIASPAGO = Conversion.ParseDecimal(read[38].ToString());
                    comp.CIGVAPLIC = converionBool(read[39].ToString());
                    comp.CAMESPROC = read[40].ToString();
                    comp.CCONCEPT = read[41].ToString();
                    comp.DFECREF = Conversion.ParseDateTime(read[42].ToString());
                    comp.NIGV = Conversion.ParseDecimal(read[43].ToString());
                    comp.NTASAIGV = Conversion.ParseDecimal(read[44].ToString());
                    comp.NCAMBESP = Conversion.ParseDecimal(read[45].ToString());
                    comp.DFECCA = Conversion.ParseDateTime(read[46].ToString());
                    comp.LANULA = Conversion.ParseBool(read[47].ToString());
                    comp.NPORCE = Conversion.ParseDecimal(read[48].ToString());
                    comp.CCODRUC = read[49].ToString();
                    comp.CRAZON = read[50].ToString();
                    comp.LREGCO = Conversion.ParseBool(read[51].ToString());
                    comp.LHONOR = Conversion.ParseBool(read[52].ToString());
                    comp.NIR4 = Conversion.ParseDecimal(read[53].ToString());
                    comp.NIES = Conversion.ParseDecimal(read[54].ToString());
                    comp.NTOTRH = Conversion.ParseDecimal(read[55].ToString());
                    comp.NRENTA2DA = Conversion.ParseDecimal(read[56].ToString());
                    comp.NISC = Conversion.ParseDecimal(read[57].ToString());
                    comp.BANCO_CODIGO = read[58].ToString();
                    comp.CAGENCIA = read[59].ToString();
                    comp.Cnltbco = read[60].ToString();
                    comp.NVALCIF = Conversion.ParseDecimal(read[61].ToString());
                    comp.NBASEIMP = Conversion.ParseDecimal(read[62].ToString());
                    comp.NINAFECTO = Conversion.ParseDecimal(read[63].ToString());
                    comp.DCONTAB = Conversion.ParseDateTime(read[64].ToString());
                    comp.NTASAPERCEPCION = Conversion.ParseDecimal(read[65].ToString());
                    comp.NPERCEPCION = Conversion.ParseDecimal(read[66].ToString());
                    comp.NTASARETENCION = Conversion.ParseDecimal(read[67].ToString());
                    comp.NRETENCION = Conversion.ParseDecimal(read[68].ToString());
                    comp.CO_L_RETE = read[69].ToString();
                    comp.CODDETRACC = read[70].ToString();
                    comp.NUMRETRAC = read[71].ToString();
                    comp.FECRETRAC = Conversion.ParseDateTime(read[72].ToString());
                    comp.LPASOIMP = Conversion.ParseBool(read[73].ToString());
                    comp.LDETRACCION = Conversion.ParseBool(read[74].ToString());
                    comp.NTASADETRACCION = Conversion.ParseDecimal(read[75].ToString());
                    comp.DETRACCION = Conversion.ParseDecimal(read[76].ToString());
                    comp.COD_SERVDETRACC = read[77].ToString();
                    comp.COD_TIPOOPERACION = read[78].ToString();
                    comp.NIMPORTEREF = Conversion.ParseDecimal(read[79].ToString());
                    comp.RCO_TIPO = read[80].ToString();
                    comp.RCO_SERIE = read[81].ToString();
                    comp.RCO_NUMERO = read[82].ToString();
                    comp.RCO_FECHA = Conversion.ParseDateTime(read[83].ToString());
                    comp.DREGISTRO = Conversion.ParseDateTime(read[84].ToString());
                    comp.USERREG = read[85].ToString();
                    comp.flg_RNTNODOMICILIADO = Conversion.Parseint(read[86].ToString());
                    comp.CAOCOMPRA = read[87].ToString();

                    comp.codigo = comp.CORDEN + comp.CAMESPROC;
                    comp.documento = comp.TIPODOCU_CODIGO + ' ' + comp.CSERIE + " - " + comp.CNUMERO;


                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return comp;

        }

        public List<Gasto> findAllGastos()
        {
            List<Gasto> listGastos = new List<Gasto>();

            string conexion = Conexion.CadenaGeneral("014", "BDCTAPAG", "Gastos");
            string findAll = $"SELECT Gastos_Codigo, Gastos_Descripcion from {conexion} ";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Gasto gasto = new Gasto();
                    gasto.Gastos_Codigo = read[0].ToString();
                    gasto.Gastos_Descripcion = read[1].ToString();
                    listGastos.Add(gasto);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return listGastos;
        }
        public List<PlanCuentaNacional> findAllCuentasNacionales(int NivelContable = 4)
        {
            List<PlanCuentaNacional> listGastos = new List<PlanCuentaNacional>();

            string conexion = Conexion.CadenaGeneral("014", "BDCONTABILIDAD", "PLAN_CUENTA_NACIONAL");
            string findAll = $"SELECT PLANCTA_CODIGO,PLANCTA_DESCRIPCION from {conexion}  WHERE  PLANCTA_NIVEL = " + NivelContable + "";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    PlanCuentaNacional plan = new PlanCuentaNacional();
                    plan.PLANCTA_CODIGO = read[0].ToString();
                    plan.PLANCTA_DESCRIPCION = read[1].ToString();
                    listGastos.Add(plan);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return listGastos;
        }

        public PlanCuentaNacional findCuentasNacionales(string xccodcuenta, int NivelContable = 4)
        {

            PlanCuentaNacional plan = null;
            string conexion = Conexion.CadenaGeneral("014", "BDCONTABILIDAD", "PLAN_CUENTA_NACIONAL");
            string findAll = $"SELECT * from {conexion}  WHERE  PLANCTA_NIVEL = {NivelContable } AND PLANCTA_CODIGO='{xccodcuenta}'";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    plan = new PlanCuentaNacional();
                    plan.PLANCTA_CODIGO = read[0].ToString();
                    plan.PLANCTA_DESCRIPCION = read[1].ToString();
                    plan.PLANCTA_NIVEL = read[2].ToString();
                    plan.TIPOANEX_CODIGO = read[3].ToString();
                    plan.PLANCTA_CENTCOST = Conversion.ParseBool(read[4].ToString());
                    plan.TIPOCTA_CODIGO = read[5].ToString();
                    plan.PLANCTA_AUTO = read[6].ToString();
                    plan.PLANCTA_CARGO1 = read[7].ToString();
                    plan.PLANCTA_CARGO2 = read[8].ToString();
                    plan.PLANCTA_CARGO3 = read[9].ToString();
                    plan.PLANCTA_ABONO1 = read[10].ToString();
                    plan.PLANCTA_ABONO2 = read[11].ToString();
                    plan.PLANCTA_ABONO3 = read[12].ToString();
                    plan.PLANCTA_PORCENT1 = read[13].ToString();
                    plan.PLANCTA_PORCENT2 = read[14].ToString();
                    plan.PLANCTA_PORCENT3 = read[15].ToString();
                    plan.PLANCTA_AJUSTE = read[16].ToString();
                    plan.PLANCTA_PARTIDA = read[17].ToString();
                    plan.PLANCTA_DIF_CAMBIO = read[18].ToString();

                    plan.PLANCTA_NATURALEZA = read[19].ToString();


                    plan.PLANCTA_MONETARIA = read[20].ToString();
                    plan.PLANCTA_CON_COSTO = read[21].ToString();
                    plan.PLANCTA_PLAN_EXTERIOR = read[22].ToString();
                    plan.PLANCTA_ESTADO = read[23].ToString();

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return plan;
        }

        public List<OrdenFabricacion> findAllOrdenFabricacion()
        {
            List<OrdenFabricacion> listGastos = new List<OrdenFabricacion>();

            string conexion = Conexion.CadenaGeneral("014", "BDCOMUN", "ORD_FAB");
            string findAll = $"SELECT OF_COD,OF_ARTNOM, OF_ARTUNI from {conexion} ";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    OrdenFabricacion plan = new OrdenFabricacion();
                    plan.OF_COD = read[0].ToString();
                    plan.OF_ARTNOM = read[1].ToString();
                    plan.OF_ARTUNI = read[2].ToString();
                    listGastos.Add(plan);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return listGastos;
        }

        public List<Maquinas> findAllMaquinas()
        {
            List<Maquinas> listGastos = new List<Maquinas>();

            string conexion = Conexion.CadenaGeneral("014", "BDCOMUN", "BSC_MAQUINAS");
            string findAll = $"SELECT * from {conexion} where flagmantenimiento=0 ";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Maquinas plan = new Maquinas();
                    plan.idmaquina = read[0].ToString();
                    plan.maq_codigo = read[1].ToString();
                    listGastos.Add(plan);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return listGastos;
        }
        public Gasto findAllGastosDetail(string codigo)
        {


            string conexion = Conexion.CadenaGeneral("014", "BDCTAPAG", "Gastos");
            string findAll = $"select GASTOS_CODIGO, GASTOS_DESCRIPCION, GASTOS_MONEDA, GASTOS_HONORARIO, GASTOS_CUENTACON,GASTOS_DSCTO1, GASTOS_DSCTO2,Gastos_Cta1,Gastos_Cta2 FROM {conexion} WHERE GASTOS_CODIGO = '" + codigo + "'";
            Gasto gasto = null;
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    gasto = new Gasto();
                    gasto.Gastos_Codigo = read[0].ToString();
                    gasto.Gastos_Descripcion = read[1].ToString();
                    gasto.Gastos_Moneda = read[2].ToString();
                    gasto.Gastos_Honorario = Conversion.ParseBool(read[3].ToString());
                    gasto.Gastos_CuentaCon = read[4].ToString();
                    gasto.Gastos_Dscto1 = Conversion.ParseDecimal(read[5].ToString());
                    gasto.Gastos_Dscto2 = Conversion.ParseDecimal(read[6].ToString());
                    gasto.Gastos_Cta1 = read[7].ToString();
                    gasto.Gastos_Cta2 = read[8].ToString();

                }
                return gasto;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }

        }

        public bool ExiteConceptos()
        {
            bool hayRegistros = false;
            string conexion = Conexion.CadenaGeneral("014", "BDCONTABILIDAD", "CONCEPTOS_GENERALES");
            string find = $"SELECT* FROM {conexion} WHERE CONCGRAL_CODIGO = 'AGERET'";
            try
            {
                comando = new SqlCommand(find, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();


            }
            catch (Exception)
            {
                // e.GetType
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return hayRegistros;
        }




        public bool ExitedataConceptos()
        {
            bool hayRegistros = false;
            bool data = false;
            string conexion = Conexion.CadenaGeneral("014", "BDCONTABILIDAD", "CONCEPTOS_GENERALES");
            string find = $"SELECT* FROM {conexion} WHERE CONCGRAL_CODIGO = 'AGERET'";

            try
            {
                comando = new SqlCommand(find, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {
                    data = Conversion.ParseBool(read[6].ToString());
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return data;
        }

        public bool ExisteConcepto(string concepto)
        {
            bool hayRegistros = false;
            bool data = false;
            string find = $"SELECT* FROM CONCEPTOS_GENERALES WHERE CONCGRAL_CODIGO = '{concepto}'";

            try
            {
                comando = new SqlCommand(conexionContabilidad(find), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {
                    data = Conversion.ParseBool(read[6].ToString());
                }
                return data;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }

        }

        // VER LA FORMA COMO USAR DE FORMA GENERAL
        public string ConceptosGenerales(string concepto)
        {

            bool hayRegistros = false;
            string data = "";
            ConceptosGenerales conceptos = new ConceptosGenerales();
            string find = $"SELECT * FROM CONCEPTOS_GENERALES WHERE CONCGRAL_CODIGO = '{concepto}'";
            try
            {
                comando = new SqlCommand(conexionContabilidad(find), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {
                    conceptos.CONCGRAL_CODIGO = read[0].ToString();
                    conceptos.CONCGRAL_DESCRIPCION = read[1].ToString();
                    conceptos.CONCGRAL_TIPO = read[2].ToString();
                    conceptos.CONCGRAL_CONTEC = read[3].ToString();
                    conceptos.CONCGRAL_CONTEN = read[4].ToString();
                    conceptos.CONCGRAL_CONTED = read[5].ToString();
                    conceptos.CONCGRAL_CONTEL = read[6].ToString();
                    switch (conceptos.CONCGRAL_TIPO)
                    {
                        case "C":
                            data = conceptos.CONCGRAL_CONTEC;
                            break;
                        case "N":
                            data = conceptos.CONCGRAL_CONTEN;
                            break;
                        case "L":
                            data = conceptos.CONCGRAL_CONTEL;
                            break;
                        default:
                            data = conceptos.CONCGRAL_CONTED;
                            break;


                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return data;
        }
        // insertar data temporal  
        public void insertdetalleTemporal(Comprobante comprobante)
        {
            bool wlresta = false;

            string CCta = "";
            string CCta2 = "";
            string CCta1 = "";
            string CCtaPercep = "";

            string CCtaImportacion = "";
            string CCtaImpPerc_Igv = "";
            ConceptosGenerales conceptosGenerales = new ConceptosGenerales();
            ConceptoGral conceptoGral = new ConceptoGral();
            deleteContableDet();
            TipoDocumento tipoDocumento = findTipoDocumento(comprobante.TIPODOCU_CODIGO);
            if (tipoDocumento != null)
            {
                wlresta = tipoDocumento.TIPDOC_RESTA;
            }
            string destino = comprobante.CDESTCOMP;
            if (destino == "001" || destino == "002" || destino == "006" || destino == "007" || destino == "008")
            {
                Gasto gasto = findAllGastosDetail(comprobante.CCONCEPT);
                if (gasto != null)
                {
                    CCta = gasto.Gastos_Cta1;
                    CCta2 = gasto.Gastos_CuentaCon;
                }
                if (comprobante.NPERCEPCION != 0)
                {
                    conceptosGenerales = ConceptosGeneralesData("PRCTAIGV");
                    CCtaPercep = conceptosGenerales.CONCGRAL_CONTEC;
                }
                if (destino == "002")
                {
                    conceptoGral = conceptoGralData("IGVSCF");
                    if (conceptoGral != null)
                    {
                        CCta1 = conceptoGral.Concepto_Caracter;
                    }
                }
            }
            else
            {
                conceptoGral = conceptoGralData("IGVSCF");
                if (conceptoGral != null)
                {
                    CCta = conceptoGral.Concepto_Caracter;
                }
                Gasto gasto = findAllGastosDetail(comprobante.CCONCEPT);
                if (gasto != null)
                {
                    CCta1 = gasto.Gastos_Cta1;
                    CCta2 = gasto.Gastos_Cta1;
                }
            }

            if (Conversion.Parseint(comprobante.RESPONSABLE_CODIGO) == 10)
            {

                CCtaImportacion = verdata("CONCGRAL_CODIGO='IMPCARGO'", "CONCEPTOS_GENERALES", 1, "CONCGRAL_CONTEC");

            }


            if (!exiteCampo("CONTABLEDET", "ORDFAB"))
            {
                ALTERContableDet();
            }

            switch (comprobante.CDESTCOMP)
            {
                case "003":
                case "001":
                    if (comprobante.NIGV != 0)
                    {
                        insert(comprobante, wlresta, CCtaPercep, CCta);
                        insertNro2(comprobante, wlresta, CCtaPercep);

                    }
                    break;
                case "002":
                    insertD(comprobante, Conversion.ParseBool(wlresta), CCtaPercep, CCta);
                    insertNroD2(comprobante, Conversion.ParseBool(wlresta), CCtaPercep, CCta1);
                    insertNroD3(comprobante, Conversion.ParseBool(wlresta));
                    break;
                case "004":
                    if (comprobante.NIR4 == 0 && comprobante.NIES == 0)
                    {
                        insertD004(comprobante, Conversion.ParseBool(wlresta));
                    }
                    break;
                case "006":
                    if (comprobante.NIR4 > 0 && comprobante.NIES > 0)
                    {
                        insertD006(comprobante);
                        insertD0062(comprobante, CCta);
                        insertD0063(comprobante, CCta2);
                    }

                    break;
                case "007":
                    if (comprobante.NIR4 > 0)
                    {
                        insertD007(comprobante);
                        insertD0072(comprobante);
                    }

                    break;
                case "008":
                    if (comprobante.NIES > 0)
                    {
                        insertD008(comprobante);
                        insertD0082(comprobante, CCta2);
                    }
                    break;
                case "005":
                    if (comprobante.NIGV != 0)
                    {
                        insertD005(comprobante, Conversion.ParseBool(wlresta), CCta1);
                        insertD0052(comprobante, Conversion.ParseBool(wlresta));
                    }

                    break;
            }

            if (CCtaPercep != "")
            {
                insertGeneral(comprobante, Conversion.ParseBool(wlresta), CCtaPercep);
                insertGeneral2(comprobante, Conversion.ParseBool(wlresta), CCta2);
            }

            if (CCtaImportacion != null && CCtaImportacion.Trim() != "")
            {
                if (comprobante.TIPODOCU_CODIGO == "CI")
                {
                    if (int.Parse(comprobante.RESPONSABLE_CODIGO) == 10)
                    {

                        CCtaImpPerc_Igv = verdata("CONCGRAL_CODIGO='IGVPERC'", "CONCEPTOS_GENERALES", 1, "CONCGRAL_CONTEC");
                    }
                    insertImpor(comprobante, CCtaImpPerc_Igv);
                    insertImpor2(comprobante);

                }
                insertImpor3(comprobante, CCtaImportacion);


            }
            //commienza cuenta 60
            /* int item = 0;
             string cta = "";
             string Fam = "";
             string resp = "";
             string frms = "SD";
             string csql = "";
                 if (frms == "SD")
             {
                 //csql += "select * from comcab where CCTD='" & FrmFacSinGuiaRapido.Text1(6).Text & "' and CCNUMSER='" & FrmFacSinGuiaRap;
             }*/
        }

        public List<ContableDet> findallContableDet()
        {
            List<ContableDet> listAreas = new List<ContableDet>();
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string findAll = " SELECT *, campo1 =CASE CTIPO WHEN 'D' THEN NVALOR WHEN 'H' THEN 0 END  ,";

            findAll += "campo2 = CASE CTIPO WHEN 'D' THEN 0 WHEN 'H' THEN NVALOR END";
            findAll += $"  FROM {conexion}";

            //string findAll = $"SELECT *,IIF(CTIPO ='D', NVALOR, 0 ) as campo1 ,IIF(CTIPO ='H', NVALOR, 0 ) as campo2  FROM {conexion}";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    ContableDet area = new ContableDet();
                    area.CNROITEM = read[0].ToString();
                    area.NVALOR = Conversion.ParseDecimal(read[1].ToString());
                    area.CCODCONTA = read[2].ToString();
                    area.CTIPO = read[3].ToString();
                    area.CCOSTO = read[4].ToString();
                    area.CCTADEST = read[5].ToString();
                    area.CCODSUBDI = read[6].ToString();
                    area.CGLOSA = read[7].ToString();
                    area.CTIPDOC = read[8].ToString();
                    area.CSERDOC = read[9].ToString();
                    area.CNUMDOC = read[10].ToString();
                    area.CANEXO = read[11].ToString();
                    area.CCODANEXO = read[12].ToString();
                    area.NUMRETRAC = read[13].ToString();
                    area.FECRETRAC = Conversion.ParseDateTime(read[14].ToString());
                    area.ORDFAB = read[15].ToString();
                    area.codmaquina = read[16].ToString();
                    area.cantidad = Conversion.ParseDecimal(read[17].ToString());
                    area.campo1 = read[18].ToString();
                    area.campo2 = read[19].ToString();

                    listAreas.Add(area);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return listAreas;
        }

        public List<ContableDet> findContableDet()
        {
            List<ContableDet> listAreas = new List<ContableDet>();

            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");


            string findAll = $"SELECT * FROM {conexion} ORDER BY CNROITEM ";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    ContableDet area = new ContableDet();
                    area.CNROITEM = read[0].ToString();
                    area.NVALOR = Conversion.ParseDecimal(read[1].ToString());
                    area.CCODCONTA = read[2].ToString();
                    area.CTIPO = read[3].ToString();
                    area.CCOSTO = read[4].ToString();
                    area.CCTADEST = read[5].ToString();
                    area.CCODSUBDI = read[6].ToString();
                    area.CGLOSA = read[7].ToString();
                    area.CTIPDOC = read[8].ToString();
                    area.CSERDOC = read[9].ToString();
                    area.CNUMDOC = read[10].ToString();
                    area.CANEXO = read[11].ToString();
                    area.CCODANEXO = read[12].ToString();
                    area.NUMRETRAC = read[13].ToString();
                    area.FECRETRAC = Conversion.ParseDateTime(read[14].ToString());
                    area.ORDFAB = read[15].ToString();

                    listAreas.Add(area);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return listAreas;
        }

        #region ===== metodos para insertar====
        public void deleteContableDet()
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string delete = $"delete from {conexion}";
            try
            {
                comando = new SqlCommand(delete, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        public void ALTERContableDet()
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string delete = $"ALTER TABLE {conexion} ADD ORDFAB varchar(10) NULL";
            try
            {
                comando = new SqlCommand(delete, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        public TipoDocumento findTipoDocumento(string codigo)
        {
            // TIPOANEX_CODIGO = '" & _
            //txtTpoAnexo
            TipoDocumento gasto = null;
            string conexion = Conexion.CadenaGeneral("014", "BDCONTABILIDAD", "TIPOS_DE_DOCUMENTOS");

            string findAll = $"SELECT * FROM {conexion}  where TIPDOC_CODIGO='{codigo}' ";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    gasto = new TipoDocumento();
                    gasto.TIPDOC_CODIGO = read[0].ToString();
                    gasto.TIPDOC_DESCRIPCION = read[1].ToString();
                    gasto.TIPDOC_SUNAT = read[2].ToString();
                    gasto.TIPDOC_RESTA = Conversion.ParseBool(read[3].ToString());
                    gasto.TIPDOC_REFERENCIA = Conversion.ParseBool(read[4].ToString());
                    gasto.TIPDOC_FILE = read[5].ToString();
                    gasto.TIPDOC_FECHAVTO = read[6].ToString();
                    gasto.TIPDOC_REGCOMP = read[7].ToString();


                }


            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return gasto;
        }
        public ConceptosGenerales ConceptosGeneralesData(string concepto)
        {

            bool hayRegistros = false;


            ConceptosGenerales conceptos = new ConceptosGenerales();
            string conexion = Conexion.CadenaGeneral("014", "BDCONTABILIDAD", "CONCEPTOS_GENERALES");
            string find = $"SELECT * FROM {conexion} WHERE CONCGRAL_CODIGO = '{concepto}'";
            try
            {
                comando = new SqlCommand(find, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {
                    conceptos.CONCGRAL_CODIGO = read[0].ToString();
                    conceptos.CONCGRAL_DESCRIPCION = read[1].ToString();
                    conceptos.CONCGRAL_TIPO = read[2].ToString();
                    conceptos.CONCGRAL_CONTEC = read[3].ToString();
                    conceptos.CONCGRAL_CONTEN = read[4].ToString();
                    conceptos.CONCGRAL_CONTED = read[5].ToString();
                    conceptos.CONCGRAL_CONTEL = read[6].ToString();

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return conceptos;
        }
        public ConceptoGral conceptoGralData(string concepto)
        {

            bool hayRegistros = false;

            ConceptoGral conceptos = null;
            string conexion = Conexion.CadenaGeneral("014", "BDCOMUN", "CONCEPTOGRAL");
            string find = $"SELECT * FROM {conexion} WHERE Concepto_Codigo = '{concepto}'";
            try
            {
                comando = new SqlCommand(find, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                hayRegistros = read.Read();
                if (hayRegistros)
                {
                    conceptos = new ConceptoGral();
                    conceptos.Concepto_Codigo = read[0].ToString();
                    conceptos.Concepto_Descripcion = read[1].ToString();
                    conceptos.Concepto_Tipo = read[2].ToString();
                    conceptos.Concepto_Caracter = read[3].ToString();
                    conceptos.Concepto_Numerico = Conversion.ParseDecimal(read[4].ToString());
                    conceptos.Concepto_Fecha = Conversion.ParseDateTime(read[5].ToString());
                    conceptos.Concepto_Logico = Conversion.ParseBool(read[6].ToString());

                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return conceptos;
        }

        public bool exiteCampo(string tabla, string campo)
        {
            return existeTabla("", "BDWENCO", tabla, campo);
        }

        private bool existeTabla(string codigo, string nombreBase, string nombreTabla, string nombreColumn)
        {
            string conexion = Conexion.CadenaGeneral(codigo, nombreBase, nombreTabla);

            string sCmd = "use BDWENCO  ";
            sCmd += $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE COLUMN_NAME = '{nombreColumn}' AND TABLE_NAME = '{nombreTabla}'";


            try
            {
                comando = new SqlCommand(sCmd, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {

                    int exiteint = Conversion.Parseint(read[0].ToString());
                    return exiteint > 0;
                }
            }
            catch (Exception)
            {

                return false;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return false;
        }

        private void insert(Comprobante comprobante, bool wlresta, string CCtaPercep, string CCta)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00001',";
            csql += "" + comprobante.NIGV.ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(CCta) + "',";
            if (wlresta)
            {
                csql += "'H',";
            }
            else
            {
                csql += "'D',";
            }
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";

            if (CCtaPercep != "")
            {
                if (obtenerValer(3, CCtaPercep, "[BDWENCO].[dbo].[PLAN_CUENTA_NACIONAL]", "PLANCTA_CODIGO", false, "ISNULL(TIPOANEX_CODIGO,' ')") == null)
                {
                    csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
                    csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";
                }
                else
                {
                    csql += "'',";
                    csql += "'',";
                }
            }
            else
            {
                csql += "'',";
                csql += "'',";
            }
            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";



            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }


        private void insertNro2(Comprobante comprobante, bool wlresta, string CCtaPercep)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00002',";
            csql += "" + comprobante.NIMPORTE.ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(comprobante.CCODCONTA) + "',";
            if (wlresta)
            {
                csql += "'D',";
            }
            else
            {
                csql += "'H',";
            }
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";

            if (CCtaPercep != "")
            {
                if (obtenerValer(3, CCtaPercep, "[BDWENCO].[dbo].[PLAN_CUENTA_NACIONAL]", "PLANCTA_CODIGO", false, "ISNULL(TIPOANEX_CODIGO,' ')") == null)
                {
                    csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
                    csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";
                }
                else
                {
                    csql += "'',";
                    csql += "'',";
                }
            }
            else
            {
                csql += "'',";
                csql += "'',";
            }
            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";



            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }


        // caso 002
        private void insertD(Comprobante comprobante, bool wlresta, string CCtaPercep, string CCta)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00001',";
            csql += "" + (comprobante.NIGV * comprobante.NPORCE / 100).ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(CCta) + "',";
            if (wlresta)
            {
                csql += "'H',";
            }
            else
            {
                csql += "'D',";
            }
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'" + "',"; // anexo 
            csql += "'" + "',";// codanexo                 
            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";



            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        private void insertNroD2(Comprobante comprobante, bool wlresta, string CCtaPercep, string CCta1)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00002',";
            csql += "" + (comprobante.NIGV - comprobante.NIGV * comprobante.NPORCE / 100).ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(comprobante.CCODCONTA) + "',";
            if (wlresta)
            {
                csql += "'D',";
            }
            else
            {
                csql += "'H',";
            }
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'" + "',";
            csql += "'" + "',";
            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        private void insertNroD3(Comprobante comprobante, bool wlresta)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00003',";
            csql += "" + comprobante.NIMPORTE.ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(comprobante.CCODCONTA) + "',";
            if (wlresta)
            {
                csql += "'D',";
            }
            else
            {
                csql += "'H',";
            }
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";
            csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
            csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";
            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        // CASO 004

        private void insertD004(Comprobante comprobante, bool wlresta)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00001',";
            csql += "" + comprobante.NIMPORTE.ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(comprobante.CCODCONTA) + "',";
            if (wlresta)
            {
                csql += "'D',";
            }
            else
            {
                csql += "'h',";
            }
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";
            csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
            csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";
            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        //caso 006 
        private void insertD006(Comprobante comprobante)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO";
            csql += ",CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,CCOSTO,CCTADEST,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00001',";
            csql += "" + (comprobante.NTOTRH - comprobante.NIR4 - comprobante.NIES).ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(comprobante.CCODCONTA) + "',";

            csql += "'H',";

            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";
            csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
            csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";
            csql += "'" + "',";
            csql += "'" + "',";
            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        private void insertD0062(Comprobante comprobante, string CCta)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST";
            csql += ",CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00002',";
            csql += "" + (comprobante.NIR4).ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(CCta) + "',";

            csql += "'H',";
            csql += "'" + "',";
            csql += "'" + "',";
            csql += "'" + "',";
            csql += "'" + "',";
            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        private void insertD0063(Comprobante comprobante, string CCta2)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST";
            csql += ",CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00003',";
            csql += "" + (comprobante.NIES).ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(CCta2) + "',";

            csql += "'H',";
            csql += "'" + "',";
            csql += "'" + "',";
            csql += "'" + "',";
            csql += "'" + "',";
            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        // caso 007 
        private void insertD007(Comprobante comprobante)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO";
            csql += ",CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,CCOSTO,CCTADEST,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00001',";
            csql += "" + (comprobante.NTOTRH - comprobante.NIR4).ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(comprobante.CCODCONTA) + "',";
            csql += "'H',";
            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";
            csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
            csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";

            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";



            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        private void insertD0072(Comprobante comprobante)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO";
            csql += ",CCOSTO,CCTADEST,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00002',";
            csql += "" + (comprobante.NIR4).ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(comprobante.CCODCONTA) + "',";
            csql += "'H',";
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
            csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";
            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        // caso 008
        private void insertD008(Comprobante comprobante)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,";
            csql += "CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,CCOSTO,CCTADEST,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00001',";
            csql += "" + (comprobante.NTOTRH - comprobante.NIES).ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(comprobante.CCODCONTA) + "',";

            csql += "'H',";

            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";
            csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
            csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";
            csql += "'" + "',";
            csql += "'" + "',";
            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        private void insertD0082(Comprobante comprobante, string CCta2)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST";
            csql += "CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00002',";
            csql += "" + (comprobante.NIES).ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(CCta2) + "',";
            csql += "'H',";
            csql += "'" + "',";
            csql += "'" + "',";
            csql += "'" + "',";
            csql += "'" + "',";
            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        // caso 005 
        private void insertD005(Comprobante comprobante, bool wlresta, string CCta1)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00001',";
            csql += "" + comprobante.NIGV.ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(CCta1) + "',";
            if (wlresta)
            {
                csql += "'H',";
            }
            else
            {
                csql += "'D',";
            }
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'',";
            csql += "'',";
            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        private void insertD0052(Comprobante comprobante, bool wlresta)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            if (comprobante.NIGV != 0)
            {
                csql += "'00002',";
            }
            else
            {
                csql += "'00001',";
            }
            if (comprobante.CCODRUC != null)
            {
                csql += "" + (comprobante.NIMPORTE + comprobante.NVALCIF).ToString("F3", CultureInfo.InvariantCulture) + ",";
            }
            else
            {
                csql += "" + comprobante.NIMPORTE.ToString("F3", CultureInfo.InvariantCulture) + ",";
            }


            csql += "'" + fValNull(comprobante.CCODCONTA) + "',";
            if (wlresta)
            {
                csql += "'D',";
            }
            else
            {
                csql += "'H',";
            }
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";
            csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
            csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";
            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        // general
        private void insertGeneral(Comprobante comprobante, bool wlresta, string CCtaPercep)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00003',";
            csql += "" + comprobante.NPERCEPCION.ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(CCtaPercep) + "',";
            csql += "'D',";
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";

            if (obtenerValer(3, CCtaPercep, "[BDWENCO].[dbo].[PLAN_CUENTA_NACIONAL]", "PLANCTA_CODIGO", false, "TIPOANEX_CODIGO") != null)
            {
                csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
                csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";
            }
            else
            {
                csql += "'" + "',";
                csql += "'" + "',";
            }

            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        private void insertGeneral2(Comprobante comprobante, bool wlresta, string CCta2)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00004',";
            csql += "" + comprobante.NPERCEPCION.ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(CCta2) + "',";
            if (wlresta)
            {
                csql += "'D',";
            }
            else
            {
                csql += "'H',";
            }

            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";
            if (CCta2 != "")
            {
                if (obtenerValer(3, CCta2, "[BDWENCO].[dbo].[PLAN_CUENTA_NACIONAL]", "PLANCTA_CODIGO", false, "TIPOANEX_CODIGO") != null)
                {
                    csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
                    csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";
                }
                else
                {
                    csql += "'" + "',";
                    csql += "'" + "',";
                }
            }
            else
            {
                csql += "'" + "',";
                csql += "'" + "',";
            }


            csql += "'" + Esnulo(comprobante.NUMRETRAC, " ") + "'," + Esnulo(dateFormat(comprobante.FECRETRAC), 0) + ")";
            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }


        private void insertImpor(Comprobante comprobante, string CCtaImpPerc_Igv)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00001',";
            csql += "" + comprobante.NPERCEPCION.ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(CCtaImpPerc_Igv);
            csql += "'D',";
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";
            csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
            csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";

            csql += "'" + "'," + 0 + ")";



            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        private void insertImpor2(Comprobante comprobante)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00002',";
            csql += "" + comprobante.NPERCEPCION.ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(comprobante.CCODCONTA);
            csql += "'H',";
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";
            csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
            csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";

            csql += "'" + "'," + 0 + ")";



            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }

        private void insertImpor3(Comprobante comprobante, string CCtaImportacion)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00003',";
            csql += "" + comprobante.NBASEIMP.ToString("F3", CultureInfo.InvariantCulture) + ",";
            csql += "'" + fValNull(CCtaImportacion) + "',";
            csql += "'D',";
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";
            csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
            csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";

            csql += "'" + "'," + 0 + ")";



            try
            {
                comando = new SqlCommand(csql, objConexion.getCon());
                objConexion.getCon().Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        private dynamic Esnulo(string expresion, dynamic valor)
        {
            if (expresion == null || expresion.Trim() == "")
            {
                return valor;
            }
            else
            {
                return expresion;
            }
        }
        private string fValNull(string CCta)
        {
            if (CCta == null)
            {
                return "";
            }
            else
            {
                if (CCta.Trim() == "")
                {
                    return "";
                }
                else
                {
                    return CCta;
                }

            }

        }

        private string obtenerValer(int TipoAnexo, string cod, string tabla, string campo, bool fecha, string CampDev)
        {
            string cf = "";
            string data = "";
            if (campo.Trim() != "")
            {
                cf += "SELECT " + CampDev + " FROM " + tabla + " WHERE " + campo + "='" + cod + "'";

            }


            try
            {
                comando = new SqlCommand(conexionComun(cf), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {

                    data = read[0].ToString();
                }
                return data;


            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }


        }

        private string devolverDato(int TipoAnexo, string cod, string tabla, string campo, bool fecha, string CampDev)
        {
            string cf = "";
            string data = "";
            if (campo.Trim() != "")
            {
                cf += "SELECT " + CampDev + " FROM " + tabla + " WHERE " + campo + "='" + cod + "'";

            }


            try
            {
                comando = new SqlCommand(cf, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {

                    data = read[0].ToString();
                }
                return data;


            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
        }
        public string verdata(string condicion, string tabla, int param, string camdev)
        {
            string verdata = "";
            string CAD = $"SELECT* FROM  {tabla} WHERE   {condicion}";
            try
            {
                //TODO ES PARTE SE PUED GENERALIZAR
                comando = new SqlCommand(conexionContabilidad(CAD), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {

                    verdata = read[0].ToString();
                }
                return verdata;


            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }

        }
        #endregion ===end ===================

        public void update(Comprobante obj)
        {
            throw new NotImplementedException();
        }
    }
}