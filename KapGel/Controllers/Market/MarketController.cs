using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;
using System.IO;

namespace KapGel.Controllers.Market
{
    public class MarketController : Controller
    {
        KapGelEntities db = new KapGelEntities();
        // GET: Market
        public ActionResult Index()
        {
            var model = db.Markets.ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult MarketEkleGuncelle(int id)
        {
            if (id == 0)
            {
                return View(new Markets());
            }
            else
            {
                var model = db.Markets.Find(id);
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult MarketEkleGuncelle(Markets mrk, HttpPostedFileBase video)
        {
            if (mrk.Id== 0)
            {
                Markets newMarket = new Markets() {
                    marketName = mrk.marketName,
                    marketPhoto = mrk.marketPhoto,
                    phoneNumber = mrk.phoneNumber,
                    recordDate = mrk.recordDate,
                    video = mrk.video,
                    userId = mrk.userId,                   
                };
             
                db.Markets.Add(newMarket);
                var fileName = Path.GetFileName(video.FileName);
               

                //store file in the Books folder
                var path = Path.Combine(Server.MapPath("~/Uploads/Videos"), fileName);
                try
                {
                    video.SaveAs(path);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                }
                db.SaveChanges();
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var marketVarmi = db.Markets.Find(mrk.Id);
                if (marketVarmi != null)
                {
                    marketVarmi.marketName = mrk.marketName;
                    marketVarmi.marketPhoto = mrk.marketPhoto;
                    marketVarmi.phoneNumber = mrk.phoneNumber;
                    marketVarmi.recordDate = mrk.recordDate;
                    marketVarmi.video = mrk.video;
                    marketVarmi.userId = mrk.userId;
                    db.Entry(marketVarmi);
                    db.SaveChanges();
                }
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult MarketSil(int id)
        {

            var MarketVarmi = this.db.Markets.Find(id);
            if (MarketVarmi != null)
            {
                this.db.Markets.Remove(MarketVarmi);
                this.db.SaveChanges();
                return RedirectToAction("Index","Market");
                // return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "error", message = "Market Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}