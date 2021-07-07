using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using katal.conexion.model.entity;
using katal.conexion.model.neg;
namespace katal.Controllers
{
    public class AreaController : BaseController
    {

        private AreaNeg areaNeg;
        public AreaController()
        {
            areaNeg = new AreaNeg();
        }
        // GET: Area
        public ActionResult Index()
        {
            return View();
        }
    }
}