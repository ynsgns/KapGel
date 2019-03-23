using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;
using System.Net.Mail;
using System.Net;

namespace KapGel.Controllers
{
    public class AdminController : Controller
    {
        KapGelEntities db = new KapGelEntities();

        // GET: Admin
        public ActionResult Index()
        {
            var model = db.Users.ToList();
            return View(model);
        }
        public JsonResult YetkileriGetir()
        {
            KapGelEntities db = new KapGelEntities();
            var yetkiler = db.Roles.ToList();
            return Json(new { yetkidiJson = yetkiler }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Ekle(int id)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                var model = db.Users.Find(id);
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult Ekle(Users usr)
        {

            if (usr.id == 0)
            {
                var a = DateTime.Now;
                Users newPers = new Users();
                newPers.authority = usr.authority;
                newPers.NameSurname = usr.NameSurname;
                newPers.eMail = usr.eMail;
                newPers.password = usr.password;
                newPers.phoneNumber = usr.phoneNumber;
                newPers.recordDate = DateTime.Now;
                newPers.isDeleted = false;
                db.Users.Add(newPers);
                this.db.SaveChanges();

                return RedirectToAction("Index", "Admin");
            }
            else
            {
                var KullaniciVarmi = db.Users.Find(usr.id);
                if (KullaniciVarmi != null)
                {
                    KullaniciVarmi.authority = usr.authority;
                    KullaniciVarmi.eMail = usr.eMail;
                    KullaniciVarmi.isDeleted = usr.isDeleted;
                    KullaniciVarmi.NameSurname = usr.NameSurname;
                    KullaniciVarmi.password = usr.password;
                    KullaniciVarmi.phoneNumber = usr.phoneNumber;
                    db.Entry(KullaniciVarmi);//update
                    this.db.SaveChanges();
                }
                return RedirectToAction("Index", "Admin");
            }

        }
        public ActionResult Sil(int id)
        {
            if (id != 0)
            {
                var kullaniciVarmi = this.db.Users.Find(id);
                this.db.Users.Remove(kullaniciVarmi);
                this.db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Admin");
            }
        }

    }
}
