using System;
using System.Collections.Generic;
using System.Web.Mvc;
using katal.conexion.model.dao;
using katal.conexion.model.entity;
using katal.conexion.model.neg;
using katal.Model;
namespace katal.Controllers
{
    public class AccountController : BaseController
    {


        private UserNeg userNeg;
        public AccountController()
        {
            userNeg = new UserNeg();
        }
        // GET: /Account/SignIn
        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new SignInViewModel() { RememberMe = true });
        }

        // POST: /Account/SignIn
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(SignInViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // DXCOMMENT: You Authentication logic

            User user = userNeg.login(model.UserName, model.Password, model.Empresa);
            if (user!=null)
            {
                AuthHelper.SignIn(model.UserName, model.Password,user.rol_id, user.empresa, user.rol, model.Empresa);
                ArticuloDao articuloDao = new ArticuloDao();
                List<Articulo> nodeModules =  articuloDao.findAll();

                return RedirectToAction("Index", "Home");
            }
               
            else
            {
                SetErrorText("Invalid login attempt.");
                ModelState.AddModelError("", ViewBag.GeneralError);
                return View(model);
            }
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                User user = new User();
                user.username = model.UserName;
                user.email = model.Email;
                user.password = AuthHelper.Encriptar(model.Password);
                user.photo = "ddd";// falta           
                user.id = int.Parse(AuthHelper.generateID());

                try
                {
                    userNeg.create(user);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }

            }
            else
            {
                ViewData["EditError"] = "porfavor , Corriga todos los errores.";
            }

            return View(model);


        }

        // POST: /Account/SignOut
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            AuthHelper.SignOut(); // DXCOMMENT: Your Signing out logic
            return RedirectToAction("Index", "Home");
        }

        public ActionResult UserMenuItemPartial()
        {
            return PartialView("UserMenuItemPartial", AuthHelper.GetLoggedInUserInfo());
        }
    }
}