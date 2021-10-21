using katal.conexion.model.entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
            string findAll = "SP_CBMOVCALCULAMTOCOMPLETO";
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

                    string darse = read[0].ToString();
                    cajaBanco.CB_C_Secue = read[nameof(cajaBanco.CB_C_Secue)].ToString();
                    cajaBanco.Opera = read[nameof(cajaBanco.Opera)].ToString();
                    cajaBanco.docu = read[nameof(cajaBanco.docu)].ToString();
                    cajaBanco.MONTO = Conversion.ParseDecimal(read[nameof(cajaBanco.MONTO)].ToString());
                    cajaBanco.Conta = read[nameof(cajaBanco.Conta)].ToString();
                    cajaBanco.Anula = read[nameof(cajaBanco.Anula)].ToString();
                    cajaBanco.CB_D_Fecha = Conversion.ParseDateTime(read[nameof(cajaBanco.CB_D_Fecha)].ToString());

                    string CB_C_Anexo = "";
                    cajaBanco.CB_C_AnexoV = read[nameof(CB_C_Anexo)].ToString();
                    cajaBanco.CB_C_CONTAV = read[10].ToString();
                    cajaBanco.CB_A_REFERV = read[11].ToString();
                    cajaBanco.CB_C_NROLIV = read[12].ToString();

                    cajaBanco.CB_C_BANCO = read[nameof(cajaBanco.CB_C_BANCO)].ToString();
                    cajaBanco.CB_C_MES = read[nameof(cajaBanco.CB_C_MES)].ToString();
                    cajaBanco.CB_C_SECUE = read[nameof(cajaBanco.CB_C_SECUE)].ToString();
                    cajaBanco.CB_C_MODO = read[nameof(cajaBanco.CB_C_MODO)].ToString();
                    cajaBanco.CB_C_OPERA = read[nameof(cajaBanco.CB_C_OPERA)].ToString();
                    cajaBanco.CB_D_FECHA = Conversion.ParseDateTime(read[nameof(cajaBanco.CB_D_FECHA)].ToString());
                    cajaBanco.CB_C_TPDOC = read[nameof(cajaBanco.CB_C_TPDOC)].ToString();

                    string documento = read[nameof(cajaBanco.CB_C_DOCUM)].ToString();
                    if (documento.Length > 4)
                    {
                        cajaBanco.serie = documento.Substring(0, 4);
                        cajaBanco.CB_C_DOCUM = documento.Substring(4);

                    }
                    else
                    {
                        cajaBanco.serie = "";
                        cajaBanco.CB_C_DOCUM = documento;
                    }
                    if (cajaBanco.CB_C_AnexoV.Trim() != "")
                    {
                        cajaBanco.TipoAnexoD = cajaBanco.CB_C_AnexoV.Substring(0, 2);
                        cajaBanco.CB_C_ANEXO = cajaBanco.CB_C_AnexoV.Substring(2);
                    }
                    else
                    {
                        cajaBanco.TipoAnexoD = "";
                        cajaBanco.CB_C_ANEXO = "";

                    }


                    cajaBanco.CB_C_CONVE = read[nameof(cajaBanco.CB_C_CONVE)].ToString();
                    cajaBanco.CB_N_CAMES = Conversion.ParseDecimal(read[nameof(cajaBanco.CB_N_CAMES)].ToString());
                    cajaBanco.CB_D_FECCA = Conversion.ParseDateTime(read[nameof(cajaBanco.CB_D_FECCA)].ToString());
                    cajaBanco.CB_N_TIPCA = Conversion.ParseDecimal(read[nameof(cajaBanco.CB_N_TIPCA)].ToString());
                    cajaBanco.CB_N_MTOMN = Conversion.ParseDecimal(read[nameof(cajaBanco.CB_N_MTOMN)].ToString());
                    cajaBanco.CB_N_MTOME = Conversion.ParseDecimal(read[nameof(cajaBanco.CB_N_MTOME)].ToString());
                    cajaBanco.CB_C_CONTA = read[nameof(cajaBanco.CB_C_CONTA)].ToString();
                    cajaBanco.CB_C_NROLI = read[nameof(cajaBanco.CB_C_NROLI)].ToString();
                    cajaBanco.CB_C_FACTU = read[nameof(cajaBanco.CB_C_FACTU)].ToString();
                    cajaBanco.CB_L_CONTA = read[nameof(cajaBanco.CB_L_CONTA)].ToString();
                    cajaBanco.CB_L_ANULA = read[nameof(cajaBanco.CB_L_ANULA)].ToString();
                    cajaBanco.CB_A_REFER = read[nameof(cajaBanco.CB_A_REFER)].ToString();
                    cajaBanco.CB_C_ESTAD = read[nameof(cajaBanco.CB_C_ESTAD)].ToString();
                    cajaBanco.CB_C_ESTRET = read[nameof(cajaBanco.CB_C_ESTRET)].ToString();
                    cajaBanco.CB_RETLET = read[nameof(cajaBanco.CB_RETLET)].ToString();
                    cajaBanco.CB_D_FECCOB = Conversion.ParseDateTime(read[nameof(cajaBanco.CB_D_FECCOB)].ToString());
                    cajaBanco.CB_TRANSBCO = read[nameof(cajaBanco.CB_TRANSBCO)].ToString();
                    cajaBanco.CB_TIPMOV = read[nameof(cajaBanco.CB_TIPMOV)].ToString();
                    cajaBanco.CB_MEDIO = read[nameof(cajaBanco.CB_MEDIO)].ToString();
                    cajaBanco.CB_DMEDIO = read[nameof(cajaBanco.CB_DMEDIO)].ToString();
                    cajaBanco.CB_USUARIO = read[nameof(cajaBanco.CB_USUARIO)].ToString();
                    cajaBanco.montoVisual = ternarioG(moneda == "MN", cajaBanco.CB_N_MTOMN, cajaBanco.CB_N_MTOME);


                    cajaBancos.Add(cajaBanco);
                }


            }
            catch (Exception ex)
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
        public CMovimientoBanco findMovimiento(string secuencia, string banco, DateTime date)
        {
            CMovimientoBanco cajaBanco = new CMovimientoBanco();
            string mes = date.Month.ToString("00.##");
            string findAll = $"SELECT * FROM CMOV_BANCO where CB_C_BANCO='{banco}' and CB_C_MES='{mes}' and  CB_C_SECUE ='{secuencia}'";
            try
            {
                comando = new SqlCommand(conexionBDCBT(findAll, date.Year), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {

                    cajaBanco.CB_C_BANCO = read[nameof(cajaBanco.CB_C_BANCO)].ToString();
                    cajaBanco.CB_C_MES = read[nameof(cajaBanco.CB_C_MES)].ToString();
                    cajaBanco.CB_C_SECUE = read[nameof(cajaBanco.CB_C_SECUE)].ToString();
                    cajaBanco.CB_C_MODO = read[nameof(cajaBanco.CB_C_MODO)].ToString();
                    cajaBanco.CB_C_OPERA = read[nameof(cajaBanco.CB_C_OPERA)].ToString();
                    cajaBanco.CB_D_FECHA = Conversion.ParseDateTime(read[nameof(cajaBanco.CB_D_FECHA)].ToString());
                    cajaBanco.CB_C_TPDOC = read[nameof(cajaBanco.CB_C_TPDOC)].ToString();
                    cajaBanco.CB_C_DOCUM = read[nameof(cajaBanco.CB_C_DOCUM)].ToString().Trim();
                    cajaBanco.CB_C_ANEXO = read[nameof(cajaBanco.CB_C_ANEXO)].ToString();
                    cajaBanco.CB_C_CONVE = read[nameof(cajaBanco.CB_C_CONVE)].ToString();
                    cajaBanco.CB_N_CAMES = Conversion.ParseDecimal(read[nameof(cajaBanco.CB_N_CAMES)].ToString());
                    cajaBanco.CB_D_FECCA = Conversion.ParseDateTime(read[nameof(cajaBanco.CB_D_FECCA)].ToString());
                    cajaBanco.CB_N_TIPCA = Conversion.ParseDecimal(read[nameof(cajaBanco.CB_N_TIPCA)].ToString());
                    cajaBanco.CB_N_MTOMN = Conversion.ParseDecimal(read[nameof(cajaBanco.CB_N_MTOMN)].ToString());
                    cajaBanco.CB_N_MTOME = Conversion.ParseDecimal(read[nameof(cajaBanco.CB_N_MTOME)].ToString());
                    cajaBanco.CB_C_CONTA = read[nameof(cajaBanco.CB_C_CONTA)].ToString();
                    cajaBanco.CB_C_NROLI = read[nameof(cajaBanco.CB_C_NROLI)].ToString();
                    cajaBanco.CB_C_FACTU = read[nameof(cajaBanco.CB_C_FACTU)].ToString();
                    cajaBanco.CB_L_CONTA = read[nameof(cajaBanco.CB_L_CONTA)].ToString();
                    cajaBanco.CB_L_ANULA = read[nameof(cajaBanco.CB_L_ANULA)].ToString();
                    cajaBanco.CB_A_REFER = read[nameof(cajaBanco.CB_A_REFER)].ToString();
                    cajaBanco.CB_C_ESTAD = read[nameof(cajaBanco.CB_C_ESTAD)].ToString();
                    cajaBanco.CB_C_ESTRET = read[nameof(cajaBanco.CB_C_ESTRET)].ToString();
                    cajaBanco.CB_RETLET = read[nameof(cajaBanco.CB_RETLET)].ToString();
                    cajaBanco.CB_D_FECCOB = Conversion.ParseDateTime(read[nameof(cajaBanco.CB_D_FECCOB)].ToString());
                    cajaBanco.CB_TRANSBCO = read[nameof(cajaBanco.CB_TRANSBCO)].ToString();
                    cajaBanco.CB_TIPMOV = read[nameof(cajaBanco.CB_TIPMOV)].ToString();
                    cajaBanco.CB_MEDIO = read[nameof(cajaBanco.CB_MEDIO)].ToString();
                    cajaBanco.CB_DMEDIO = read[nameof(cajaBanco.CB_DMEDIO)].ToString();
                    cajaBanco.CB_USUARIO = read[nameof(cajaBanco.CB_USUARIO)].ToString();

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

        public List<DMovimientoBanco> findDetailMovimientos(string secuencia, string banco, string moneda, DateTime dateTime, string tipo)
        {

            List<DMovimientoBanco> DetailMovimientos = new List<DMovimientoBanco>();

            int anios = dateTime.Year;
            string mes = dateTime.Month.ToString("00.##");

            string texto = ternario(moneda == "MN", "CB_N_MTOMN  as monto", "CB_N_MTOME  as monto");





            string findAll = $"SELECT CB_C_SecDE  as secd,CB_C_Conce+' '+D.CB_A_DESCR,Cb_C_TpDoc+LEFT(CB_C_Docum,21),{texto},CB_A_Refer as refer,*  ";
            findAll += $"from DMOV_BANCO inner join (select CB_C_CODIG, CB_A_DESCR from [014BDCAJABANCO].[dbo].[CONCEPTO_CAJA_BANCO]  where CB_C_TIPO = 'B' and CB_C_MODO = '{tipo}') as D on DMOV_BANCO.CB_C_CONCE = D.CB_C_CODIG WHERE CB_C_Banco = '{banco}'";
            findAll += $" AND  CB_C_Mes='{mes}' AND CB_C_Secue ='{secuencia}' ORDER BY CB_C_SECDE";
            try
            {
                comando = new SqlCommand(conexionBDCBT(findAll, anios), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    DMovimientoBanco cajaBanco = new DMovimientoBanco();
                    cajaBanco.secd = read[0].ToString();
                    cajaBanco.CB_C_Concep = read[1].ToString();
                    cajaBanco.CB_C_docum = read[2].ToString();

                    string documento = read[13].ToString();
                    if (documento.Length > 4)
                    {
                        cajaBanco.serieD = documento.Substring(0, 4);
                        cajaBanco.CB_C_DOCUMD = documento.Substring(4);

                    }
                    else
                    {
                        cajaBanco.serieD = "";
                        cajaBanco.CB_C_DOCUMD = documento;
                    }

                    if (moneda == "MN")
                    {
                        cajaBanco.montomn = Conversion.ParseDecimal(read[3].ToString());
                    }
                    else
                    {
                        cajaBanco.montome = Conversion.ParseDecimal(read[3].ToString());
                    }
                    cajaBanco.CB_A_REFERD = read[4].ToString();

                    cajaBanco.CB_C_BANCO = read[5].ToString();
                    cajaBanco.CB_C_MES = read[6].ToString();
                    cajaBanco.CB_C_SECUE = read[7].ToString();
                    cajaBanco.CB_C_SECDE = read[8].ToString();
                    cajaBanco.CB_C_MODO = read[9].ToString();
                    cajaBanco.CB_C_CONCE = read[10].ToString();


                    string CB_C_ANEXOD = read[11].ToString();
                    if (CB_C_ANEXOD.Trim() != "")
                    {
                        cajaBanco.TipoAnexo = CB_C_ANEXOD.Substring(0, 2);
                        cajaBanco.CB_C_ANEXOD = CB_C_ANEXOD.Substring(2);
                    }
                    else
                    {
                        cajaBanco.TipoAnexo = "";
                        cajaBanco.CB_C_ANEXOD = "";

                    }

                    cajaBanco.CB_C_TPDOCD = read[12].ToString();

                    cajaBanco.CB_D_FECDC = Conversion.ParseDateTime(read[14].ToString());
                    cajaBanco.CB_A_REFERD = read[15].ToString();
                    cajaBanco.CB_C_CUENT = read[16].ToString();
                    cajaBanco.CB_C_CENCO = read[17].ToString();
                    cajaBanco.CB_C_DESTI = read[18].ToString();
                    cajaBanco.CB_N_MTOMND = Conversion.ParseDecimal(read[19].ToString());
                    cajaBanco.CB_N_MTOMED = Conversion.ParseDecimal(read[20].ToString());
                    cajaBanco.CB_L_ANULA = Conversion.ParseBool(read[21].ToString());
                    cajaBanco.CB_L_PROGR = Conversion.ParseBool(read[22].ToString());
                    cajaBanco.CODDETPLA = read[23].ToString();
                    cajaBanco.CB_L_INT = Conversion.ParseBool(read[24].ToString());
                    cajaBanco.CB_ACUENTA = read[25].ToString();
                    cajaBanco.monedaD = moneda;
                    if (tipo == "I")
                    {
                        cajaBanco.IS = ternarioG(cajaBanco.CB_C_MODO == "I", 0, 1);
                    }
                    else
                    {
                        cajaBanco.IS = ternarioG(cajaBanco.CB_C_MODO == "I", 1, 0);
                    }

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
       
        /*
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
       
        */
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
                    cajaBanco.CB_C_CODIG = read[0].ToString().Trim();
                    cajaBanco.CB_A_DESCR = read[1].ToString();
                    cajaBanco.CB_C_CUENT = read[2].ToString().Trim();
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

            string mes = fechaoperacion.Month.ToString("00.##");
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

        public decimal tipoCambio(string cambio, string CC1, decimal CE, DateTime FE, DateTime FR, DateTime dateTime)
        {
            List<TipoCambio> tipoCambio = new List<TipoCambio>();

            int anio = dateTime.Year;
            bool ok = true;
            string findAll = $" SELECT * FROM TIPO_CAMBIO WHERE TIPOMON_CODIGO = 'ME' AND YEAR(TIPOCAMB_FECHA)= {anio}  ORDER BY TIPOCAMB_FECHA";
            decimal valor = 0;
            try
            {

                comando = new SqlCommand(conexionContabilidad(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    TipoCambio tipoCambio1 = new TipoCambio();
                    tipoCambio1.TIPOMON_CODIGO = read[0].ToString();
                    tipoCambio1.TIPOCAMB_FECHA = Conversion.ParseDateTime(read[1].ToString());
                    tipoCambio1.TIPOCAMB_VENTA = Conversion.ParseDecimal(read[2].ToString());
                    tipoCambio1.TIPOCAMB_COMPRA = Conversion.ParseDecimal(read[4].ToString());
                    tipoCambio.Add(tipoCambio1);
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

            switch (CC1)
            {
                case "ESP":
                    if (cambio == "MN")
                        if (CE != 0)
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
                    TipoCambio date = tipoCambio.Find(x => x.TIPOCAMB_FECHA.Date == FE.Date);

                    if (date != null)
                    {
                        if (cambio == "ME")
                        {
                            valor = date.TIPOCAMB_COMPRA;
                        }
                        else
                        {
                            valor = Math.Round(1 / date.TIPOCAMB_COMPRA, 8);
                        }
                    }
                    else
                        ok = false;
                    break;
                case "COM":
                case "VTA":
                    bool flagv = false;

                    TipoCambio date2 = tipoCambio.Find(x => x.TIPOCAMB_FECHA.Date == FR.Date);

                    DateTime datev = date2.TIPOCAMB_FECHA;
                    decimal compra = date2.TIPOCAMB_COMPRA;
                    decimal venta = date2.TIPOCAMB_VENTA;

                    if (datev.Date == FR.Date)
                        flagv = true;
                    if (flagv)
                    {
                        if (cambio == "ME")
                        {

                            valor = ternarioG(CC1 == "COM", compra, venta);
                        }
                        else
                        {
                            compra = Math.Round(1 / compra, 8);
                            venta = Math.Round(1 / venta, 8);
                            valor = ternarioG(CC1 == "COM", compra, venta);
                        }
                    }
                    else
                        ok = false;
                    break;

            }

            if (!ok)
            {
                valor = 999999;// error
            }
            return valor;
        }
        public void create(CMovimientoBanco obj, string codigoBanco, DateTime dateTime)
        {

            string mes = dateTime.Month.ToString("00.##");
            string create = "INSERT INTO CMOV_BANCO  ([CB_C_BANCO],[CB_C_MES],[CB_C_SECUE],[CB_C_MODO],[CB_C_OPERA],[CB_D_FECHA]";
            create += ",[CB_C_TPDOC] ,[CB_C_DOCUM] ,[CB_C_ANEXO]  ,[CB_C_CONVE] ,[CB_N_CAMES] ,[CB_N_TIPCA],[CB_N_MTOMN]  ,[CB_N_MTOME] ,[CB_C_CONTA]";
            create += ",[CB_A_REFER]  ,[CB_C_ESTAD] ,[CB_D_FECCOB],[CB_TIPMOV] ,[CB_MEDIO] ,[CB_DMEDIO] ,[CB_USUARIO])";
            create += $" VALUES('{codigoBanco}', '{mes}','{obj.CB_C_SECUE}','{obj.CB_C_MODO}'";
            create += $",'{obj.CB_C_OPERA}',{this.dateFormat(obj.CB_D_FECHA)},'{obj.CB_C_TPDOC}','{obj.serie}{obj.CB_C_DOCUM}'";
            create += $",'{obj.CB_C_ANEXO}','{obj.CB_C_CONVE}',{obj.CB_N_CAMES},{obj.CB_N_TIPCA}";
            create += $",{obj.CB_N_MTOMN},{obj.CB_N_MTOME},'{obj.CB_C_CONTA}' , '{obj.CB_A_REFER}','{obj.CB_C_ESTAD}'";
            create += $",{dateFormat(obj.CB_D_FECCOB)},'{obj.CB_TIPMOV}','{obj.CB_MEDIO}','{obj.CB_DMEDIO}'";
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
        public void Update(CMovimientoBanco obj, string codigoBanco, DateTime dateTime)
        {
            string update = "UPDATE CMOV_BANCO ";
            update += $"SET [CB_C_OPERA] = '{obj.CB_C_OPERA}', ";
            update += $"[CB_D_FECHA] = {dateFormat(obj.CB_D_FECHA)}, ";
            update += $"[CB_C_TPDOC] = '{obj.CB_C_TPDOC}', ";
            update += $"[CB_C_DOCUM] = '{obj.serie}{obj.CB_C_DOCUM}', ";
            update += $"[CB_C_ANEXO] = '{obj.TipoAnexoD}{obj.CB_C_ANEXO}', ";
            update += $"[CB_C_CONVE] = '{obj.CB_C_CONVE}', ";
            update += $"[CB_N_CAMES] = {obj.CB_N_CAMES}, ";
            update += $"[CB_N_TIPCA] = {obj.CB_N_TIPCA}, ";
            update += $"[CB_N_MTOMN] = {obj.CB_N_MTOMN}, ";
            update += $"[CB_N_MTOME] = {obj.CB_N_MTOME}, ";
            update += $"[CB_A_REFER] = '{obj.CB_A_REFER}', ";
            update += $"[CB_C_ESTAD] = '{obj.CB_C_ESTAD}', ";
            update += $"[CB_D_FECCOB] = {dateFormat(obj.CB_D_FECCOB)}, ";
            update += $"[CB_TIPMOV] = '{obj.CB_TIPMOV}', ";
            update += $"[CB_MEDIO] = '{obj.CB_MEDIO}', ";
            update += $"[CB_DMEDIO] = '{obj.CB_DMEDIO}', ";
            update += $"[CB_USUARIO] = '{obj.CB_USUARIO}' ";
            update += $"WHERE CB_C_BANCO = '{obj.CB_C_BANCO}' AND CB_C_MES = '{obj.CB_C_MES}' AND CB_C_SECUE = '{obj.CB_C_SECUE}'";
            try
            {


                comando = new SqlCommand(conexionBDCBT(update, dateTime.Year), objConexion.getCon());
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
        public void createCartera(CMovimientoBanco objC, DMovimientoBanco obj, string codigoBanco, DateTime dateTime, string tipoCambio, string CB_C_BANCO, string tipoIS, string moneda)

        {
            string nroDocumento = obj.serieD + " " + obj.CB_C_DOCUMD;

            decimal valorCambio = ternarioG(tipoCambio == "ESP", objC.CB_N_CAMES, objC.CB_N_TIPCA);
            decimal monto = ternarioG(moneda == "MN", obj.CB_N_MTOMND, obj.CB_N_MTOMED);
            nroDocumento = nroDocumento.Substring(0, 10);

            if (tipoIS == "S")
            {

            }


            string SQL = "Insert Into Cartera (CDOCODCLI,CDOTIPDOC,CDONRODOC,CDOFECDOC,";
            SQL += "CDOFECVEN,CDOCODVEN,CDOIMPORTE,CDOSALDO,CDOTIPMON,";
            SQL += "CDOTIPCAM,CDODEBHAB,CDOESTADO,CDOFECCRE,CDOFECACT,CDOUSUARI,";
            SQL += "CDOTIPFAC,CDOSALINI,CDOCUENTA,CDFORVEN,COD_BANCO,DES_BANCO) Values ";
            SQL += "('" + obj.CB_C_ANEXOD + "','" + obj.CB_C_TPDOCD + "','" + nroDocumento + "',";
            SQL += "" + dateFormat(obj.CB_D_FECDC) + "," + dateFormat(obj.CB_D_FECDC) + ",";
            SQL += "'01'," + -1 * monto + "," + -1 * monto + ",";
            SQL += "'" + obj.monedaD + "'," + valorCambio + ",'H','V'," + dateFormat(dateTime) + ",";

            SQL += "" + dateFormat(dateTime) + ",'','" + obj.CB_C_TPDOCD + "'," + -1 * monto + ",";
            SQL += "'" + obj.CB_C_CUENT + "','','" + CB_C_BANCO + "','" + codigoBanco + "')";

            try
            {
                comando = new SqlCommand(conexionComun(SQL), objConexion.getCon());
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

        public void createComprobante(DateTime dateTime)
        {

            int anio = dateTime.Year;
            string mes = dateTime.Month.ToString("00.##");

            
            string CORDEN = funcAutoNum(anio + mes);
            string Danexo= verdata("TIPOANEX_CODIGO='" + BuscaGen("ANEXOPROV") + "' AND ANEX_CODIGO='" + Trim(Text111(3)) + "'")


            SQL = "Insert Into COMPROBANTECAB (";
            SQL = SQL + "CORDEN,ANEX_CODIGO,ANEX_DESCRIPCION,TIPODOCU_CODIGO,CSERIE, ";
            SQL = SQL + "CNUMERO, DEMISION, DVENCE, DRECEPCIO, TIPOMON_CODIGO, ";
            SQL = SQL + "NIMPORTE, TIPOCAMBIO_VALOR, CDESCRIPC, RESPONSABLE_CODIGO, CESTADO, ";
            SQL = SQL + "NSALDO, CCODCONTA, CFORMPAGO, CSERREFER, CNUMREFER, ";
            SQL = SQL + "CTDREFER, CONVERSION_CODIGO, DREGISTRO, CTIPPROV, CNRORUC, ";
            SQL = SQL + "ESTCOMPRA, CDESTCOMP, DIASPAGO, CIGVAPLIC, CCONCEPT, ";
            SQL = SQL + "DFECREF, NTASAIGV, NIGV, NPORCE, CCODRUC, ";
            SQL = SQL + "LHONOR, NIR4, NIES, NTOTRH, NBASEIMP, ";
            SQL = SQL + "NVALCIF, DCONTAB, CAMESPROC, CSALDINI,LCANJEADO,LREGCO,CONTAB,LANULA,CINT_CAJ)";
            SQL = SQL + " Values ";
            SQL = SQL + "('" + Ver_Data_Cam("CAMESPROC='" + Format(VGFecTrb, "YYYYMM") + "'", "COMPROBANTECAB", cConexCtaPag, 1, "MAXORDEN", "REPLICATE('0',5-LEN(MAX(CORDEN)+1))+CAST(MAX(CORDEN)+1 AS VARCHAR(30)) AS MAXORDEN") + "','" + Trim(Text111(3)) + "','" + Ver_Data_Cam("TIPOANEX_CODIGO='" + Busca_Gen("ANEXOPROV") + "' AND ANEX_CODIGO='" + Trim(Text111(3)) + "'", "ANEXO", VGDB, 1, "ANEX_DESCRIPCION", "ANEX_DESCRIPCION") + "',";
            SQL = SQL + "'" + Text111(4) + "','" + Trim(Text111(5)) + Space(4 - Len(Trim(Text111(5)))) + "','" + Trim(Text111(6)) + "'," + IIf(MaskEdBox112(0) == "__/__/____", FechS(VGFecTrb, Sqlf), FechS(MaskEdBox112(0), Sqlf)) + ",";
            SQL = SQL + "" + IIf(MaskEdBox112(0) == "__/__/____", FechS(VGFecTrb, Sqlf), FechS(MaskEdBox112(0), Sqlf)) + "," + IIf(MaskEdBox112(0) == "__/__/____", FechS(VGFecTrb, Sqlf), FechS(MaskEdBox112(0), Sqlf)) + ",'" + Text111(7) + "',";
            SQL = SQL + "" + Text111(8) + "," + IIf(CBfrmMovBancoMes.Text2(10).Text == "ESP", CBfrmMovBancoMes.Text2(11).Text, CBfrmMovBancoMes.Text2(12).Text) + ",'" + Text111(10) + "',";
            SQL = SQL + "'01','1'," + Text111(8) + ",'" + Ver_Data_Cam("CB_C_CODIG='" + Text111(1) + "' and CB_C_MODO='" + SINGSAL + "'", "CONCEPTO_CAJA_BANCO", VGDBC, 1, "CB_C_CUENT", "CB_C_CUENT") + "',";
            SQL = SQL + "' ','','','','ESP'," + IIf(MaskEdBox112(0) == "__/__/____", FechS(VGFecTrb, Sqlf), FechS(MaskEdBox112(0), Sqlf)) + ",'" + Busca_Gen("ANEXOPROV") + "','" + Ver_Data_Cam("TIPOANEX_CODIGO='" + Busca_Gen("ANEXOPROV") + "' AND ANEX_CODIGO='" + Trim(Text111(3)) + "'", "ANEXO", VGDB, 1, "ANEX_RUC", "ANEX_RUC") + "',";
            SQL = SQL + "0,'001','','N','" + Ver_Data_Cam("CB_C_CODIG='" + Text111(1) + "' and CB_C_MODO='" + SINGSAL + "'", "CONCEPTO_CAJA_BANCO INNER JOIN [" + VGEMP_CODIGO + "BDCTAPAG].dbo.GASTOS ON CB_C_CUENT=Gastos_CuentaCon", VGDBC, 1, "Gastos_Codigo", "Gastos_Codigo") + "',";
            SQL = SQL + "" + IIf(MaskEdBox112(0) == "__/__/____", FechS(VGFecTrb, Sqlf), FechS(MaskEdBox112(0), Sqlf)) + ",0,0,0,'',0,0,0,0,0,";
            SQL = SQL + "0," + IIf(MaskEdBox112(0) == "__/__/____", FechS(VGFecTrb, Sqlf), FechS(MaskEdBox112(0), Sqlf)) + ",'" + Format(VGFecTrb, "YYYYMM") + "',0,0,0,0,0,1)";
            cConexCtaPag.Execute(SQL);
        }

        public void updateCartera(CMovimientoBanco objC, DMovimientoBanco obj, string codigoBanco, DateTime dateTime, string tipoCambio, string CB_C_BANCO, string tipoIS, string moneda)

        {
            string nroDocumento = obj.serieD + " " + obj.CB_C_DOCUMD;

            decimal valorCambio = ternarioG(tipoCambio == "ESP", objC.CB_N_CAMES, objC.CB_N_TIPCA);
            decimal monto = ternarioG(moneda == "MN", obj.CB_N_MTOMND, obj.CB_N_MTOMED);
            nroDocumento = nroDocumento.Substring(0, 10);

            if (tipoIS == "S")
            {

            }

            string SQL = "UPDATE CARTERA SET CDOCODCLI='" + obj.CB_C_ANEXOD + "',";
            SQL += "CDOTIPDOC='" + obj.CB_C_TPDOCD + "',";
            SQL += "CDONRODOC='" + nroDocumento + " ',";
            SQL += "CDOFECDOC=" + dateFormat(obj.CB_D_FECDC) + ",";
            SQL += "CDOFECVEN=" + dateFormat(obj.CB_D_FECDC) + ",";
            SQL += "CDOTIPMON='" + moneda + "',";
            SQL += "CDOIMPORTE=" + - 1 * monto +",";
            SQL += "CDOSALDO=" + -1 * monto + ",";
            SQL += "COD_BANCO='" + CB_C_BANCO + "',DES_BANCO='" + codigoBanco + "'";
            SQL += ",CDOSALINI=" + -1 * monto;
            SQL += " WHERE CDONRODOC='" + nroDocumento + "' AND CDOTIPDOC='";
            SQL += obj.CB_C_TPDOCD + "' AND CDOCODCLI='" + obj.CB_C_ANEXOD + "'";



            try
            {
                comando = new SqlCommand(conexionComun(SQL), objConexion.getCon());
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




        public void crearteDetail(CMovimientoBanco objC, DMovimientoBanco obj, string codigoBanco, DateTime dateTime, string moneda, decimal valortipoCambio)
        {
            string mes = dateTime.Month.ToString("00.##");
            decimal monto = 0;
            if (obj.monedaD=="MN")
            {
                monto = obj.CB_N_MTOMND;
            }
            else
            {
                monto = obj.CB_N_MTOMED;
            }
            if (moneda==obj.monedaD)
            {
                if (obj.monedaD=="MN")
                {
                    obj.CB_N_MTOMND = monto;
                    obj.CB_N_MTOMED = monto*valortipoCambio;

                }
                else
                {
                    obj.CB_N_MTOMND = monto * valortipoCambio;
                    obj.CB_N_MTOMED = monto ;
                }
            }
            else
            {
                if (obj.monedaD == "MN")
                {
                    obj.CB_N_MTOMND = monto * valortipoCambio;
                    obj.CB_N_MTOMED = monto ;

                }
                else
                {
                    obj.CB_N_MTOMND = monto;
                    obj.CB_N_MTOMED = monto * valortipoCambio;
                }
            }
            if (obj.serieD.Length < 4)
            {

                obj.serieD = rellenar(obj.serieD, 4, obj.serieD.Length, " ", false);
            }
            string create = "INSERT INTO DMOV_BANCO  ([CB_C_BANCO],[CB_C_MES],[CB_C_SECDE],[CB_C_SECUE],[CB_C_MODO],[CB_C_CONCE],[CB_C_ANEXO]";
            create += ",[CB_C_TPDOC] ,[CB_C_DOCUM] ,[CB_D_FECDC]  ,[CB_A_REFER] ,[CB_C_CUENT] ,[CB_C_DESTI],[CB_N_MTOMN]  ,[CB_N_MTOME],CB_C_CENCO  )";
            create += $" VALUES('{codigoBanco}', '{mes}','{obj.CB_C_SECDE}','{objC.CB_C_SECUE}','{obj.CB_C_MODO}'";
            create += $",'{obj.CB_C_CONCE}','{obj.TipoAnexo}{obj.CB_C_ANEXOD}','{obj.CB_C_TPDOCD}','{obj.serieD} {obj.CB_C_DOCUMD}'";
            create += $",{dateFormat( obj.CB_D_FECDC)},'{obj.CB_A_REFERD}','{obj.CB_C_CUENT}','{obj.CB_C_DESTI}'";
            create += $",{obj.CB_N_MTOMND},{obj.CB_N_MTOMED},'{obj.CB_C_CENCO}')";
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


        public void UpdateDetail(CMovimientoBanco objC, DMovimientoBanco obj, string codigoBanco, DateTime dateTime, string moneda, decimal valortipoCambio)
        {
            string mes = dateTime.Month.ToString("00.##");
            decimal monto = 0;
            if (moneda == "MN")
            {
                monto = obj.CB_N_MTOMND;
            }
            else
            {
                monto = obj.CB_N_MTOMED;
            }

            if (moneda == "MN")
            {
                obj.CB_N_MTOMND = monto;
                obj.CB_N_MTOMED = monto * valortipoCambio;

            }
            else
            {
                obj.CB_N_MTOMND = monto * valortipoCambio;
                obj.CB_N_MTOMED = monto;
            }
            /*
            if (moneda == obj.monedaD)
            {
                if (obj.monedaD == "MN")
                {
                    obj.CB_N_MTOMND = monto;
                    obj.CB_N_MTOMED = monto * valortipoCambio;

                }
                else
                {
                    obj.CB_N_MTOMND = monto * valortipoCambio;
                    obj.CB_N_MTOMED = monto;
                }
            }
            else
            {
                if (obj.monedaD == "MN")
                {
                    obj.CB_N_MTOMND = monto * valortipoCambio;
                    obj.CB_N_MTOMED = monto;

                }
                else
                {
                    obj.CB_N_MTOMND = monto;
                    obj.CB_N_MTOMED = monto * valortipoCambio;
                }
            }
            */
            if (obj.serieD.Length < 4)
            {

                obj.serieD = rellenar(obj.serieD, 4, obj.serieD.Length, " ",false);
            }
            string create = "UPDATE  DMOV_BANCO SET";
            create += $"[CB_C_BANCO]= '{codigoBanco}',";
            create += $"[CB_C_MES]= '{mes}',";    
            create += $"[CB_C_SECUE]= '{objC.CB_C_SECUE}',";
            create += $"[CB_C_MODO]= '{obj.CB_C_MODO}',";
            create += $"[CB_C_CONCE]= '{obj.CB_C_CONCE}',";
            create += $"[CB_C_ANEXO]= '{obj.TipoAnexo}{obj.CB_C_ANEXOD}',";
            create += $"[CB_C_TPDOC]= '{obj.CB_C_TPDOCD}',";
            create += $"[CB_C_DOCUM]= '{obj.serieD} {obj.CB_C_DOCUMD}',";
            create += $"[CB_D_FECDC]= {dateFormat(obj.CB_D_FECDC)},";
            create += $"[CB_A_REFER]= '{obj.CB_A_REFERD}',";
            create += $"[CB_C_CUENT]= '{obj.CB_C_CUENT}',";
            create += $"[CB_C_DESTI]= '{obj.CB_C_DESTI}',";
            create += $"[CB_N_MTOMN]= {obj.CB_N_MTOMND},";
            create += $"[CB_N_MTOME]= {obj.CB_N_MTOMED},";
            create += $"[CB_C_CENCO]= '{obj.CB_C_CENCO}'";
            create += $"where   [CB_C_SECDE]= '{obj.CB_C_SECDE}' and  [CB_C_SECUE]='{objC.CB_C_SECUE}' and [CB_C_BANCO]= '{codigoBanco}' and [CB_C_MES]= '{mes}'  ";
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
        public List<Cobranzas> AllConbranzas(DateTime dateTime)
        {
            string ValFiltro = "Month(COCFECPLA)=" + dateTime.Month + " and year(COCFECPLA)=" + dateTime.Year + "";
            List<Cobranzas> tipoMonedas = new List<Cobranzas>();
            string findAll = "";
            findAll += "Select distinct COCNROPLA, COCFECPLA, COCTCOBMN, COCTCOBUS ";
            findAll += "From (Plan_Cob_Cab INNER JOIN Plan_Cob_Det ON COCNROPLA=DEPNROPLA AND COCFECPLA=DEPFECCOB)";
            findAll += "INNER JOIN Tipo_Cobranza ON Plan_Cob_Det.DEPCONCEP=Tipo_Cobranza.COD_COBRANZA ";
            findAll += "Where " + ValFiltro + " AND Tipo_Cobranza.TIP in (1,3) ORDER BY COCNROPLA, COCFECPLA";
            try
            {
                comando = new SqlCommand(conexionComun(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Cobranzas cajaBanco = new Cobranzas();
                    cajaBanco.COCNROPLA = read[0].ToString();
                    cajaBanco.COCFECPLA = Conversion.ParseDateTime(read[1].ToString());
                    cajaBanco.COCTCOBMN = Conversion.ParseDecimal(read[2].ToString());
                    cajaBanco.COCTCOBUS = read[3].ToString();
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


      
       

        public void delete(Area obj)
        {
            string delete = "delete from AREA where AREA_CODIGO='" + obj.AREA_CODIGO + "'";
            try
            {
                comando = new SqlCommand(conexionComun(delete), objConexion.getCon());
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
        public void deleteMovimientoBancoDetalle(string codigobanco , DateTime dateTime, string secuencia )
        {
            string mes = dateTime.Month.ToString("00.##");

            string delete = "DELETE FROM DMOV_BANCO WHERE CB_C_BANCO = '" + codigobanco + "' AND  CB_C_MES='" + mes + "' AND CB_C_SECUE ='" + secuencia + "'";
            try
            {
                comando = new SqlCommand(conexionBDCBT(delete, dateTime.Year), objConexion.getCon());
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
        public void deleteMovimientoBancoDetalleEspecifico(string codigobanco, DateTime dateTime, string secuencia, string  secuenciaD)
        {
            string mes = dateTime.Month.ToString("00.##");

            string delete = "DELETE FROM DMOV_BANCO WHERE CB_C_BANCO = '" + codigobanco + "' AND  CB_C_MES='" + mes + "' AND CB_C_SECUE ='" + secuencia + "'"  + " AND CB_C_SECDE='" + secuenciaD + "'"
;
            try
            {
                comando = new SqlCommand(conexionBDCBT(delete, dateTime.Year), objConexion.getCon());
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
        public void deleteMovimientoBanco(string codigobanco, DateTime dateTime, string secuencia)
        {
            string mes = dateTime.Month.ToString("00.##");

            string delete = "DELETE FROM CMOV_BANCO WHERE CB_C_BANCO = '" + codigobanco + "' AND  CB_C_MES='" + mes + "' AND CB_C_SECUE ='" + secuencia + "'";
            try
            {
                comando = new SqlCommand(conexionBDCBT(delete, dateTime.Year), objConexion.getCon());
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


        #region
        protected List<Planillas> AllPlantilas(string nroPlantilla)
        {
            
            List<Planillas> tipoMonedas = new List<Planillas>();
            string findAll = "";
            findAll += "SELECT * FROM Plan_Cob_Det INNER JOIN Tipo_Cobranza ON Plan_Cob_Det.DEPCONCEP=Tipo_Cobranza.COD_COBRANZA ";
            findAll += "Where DEPNROPLA = '" + nroPlantilla + "' "+ " And F_CJABCO=0 And Tipo_Cobranza.TIP in (1,3) ORDER BY DEPRFNUMDOC;";
            if (nroPlantilla != "")
            {
                try
                {
                comando = new SqlCommand(conexionComun(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Planillas planilla  = new Planillas();
                    planilla.DEPNROPLA  = read["DEPNROPLA"].ToString();
                    planilla.DEPSECUENC = read["DEPSECUENC"].ToString();
                    planilla.DEPTIPDOC  = read["DEPTIPDOC"].ToString();
                    planilla.DEPNRODOC  = read["DEPNRODOC"].ToString();
                    planilla.DEPTIPOPER = read["DEPTIPOPER"].ToString();
                    planilla.DEPCONCEP  = read["DEPCONCEP"].ToString();
                    planilla.DEPFECCOB  = Conversion.ParseDateTime( read["DEPFECCOB"].ToString());
                    planilla.DEPIMPORTE = Conversion.ParseDecimal(read["DEPIMPORTE"].ToString());
                    planilla.DEPTIPMON = read["DEPTIPMON"].ToString();
                    planilla.DEPTIPCAM = Conversion.ParseDecimal(read["DEPTIPCAM"].ToString());
                    planilla.DEPFECCRE = Conversion.ParseDateTime(read["DEPFECCRE"].ToString());
                    planilla.DEPUSUARI = read["DEPUSUARI"].ToString();
                    planilla.DEPGLOSA = read["DEPGLOSA"].ToString();
                    planilla.DEPCOBRA = read["DEPCOBRA"].ToString();
                    planilla.DEPCODBAN = read["DEPCODBAN"].ToString();
                    planilla.DEPRFTIPDOC = read["DEPRFTIPDOC"].ToString();
                    planilla.DEPRFNUMDOC = read["DEPRFNUMDOC"].ToString();
                    planilla.CODDETPLA = Conversion.Parseint(read["CODDETPLA"].ToString());
                    planilla.F_CJABCO = read["F_CJABCO"].ToString();
                    planilla.DEPIMPORTEPERC = Conversion.ParseDecimal( read["DEPIMPORTEPERC"].ToString());
                    planilla.DEPBCOGIR = read["DEPBCOGIR"].ToString();
                    planilla.DEPCTABANCH = read["DEPCTABANCH"].ToString();
                    planilla.DEPPERAUTO = read["DEPPERAUTO"].ToString();
                    planilla.FlgPercepcion = read["FlgPercepcion"].ToString();
                    planilla.FECPAGDOC = Conversion.ParseDateTime(read["FECPAGDOC"].ToString());
                    planilla.ImpDeposito = Conversion.ParseDecimal(read["ImpDeposito"].ToString());

                    planilla.TIPO = read["TIPO"].ToString();
                    planilla.COD_COBRANZA = read["COD_COBRANZA"].ToString();
                    planilla.DESCRIPCION = read["DESCRIPCION"].ToString();
                    planilla.MONEDA = read["MONEDA"].ToString();
                    planilla.CUENTA = read["CUENTA"].ToString();
                    planilla.ANEX_PROV = Conversion.ParseBool( read["ANEX_PROV"].ToString());
                    planilla.BANCOS = read["BANCOS"].ToString();
                    planilla.USUARIO = read["USUARIO"].ToString();
                    planilla.FECHA = Conversion.ParseDateTime( read["FECHA"].ToString());
                    planilla.FECACT = Conversion.ParseDateTime( read["FECACT"].ToString());
                    planilla.CHEQ_DIFER = Conversion.ParseBool(read["CHEQ_DIFER"].ToString());
                    planilla.APLIC_DOC = Conversion.ParseBool(read["APLIC_DOC"].ToString());
                    planilla.TIP = Conversion.Parseint(read["TIP"].ToString());
                    planilla.TARJCREDITO = read["TARJCREDITO"].ToString();




                    tipoMonedas.Add(planilla);
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
            
            return tipoMonedas;
        }
        protected string codigoCliente(string DEPTIPDOC, string CDONRODOC)
        {

            string codCliente = "";
            string findAll = "";
            findAll += "Select CDOCODCLI From Cartera Where CDOTIPDOC= '" + DEPTIPDOC + "' And CDONRODOC='" + CDONRODOC + "'";

            try
            {
                comando = new SqlCommand(conexionComun(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    codCliente = read["CDOCODCLI"].ToString();
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
            return codCliente;
        }

        public List<PlantillaDetalle> AllPlanillasDetalle(string nroPlantilla)
        {
            List<PlantillaDetalle> plantillaDetalles = new List<PlantillaDetalle>();

            List<Planillas> planillas = AllPlantilas(nroPlantilla);
            int sec = 0;
            planillas.ForEach(X =>
            {
                sec++;
                PlantillaDetalle plantillaDetalle = new PlantillaDetalle();
                plantillaDetalle.Sec = sec.ToString("000.##");
                string codigo = codigoCliente(X.DEPTIPDOC, X.DEPNRODOC);
                plantillaDetalle.Cliente = codigo;
                plantillaDetalle.TpoDoc = X.DEPTIPDOC;
                int    LONGCAMPO = X.DEPNRODOC.Length;
                string nroDoc = "";
                string texto = X.DEPNRODOC;
                if (X.DEPNRODOC.Substring(0, 1) == "F")
                {
                    nroDoc = texto.Substring(0, 4) + " " + texto.Substring(4);
                }
                else
                {
                    nroDoc = texto.Substring(0, 3) + " " + texto.Substring(4);
                }

                plantillaDetalle.Documento = nroDoc;
                plantillaDetalle.NroOP = X.DEPRFNUMDOC;
                plantillaDetalle.Banco = X.DEPDESBAN;
                plantillaDetalle.Moneda = X.DEPTIPMON;
                if (plantillaDetalle.Moneda == "MN")
                {

                }
                else
                {

                }
                plantillaDetalle.Importe = X.DEPIMPORTE;
                plantillaDetalle.DetKey = X.CODDETPLA;
                plantillaDetalles.Add(plantillaDetalle);


            });

            return plantillaDetalles;
        }


        public ParametrosCuentas findParametrosCuentas(string TIPO_FAC)
        {

            ParametrosCuentas parametros = new ParametrosCuentas();
            string findAll = "";
            findAll += "Select CTA_SOLES, CTA_DOLA From PARAMETROS_CTAS Where TIPO_FAC='" + TIPO_FAC+ "'";
            
               try 
                {
                comando = new SqlCommand(conexionComun(findAll), objConexion.getCon());
                    objConexion.getCon().Open();
                    SqlDataReader read = comando.ExecuteReader();
                    if (read.Read())
                    {                     
                        parametros.CTA_SOLES = read["CTA_SOLES"].ToString();
                        parametros.CTA_DOLA = read["CTA_DOLA"].ToString();                  
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
            

            return parametros;
        }


        public Cartera findCartera(string cliente, string tipdoc,string numDoc)
        {

            Cartera parametros = new Cartera();

            string nroDoc = ternario(numDoc.Trim() == "", numDoc, numDoc.Substring(0, 3) + numDoc.Substring(4));

            string findAll = "";
            findAll += "SELECT CDOTIPMON,CDOFECDOC FROM CARTERA WHERE CDOCODCLI='" + cliente + "' AND ";
            findAll +=  "CDOTIPDOC='" + tipdoc + "' AND CDONRODOC='"+nroDoc+ "'";

            try
            {
                comando = new SqlCommand(conexionComun(findAll), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    parametros.CDOTIPMON = read["CDOTIPMON"].ToString();
                    parametros.CDOFECDOC = Conversion.ParseDateTime( read["CDOFECDOC"].ToString());
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


            return parametros;
        }

        public void crearteDetailxPlantilla(PlantillaDetalle objC,int sec, string seccab,  string codigoBanco, DateTime dateTime, string anexocliente, DateTime CDOFECDOC ,string refer,string cuenta ,   string moneda, decimal valortipoCambio)
        {

            if (valortipoCambio == 0)
            {
                valortipoCambio = 1;
            }
            decimal monmn = ternarioG(moneda == "MN", objC.Importe,objC.Importe* valortipoCambio);
            decimal monme = ternarioG(moneda == "ME", objC.Importe,objC.Importe/ valortipoCambio);
            string mes = dateTime.Month.ToString("00.##");
            string secd = sec.ToString("0000.##");
            string ValCpto = "999";
            string create = "INSERT INTO DMOV_BANCO  ([CB_C_BANCO],[CB_C_MES],[CB_C_SECDE],[CB_C_SECUE],[CB_C_MODO],[CB_C_CONCE],[CB_C_ANEXO]";
            create += ",[CB_C_TPDOC] ,[CB_C_DOCUM] ,[CB_D_FECDC]  ,[CB_A_REFER] ,[CB_C_CUENT] ,[CB_N_MTOMN]  ,[CB_N_MTOME]  )";
            create += $" VALUES('{codigoBanco}', '{mes}','{secd}','{seccab}','I'";
            create += $",'{ValCpto}','{anexocliente}{objC.Cliente}','{objC.TpoDoc}','{objC.Documento}'";
            create += $",{dateFormat(CDOFECDOC)},'{refer}','{cuenta}'";
            create += $",{monmn},{monme})";
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

        public void UpdatePlantconDet(string keyPlan)
        {
           

            string create = "UPDATE  Plan_Cob_Det  ";
            create += $"SET [F_CJABCO]= 1 ";
         
            create += $"where   [CODDETPLA]= {keyPlan}  ";
            try
            {
                comando = new SqlCommand(conexionComun(create), objConexion.getCon());
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
        public void UpdateMontosMbanco(DateTime dateTime, string codigobanco, string sec)
        {
            Montos montos = calcularMonto(sec, codigobanco, dateTime);
            updateMontos(montos, dateTime, codigobanco, sec);
        }
        public void updateMontos(Montos montos, DateTime dateTime, string  codigobanco, string sec )
        {

            string mes = dateTime.Month.ToString("00.##");
            string create = "UPDATE CMOV_BANCO SET CB_N_MtoMN = " + montos.SUMTOTALMN + ",CB_N_MtoME = " + montos.SUMTOTALME + " WHERE CB_C_Banco = '" + codigobanco + "' AND  CB_C_Mes='" + mes + "' AND CB_C_SECUE='" + sec + "'";
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

        public Montos calcularMonto(string  sec, string codigoBanco, DateTime dateTime)
        {
            string mes = dateTime.Month.ToString("00.##");
            Montos parametros = new Montos();
            string findAll = "";
            findAll += "SELECT SUM(CB_N_MTOMN),SUM(CB_N_MTOME) FROM DMOV_BANCO WHERE CB_C_Banco = '" + codigoBanco + "' AND  CB_C_Mes='" + mes + "' AND CB_C_Secue ='" + sec+ "'";

            try
            {
                comando = new SqlCommand(conexionBDCBT(findAll, dateTime.Year), objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    parametros.SUMTOTALMN = Conversion.ParseDecimal(  read[0].ToString());
                    parametros.SUMTOTALME = Conversion.ParseDecimal(read[1].ToString());
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


            return parametros;
        }
        #endregion

    }
}