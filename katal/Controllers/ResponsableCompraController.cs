using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using katal.conexion.model.neg;
using katal.conexion.model.entity;
namespace katal.Controllers
{
    public class ResponsableCompraController : BaseController
    {
        private ResponsableCmpNeg responsableNeg;
        public ResponsableCompraController()
        {
            responsableNeg = new ResponsableCmpNeg();
        }
        public ActionResult Index()
        {
            return View("Index", responsableNeg.findAll());
        }
        public ActionResult ResponsablePartial()
        {
            return PartialView("ResponsablePartial", responsableNeg.findAll());
        }

        [ValidateInput(false)]
        public ActionResult ToolbarAddNewPartial(ResponsableCompra product)
        {
            if (ModelState.IsValid)
                SafeExecute(() => InsertProduct(product));
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return ResponsablePartial();
        }
        private void InsertProduct(ResponsableCompra area)
        {
            responsableNeg.create(area);
        }

        [ValidateInput(false)]
        public ActionResult ToolbarUpdatePartial(ResponsableCompra product)
        {
            if (ModelState.IsValid)
                SafeExecute(() => UpdateProduct(product));
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return ResponsablePartial();
        }
        private void UpdateProduct(ResponsableCompra area)
        {
            responsableNeg.update(area);
        }

        [ValidateInput(false)]
        public ActionResult ToolbarDeletePartial(string RESPONSABLE_CODIGO = "")
        {
            if (RESPONSABLE_CODIGO != "")
                SafeExecute(() => DeleteProduct(RESPONSABLE_CODIGO));
            return ResponsablePartial();
        }
        private void DeleteProduct(string codigo)
        {
            responsableNeg.delete(codigo);
        }
    }
}