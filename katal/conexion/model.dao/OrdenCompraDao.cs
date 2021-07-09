using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using katal.conexion.model.entity;
using katal.Model;

namespace katal.conexion.model.dao
{
    public class OrdenCompraDao : Obligatorio<OrdenCompra>
    {
        private Conexion objConexion;
        private SqlCommand comando;


        List<OrdenCompra> OrdenCompras;
        public OrdenCompraDao()
        {
            if (OrdenCompras == null)
            {
                OrdenCompras = new List<OrdenCompra>();
            }

            ApplicationUser user = AuthHelper.GetLoggedInUserInfo();
            if (user != null)
            {
                objConexion = Conexion.saberEstado(user.codEmpresa + "BDCOMUN");
            }
        }
        public void create(OrdenCompra orden)
        {
            string insert = "INSERT INTO comovc(oc_cnumord, oc_principal, oc_dfecdoc, oc_ccodpro, oc_crazsoc, ";
            insert += "oc_cdirpro,oc_ccotiza,oc_ccodmon,oc_cforpag,oc_ntipcam,oc_dfecent,";
            insert += "oc_cobserv,oc_csolict,oc_centreg,oc_csitord,oc_nimport,oc_ndescue,";
            insert += "oc_nigv,oc_nventa,oc_dfecact,oc_chora,oc_cusuari,oc_cconver, oc_cfacnombre,";
            insert += "oc_cfacruc, oc_cfacdirec, oc_cdocref, oc_cnrodocref,oc_ordfab,OC_SOLICITA,OC_TIPO,oc_lote) VALUES ('";
            insert += orden.OC_CNUMORD + "','" + orden.OC_PRINCIPAL + "','" + parseDate(orden.OC_DFECDOC) + "','" + orden.oc_ccodpro + "','";
            insert += orden.OC_CRAZSOC + "','" + orden.DIRECCION;
            insert += "','" + orden.OC_CCOTIZA + "','" + orden.OC_CCODMON + "','" + orden.OC_CFORPAG + "',";
            insert += orden.OC_NTIPCAM + ",'" + parseDate(orden.OC_DFECENT) + "','";
            insert += orden.OC_COBSERV + "','" + orden.OC_CSOLICT + "','" + orden.OC_CENTREG + "','00',";
            insert += orden.OC_NIMPORT + "," + orden.OC_NDESCUE + "," + orden.OC_NIGV + "," + orden.OC_NVENTA;
            insert += ",'" + orden.OC_CENTREG + "','" + DateTime.Now.ToString("hh:mm:ss") + "','" + AuthHelper.GetLoggedInUserInfo().UserName;
            insert += "','" + orden.OC_CCONVER + "', '" + orden.OC_CFACNOMBRE + "', '" + orden.OC_CFACRUC + "', '" + orden.OC_CFACDIREC;
            insert += "','" + orden.OC_CDOCREF + "', '" + orden.OC_CNRODOCREF + "','" + orden.OC_ORDFAB + "','" + orden.OC_SOLICITA + "','" + orden.OC_TIPO + "','" + orden.OC_LOTE + "')";

            try
            {
                comando = new SqlCommand(insert, objConexion.getCon());
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
        public void createDetail(OrdenCompra orden)
        {
            string item = "";
            string fmt = "000.##";
            int nextDocumet = 0;
            string next = nextDocumet.ToString(fmt);
            // INSERTAR ELEMENTO 
            orden.detalles.ForEach(element => {
                ++nextDocumet;
                next = nextDocumet.ToString(fmt);
                item += "INSERT INTO comovd (oc_cnumord,oc_ccodpro,oc_dfecdoc,oc_citem,";
                item += "oc_ccodigo,oc_ccodref,oc_cdesref,oc_cunidad,oc_cuniref,oc_nfactor,";
                item += "oc_saldo,oc_ncantid,oc_npreuni,oc_ndscpor,oc_ndescto,oc_nigv,oc_nigvpor,";
                item += "oc_nprenet,oc_ntotven,oc_ntotnet,oc_cestado,centcost,oc_ccomen1,oc_ccomen2, oc_glosa, oc_precioven) ";
                item += "VALUES ('" + orden.OC_CNUMORD + "','" + orden.oc_ccodpro + "','" + parseDate( orden.OC_DFECDOC);
                item += "','" + next + "','";
                item += element.oc_ccodigo + "','" + element.OC_CCODREF + "','";
                item += element.OC_CDESREF + "','" + element.OC_CUNIDAD + "','";
                item += element.OC_CUNIREF + "'," + element.OC_NFACTOR + "," + element.OC_SALDO + "," + element.OC_NCANTID + ",";
                item += element.OC_NPREUNI + "," + element.OC_NDSCPOR + "," + element.OC_NDESCTO + "," + element.OC_NIGV + ",";
                item += element.OC_NIGVPOR + "," + element.OC_NPRENET + "," + element.OC_NTOTVEN + "," + element.OC_NTOTNET;
                item += ",'00','" + element.CENTCOST + "','";
                item += element.OC_CCOMEN1 + "','" + element.OC_CCOMEN2 + "','" + element.OC_GLOSA + "'," + element.OC_PRECIOVEN + ")\n";
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

        public void DeleteDetail(string NROREQUI)
        {
            string SQLd = "DELETE FROM comovd WHERE oc_cnumord='" + NROREQUI + "'";

            try
            {
                comando = new SqlCommand(SQLd, objConexion.getCon());
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
        public void delete(OrdenCompra obj)
        {
            string strsql = "DELETE FROM COMOVC WHERE OC_CNUMORD='" + obj.OC_CNUMORD + "'";
            try
            {
                comando = new SqlCommand(strsql, objConexion.getCon());
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

        public bool find(OrdenCompra obj)
        {
            throw new NotImplementedException();
        }
        public OrdenCompra find(string codigo)
        {
            if (codigo == "-1")
            {
                return null;
            }
            string findAll = "SELECT TOP (100) * FROM comovc, estado_oc WHERE comovc.oc_csitord = estado_oc.est_codigo and  comovc.OC_CNUMORD='" + codigo + "'";

            OrdenCompra user = new OrdenCompra();
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {

                    user.OC_CNUMORD = read[0].ToString();
                    user.OC_PRINCIPAL = read[1].ToString();
                    user.OC_DFECDOC = DateTime.Parse(read[2].ToString());
                    user.OC_LOTE = read[3].ToString();
                    user.oc_ccodpro = read[4].ToString();
                    user.OC_CRAZSOC = read[5].ToString();
                    user.OC_CDIRPRO = read[6].ToString();
                    user.OC_CCOTIZA = read[7].ToString();
                    user.OC_CCODMON = read[8].ToString();
                    user.OC_CFORPAG = read[9].ToString();
                    user.OC_NTIPCAM = read[10].ToString();
                    user.OC_DFECENT = DateTime.Parse(read[11].ToString());
                    user.OC_COBSERV = read[12].ToString();
                    user.OC_CSOLICT = read[13].ToString();
                    user.OC_CTIPENV = read[14].ToString();
                    user.OC_CENTREG = read[15].ToString();
                    user.OC_CSITORD = read[16].ToString();
                    user.OC_NIMPORT = ParseDecimal(read[17].ToString());
                    user.OC_NDESCUE = ParseDecimal(read[18].ToString());
                    user.OC_NIGV = ParseDecimal(read[19].ToString());
                    user.OC_NVENTA = ParseDecimal(read[20].ToString());
                    user.OC_DFECACT = DateTime.Parse(read[21].ToString());
                    user.OC_CHORA = read[22].ToString();
                    user.OC_CUSUARI = read[23].ToString();
                    user.OC_CFECDOC = read[24].ToString();
                    user.OC_CCONVER = read[25].ToString();
                    user.OC_CFACNOMBRE = read[26].ToString();
                    user.OC_CFACRUC = read[27].ToString();
                    user.OC_CFACDIREC = read[28].ToString();
                    user.OC_CDOCREF = read[29].ToString();
                    user.OC_CNRODOCREF = read[30].ToString();
                    user.OC_ORDFAB = read[31].ToString();
                    user.OC_SOLICITA = read[32].ToString();
                    user.OC_NFLETE = ParseDecimal(read[33].ToString());
                    user.OC_NSEGURO = ParseDecimal(read[34].ToString());
                    user.OC_CTIPOC = read[35].ToString();
                    user.OC_DESPACHO = read[36].ToString();
                    user.OC_TIPO = read[37].ToString();
                    user.EST_CODIGO = read[38].ToString();
                    user.EST_NOMBRE = read[39].ToString();


                    OrdenCompras.Add(user);
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
            return user;
        }

        public List<OrdenCompra> findAll()
        {


            string findAll = "SELECT TOP (100) * FROM comovc, estado_oc WHERE comovc.oc_csitord = estado_oc.est_codigo ";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    OrdenCompra user = new OrdenCompra();
                    user.OC_CNUMORD = read[0].ToString();
                    user.OC_PRINCIPAL = read[1].ToString();
                    user.OC_DFECDOC = DateTime.Parse(read[2].ToString());
                    user.OC_LOTE = read[3].ToString();
                    user.oc_ccodpro = read[4].ToString();
                    user.OC_CRAZSOC = read[5].ToString();
                    user.OC_CDIRPRO = read[6].ToString();
                    user.OC_CCOTIZA = read[7].ToString();
                    user.OC_CCODMON = read[8].ToString();
                    user.OC_CFORPAG = read[9].ToString();
                    user.OC_NTIPCAM = read[10].ToString();
                    user.OC_DFECENT = DateTime.Parse(read[11].ToString());
                    user.OC_COBSERV = read[12].ToString();
                    user.OC_CSOLICT = read[13].ToString();
                    user.OC_CTIPENV = read[14].ToString();
                    user.OC_CENTREG = read[15].ToString();
                    user.OC_CSITORD = read[16].ToString();
                    user.OC_NIMPORT = ParseDecimal(read[17].ToString());
                    user.OC_NDESCUE = ParseDecimal(read[18].ToString());
                    user.OC_NIGV = ParseDecimal(read[19].ToString());
                    user.OC_NVENTA = ParseDecimal(read[20].ToString());
                    user.OC_DFECACT = DateTime.Parse(read[21].ToString());
                    user.OC_CHORA = read[22].ToString();
                    user.OC_CUSUARI = read[23].ToString();
                    user.OC_CFECDOC = read[24].ToString();
                    user.OC_CCONVER = read[25].ToString();
                    user.OC_CFACNOMBRE = read[26].ToString();
                    user.OC_CFACRUC = read[27].ToString();
                    user.OC_CFACDIREC = read[28].ToString();
                    user.OC_CDOCREF = read[29].ToString();
                    user.OC_CNRODOCREF = read[30].ToString();
                    user.OC_ORDFAB = read[31].ToString();
                    user.OC_SOLICITA = read[32].ToString();
                    user.OC_NFLETE = ParseDecimal(read[33].ToString());
                    user.OC_NSEGURO = ParseDecimal(read[34].ToString());
                    user.OC_CTIPOC = read[35].ToString();
                    user.OC_DESPACHO = read[36].ToString();
                    user.OC_TIPO = read[37].ToString();
                    user.EST_CODIGO = read[38].ToString();
                    user.EST_NOMBRE = read[39].ToString();


                    OrdenCompras.Add(user);
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
            return OrdenCompras;
        }
        public List<OrdenCompra> findAll(DateRangePickerModel dateRange)
        {


            string findAll = "SELECT * FROM comovc, estado_oc WHERE comovc.oc_csitord = estado_oc.est_codigo and OC_DFECDOC Between '" + dateRange.Start.ToString("yyyy-MM-ddTHH:mm:ss") + "' and '" + dateRange.End.ToString("yyyy-MM-ddTHH:mm:ss") + "'";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    OrdenCompra user = new OrdenCompra();
                    user.OC_CNUMORD = read[0].ToString();
                    user.OC_PRINCIPAL = read[1].ToString();
                    user.OC_DFECDOC = DateTime.Parse(read[2].ToString());
                    user.OC_LOTE = read[3].ToString();
                    user.oc_ccodpro = read[4].ToString();
                    user.OC_CRAZSOC = read[5].ToString();
                    user.OC_CDIRPRO = read[6].ToString();
                    user.OC_CCOTIZA = read[7].ToString();
                    user.OC_CCODMON = read[8].ToString();
                    user.OC_CFORPAG = read[9].ToString();
                    user.OC_NTIPCAM = read[10].ToString();
                    user.OC_DFECENT = DateTime.Parse(read[11].ToString());
                    user.OC_COBSERV = read[12].ToString();
                    user.OC_CSOLICT = read[13].ToString();
                    user.OC_CTIPENV = read[14].ToString();
                    user.OC_CENTREG = read[15].ToString();
                    user.OC_CSITORD = read[16].ToString();
                    user.OC_NIMPORT = ParseDecimal(read[17].ToString());
                    user.OC_NDESCUE = ParseDecimal(read[18].ToString());
                    user.OC_NIGV = ParseDecimal(read[19].ToString());
                    user.OC_NVENTA = ParseDecimal(read[20].ToString());
                    user.OC_DFECACT = DateTime.Parse(read[21].ToString());
                    user.OC_CHORA = read[22].ToString();
                    user.OC_CUSUARI = read[23].ToString();
                    user.OC_CFECDOC = read[24].ToString();
                    user.OC_CCONVER = read[25].ToString();
                    user.OC_CFACNOMBRE = read[26].ToString();
                    user.OC_CFACRUC = read[27].ToString();
                    user.OC_CFACDIREC = read[28].ToString();
                    user.OC_CDOCREF = read[29].ToString();
                    user.OC_CNRODOCREF = read[30].ToString();
                    user.OC_ORDFAB = read[31].ToString();
                    user.OC_SOLICITA = read[32].ToString();
                    user.OC_NFLETE = ParseDecimal(read[33].ToString());
                    user.OC_NSEGURO = ParseDecimal(read[34].ToString());
                    user.OC_CTIPOC = read[35].ToString();
                    user.OC_DESPACHO = read[36].ToString();
                    user.OC_TIPO = read[37].ToString();
                    user.EST_CODIGO = read[38].ToString();
                    user.EST_NOMBRE = read[39].ToString();
                    OrdenCompras.Add(user);
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
            return OrdenCompras;
        }

        public List<DetalleOrdenCompra> findAllDetail(string oc_cnumord)
        {
            List<DetalleOrdenCompra> listUsers = new List<DetalleOrdenCompra>();

            OrdenCompra ordenCompra = OrdenCompras.Find(X => X.OC_CNUMORD == oc_cnumord);// ver si exite
            string findAll = "SELECT * FROM comovd WHERE comovd.oc_cnumord = '" + oc_cnumord + "'";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    DetalleOrdenCompra user = new DetalleOrdenCompra();
                    user.OC_CNUMORD = read[0].ToString();
                    user.oc_ccodpro = read[1].ToString();
                    user.OC_DFECDOC = DateTime.Parse(read[2].ToString());
                    user.OC_CITEM = read[3].ToString();
                    user.oc_ccodigo = read[4].ToString();
                    user.OC_CCODREF = read[5].ToString();
                    user.OC_CDESREF = read[6].ToString();
                    user.OC_CUNIDAD = read[7].ToString();
                    user.OC_CUNIREF = read[8].ToString();
                    user.OC_NFACTOR = ParseDecimal(read[9].ToString());
                    user.OC_NCANTID = ParseDecimal(read[10].ToString());

                    user.OC_NPREUNI = ParseDecimal(read[11].ToString());
                    //  user.OC_NDSCPOR = ParseDecimal(read[12].ToString());
                    user.OC_NDSCPOR = ParseDecimal(read[12].ToString());
                    user.OC_NDESCTO = ParseDecimal(read[13].ToString());
                    user.OC_NIGV = ParseDecimal(read[14].ToString());
                    user.OC_NIGVPOR = ParseDecimal(read[15].ToString());
                    user.OC_NPRENET = ParseDecimal(read[16].ToString());
                    user.OC_NTOTVEN = ParseDecimal(read[17].ToString());
                    user.OC_NTOTNET = ParseDecimal(read[18].ToString());
                    user.OC_NCANTEN = ParseDecimal(read[19].ToString());
                    user.OC_NCANSAL = ParseDecimal(read[20].ToString());
                    user.OC_ENTREGADO = ParseDecimal(read[21].ToString());
                    user.OC_SALDO = ParseDecimal(read[22].ToString());
                    user.OC_COMENTA = read[23].ToString();
                    user.OC_CESTADO = read[24].ToString();
                    user.OC_FUNICOM = read[25].ToString();
                    user.OC_NRECIBI = ParseDecimal(read[26].ToString());
                    user.OC_CCOMEN1 = read[27].ToString();
                    user.OC_CCOMEN2 = read[28].ToString();
                    user.OC_GLOSA = read[29].ToString();
                    user.OC_PRECIOVEN = ParseDecimal(read[30].ToString());
                    user.CENTCOST = read[31].ToString();



                    listUsers.Add(user);
                }
                ordenCompra.detalles = listUsers;
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
            return listUsers;
        }

        public List<FormaPago> findAllFormasPago()
        {
            List<FormaPago> listUsers = new List<FormaPago>();


            string findAll = "SELECT COD_FP,DES_FP FROM FORMA_PAGO";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    FormaPago user = new FormaPago();
                    user.COD_FP = read[0].ToString();
                    user.DES_FP = read[1].ToString();
                    listUsers.Add(user);
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
            return listUsers;
        }
        public List<Solicitud> findAllSolitud()
        {
            List<Solicitud> listUsers = new List<Solicitud>();


            string findAll = "Select TCLAVE,TDESCRI from TABAYU  where TCOD= '12'";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    Solicitud user = new Solicitud();
                    user.TCLAVE = read[0].ToString();
                    user.TDESCRI = read[1].ToString();
                    listUsers.Add(user);
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
            return listUsers;
        }


        public List<NumDocCompras> findAllDocRef()
        {
            List<NumDocCompras> listUsers = new List<NumDocCompras>();


            string findAll = "SELECT CTNCODIGO, CTDESCRIP FROM NUM_DOCCOMPRAS WHERE CTNCODIGO = 'RQ' OR CTNCODIGO = 'SC'";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                while (read.Read())
                {
                    NumDocCompras user = new NumDocCompras();
                    user.CTNCODIGO = read[0].ToString();
                    user.CTDESCRIP = read[1].ToString();
                    listUsers.Add(user);
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
            return listUsers;
        }

        public string findLastDocRef()
        {
            string findAll = "SELECT ctnnumero FROM num_doccompras WHERE ctncodigo='OC'";
            NumDocCompras user = new NumDocCompras();
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    user.CTNNUMERO = read[0].ToString();
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
            return user.CTNNUMERO;
        }


        public string direccion(string oc_ccodpro)
        {
            string findAll = "select prvcdirecc from maeprov where prvccodigo= '" + oc_ccodpro + "'";
            string direccion = "";
            try
            {
                comando = new SqlCommand(findAll, objConexion.getCon());
                objConexion.getCon().Open();
                SqlDataReader read = comando.ExecuteReader();
                if (read.Read())
                {
                    direccion = read[0].ToString();
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
            return direccion;
        }

        public void updateNroOrden(string nroOrden)
        {

            string updateNumCompras = "UPDATE num_doccompras SET ctnnumero='" + nroOrden + "' WHERE ctncodigo='OC'";
            try
            {
                comando = new SqlCommand(updateNumCompras, objConexion.getCon());
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

        public decimal ParseDecimal(string data)
        {
            if (data == "")
            {
                return 0;
            }
            else
            {
                return Decimal.Round(Decimal.Parse(data));
            }
        }
        public void update(OrdenCompra obj)
        {
            throw new NotImplementedException();
        }
        public String parseDate(DateTime date)
        {
            return date.ToString("MM/dd/yyyy HH:mm:ss");
        }
    }
}