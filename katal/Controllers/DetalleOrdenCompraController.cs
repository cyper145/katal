using DevExpress.Web.Mvc;
using katal.conexion.model.entity;
using katal.conexion.model.neg;
using katal.Models;
using System.Web.Mvc;

namespace katal.Controllers
{
    public class DetalleOrdenCompraController : BaseController
    {
        // GET: DetalleOrdenCompra
        private ProveedorNeg userNeg; // cambiar Proveedor po detalle de orden compra
        // el mainmodel es el model de orden de compra
        public DetalleOrdenCompraController()
        {
            userNeg = new ProveedorNeg(codEmpresa);
        }
        public ActionResult Index()
        {
            return View(new Proveedor());
        }
        public ActionResult GridViewPartial()
        {

            return PartialView(userNeg.findAll());
        }
        public ActionResult GridViewCustomActionUpdate(Proveedor mainModel)
        {
            ViewData["SuccessFlag"] = UpdateAllValues(null, mainModel);
            return PartialView("GridViewPartial", userNeg.findAll());
        }
        /* save all changes to a data base in this action */
        public bool UpdateAllValues(MVCxGridViewBatchUpdateValues<GridDataItem, int> batchValues, Proveedor mainModel)
        {
            if (batchValues != null)
            {
                foreach (var item in batchValues.Insert)
                {
                    if (batchValues.IsValid(item))
                        BatchEditRepository.InsertNewItem(item, batchValues);
                    else
                        batchValues.SetErrorText(item, "Correct validation errors");
                }
                foreach (var item in batchValues.Update)
                {
                    if (batchValues.IsValid(item))
                        BatchEditRepository.UpdateItem(item, batchValues);
                    else
                        batchValues.SetErrorText(item, "Correct validation errors");
                }
                foreach (var itemKey in batchValues.DeleteKeys)
                {
                    BatchEditRepository.DeleteItem(itemKey, batchValues);
                }
            }
            bool result = false;
            if (mainModel != null)
            {
                //custom actions
                result = true;
            }
            return result && (batchValues == null || batchValues.EditorErrors.Count == 0);
        }
        public ActionResult BatchUpdatePartial(MVCxGridViewBatchUpdateValues<GridDataItem, int> batchValues, Proveedor mainModel)
        {
            ViewData["SuccessFlag"] = UpdateAllValues(batchValues, mainModel);
            return PartialView("GridViewPartial", BatchEditRepository.GridData);
        }
        public ActionResult Success()
        {
            return View("Success");
        }
    }
}
