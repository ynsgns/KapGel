using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;

namespace KapGel.Controllers.Admin
{
    public class CategoryController : Controller
    {
        // GET: Category
        KapGelEntities db = new KapGelEntities();
        public ActionResult Index()
        {
            var model = db.Categories.ToList();//.Where(x=>x.topCategoryId == 0)
            return View(model);
        }

        public JsonResult AltKategoriListele(int id)
        {
            var model = db.Categories.Where(x => x.topCategoryId == id).ToList();
            return Json(new { result = "ok" , data =model}, JsonRequestBehavior.AllowGet);
 
        }

        [HttpGet]
        public ActionResult KategoriEkleGuncelle(int id)
        {
            if (id == 0)
            {
                return View(new Categories());
            }
            else
            {
                var model = db.Categories.Find(id);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult KategoriEkleGuncelle(Categories ct)
        {
            Categories newCategories = new Categories()
            {
                Id = ct.Id,
                topCategoryId = ct.topCategoryId,
                categoryName = ct.categoryName,
                icon = ct.icon,
                IsitApproved = false
            };
            db.Categories.Add(newCategories);
            db.SaveChanges();
            return RedirectToAction("Index", "Category");
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Sil(int id)
        {
            var varmi = db.Categories.Find(id);
            if (varmi != null)
            {
                var bagimliListe = db.Categories.Where(x => x.topCategoryId == varmi.Id).ToList();
                if (bagimliListe == null)
                {
                    db.Categories.Remove(varmi);
                    db.SaveChanges();
                    return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "error", message = "ilişkili olan alt kategoriler bulundu.", data = bagimliListe }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = "error", message = "Kategori bulunamadi." }, JsonRequestBehavior.AllowGet);
        }
    }
}