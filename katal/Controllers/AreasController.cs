using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using katal.conexion.model.entity;
using katal.conexion.model.neg;
namespace katal.Controllers
{
    public class AreasController : BaseController
    {
        // GET: Areas
        private AreaNeg areaNeg;
        public AreasController()
        {
            areaNeg = new AreaNeg(codEmpresa);
        }
        public ActionResult Index()
        {
            return View("Index", areaNeg.findAll());
        }
        public ActionResult AreasPartial()
        {
            return PartialView("AreasPartial", areaNeg.findAll());
        }

        [ValidateInput(false)]
        public ActionResult ToolbarAddNewPartial(Area product)
        {
            if (ModelState.IsValid)
                SafeExecute(() => InsertProduct(product));
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return AreasPartial();
        }
         private void InsertProduct(Area area)
        {
            areaNeg.create(area);
        }

        [ValidateInput(false)]
        public ActionResult ToolbarUpdatePartial(Area product)
        {
            if (ModelState.IsValid)
                SafeExecute(() => UpdateProduct(product));
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return AreasPartial();
        }
        private void UpdateProduct(Area area)
        {
            areaNeg.update(area);
        }

        [ValidateInput(false)]
        public ActionResult ToolbarDeletePartial(string AREA_CODIGO = "")
        {
            if (AREA_CODIGO != "")
                SafeExecute(() => DeleteProduct(AREA_CODIGO));
            return AreasPartial();
        }
        private void DeleteProduct(string codigo)
        {
            areaNeg.delete(codigo);
        }


        //  public ActionResult ExportTo(string customExportCommand)
        // {
        //   switch (customExportCommand)
        // {
        //   case "CustomExportToXLS":
        //       case "CustomExportToXLSX":
        //        return GridViewExportDemoHelper.ExportFormatsInfo[customExportCommand](
        //             GridViewToolbarHelper.ExportGridSettings, areaNeg.findAll());
        //     default:
        ////         return RedirectToAction("index");
        //    }
        // }


    }
}