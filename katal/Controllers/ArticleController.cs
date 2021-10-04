using katal.conexion.model.entity;
using katal.conexion.model.neg;
using katal.Model;
using System;
using System.Web.Mvc;


namespace katal.Controllers
{
    public class ArticleController : BaseController
    {


        private ArticuloNeg userNeg;
        public ArticleController()
        {
            userNeg = new ArticuloNeg(codEmpresa);
        }
        // GET: Article
        public ActionResult Index()
        {
            return View(userNeg.findAll());
        }
        public ActionResult dd()
        {
            return View(userNeg.findAll());
        }
        public ActionResult GridViewPartial()
        {

            return PartialView("GridViewPartial", userNeg.findAll());
        }

        public ActionResult GridViewCustomActionPartial(string customAction)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return GridViewPartial();
        }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewAddNewPartial(User issue)
        {

            return UpdateModelWithDataValidation(issue, GridViewHelper.AddNewRecord);
        }

        [ValidateAntiForgeryToken]
        public ActionResult GridViewUpdatePartial(User issue)
        {

            return UpdateModelWithDataValidation(issue, GridViewHelper.UpdateRecord);
        }

        private ActionResult UpdateModelWithDataValidation(User issue, Action<User> updateMethod)
        {
            if (ModelState.IsValid)
                SafeExecute(() => updateMethod(issue));
            else
                ViewBag.GeneralError = "Please, correct all errors.";
            return GridViewPartial();
        }
        private void PerformDelete()
        {
            if (!string.IsNullOrEmpty(Request.Params["SelectedRows"]))
                GridViewHelper.DeleteRecords(Request.Params["SelectedRows"]);
        }
    }
}