using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;

namespace KapGel.Controllers.Admin
{
    public class ConfirmationController : Controller
    {
        // GET: Confirmation
        private KapGelEntities db = new KapGelEntities();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult KategoriListele()
        {
            var model = db.Categories.Where(x => x.IsitApproved != true).ToList();
            return Json(new { result = "ok", data = model }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult KategoriOnayla(int id)
        {
            Categories ktg = new Categories();
            ktg = db.Categories.Find(id);
            if (ktg != null)
            {
                ktg.IsitApproved = true;
                db.Entry(ktg);
                db.SaveChanges();
                return Json(new { result = "ok", }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error", message = "Bulunamadı." }, JsonRequestBehavior.AllowGet);
        }
    }
}