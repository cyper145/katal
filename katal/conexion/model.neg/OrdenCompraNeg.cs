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

            
            string ocprincipal = "";
            if (orden.OC_PRINCIPAL==null || orden.OC_PRINCIPAL.Trim().Length == 0  )
            {
                ocprincipal = orden.OC_CNUMORD;
                orden.OC_PRINCIPAL = ocprincipal;

            }
            else
            {
                ocprincipal = orden.OC_PRINCIPAL;
                orden.OC_PRINCIPAL = ocprincipal;
            }
            orden.DIRECCION = objUserDao.direccion(orden.oc_ccodpro);
            objUserDao.create(orden);
            objUserDao.updateNroOrden(orden.OC_CNUMORD);
            objUserDao.createDetail(orden);

           
            

            //obtener la direccion

            string Direccion = objUserDao.direccion(orden.oc_ccodpro);
            // RESPONSABLE => oc_csolict
            // ENTREGA  EN=>oc_centreg

            //tipo cambio =>oc_cconver

            // glosa 1=>Facturanombre oc_cfacnombre

            // ruc => oc_cfacruc
            // glosa 2 =>oc_cfacdirec

            // ord Fab =>oc_ordfab

            // SOLICITADO POR => OC_SOLICITA


            

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
        public void delete(string OC_CNUMORD)
        {
            OrdenCompra objRequision = objUserDao.find(OC_CNUMORD);
           
                objUserDao.DeleteDetail(objRequision.OC_CNUMORD);
                objUserDao.delete(objRequision);          
            // ver como trabajar un respuesta alterna
        }
    }
}