using katal.Model;
using System;
using System.Web.Mvc;

namespace katal.Controllers
{
    public class BaseController : Controller
    {
        public string codEmpresa;

        public BaseController()
        {
            if (AuthHelper.IsAuthenticated())
            {              
                codEmpresa = GridViewHelper.codEmpresa;
            }
            

        }
        protected void SafeExecute(Action method)
        {
            try
            {
                method();
            }
            catch (Exception e)
            {
                SetErrorText(e.Message);
            }
        }
        protected object SafeExecute(Func<object> method)
        {
            try
            {
                return method();
            }
            catch (Exception e)
            {
                SetErrorText(e.Message);
            }
            return null;
        }
        protected void SetErrorText(string message)
        {
            ViewBag.GeneralError = message;
        }
    }
}