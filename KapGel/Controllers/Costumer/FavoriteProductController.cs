using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;

namespace KapGel.Controllers.Costumer
{

    public class FavoriteProductController : Controller
    {
        KapGelEntities db = new KapGelEntities();
        // GET: FavoriteProduct
        public ActionResult Index()
        {
            var model = db.ProductFavorite.ToList();
            return View(model);
        }
        [HttpGet]
        
        public ActionResult FavoriUrunEkleGuncelle(int id)
        {

            if (id == 0)
            {
                return View(new ProductFavorite());
            }
            else
            {
                var model = db.ProductFavorite.Find(id);
                return View(model);
            }
        }
        [HttpPost]
        public JsonResult FavoriUrunEkleGuncelle (ProductFavorite prd)
        {
            if (prd.Id == 0)
            {
                ProductFavorite newFavori = new ProductFavorite();
                newFavori.ProductId = prd.ProductId;
                newFavori.UserId = prd.UserId;
                db.ProductFavorite.Add(newFavori);
                db.SaveChanges();
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var urunVarmi = db.ProductFavorite.Find(prd.Id);
                if (urunVarmi != null)
                {
                    urunVarmi.ProductId = prd.ProductId;
                    urunVarmi.UserId = prd.UserId;
                    db.SaveChanges();
                }
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
                
            }
        }
        public JsonResult FavoriUrunSil(int id)
        {
            var urunVarmi = db.ProductFavorite.Find(id);
            if (urunVarmi != null)
            {
                db.ProductFavorite.Remove(urunVarmi);
                db.SaveChanges();
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "error" , message="Favori ürün bulunamadı"}, JsonRequestBehavior.AllowGet);
            }
        }

    }
}