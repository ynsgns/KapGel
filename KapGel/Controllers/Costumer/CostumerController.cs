using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;
namespace KapGel.Controllers.Costumer
{
    using KapGel.Models.EntityFramework;

    public class CostumerController : Controller
    {
        // GET: Costumer
        KapGelEntities db = new KapGelEntities();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult KampanyadanHaberdarOl(Campaign cmp)
        { 
            Campaign newCampaign = new Campaign();
            newCampaign.MarketId = cmp.MarketId;
            newCampaign.UserId = cmp.UserId;
            db.SaveChanges();
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MarketVote(MarketVote mv)
        {
             MarketVote newMarketVote = new MarketVote();
            newMarketVote.customerId = mv.customerId;
            newMarketVote.marketId = mv.marketId;
            newMarketVote.marketPoint = mv.marketPoint;
            db.SaveChanges();
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProductVote(ProductVote pv)
        {
             ProductVote newProductVote = new ProductVote();
            newProductVote.customerId = pv.customerId;
            newProductVote.marketId = pv.marketId;
            newProductVote.productPoint = pv.productPoint;
            db.SaveChanges();
            return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GecmisSprisler(int id)
        {
           
            var user = db.Users.Find(id);
            if (user != null)
            {
                var Gecmis = db.Database.SqlQuery<object>(
                    "select * from MyBasket as mb inner join ProductsInBasket as pb on mb.Id = pb.BasketId");
                return Json(new { result = "ok", GecmisSprisler = Gecmis }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HesapSil(int id)
        {
            Users users = new Users();
            users = db.Users.Find(id);
            if (users != null)
            {
                users.isDeleted = true;
                this.db.Entry(users);
                this.db.SaveChanges();
                
                return Json(new { result = "ok",  }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }
    }
}