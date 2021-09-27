using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using katal.conexion.model.entity;

namespace katal.conexion.model.dao
{
    public class CajaBancoDao:Obligatorio
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


        public List<CMovimientoBanco> findAllMovimientos(string banco, string moneda )
        {

            List<CMovimientoBanco> cajaBancos = new List<CMovimientoBanco>();
            DateTime date = DateTime.Now;
           
            string anios = date.Year.ToString("0000.##");
            string mes = date.Month.ToString("00.##");
            string findAll = "SP_CBMOVCALCULAMTO";
            try
            {
                SqlParameter[] parametros= new SqlParameter[]{
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

                    CMovimientoBanco cajaBanco   = new CMovimientoBanco();
                    cajaBanco.CB_C_SECUE         = read[0].ToString();
                    cajaBanco.CB_C_OPERA         = read[1].ToString();
                    cajaBanco.CB_C_DOCUM         = read[2].ToString();
                    cajaBanco.CB_N_MTOMN         = Conversion.ParseDecimal( read[3].ToString());
                    cajaBanco.CB_L_CONTA         = read[4].ToString();
                    cajaBanco.CB_L_ANULA         = read[5].ToString();
                    cajaBanco.CB_D_FECCA         = Conversion.ParseDateTime( read[6].ToString());
                    cajaBanco.CB_C_ANEXO         = read[7].ToString();
                    cajaBanco.CB_C_CONTA         = read[8].ToString();
                    cajaBanco.CB_A_REFER         = read[9].ToString();
                    cajaBanco.CB_C_NROLI         = read[10].ToString();


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

        public List<DMovimientoBanco> findDetailMovimientos(string secuencia, string banco, string moneda, DateTime dateTime)
        {

            List<DMovimientoBanco> DetailMovimientos = new List<DMovimientoBanco>();
           
            int anios = dateTime.Year;
            string mes = dateTime.Month.ToString("00.##");
            
            string texto = ternario(moneda == "MN", "CB_N_MTOMN", "CB_N_MTOME");

            string findAll = $"SELECT CB_C_SecDE,CB_C_Conce+' ',Cb_C_TpDoc+LEFT(CB_C_Docum,21),{texto},CB_A_Refer  from DMOV_BANCO WHERE CB_C_Banco = '{banco}'";
                findAll+= $" AND  CB_C_Mes='{mes}' AND CB_C_Secue ='{secuencia}' ORDER BY CB_C_SECDE";
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
                        cajaBanco.CB_N_MTOMN = Conversion.ParseDecimal( read[3].ToString());
                    }
                    else
                    {
                        cajaBanco.CB_N_MTOME = Conversion.ParseDecimal(read[3].ToString());
                    }
                    cajaBanco.CB_A_REFER = read[4].ToString();

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



        public string Genera_Secuencia( string codigoBanco, DateTime fechaoperacion)
        {
            List<MedioPago> tiposcajaBancos = new List<MedioPago>();
            string mes = fechaoperacion.ToString("00.##");
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
                    nro =Conversion.Parseint(  read[0].ToString());
                    nro++;
                }

                secuencia= nro.ToString("0000.##");
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

    }
}