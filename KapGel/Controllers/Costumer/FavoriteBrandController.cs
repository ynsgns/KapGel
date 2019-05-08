using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;

namespace KapGel.Controllers.Costumer
{
    public class FavoriteBrandController : Controller
    {
        KapGelEntities db = new KapGelEntities();
        // GET: FavoriteBrand
        public ActionResult Index()
        {
            var model = db.BrandFavorite.ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult FavoriMarkaEkleGuncelle(int id)
        {
            if (id == 0)
            {
                return View(new BrandFavorite());
            }
            else
            {
                var model = db.BrandFavorite.Find(id);
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult FavoriMarkaEkleGuncelle(BrandFavorite fvr)
        {
            if (fvr.id == 0)
            {
                BrandFavorite newFavori = new BrandFavorite();
                newFavori.BrandId = fvr.BrandId;
                newFavori.UserId = fvr.UserId;
                db.BrandFavorite.Add(newFavori);
                db.SaveChanges();
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var markaVarmi = db.BrandFavorite.Find(fvr.id);
                if (markaVarmi != null)
                {
                    markaVarmi.BrandId = fvr.BrandId;
                    markaVarmi.UserId = fvr.UserId;
                    db.SaveChanges();
                }
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult FavoriMarkaSil(int id)
        {

            var MarkaVarmi = this.db.BrandFavorite.Find(id);
            if (MarkaVarmi != null)
            {
                this.db.BrandFavorite.Remove(MarkaVarmi);
                this.db.SaveChanges();
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "error", message="Favori Marka Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}