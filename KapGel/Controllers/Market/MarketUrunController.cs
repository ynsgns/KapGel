using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;
using System.IO;
using KapGel.Models;


namespace KapGel.Controllers.Market
{
    public class MarketUrunController : Controller
    {
        // GET: MarketUrun
        KapGelEntities db = new KapGelEntities();

        public ActionResult Index()
        {
            //var data = db.Database.SqlQuery<ViewModelUrun>("select   productName, stockNumber, discountRate, productPoint, price, productPicture from Products as pr" +
            //"left join productPicture as pp on  pr.Id = pp.productId").ToList();
            //var model = db.Products.Join(
            //    db.productPicture,
            //    pp => pp.Id,
            //    pr => pr.productId,
            //    (pp, pr) => new { pp, pr },
            //        Select(new ViewModelUrun
            //        {
            //            productPicture = pr.
            //        })
            //).ToList();

            var model = (from pp in db.Products
                         join pr in db.productPicture on pp.Id equals pr.productId
                         select new ViewModelUrun
                         {
                             productPicture = pr.productPicture1,
                             Id = pp.Id,
                             price = pp.Id,
                             stockNumber = pp.Id,
                             productName = pp.productName,
                             discountRate = pp.discountRate,
                             productPoint = pp.productPoint

                         }
                ).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult UrunEkleGuncelle(int id)
        {
            if (id == 0) return View(new Products());
            else
            {
                var model = db.Products.Find(id);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult UrunEkleGuncelle(Products pr, HttpPostedFileBase productPicture1)
        {
            var a = productPicture1.FileName;
            if (pr.Id == 0)
            {
                Products products = new Products
                {
                    categoryId = pr.categoryId,
                    discountRate = pr.discountRate,
                    marketId = pr.marketId,
                    price = pr.price,
                    productName = pr.productName,
                    productPoint = pr.productPoint,
                    stockNumber = pr.stockNumber
                };
                db.Products.Add(products);
                db.SaveChanges();
                int sonId = db.Products.Max(x => x.Id);
                productPicture productPicture = new productPicture
                {
                    productId = sonId,
                    productPicture1 = Path.GetFileName(productPicture1.FileName),
                };

                var fileName = Path.GetFileName(productPicture1.FileName);

                //store file in the Books folder
                var path = Path.Combine(Server.MapPath("~/Uploads/ProductsPictures"), fileName);
                try
                {
                    productPicture1.SaveAs(path);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                }

                db.productPicture.Add(productPicture);
                db.SaveChanges();

                return RedirectToAction("Index", "MarketUrun");
            }
            else if (pr.Id > 0)
            {
                Products products = db.Products.Find(pr.Id);
                products.categoryId = pr.categoryId;
                products.discountRate = pr.discountRate; ;
                products.marketId = pr.marketId;
                products.price = pr.price;
                products.productName = pr.productName;
                products.productPoint = pr.productPoint;
                products.stockNumber = pr.stockNumber;

                db.Entry(products);
                db.SaveChanges();

                productPicture productPicture = db.productPicture.Where(x => x.productId == pr.Id).FirstOrDefault();
                productPicture.productPicture1 = productPicture1.FileName;
                db.Entry(productPicture);
                db.SaveChanges();

                return RedirectToAction("Index", "MarketUrun");
            }
            else
            {
                return RedirectToAction("Index", "MarketUrun");
            }
        }


        public JsonResult UstKategoriGet()
        {
            var model = db.Categories.Where(x => x.topCategoryId == 0).ToList();
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult AltKategoriGet(int id)
        {
            var model = db.Categories.Where(x => x.topCategoryId == id).ToList();
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);

        }
         
        public ActionResult Sil(int id)
        {
            var model = db.Products.Find(id);
            if (model != null)
            {
                var img = db.productPicture.Where(x => x.productId == model.Id);
                if (img != null)
                {
                    foreach (var i in img)
                    {
                        db.productPicture.Remove(i);
                    }
                }
                db.Products.Remove(model);
                db.SaveChanges();
                
                //return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index", "MarketUrun");

        }
    }
}