using katal.conexion.model.neg;
using System.Web.Mvc;
namespace katal.Controllers
{
    public class ProveedorController : BaseController
    {
        // GET: Proveedor

        private ProveedorNeg userNeg;
        public ProveedorController()
        {
            userNeg = new ProveedorNeg(codEmpresa);
        }
        public ActionResult Index()
        {
            return View(userNeg.findAll());
        }
        public ActionResult GridViewPartial()
        {

            return PartialView("GridViewPartial", userNeg.findAll());
        }
    }
}