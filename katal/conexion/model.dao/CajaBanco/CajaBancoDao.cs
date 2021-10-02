using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using katal.conexion.model.entity;

namespace katal.conexion.model.dao
{
    public class CajaBancoDao : Obligatorio
    {

        public CajaBancoDao(string codEmpresa) : base(codEmpresa)
        {
            objConexion = Conexion.saberEstado();
            ///014BDCOMUN
        }

        public List<CajaBanco> findAll()
        {

            List<CajaBanco> cajaBancos = new List<CajaBanco>();

            string findAll = "SELECT CB_C_CODIG,CB_A_DESCR,CB_C_MONED FROM CAJA_BANCO where CB_C_TIPO='B' and CB_C_ESTADO='0'";
            try
            {
                comando = new SqlCommand(conexionCajaBanco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    CajaBanco cajaBanco = new CajaBanco();
                    cajaBanco.CB_C_CODIG = read[0].ToString();
                    cajaBanco.CB_A_DESCR = read[1].ToString();
                    cajaBanco.CB_C_MONED = read[2].ToString();

                    cajaBancos.Add(cajaBanco);
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
            return cajaBancos;
        }

        public CajaBanco findBanco(string codigo)
        {

            CajaBanco cajaBanco = new CajaBanco();

            string findAll = $"SELECT CB_C_CODIG,CB_A_DESCR,CB_C_MONED FROM CAJA_BANCO where CB_C_TIPO='B' and CB_C_ESTADO='0' and  CB_C_CODIG ='{codigo}'";
            try
            {
                comando = new SqlCommand(conexionCajaBanco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {

                    cajaBanco.CB_C_CODIG = read[0].ToString();
                    cajaBanco.CB_A_DESCR = read[1].ToString();
                    cajaBanco.CB_C_MONED = read[2].ToString();

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
            return cajaBanco;
        }


        public List<CMovimientoBanco> findAllMovimientos(string banco, string moneda, DateTime date)
        {

            List<CMovimientoBanco> cajaBancos = new List<CMovimientoBanco>();

            string anios = date.Year.ToString("0000.##");
            string mes = date.Month.ToString("00.##");
            string findAll = "SP_CBMOVCALCULAMTO";
            try
            {
                SqlParameter[] parametros = new SqlParameter[]{
                new SqlParameter("@EMP", CodEmpresa),
                new SqlParameter("@ANNO", anios),
                new SqlParameter("@BANCO", banco),
                new SqlParameter("@MES", mes),
                new SqlParameter("@MONEDA", moneda),

                };

                comando = new SqlCommand(findAll, objConexion.getCon());
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.AddRange(parametros);
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    CMovimientoBanco cajaBanco = new CMovimientoBanco();
                    cajaBanco.CB_C_SECUE = read[0].ToString();
                    cajaBanco.CB_C_OPERA = read[1].ToString();
                    cajaBanco.CB_C_DOCUM = read[2].ToString();
                    cajaBanco.CB_N_MTOMN = Conversion.ParseDecimal(read[3].ToString());
                    cajaBanco.CB_L_CONTA = read[4].ToString();
                    cajaBanco.CB_L_ANULA = read[5].ToString();
                    cajaBanco.CB_D_FECHA = Conversion.ParseDateTime(read[6].ToString());
                    cajaBanco.CB_C_ANEXO = read[7].ToString();
                    cajaBanco.CB_C_CONTA = read[8].ToString();
                    cajaBanco.CB_A_REFER = read[9].ToString();
                    cajaBanco.CB_C_NROLI = read[10].ToString();
                    cajaBancos.Add(cajaBanco);
                }


            }
            catch (Exception)
            {

                cajaBancos = new List<CMovimientoBanco>();
            }
            finally
            {
                objConexion.getCon().Close();
                objConexion.cerrarConexion();
            }
            return cajaBancos;
        }

        public List<DMovimientoBanco> findDetailMovimientos(string secuencia, string banco, string moneda, DateTime dateTime)
        {

            List<DMovimientoBanco> DetailMovimientos = new List<DMovimientoBanco>();

            int anios = dateTime.Year;
            string mes = dateTime.Month.ToString("00.##");

            string texto = ternario(moneda == "MN", "CB_N_MTOMN", "CB_N_MTOME");

            string findAll = $"SELECT CB_C_SecDE,CB_C_Conce+' ',Cb_C_TpDoc+LEFT(CB_C_Docum,21),{texto},CB_A_Refer  from DMOV_BANCO WHERE CB_C_Banco = '{banco}'";
            findAll += $" AND  CB_C_Mes='{mes}' AND CB_C_Secue ='{secuencia}' ORDER BY CB_C_SECDE";
            try
            {
                comando = new SqlCommand(conexionBDCBT(findAll, anios), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    DMovimientoBanco cajaBanco = new DMovimientoBanco();
                    cajaBanco.CB_C_SECDE = read[0].ToString();
                    cajaBanco.CB_C_CONCE = read[1].ToString();
                    cajaBanco.CB_C_TPDOC = read[2].ToString();
                    if (moneda == "MN")
                    {
                        cajaBanco.CB_N_MTOMND = Conversion.ParseDecimal(read[3].ToString());
                    }
                    else
                    {
                        cajaBanco.CB_N_MTOMED = Conversion.ParseDecimal(read[3].ToString());
                    }
                    cajaBanco.CB_A_REFERD = read[4].ToString();

                    DetailMovimientos.Add(cajaBanco);
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
            return DetailMovimientos;
        }

        public List<TipoOpcionCajaBanco> findAllTipoOpciones(string tipoIngresoSalida)
        {

            List<TipoOpcionCajaBanco> tiposcajaBancos = new List<TipoOpcionCajaBanco>();

            string findAll = $"Select CB_C_CODIG,CB_A_DESCR,CB_C_TPDOC,CB_C_FPAGO, CB_C_AUTOM from TIPO_OP_CAJA_BANCO where CB_C_TIPO = 'B'  and CB_C_MODO='{tipoIngresoSalida}'";


            try
            {
                comando = new SqlCommand(conexionCajaBanco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    TipoOpcionCajaBanco cajaBanco = new TipoOpcionCajaBanco();
                    cajaBanco.CB_C_CODIG = read[0].ToString();
                    cajaBanco.CB_A_DESCR = read[1].ToString();
                    cajaBanco.CB_C_TPDOC = read[2].ToString();
                    cajaBanco.CB_C_FPAGO = read[3].ToString();
                    cajaBanco.CB_C_AUTOM = read[4].ToString();
                    tiposcajaBancos.Add(cajaBanco);
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
            return tiposcajaBancos;
        }

        public TipoOpcionCajaBanco findTipoOpciones(string tipoIngresoSalida, string codigo)
        {

            TipoOpcionCajaBanco cajaBanco = new TipoOpcionCajaBanco();

            string findAll = $"Select CB_C_CODIG,CB_A_DESCR,CB_C_TPDOC,CB_C_FPAGO, CB_C_AUTOM from TIPO_OP_CAJA_BANCO where CB_C_TIPO = 'B'  and CB_C_MODO='{tipoIngresoSalida}' and CB_C_CODIG='{codigo}'";


            try
            {
                comando = new SqlCommand(conexionCajaBanco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {

                    cajaBanco.CB_C_CODIG = read[0].ToString();
                    cajaBanco.CB_A_DESCR = read[1].ToString();
                    cajaBanco.CB_C_TPDOC = read[2].ToString();
                    cajaBanco.CB_C_FPAGO = read[3].ToString();
                    cajaBanco.CB_C_AUTOM = read[4].ToString();

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
            return cajaBanco;
        }

        public List<TipoEstadoOperacion> findAllTipoEstadosOperaciones(string tipo)
        {

            List<TipoEstadoOperacion> tiposcajaBancos = new List<TipoEstadoOperacion>();

            string findAll = $"Select CB_C_CODIG,CB_A_DESCR FROM TIPO_TARJ_CTA_EST_OPERACION where CB_C_TIPO = '{tipo}'";
            try
            {
                comando = new SqlCommand(conexionCajaBanco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    TipoEstadoOperacion cajaBanco = new TipoEstadoOperacion();
                    cajaBanco.CB_C_CODIG = read[0].ToString();
                    cajaBanco.CB_A_DESCR = read[1].ToString();
                    tiposcajaBancos.Add(cajaBanco);
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
            return tiposcajaBancos;
        }
        public List<TipoMovimientos> findAllTipoMovimientos(string tipoIngresoSalida)
        {

            List<TipoMovimientos> tiposcajaBancos = new List<TipoMovimientos>();

            string findAll = $"SELECT CB_C_CODIG,CB_A_DESCR FROM TIPO_MOVIMIENTOS WHERE CB_C_CODIG<>'000' AND CB_C_CODIG<>'999' AND CB_C_TIPO='{tipoIngresoSalida}' ";
            try
            {
                comando = new SqlCommand(conexionCajaBanco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    TipoMovimientos cajaBanco = new TipoMovimientos();
                    cajaBanco.CB_C_CODIG = read[0].ToString();
                    cajaBanco.CB_A_DESCR = read[1].ToString();
                    tiposcajaBancos.Add(cajaBanco);
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
            return tiposcajaBancos;
        }
        public List<MedioPago> findAllMedioPago()
        {

            List<MedioPago> tiposcajaBancos = new List<MedioPago>();

            string findAll = "SELECT codigo,descripcion FROM medio_pago ";
            try
            {
                comando = new SqlCommand(conexionCajaBanco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    MedioPago cajaBanco = new MedioPago();
                    cajaBanco.CODIGO = read[0].ToString();
                    cajaBanco.DESCRIPCION = read[1].ToString();
                    tiposcajaBancos.Add(cajaBanco);
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
            return tiposcajaBancos;
        }


        public string numSec(string tipoDoc, string codigoBanco, DateTime fechaoperacion)
        {
            List<MedioPago> tiposcajaBancos = new List<MedioPago>();
            string mes = "";
            int anio = fechaoperacion.Year;
            string findAll = $"Select Max(cb_c_docum) from CMOV_BANCO where  cb_c_tpdoc = ' { tipoDoc} ' and cb_c_banco ='{codigoBanco}' and CB_C_MES='{mes}'";
            try
            {
                comando = new SqlCommand(conexionBDCBT(findAll, anio), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    MedioPago cajaBanco = new MedioPago();
                    cajaBanco.CODIGO = read[0].ToString();
                    cajaBanco.DESCRIPCION = read[1].ToString();
                    tiposcajaBancos.Add(cajaBanco);
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



        public string Genera_Secuencia(string codigoBanco, DateTime fechaoperacion)
        {
            List<MedioPago> tiposcajaBancos = new List<MedioPago>();
            string mes = fechaoperacion.Month.ToString("00.##");
            int anio = fechaoperacion.Year;
            string findAll = "SELECT max(C.CB_C_SECUE) FROM CMOV_BANCO C ";
            findAll += $" WHERE C.CB_C_Banco='{codigoBanco}' AND C.CB_C_Mes='{mes}'";

            int nro = 1;
            string secuencia = "0001";
            try
            {
                comando = new SqlCommand(conexionBDCBT(findAll, anio), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    nro = Conversion.Parseint(read[0].ToString());
                    nro++;
                }

                secuencia = nro.ToString("0000.##");
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
            return secuencia;
        }




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



        public List<TemporalGC> allTemporal()
        {
            List<TemporalGC> tiposcajaBancos = new List<TemporalGC>();
            ;

            string find = $"SELECT distinct * FROM TemporalGC ORDER BY FECHA";
            try
            {
                comando = new SqlCommand(conexionTemp(find), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();

                while (read.Read())
                {
                    TemporalGC conceptos = new TemporalGC();
                    conceptos.secuencia = read[0].ToString();
                    conceptos.ANEXO = read[1].ToString();
                    conceptos.DOC = read[2].ToString();
                    conceptos.fecha = read[3].ToString();
                    conceptos.cjavco = read[4].ToString();
                    conceptos.concepto = read[5].ToString();
                    conceptos.documento = read[6].ToString();
                    conceptos.documento = read[6].ToString();
                    conceptos.tc = read[6].ToString();
                    conceptos.tipmon = read[6].ToString();
                    conceptos.importe = read[6].ToString();
                    tiposcajaBancos.Add(conceptos);
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
            return tiposcajaBancos;
        }
        //public 

        public List<ConceptoCajaBanco> findAllConceptoCajaBanco(string tipo, string ingresoSalida)
        {

            List<ConceptoCajaBanco> cajaBancos = new List<ConceptoCajaBanco>();
            string findAll = "";
            if (ingresoSalida == "S")
            {
                findAll = $"Select CB_C_CODIG,CB_A_DESCR,CB_C_CUENT FROM CONCEPTO_CAJA_BANCO where CB_C_TIPO = '{tipo}'  and CB_C_MODO='I'";
            }
            else
            {
                findAll = $"Select CB_C_CODIG,CB_A_DESCR,CB_C_CUENT FROM CONCEPTO_CAJA_BANCO where CB_C_TIPO = '{tipo}'  and CB_C_MODO='S'";
            }
            try
            {
                comando = new SqlCommand(conexionCajaBanco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    ConceptoCajaBanco cajaBanco = new ConceptoCajaBanco();
                    cajaBanco.CB_C_CODIG = read[0].ToString();
                    cajaBanco.CB_A_DESCR = read[1].ToString();
                    cajaBanco.CB_C_CUENT = read[2].ToString();
                    cajaBanco.codigo = tipo + ingresoSalida + cajaBanco.CB_C_CODIG;
                    cajaBancos.Add(cajaBanco);
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
            return cajaBancos;
        }

        public List<ConceptoCajaBanco> findAllConceptoCajaBanco(string tipo, string ingresoSalida, string operacion)
        {

            List<ConceptoCajaBanco> cajaBancos = new List<ConceptoCajaBanco>();
            string findAll = "";
            findAll = $"Select CB_C_CODIG,CB_A_DESCR,CB_C_CUENT FROM CONCEPTO_CAJA_BANCO where CB_C_TIPO = '{tipo}'  and CB_C_MODO='{ingresoSalida}' and CB_C_OPERA='{operacion}'";

            try
            {
                comando = new SqlCommand(conexionCajaBanco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    ConceptoCajaBanco cajaBanco = new ConceptoCajaBanco();
                    cajaBanco.CB_C_CODIG = read[0].ToString();
                    cajaBanco.CB_A_DESCR = read[1].ToString();
                    cajaBanco.CB_C_CUENT = read[2].ToString();
                    cajaBanco.codigo = tipo + ingresoSalida + cajaBanco.CB_C_CODIG;
                    cajaBancos.Add(cajaBanco);
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
            return cajaBancos;
        }


        public string Genera_Secuencia_detalle(string codigoBanco, DateTime fechaoperacion, string secuenciaCab)
        {

            string mes = fechaoperacion.ToString("00.##");
            int anio = fechaoperacion.Year;
            string findAll = "SELECT MAX(CB_C_SECDE) FROM DMOV_BANCO C ";
            findAll += $" WHERE C.CB_C_Banco='{codigoBanco}' AND C.CB_C_Mes='{mes}' AND CB_C_Secue='{secuenciaCab}'";

            int nro = 1;
            string secuencia = "0001";
            try
            {
                comando = new SqlCommand(conexionBDCBT(findAll, anio), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    nro = Conversion.Parseint(read[0].ToString());
                    nro++;
                }

                secuencia = nro.ToString("0000.##");
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
            return secuencia;
        }


        public List<TipoMoneda> tipoMonedas()
        {
            List<TipoMoneda> tipoMonedas = new List<TipoMoneda>();
            string findAll = "";
            findAll = "SELECT TIPOMON_CODIGO,TIPOMON_DESCRIPCION FROM TIPO_MONEDA";

            try
            {
                comando = new SqlCommand(conexionWenco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    TipoMoneda cajaBanco = new TipoMoneda();
                    cajaBanco.TIPOMON_CODIGO = read[0].ToString();
                    cajaBanco.TIPOMON_DESCRIPCION = read[1].ToString();

                    tipoMonedas.Add(cajaBanco);
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
            return tipoMonedas;
        }
        #region insertarCabecera 

        //LN_TipCam = CONV_MONEDA(Label6(2), Text2(10), val(Text2(11)), MaskEdBox1, MaskEdBox1)
        public decimal tipoCambio(string cambio, string CC1, decimal CE, DateTime FE, DateTime FR , DateTime dateTime)
        {
            int anio = dateTime.Year;
            bool ok = true;
            string findAll =$" SELECT * FROM TIPO_CAMBIO WHERE TIPOMON_CODIGO = 'ME' AND YEAR(TIPOCAMB_FECHA)= {anio}  ORDER BY TIPOCAMB_FECHA";
            decimal valor = 0;
            try
            {
                
                comando = new SqlCommand(conexionWenco(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    switch (CC1)
                    {
                        case "ESP":
                            if (cambio == "MN")
                                if(CE!=0)
                                    valor = Math.Round(1 / CE, 8);
                                else
                                     valor = 999999;
                            else
                            {
                                valor = CE;
                            }
                            break;
                        case "FEC":
                            bool flag = false;
                            DateTime date = Conversion.ParseDateTime(read[1].ToString());
                            if (date.Date == FE.Date)
                                flag = true;
                            if (flag)
                            {
                                if (cambio == "ME")
                                {
                                    valor= Conversion.ParseDecimal(read[2].ToString());
                                }
                                else
                                {
                                    valor = Math.Round(1 / Conversion.ParseDecimal(read[2].ToString()), 8);
                                }
                            }
                            else
                                ok = false;
                            break;
                        case "COM":
                        case "VTA":
                            bool flagv = false;
                            DateTime datev = Conversion.ParseDateTime(read[1].ToString());
                            decimal compra = Conversion.ParseDecimal(read[2].ToString());
                            decimal venta = Conversion.ParseDecimal(read[4].ToString());

                            if (datev.Date == FR.Date)
                                flagv = true;
                            if (flagv)
                            {
                                if (cambio == "ME")
                                {
                                    valor = Conversion.ParseDecimal(read[2].ToString());
                                    valor = ternarioG(CC1 == "COM", compra, venta);
                                }
                                else
                                {
                                    compra= Math.Round(1 / compra, 8);
                                    venta= Math.Round(1 / venta, 8);
                                    valor = ternarioG(CC1 == "COM", compra, venta);
                                }
                            }
                            else
                                ok = false;
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
            if (!ok)
            {
                valor = 999999;// error
            }
            return valor;
        }
        public  void create(CMovimientoBanco obj,string codigoBanco, DateTime dateTime)
        {

            string mes = dateTime.Month.ToString("00.##");
            string create = "INSERT INTO CMOV_BANCO  ([CB_C_BANCO],[CB_C_MES],[CB_C_SECUE],[CB_C_MODO],[CB_C_OPERA],[CB_D_FECHA]";
            create += ",[CB_C_TPDOC] ,[CB_C_DOCUM] ,[CB_C_ANEXO]  ,[CB_C_CONVE] ,[CB_N_CAMES] ,[CB_N_TIPCA],[CB_N_MTOMN]  ,[CB_N_MTOME] ,[CB_C_CONTA]";
            create += ",[CB_A_REFER]  ,[CB_C_ESTAD] ,[CB_D_FECCOB],[CB_TIPMOV] ,[CB_MEDIO] ,[CB_DMEDIO] ,[CB_USUARIO])";
            create += $" VALUES('{codigoBanco}', '{mes}','{obj.CB_C_SECUE}','{obj.CB_C_MODO}'";
            create += $",'{obj.CB_C_OPERA}',{this.dateFormat( obj.CB_D_FECHA)},'{obj.CB_C_TPDOC}','{obj.CB_C_DOCUM}'";
            create += $",'{obj.CB_C_ANEXO}','{obj.CB_C_CONVE}',{obj.CB_N_CAMES},{obj.CB_N_TIPCA}";
            create += $",{obj.CB_N_MTOMN},{obj.CB_N_MTOME},'{obj.CB_C_CONTA}' , '{obj.CB_A_REFER}','{obj.CB_C_ESTAD}'";
            create += $",{dateFormat( obj.CB_D_FECCOB)},'{obj.CB_TIPMOV}','{obj.CB_MEDIO}','{obj.CB_DMEDIO}'";
            create += $",'{obj.CB_USUARIO}')";
            try
            {
                comando = new SqlCommand(conexionBDCBT(create, dateTime.Year), objConexion.getCon());
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

        public void updateNroAutomaticoOperacion(CMovimientoBanco obj, string codigo)
        {
            string updateNumCompras = $"UPDATE NUM_AUT_DOC SET ctnnumero='{obj.CB_C_DOCUM}' WHERE CB_C_Tipo='B' and CB_C_BANCO='{codigo}' and CB_C_TPDOC='{obj.CB_C_TPDOC}' ";
            try
            {
                comando = new SqlCommand(conexionCajaBanco(updateNumCompras), objConexion.getCon());
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
        #endregion
    }
}