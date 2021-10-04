using System;
using System.Web;

namespace katal.Model
{
    public class ApplicationUser
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string Empresa { get; set; }
        public string Rol { get; set; }
        public string codEmpresa { get; set; }
    }

    public static class AuthHelper
    {


        public static bool SignIn(string userName, string password, int rol_id, string empresa, string rol, string codEmpresa)
        {

            ApplicationUser user = CreateDefualtUser(userName, empresa, rol, codEmpresa);
            HttpContext.Current.Session["User"] = user;
            GridViewHelper.user = user;
            HttpContext.Current.Session["Rol"] = rol_id; // Mock user data
            return true;
        }
        public static void SignOut()
        {
            HttpContext.Current.Session["User"] = null;
            HttpContext.Current.Session["Rol"] = null;
        }
        public static bool IsAuthenticated()
        {
            return GetLoggedInUserInfo() != null;
        }

        public static ApplicationUser GetLoggedInUserInfo()
        {
            return HttpContext.Current.Session["User"] as ApplicationUser;
        }
        private static ApplicationUser CreateDefualtUser(string username, string empresa, string rol, string codEmpresa)
        {
            return new ApplicationUser
            {
                UserName = username,
                FirstName = "Julia",
                LastName = "Bell",
                Email = username,
                AvatarUrl = "~/Content/Photo/Alberto_Alonso.jpg",
                Empresa = empresa,
                Rol = rol,
                codEmpresa = codEmpresa,
            };
        }
        public static string Encriptar(this string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }
        public static string DesEncriptar(this string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }
        public static string generateID()
        {
            long i = 1;

            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }

            string number = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);

            return number;
        }
    }
}