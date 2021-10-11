using katal.conexion.model.entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace katal.conexion.model.dao
{
    public class ComprobanteDetraccionDao : Obligatorio
    {

        public ComprobanteDetraccionDao(string codEmpresa) : base(codEmpresa)
        {
            objConexion = Conexion.saberEstado();
            ///014BDCOMUN
        }

        public List<ComprobanteDetraccion> findAll(DateRangePickerModel dateRange)
        {

            List<ComprobanteDetraccion> listAreas = new List<ComprobanteDetraccion>();
            DateTime DATE = DateTime.Now;
            string findAll = "Select * From ";
            findAll += "(SELECT anex_codigo, cnroruc, anex_descripcion, tipodocu_codigo, cserie, cnumero, demision, tipomon_codigo, nimporte, ntasadetraccion, detraccion, tipocambio_valor as tc, isnull(round(CASE tipomon_codigo WHEN 'MN' THEN detraccion ELSE detraccion * tipocambio_valor END, 0) - isnull(b.pagos, 0), 0) as saldo, cod_servdetracc, cod_tipooperacion, LESTADODETRACCION,RESTANTE";
            findAll += $" From[{this.CodEmpresa}BDCOMUN].dbo.comprobanteCab a left join(Select cb_c_anexo, cb_c_tpdoc, REPLACE(cb_c_docum, ' ', '') ";
            findAll += $"as cb_c_docum,CB_L_ESTADO,CB_N_RESTANTE,  sum(cb_n_mtomn) as pagos From [{this.CodEmpresa}BDCBT{DATE.Year}].dbo.dmov_banco Where cb_c_conce = '015' Group by cb_c_anexo, cb_c_tpdoc, cb_c_docum,CB_L_ESTADO,CB_N_RESTANTE)b On '03' + a.anex_codigo = b.cb_c_anexo and a.tipodocu_codigo = b.cb_c_tpdoc and a.cserie + a.cnumero = b.cb_c_docum Where ldetraccion = 1 and len(a.NUMRETRAC) = 0 and case tipomon_codigo When 'MN' then nimporte else nimporte* tipocambio_valor end > 700) as Det Where saldo> 0";
            findAll += $" and demision Between  {dateFormat(dateRange.Start)} and {dateFormat(dateRange.End)}";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                int i = 0;
                while (read.Read())
                {
                    i++;
                    ComprobanteDetraccion area = new ComprobanteDetraccion();
                    area.anex_codigo = read[0].ToString();
                    area.cnroruc = read[1].ToString();
                    area.anex_descripcion = read[2].ToString();
                    area.tipodocu_codigo = read[3].ToString();
                    area.cserie = read[4].ToString();
                    area.cnumero = read[5].ToString();
                    area.demision = Conversion.ParseDateTime(read[6].ToString());
                    area.tipomon_codigo = read[7].ToString();
                    area.nimporte = Conversion.ParseDecimal(read[8].ToString());
                    area.ntasadetraccion = Conversion.ParseDecimal(read[9].ToString());
                    area.detraccion = Conversion.ParseDecimal(read[10].ToString());
                    area.tipocambio_valor = Conversion.ParseDecimal(read[11].ToString());
                    area.saldo = Conversion.ParseDecimal(read[12].ToString());
                    area.cod_servdetracc = read[13].ToString();
                    area.cod_tipooperacion = read[14].ToString();
                    area.estado = read[15].ToString() == "" ? "Pendiente" : read[15].ToString();
                    area.restante = Conversion.ParseDecimal(read[16].ToString());
                    area.codigo = i;
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


        public void updateDetail(List<ComprobanteDetraccion> obj)
        {

            string item = "";
            obj.ForEach(element =>
            {

                // anex_codigo
                //cnumero
                //cserie//}
                // LESTADODETRACCION,RESTANTE
                //
                //tipodocu_codigo
                item += $"UPDATE COMPROBANTECAB SET LESTADODETRACCION = '{element.estado}',RESTANTE = {element.restante}  ";
                item += $"WHERE anex_codigo ='{element.anex_codigo}' and ";
                item += $" cserie ='{element.cserie}' and ";
                item += $" cnumero ='{element.cnumero}' and ";
                item += $" tipodocu_codigo ='{element.tipodocu_codigo}'\n";
            });

            try
            {
                comando = new SqlCommand(conexionComun(item), objConexion.getCon());
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
}