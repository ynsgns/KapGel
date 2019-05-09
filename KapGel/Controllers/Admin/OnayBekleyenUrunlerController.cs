using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models;
using KapGel.Models.EntityFramework;

namespace KapGel.Controllers.Admin
{
    public class OnayBekleyenUrunlerController : Controller
    {
        // GET: OnayBekleyenUrunler
        KapGelEntities db = new KapGelEntities();
        public ActionResult Index()
        {
            var model = (from pp in db.Products
                    join pr in db.productPicture on pp.Id equals pr.productId
                    select new ViewModelUrun
                    {
                        productPicture = pr.productPicture1,
                        Id = pp.Id,
                        price = pp.price,
                        stockNumber = pp.stockNumber,
                        productName = pp.productName,
                        discountRate = pp.discountRate,
                        productPoint = pp.productPoint,
                        IsitApproved = pp.IsitApproved
                    }
                ).Where(x=>x.IsitApproved == false).ToList();
            return View(model);
        }
         
        public JsonResult UrunOnayla(int id)
        {
            var products = db.Products.Find(id);
            if (products != null)
            { 
                products.IsitApproved = true;
                db.Entry(products);
                db.SaveChanges();
                return Json(new {result = "ok"}, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        } 
    }
}