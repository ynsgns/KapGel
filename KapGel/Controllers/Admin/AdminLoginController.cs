using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KapGel.Controllers.Admin
{
    public class AdminLoginController : Controller
    {
        // GET: AdminLogin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CikisYap()
        {
            TokenController tk = new TokenController();
            tk.TokenSil();
        }
    }
}