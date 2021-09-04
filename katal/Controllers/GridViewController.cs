using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using katal.conexion.model.entity;
using katal.conexion.model.neg;
using katal.Model;

namespace katal.Controllers
{
    public class GridViewController : BaseController
    {

        private UserNeg userNeg;
        public GridViewController()
        {
            userNeg = new UserNeg();
        }
        // GET: GridView
        public ActionResult Index()
        {
            return View(userNeg.findAll());
        }
        public ActionResult GridViewDetailsPage(int id)
        {
            ViewBag.ShowBackButton = true;

            return View(GridViewHelper.getEmpresas());
        }

        public ActionResult GridViewRolPage(int id)
        {
            ViewBag.ShowBackButton = true;

            return View(GridViewHelper.getEmpresas());
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult recibirdataEmpresa(FormCollection data)
        {

            int rol_id = Convert.ToInt32(data["rol"]);
            string value4 = data["__RequestVerificationToken"];

            string value2 = HttpUtility.HtmlDecode(data["dark"]);
            DatatreeView htmlAttributes =
            JsonConvert.DeserializeObject<DatatreeView>(value2);
            string datad = htmlAttributes.nodesState[2].ToString();
            Dictionary<string, string> nodes = JsonConvert.DeserializeObject<Dictionary<string, string>>(datad);

            nodes = nodes.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            List<NodeModule> nodeModules = new List<NodeModule>();
            NodeModule currentModule;
            NodePrograma currentPrograma = null;
            NodeOperacion currentOperacion;
            foreach (var entry in nodes)
            {
                // igual que cargar la data para subir al treeview
                // System.Console.WriteLine(entry.Key + ":" + entry.Value);
                List<String> dataniveles = entry.Key.Split('_').ToList();

                currentModule = nodeModules.Find(x => x.name == dataniveles[0]);
                switch (dataniveles.Count)
                {

                    case 1:
                        currentModule = new NodeModule(entry.Key, entry.Value);
                        nodeModules.Add(currentModule);
                        break;
                    case 2:
                        currentPrograma = new NodePrograma(entry.Key, entry.Value);
                        currentModule.programas.Add(currentPrograma);
                        break;
                    case 3:
                        currentOperacion = new NodeOperacion(entry.Key, entry.Value);
                        currentPrograma.operacions.Add(currentOperacion);
                        break;
                }
            }
            string funciones = JsonConvert.SerializeObject(nodeModules);

            //negocio.update(rol_id, funciones);
          
            return RedirectToAction("Index", "Role");
        }
        public ActionResult GridViewEmpresaPage(int id)
        {
            ViewBag.ShowBackButton = true;

            return View(GridViewHelper.getEmpresas());
        }

        public ActionResult GridViewPartial()
        {
            return PartialView("GridViewPartial", GridViewHelper.getUsers());
        }
        [ValidateAntiForgeryToken]
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