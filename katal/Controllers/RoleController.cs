using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using katal.Code;
using katal.conexion.model.entity;
using katal.conexion.model.neg;

namespace katal.Controllers
{
    public class RoleController : BaseController
    {

        private RoleNeg negocio;
        public RoleController()
        {
            negocio = new RoleNeg();
        }

        // GET: Role
        public ActionResult Index()
        {
            return View(negocio.findAll());
        }
        // POST: /Account/SignIn
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult recibirdata(FormCollection data)
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

            negocio.update(rol_id, funciones);
            //return View();
            return RedirectToAction("Index", "Role");
        }


        public ActionResult GridViewDetailsPage(int id)
        {
            ViewBag.ShowBackButton = true;
            ViewBag.rol = id;
            ViewBag.rol = id;
            return View(negocio.lista(id));
        }


        public ActionResult GridViewPartial()
        {
            return PartialView("GridViewPartial", RoleHelper.getroles());
        }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewCustomActionPartial(string customAction)
        {
            if (customAction == "delete")
                SafeExecute(() => PerformDelete());
            return GridViewPartial();
        }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewAddNewPartial(Role issue)
        {
            return UpdateModelWithDataValidation(issue, RoleHelper.AddNewRecord);
        }
        [ValidateAntiForgeryToken]
        public ActionResult GridViewUpdatePartial(Role issue)
        {
            return UpdateModelWithDataValidation(issue, RoleHelper.UpdateRecord);
        }
        private ActionResult UpdateModelWithDataValidation(Role issue, Action<Role> updateMethod)
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
                RoleHelper.DeleteRecords(Request.Params["SelectedRows"]);
        }

    }
}
