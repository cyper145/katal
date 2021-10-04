using katal.conexion.model.neg;
using System.Web.Mvc;
namespace katal.Controllers
{
    public class AreaController : BaseController
    {

        private AreaNeg areaNeg;
        public AreaController()
        {

            areaNeg = new AreaNeg(codEmpresa);
        }
        // GET: Area
        public ActionResult Index()
        {
            return View();
        }
    }
}