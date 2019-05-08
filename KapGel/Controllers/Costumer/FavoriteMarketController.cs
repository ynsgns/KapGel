using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;

namespace KapGel.Controllers.Costumer
{
    public class FavoriteMarketController : Controller
    {
        KapGelEntities db = new KapGelEntities();
        // GET: FavoriteMarket
        public ActionResult Index()
        {
            var model = db.MarketFavorite.ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult FavoriMarketEkleGuncelle(int id)
        {

            if (id == 0)
            {
                return View(new MarketFavorite());
            }
            else
            {
                var model = db.MarketFavorite.Find(id);
                return View(model);
            }
        }
        [HttpPost]
        public JsonResult FavoriMarketEkleGuncelle(MarketFavorite mrk)
        {
            if (mrk.Id == 0)
            {
                MarketFavorite newFavori = new MarketFavorite();
                newFavori.MarketId = mrk.MarketId;
                newFavori.UserId = mrk.UserId;
                db.MarketFavorite.Add(newFavori);
                db.SaveChanges();
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var marketVarmi = db.MarketFavorite.Find(mrk.Id);
                if (marketVarmi != null)
                {
                    marketVarmi.MarketId = mrk.MarketId;
                    marketVarmi.UserId = mrk.UserId;
                    db.SaveChanges();
                }
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult FavoriMarketSil(int id)
        {
            var marketVarmi = db.MarketFavorite.Find(id);
            if (marketVarmi != null)
            {

                this.db.MarketFavorite.Remove(marketVarmi);
                this.db.SaveChanges();
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "error", message = "Favori Market Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}