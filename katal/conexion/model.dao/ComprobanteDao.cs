using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private string table;

        public ComprobanteDao( string bd)
        {
            table = bd;
            ApplicationUser user = AuthHelper.GetLoggedInUserInfo();

            if (user != null)
            {
                objConexion = Conexion.saberEstado(user.codEmpresa + "BDCOMUN");
            }///0      
        }
        public void create(Comprobante obj)
        {
            throw new NotImplementedException();
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
            string mes = date.Month.ToString("00.##");
            string msAnoMesProc = anios + mes;
            //string findAll = $"SELECT * FROM {table} WHERE CAMESPROC='" + msAnoMesProc +"' AND CSALDINI=0";
            string findAll = "SELECT * FROM COMPROBANTECAB  WHERE CAMESPROC='" + msAnoMesProc + "' AND CSALDINI=0";
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
                    comp.CIGVAPLIC = read[39].ToString();
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

        public List<Gasto> findAllGastos()
        {
            List<Gasto> listGastos = new List<Gasto>();


            string findAll = "SELECT Gastos_Codigo, Gastos_Descripcion FROM GASTOS ";
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


        public void update(Comprobante obj)
        {
            throw new NotImplementedException();
        }
    }
}