using katal.conexion.model.dao;
using katal.conexion.model.entity;
using System.Collections.Generic;

namespace katal.conexion.model.neg
{
    public class RequisicionCompraNeg
    {

        private RequisicionCompraDao objUserDao;
        public RequisicionCompraNeg(string codEmpresa)
        {
            objUserDao = new RequisicionCompraDao(codEmpresa);
        }
        public List<RequisicionCompra> findAll()
        {
            return objUserDao.findAll();
        }

        public List<RequisicionCompra> findAll(DateRangePickerModel dateRange)
        {
            return objUserDao.findAll(dateRange);
        }
        public List<DetalleRequisicion> findAllDetail(string NROREQUI)
        {
            return objUserDao.findAllDetail(NROREQUI);

        }
        public List<RequisicionCompra> findAllPendientes()
        {
            return objUserDao.findAllPendientes();
        }

        public RequisicionCompra find(string codigo)
        {
            return objUserDao.find(codigo);
        }

        public List<CentroCosto> findAllCentroCostos()
        {
            return objUserDao.findAllCentroCostos();
        }
        public string nextNroDocument()
        {
            string last = objUserDao.newNroRequerimiento();
            int nextDocumet = 0;
            if (last != "")
            {
                nextDocumet = int.Parse(last) + 1;
            }
            string fmt = "0000000000.##";
            string next = nextDocumet.ToString(fmt);
            return next;
        }

        public void create(RequisicionCompra objUser)
        {
            objUserDao.updateNroRequerimiento(objUser.NROREQUI);
            objUserDao.create(objUser);
            objUserDao.createDetail(objUser);
        }
        public void update(RequisicionCompra objUser)
        {
            objUserDao.update(objUser);
            objUserDao.DeleteDetail(objUser.NROREQUI);
            objUserDao.createDetail(objUser);
        }
        public void delete(string NROREQUI)
        {
            RequisicionCompra objRequision = objUserDao.find(NROREQUI);
            if (objRequision.ESTREQUI != "A")
            {
                objUserDao.DeleteDetail(objRequision.NROREQUI);
                objUserDao.delete(objRequision);
            }
            // ver como trabajar un respuesta alterna

        }

    }
}