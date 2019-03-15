using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;

namespace KapGel.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Giris(Users usr)
        {
            KapGelEntities db = new KapGelEntities();
            var model = db.Users.Where(x => x.eMail.Equals(usr.eMail)).FirstOrDefault();
            if (model != null)
            {
                if (model.password == usr.password)
                {
                    return Json(new { result = "Giris Ba sarılı" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "Şifre hatalı" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = "Kayıtlı böyle bir email adresi bulanamadı" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}