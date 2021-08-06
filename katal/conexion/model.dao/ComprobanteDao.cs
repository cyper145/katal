﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using katal.conexion.model.entity;
using katal.Model;

namespace katal.conexion.model.dao
{
    public class ComprobanteDao : Obligatorio<Comprobante>
    {

        private Conexion objConexion;
        private SqlCommand comando;
        private string Bd;

        public ComprobanteDao(string bd)
        {
            Bd = bd;
            ApplicationUser user = AuthHelper.GetLoggedInUserInfo();

            if (user != null)
            {
                objConexion = Conexion.saberEstado(user.codEmpresa + "BDCOMUN");
            }

            ///0      
        }

        public void create(Comprobante obj)
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
                    CadenaD += "" + IIfData(true, Math.Round(obj.NTOTRH, 2) - Math.Round(obj.NIR4, 2), Math.Round(obj.NIMPORTE, 2)) + ", ";
                }
                else
                    if (obj.CDESTCOMP.Trim() == "006")
                {
                    CadenaD += "" + IIfData(true, Math.Round(obj.NTOTRH, 2) - Math.Round(obj.NIR4, 2) + Math.Round(obj.NIMPORTE, 2), Math.Round(obj.NIMPORTE, 2)) + ", ";
                }
                else
                {
                    CadenaD += "" + IIfData(true, Math.Round(obj.NTOTRH, 2), Math.Round(obj.NIMPORTE, 2)) + ", ";
                }

                //   CadenaD += "" + obj.CONVERSION_CODIGO == "ESP" ? obj.TIPOCAMBIO_VALOR  : 0    + ", '" + obj.CDESCRIPC + "', '" + obj.RESPONSABLE_CODIGO + "', '" + sEstado + "', ";
                CadenaD += "" + obj.TIPOCAMBIO_VALOR.ToString("F3", CultureInfo.InvariantCulture) + ", '" + obj.CDESCRIPC + "', '" + obj.RESPONSABLE_CODIGO + "', '" + sEstado + "', ";
                //
                if (obj.CDESTCOMP.Trim() == "007" || obj.CDESTCOMP.Trim() == "008")
                {
                    // CadenaD += "" + IIf(true, Math.Round(Val(obj.NTOTRH), 2) - Math.Round(Val(obj.NIR4), 2), Math.Round(Val(obj.NIMPORTE), 2)) + ", ";
                    CadenaD += "" + IIfData(true, Math.Round(obj.NTOTRH + obj.NPERCEPCION, 2) - Math.Round(obj.NIR4, 2), Math.Round(obj.NIMPORTE + obj.NPERCEPCION, 2)) + ", ";
                }
                else
                    if (obj.CDESTCOMP.Trim() == "006")
                {
                    CadenaD += "" + IIfData(true, Math.Round(obj.NTOTRH + obj.NPERCEPCION, 2) - Math.Round(obj.NIR4, 2) + Math.Round(obj.NIES, 2), Math.Round(obj.NIMPORTE + obj.NPERCEPCION, 2)) + ", ";
                }
                else
                {
                    CadenaD += "" + IIfData(true, Math.Round(obj.NTOTRH + obj.NPERCEPCION, 2), Math.Round(obj.NIMPORTE + obj.NPERCEPCION, 2)) + ", ";
                }
                // cuando hay honorarios
                CadenaD += "'" + obj.CCODCONTA.Trim() + "', '" + funcBlanco(obj.CFORMPAGO) + "', '" + funcBlanco(obj.CSERREFER) + "', '" + funcBlanco(obj.CNUMREFER) + "',";
                CadenaD += "'" + funcBlanco(obj.CTDREFER) + "', '" + obj.CONVERSION_CODIGO + "', " + dateFormat(obj.DREGISTRO) + ", '" + obj.CTIPPROV + "', '" + obj.CNRORUC + "', ";
                CadenaD += "" + boolToInt(obj.ESTCOMPRA) + ", '" + funcBlanco(obj.CDESTCOMP) + "', " + obj.DIASPAGO + ", '" + boolToInt(obj.CIGVAPLIC) + "', '" + obj.CCONCEPT + "',";
                CadenaD += "" + dateFormat(obj.DFECREF) + ", " + obj.NTASAIGV + ", " + obj.NIGV + ", " + nPorcen + ", '" + funcBlanco(obj.CCODRUC) + "', ";
                CadenaD += "" + 0 + ", " + obj.NIR4 + ", " + obj.NIES + ", " + obj.NTOTRH + ", " + obj.NBASEIMP + ",";
                CadenaD += "" + nValCIF + ", " + dateFormat(obj.DEMISION) + ", '" + msAnoMesProc + "', " + boolToInt(obj.CSALDINI) + "," + obj.NPERCEPCION + ",'" + obj.NUMRETRAC + "'";
                CadenaD += "," + dateFormat(obj.FECRETRAC) + "";

                CadenaD += ",'" + funcBlanco(obj.CNUMORDCO).Trim() + "','" + obj.CO_L_RETE + "',";


                if (obj.NTASAIGV == 0) {
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
        public string dateFormat(DateTime date)
        {
            DateTime dateTime = DateTime.MinValue;
            if (date == dateTime)
            {
                date = new DateTime(1900, 1, 1);
            }
            return "'" + date.ToString("yyyy-dd-MM") + "'";
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
                comando = new SqlCommand(CadenaD, objConexion.getCon());
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
        public  void Trasferencia_CxP (string cCorre,string  txtTpoDcto,  string txtSerie, string txtNumero, string txNumDetraccion,bool detraacion , decimal TxtTasaDet, decimal TxTImporteRef, DateTime datedetraccion)
        {

        }

        public string funcAutoNum(string msAnoMesProc)
        {
            string cadena = "SELECT Concepto_Logico FROM CONCEPTOGRAL WHERE Concepto_Codigo='NUMEAUTO'";
            try
            {
                comando = new SqlCommand(cadena, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                bool hayRegistros = read.Read();
                if (hayRegistros)
                {

                    bool codigo = Conversion.ParseBool(read[0].ToString());
                    read.Close();
                    cadena = "SELECT MAX(CORDEN) AS MAXORDEN FROM COMPROBANTECAB WHERE CAMESPROC = '" + msAnoMesProc + "'";
                    comando = new SqlCommand(cadena, objConexion.getCon());
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
                    comp.CESTADO = read[16].ToString();
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
                    comp.CIGVAPLIC = converionBool(  read[39].ToString());
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
        private string  converionBoolReci(bool dale)
        {
            if (dale )
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
        public Comprobante findAllConta(string sCorrelativo , string TIPODOCU_CODIGO, string CSERIE,string CNUMERO)
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
            sql += "CAMESPROC='" + msAnoMesProc + "' AND A.CORDEN='" + sCorrelativo + "' AND TIPODOCU_CODIGO='" + TIPODOCU_CODIGO + "' AND CSERIE='" + CSERIE + "' AND   CNUMERO='" + CNUMERO+ "'  order by COrden";
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
                    comp.CESTADO = read[16].ToString();
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
                    comp.CIGVAPLIC = converionBool( read[39].ToString());
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
        public List<PlanCuentaNacional> findAllCuentasNacionales(int NivelContable=4)
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
            string findAll = $"select GASTOS_CODIGO, GASTOS_DESCRIPCION, GASTOS_MONEDA, GASTOS_HONORARIO, GASTOS_CUENTACON,GASTOS_DSCTO1, GASTOS_DSCTO2,Gastos_Cta1,Gastos_Cta2 FROM {conexion} WHERE GASTOS_CODIGO = '" + codigo + "'" ;
            Gasto gasto = new Gasto();
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {                    
                    gasto.Gastos_Codigo = read[0].ToString();
                    gasto.Gastos_Descripcion = read[1].ToString();
                    gasto.Gastos_Moneda= read[2].ToString();
                    gasto.Gastos_Honorario=Conversion.ParseBool (read[3].ToString());
                    gasto.Gastos_CuentaCon= read[4].ToString();
                    gasto.Gastos_Dscto1=Conversion.ParseDecimal( read[5].ToString());
                    gasto.Gastos_Dscto2= Conversion.ParseDecimal(read[6].ToString());
                    gasto.Gastos_Cta1= read[7].ToString();
                    gasto.Gastos_Cta2= read[8].ToString();
                   
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
            catch (Exception )
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
                     data = Conversion.ParseBool( read[6].ToString());
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

        // VER LA FORMA COMO USAR DE FORMA GENERAL
        public string ConceptosGenerales(string concepto )
        {
            
            bool hayRegistros = false;
            string data = "";
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
                    conceptos.CONCGRAL_CONTEL =  read[6].ToString();
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
                        default :
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
            string wlresta = "";
            int var = 0;
            string CCta = "";
            string CCta2 = "";
            string CCta1 = "";
            string CCtaPercep = "";
            decimal TDebe = 0;
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
            if (destino=="001" || destino == "002" || destino == "006" || destino == "007" || destino == "008")
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
                if (gasto!=null)
                {
                    CCta1 = gasto.Gastos_Cta1;
                    CCta2 = gasto.Gastos_Cta1;
                }
            }

            if (Conversion.Parseint( comprobante.RESPONSABLE_CODIGO) == 10)
            {
                string conexion = Conexion.CadenaGeneral("014", "BDCONTABILIDAD", "CONCEPTOS_GENERALES");
                CCtaImportacion = verdata("CONCGRAL_CODIGO='IMPCARGO'", conexion, 1, "CONCGRAL_CONTEC");
               
            }
            

            if(!exiteCampo("CONTABLEDET", "ORDFAB"))
            {
                ALTERContableDet();
            }

            switch(comprobante.CDESTCOMP)
            {
                case "003":
                case "001":
                    if (comprobante.NIGV != 0)
                    {
                        insert(comprobante,Conversion.ParseBool( wlresta), CCtaPercep, CCta);
                        insertNro2(comprobante,Conversion.ParseBool( wlresta), CCtaPercep);
                       
                    }
                    break;
                case "002":
                    insertD(comprobante, Conversion.ParseBool(wlresta), CCtaPercep, CCta);
                    insertNroD2(comprobante, Conversion.ParseBool(wlresta), CCtaPercep, CCta1);
                    insertNroD3(comprobante, Conversion.ParseBool(wlresta));
                    break;
                case "004":
                    if(comprobante.NIR4==0 && comprobante.NIES == 0)
                    {
                        insertD004(comprobante, Conversion.ParseBool(wlresta));
                    }
                    break;
                case "006":
                    if(comprobante.NIR4 > 0 && comprobante.NIES > 0)
                    {
                        insertD006(comprobante);
                        insertD0062(comprobante, CCta);
                        insertD0063(comprobante, CCta2);
                    }
                       
                    break;
                case "007":
                    if(comprobante.NIR4>0)
                    {
                        insertD007(comprobante);
                        insertD0072(comprobante);
                    }
                        
                    break;
                case "008":
                    if (comprobante.NIES > 0)
                    {
                        insertD008(comprobante);
                        insertD0082(comprobante,CCta2);
                    }
                    break;
                case "005":
                    if (comprobante.NIGV != 0)
                    {
                        insertD005(comprobante, Conversion.ParseBool(wlresta),CCta1);
                        insertD0052(comprobante, Conversion.ParseBool(wlresta));
                    }

                    break;
            }

            if (CCtaPercep != null)
            {
                insertGeneral(comprobante, Conversion.ParseBool(wlresta), CCtaPercep);
                insertGeneral2(comprobante, Conversion.ParseBool(wlresta), CCta2);
            }
           
            if(CCtaImportacion!=null || CCtaImportacion.Trim() != "")
            {
                if (comprobante.TIPODOCU_CODIGO == "CI")
                {
                    if(int.Parse(comprobante.RESPONSABLE_CODIGO) == 10)
                    {
                        string conexion = Conexion.CadenaGeneral("014", "BDCONTABILIDAD", "CONCEPTOS_GENERALES");
                        CCtaImpPerc_Igv = verdata("CONCGRAL_CODIGO='IGVPERC'", conexion, 1, "CONCGRAL_CONTEC");
                    }
                    insertImpor(comprobante, CCtaImpPerc_Igv);
                    insertImpor2(comprobante);

                }
                insertImpor3(comprobante, CCtaImportacion);


            }
            //commienza cuenta 60
            int item = 0;
            string cta = "";
            string Fam = "";
            string resp = "";
            string frms = "SD";
            string csql = "";
                if (frms == "SD")
            {
                //csql += "select * from comcab where CCTD='" & FrmFacSinGuiaRapido.Text1(6).Text & "' and CCNUMSER='" & FrmFacSinGuiaRap;
            }
        }

        public List<ContableDet> findallContableDet()
        {
            List<ContableDet> listAreas = new List<ContableDet>();
            string conexion = Conexion.CadenaGeneral("014", "BDWENCO", "ContableDet");


            string findAll = $"SELECT *,IIF(CTIPO ='D', NVALOR, 0 ) as campo1 ,IIF(CTIPO ='H', NVALOR, 0 ) as campo2  FROM {conexion}";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    ContableDet area = new ContableDet();
                    area.CNROITEM = read[0].ToString();
                    area.NVALOR =Conversion.ParseDecimal( read[1].ToString());
                    area.CTIPO = read[2].ToString();
                    area.CCOSTO = read[3].ToString();
                    area.CCTADEST = read[4].ToString();
                    area.CCODSUBDI = read[5].ToString();
                    area.CGLOSA = read[6].ToString();
                    area.CTIPDOC = read[7].ToString();
                    area.CSERDOC = read[8].ToString();
                    area.CNUMDOC = read[9].ToString();
                    area.CANEXO = read[10].ToString();
                    area.CCODANEXO = read[11].ToString();
                    area.NUMRETRAC = read[12].ToString();
                    area.FECRETRAC = read[13].ToString();
                    area.campo1 = read[14].ToString();
                    area.campo2 = read[15].ToString();

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
            TipoDocumento gasto = new TipoDocumento();
            string conexion = Conexion.CadenaGeneral("014", "BDCONTABILIDAD", "TIPOS_DE_DOCUMENTOS");
           
            string findAll = $"SELECT TIPDOC_CODIGO, TIPDOC_DESCRIPCION, tipdoc_referencia FROM {conexion}  where TIPDOC_CODIGO='{codigo}' ";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {

                    gasto.TIPDOC_CODIGO = read[0].ToString();
                    gasto.TIPDOC_DESCRIPCION = read[1].ToString();
                    gasto.TIPDOC_SUNAT = read[2].ToString();
                    gasto.TIPDOC_RESTA = read[2].ToString();
                    gasto.TIPDOC_REFERENCIA = Conversion.ParseBool(read[2].ToString());
                    gasto.TIPDOC_FECHAVTO = read[2].ToString();
                    gasto.TIPDOC_REGCOMP = read[2].ToString();

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

        public ConceptosGenerales ConceptosGeneralesData(string  concepto)
        {

            bool hayRegistros = false;
            string data = "";

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
            string data = "";

            ConceptoGral conceptos = new ConceptoGral();
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
                    conceptos.Concepto_Codigo = read[0].ToString();
                    conceptos.Concepto_Descripcion = read[1].ToString();
                    conceptos.Concepto_Tipo = read[2].ToString();
                    conceptos.Concepto_Caracter = read[3].ToString();
                    conceptos.Concepto_Numerico =Conversion.ParseDecimal( read[4].ToString());
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
            bool exite = false;

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

        private void insert(Comprobante comprobante, bool wlresta , string CCtaPercep, string CCta)
        {
           
           string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00001',";
            csql +=  "" + comprobante.NIGV  + ",";
            csql += "'" + fValNull(CCta) + "',";
            if (wlresta)
            {
                csql +=  "'H',";
            }
            else
            {
                csql += "'D',";
            }
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql +=   "'" + fValNull(comprobante.CCTADEST ) + "',";
            csql +=   "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql +=   "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql +=   "'" + fValNull(comprobante.CSERIE ) + "',";
            csql +=   "'" + fValNull(comprobante.CNUMERO ) + "',";

            if (CCtaPercep!="")
            {
                if(obtenerValer(3, CCtaPercep, "[BDWENCO].[dbo].[PLAN_CUENTA_NACIONAL]", "PLANCTA_CODIGO", false, "ISNULL(TIPOANEX_CODIGO,' ')") == null)
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
            csql += "" + comprobante.NIMPORTE.ToString("##############0.00") + ",";
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
            csql += "" + (comprobante.NIGV*comprobante.NPORCE/100).ToString("##############0.00") + ",";
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
            csql += "" + (comprobante.NIGV-comprobante.NIGV * comprobante.NPORCE / 100).ToString("##############0.00") + ",";
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
            csql += "" + comprobante.NIMPORTE.ToString("##############0.00") + ",";
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
            csql += "" + comprobante.NIMPORTE.ToString("##############0.00") + ",";
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
            csql += "" + (comprobante.NTOTRH- comprobante.NIR4- comprobante.NIES).ToString("##############0.00") + ",";
            csql += "'" + fValNull(comprobante.CCODCONTA) + "',";
            
            csql += "'H',";          
          
            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";
            csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
            csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',"; 
            csql += "'"  + "',";
            csql += "'"  + "',";          
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
            csql += "" + (comprobante.NIR4).ToString("##############0.00") + ",";
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
            csql += "" + (comprobante.NIES).ToString("##############0.00") + ",";
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
            csql += "" + (comprobante.NTOTRH- comprobante.NIR4).ToString("##############0.00") + ",";
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
            csql += "" + (comprobante.NIR4).ToString("##############0.00") + ",";
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
            csql += "" + (comprobante.NTOTRH- comprobante.NIES).ToString("##############0.00") + ",";
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
        private void insertD0082(Comprobante comprobante,string CCta2)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST";
            csql += "CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00002',";
            csql += "" + (comprobante.NIES).ToString("##############0.00") + ",";
            csql += "'" + fValNull(CCta2) + "',";
            csql += "'H',";
            csql += "'"  + "',";
            csql += "'"  + "',";
            csql += "'"  + "',";
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
            csql += "" + comprobante.NIGV.ToString("##############0.00") + ",";
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
            if (comprobante.CCODRUC!=null)
            {
                csql += "" + (comprobante.NIMPORTE+comprobante.NVALCIF ).ToString("##############0.00") + ",";
            }
            else
            {
                csql += "" + comprobante.NIMPORTE.ToString("##############0.00") + ",";
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
            csql += "" + comprobante.NPERCEPCION.ToString("##############0.00") + ",";           
            csql += "'" + fValNull(CCtaPercep) + "',";          
            csql += "'D',";         
            csql += "'" + fValNull(comprobante.CCOSTO) + "',";
            csql += "'" + fValNull(comprobante.CCTADEST) + "',";
            csql += "'" + fValNull(comprobante.SUBDIARIO_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CDESCRIPC) + "',";
            csql += "'" + fValNull(comprobante.TIPODOCU_CODIGO) + "',";
            csql += "'" + fValNull(comprobante.CSERIE) + "',";
            csql += "'" + fValNull(comprobante.CNUMERO) + "',";

            if (obtenerValer(3, CCtaPercep, "[BDWENCO].[dbo].[PLAN_CUENTA_NACIONAL]", "PLANCTA_CODIGO", false, "TIPOANEX_CODIGO")!=null)
            {
                    csql += "'" + fValNull(comprobante.CTIPPROV) + "',";
                    csql += "'" + fValNull(comprobante.ANEX_CODIGO) + "',";
            }
            else
            {
                csql += "'"  + "',";
                csql += "'"  + "',";
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
            csql += "" + comprobante.NPERCEPCION.ToString("##############0.00") + ",";
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
            csql += "" + comprobante.NPERCEPCION.ToString("##############0.00") + ",";
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
           
            csql += "'" +  "'," + 0 + ")";



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
            csql += "" + comprobante.NPERCEPCION.ToString("##############0.00") + ",";
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

        private void insertImpor3(Comprobante comprobante,string CCtaImportacion)
        {
            string conexion = Conexion.CadenaGeneral("", "BDWENCO", "ContableDet");
            string csql = $"Insert Into {conexion}(CNROITEM,NVALOR,CCODCONTA,CTIPO,CCOSTO,CCTADEST,CCODSUBDI,";
            csql += "CGLOSA,CTIPDOC,CSERDOC,CNUMDOC,CANEXO,CCODANEXO,NUMRETRAC,FECRETRAC) Values(";
            csql += "'00003',";
            csql += "" + comprobante.NBASEIMP.ToString("##############0.00") + ",";
            csql += "'" + fValNull(CCtaImportacion);
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
            if(expresion==null || expresion.Trim() == "")
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
            if(campo .Trim() != "")
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

        private string verdata(string condicion, string tabla, int param, string camdev)
        {
            string verdata = "";
            string CAD = $"SELECT* FROM  {tabla} WHERE   {condicion}";
            try
            {
                comando = new SqlCommand(CAD, objConexion.getCon());
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