using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using katal.conexion.model.dao;
using katal.conexion.model.entity;
using katal.Model;

namespace katal.conexion.model.neg
{
    public class OrdenCompraNeg
    {

        private OrdenCompraDao objUserDao;
        public OrdenCompraNeg()
        {
            objUserDao = new OrdenCompraDao();
        }
        public List<OrdenCompra> findAll(DateRangePickerModel dateRange)
        {
            return objUserDao.findAll(dateRange);
        }
        public List<DetalleOrdenCompra> findAllDetail( string OC_CNUMORD)
        {
            return objUserDao.findAllDetail(OC_CNUMORD);
        }
        public List<FormaPago> findAllFormasPago()
        {
            return objUserDao.findAllFormasPago();
        }
        public List<Solicitud> findAllSolicitud()
        {
            return objUserDao.findAllSolitud();
        }
        public List<NumDocCompras> findAllDocRef()
        {
            return objUserDao.findAllDocRef();
        }
        public OrdenCompra find(string id)
        {
            return objUserDao.find(id);
        }

        public string nextNroDocument()
        {
            string last = objUserDao.findLastDocRef();
            int nextDocumet = 0;
            if (last != "")
            {
                nextDocumet= int.Parse(last)+1;
            }
            string fmt = "0000000000000.##";
            string next = nextDocumet.ToString(fmt);
            return next;
        }
        // crear orden de compra
        public void create(OrdenCompra orden)
        {
            // actualizar nro
            string updateNumCompras = "UPDATE num_doccompras SET ctnnumero='" + orden.OC_CNUMORD + "' WHERE ctncodigo='OC'";

            string ocprincipal = "";
            if (orden.OC_PRINCIPAL.Trim().Length == 0)
            {
                ocprincipal = orden.OC_CNUMORD;
            }
            else
            {
                ocprincipal = orden.OC_PRINCIPAL;
            }

            //obtener la direccion

            string Direccion = "select prvcdirecc from maeprov where prvccodigo= '" + orden.oc_ccodpro + "'";
            // RESPONSABLE => oc_csolict
            // ENTREGA  EN=>oc_centreg

            //tipo cambio =>oc_cconver

            // glosa 1=>Facturanombre oc_cfacnombre

            // ruc => oc_cfacruc
            // glosa 2 =>oc_cfacdirec

            // ord Fab =>oc_ordfab

            // SOLICITADO POR => OC_SOLICITA


            string insert = "INSERT INTO comovc(oc_cnumord, oc_principal, oc_dfecdoc, oc_ccodpro, oc_crazsoc, ";
            insert += "oc_cdirpro,oc_ccotiza,oc_ccodmon,oc_cforpag,oc_ntipcam,oc_dfecent,";
            insert += "oc_cobserv,oc_csolict,oc_centreg,oc_csitord,oc_nimport,oc_ndescue,";
            insert += "oc_nigv,oc_nventa,oc_dfecact,oc_chora,oc_cusuari,oc_cconver, oc_cfacnombre,";
            insert += "oc_cfacruc, oc_cfacdirec, oc_cdocref, oc_cnrodocref,oc_ordfab,OC_SOLICITA,OC_TIPO,oc_lote) VALUES ('";
            insert += orden.OC_CNUMORD + "','" + ocprincipal + "','" + orden.OC_DFECDOC + "','" + orden.oc_ccodpro + "','";
            insert += orden.OC_CRAZSOC + "','" + Direccion;
            insert += "','" + orden.OC_CCOTIZA + "','" + orden.OC_CCODMON + "','" + orden.OC_CFORPAG + "',";
            insert += orden.OC_NTIPCAM + ",'" + orden.OC_DFECENT + "','";
            insert += orden.OC_COBSERV + "','" + orden.OC_CSOLICT + "','" + orden.OC_CENTREG + "','00',";
            insert += orden.OC_NIMPORT + "," + orden.OC_NDESCUE + "," + orden.OC_NIGV + "," + orden.OC_NVENTA;
            insert += ",'" + orden.OC_CENTREG + "','" + DateTime.Now.ToString("hh:mm:ss") + "','" + AuthHelper.GetLoggedInUserInfo().UserName;
            insert += "','" + orden.OC_CCONVER + "', '" + orden.OC_CFACNOMBRE + "', '" + orden.OC_CFACRUC + "', '" + orden.OC_CFACDIREC;
            insert += "','" + orden.OC_CDOCREF + "', '" + orden.OC_CNRODOCREF + "','" + orden.OC_ORDFAB + "','" + orden.OC_SOLICITA + "','" + orden.OC_TIPO + "','" + orden.OC_LOTE + "')";


            string fmt = "000.##";
            int nextDocumet = 0;
            string next = nextDocumet.ToString(fmt);
            // INSERTAR ELEMENTO 
            orden.detalles.ForEach(element => {
                ++nextDocumet;
                next = nextDocumet.ToString(fmt);
                string item = "INSERT INTO comovd (oc_cnumord,oc_ccodpro,oc_dfecdoc,oc_citem,";
                item += "oc_ccodigo,oc_ccodref,oc_cdesref,oc_cunidad,oc_cuniref,oc_nfactor,";
                item += "oc_saldo,oc_ncantid,oc_npreuni,oc_ndscpor,oc_ndescto,oc_nigv,oc_nigvpor,";
                item += "oc_nprenet,oc_ntotven,oc_ntotnet,oc_cestado,centcost,oc_ccomen1,oc_ccomen2, oc_glosa, oc_precioven) ";
                item += "VALUES ('" + orden.OC_CNUMORD + "','" + orden.oc_ccodpro + "','" + orden.OC_DFECDOC;
                item += "','" + next + "','";
                item += element.oc_ccodigo + "','" + element.OC_CCODREF + "','";
                item += element.OC_CDESREF + "','" + element.OC_CUNIDAD + "','";
                item += element.OC_CUNIREF + "'," + element.OC_NFACTOR + "," + element.OC_SALDO + "," + element.OC_NCANTID + ",";
                item += element.OC_NPREUNI + "," + element.OC_NDSCPOR + "," + element.OC_NDESCTO + "," + element.OC_NIGV + ",";
                item += element.OC_NIGVPOR + ","+ element.OC_NPRENET  + "," + element.OC_NTOTVEN+ "," + element.OC_NTOTNET;
                item += ",'00','" + element.CENTCOST + "','";
                item += element.OC_CCOMEN1 + "','" + element.OC_CCOMEN2 + "','" + element.OC_GLOSA + "'," + element.OC_PRECIOVEN + ")";
            });
          
                
                }
        
    }
}