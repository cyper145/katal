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


        public List<MovimientoBanco> findAllMovimientos(string banco, string moneda )
        {

            List<MovimientoBanco> cajaBancos = new List<MovimientoBanco>();
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
                new SqlParameter("@MONEDA", anios),
              
                };

                comando = new SqlCommand(findAll, objConexion.getCon());
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.AddRange(parametros);
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {

                    MovimientoBanco cajaBanco   = new MovimientoBanco();
                    cajaBanco.CB_C_Secue        = read[0].ToString();
                    cajaBanco.Opera             = read[1].ToString();
                    cajaBanco.docu              = read[2].ToString();
                    cajaBanco.MONTO             = Conversion.ParseDecimal( read[3].ToString());
                    cajaBanco.Conta             = read[4].ToString();
                    cajaBanco.Anula             = read[5].ToString();
                    cajaBanco.CB_C_Fecha        = Conversion.ParseDateTime( read[6].ToString());
                    cajaBanco.CB_C_Anexo        = read[7].ToString();
                    cajaBanco.CB_C_CONTA        = read[8].ToString();
                    cajaBanco.CB_A_REFER        = read[9].ToString();
                    cajaBanco.CB_C_NROLI        = read[10].ToString();


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


        public List<TipoOpcionCajaBanco> findAllTipoOpciones(string tipoIngresoSalida)
        {

            List<TipoOpcionCajaBanco> tiposcajaBancos = new List<TipoOpcionCajaBanco>();

            string findAll = $"Select CB_C_CODIG,CB_A_DESCR,CB_C_TPDOC,CB_C_FPAGO from TIPO_OP_CAJA_BANCO where CB_C_TIPO = 'B'  and CB_C_MODO='{tipoIngresoSalida}'";
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
    }
}