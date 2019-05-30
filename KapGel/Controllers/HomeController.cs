using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;
using KapGel.Models;
using PagedList;

namespace KapGel.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        KapGelEntities db = new KapGelEntities();
        public ActionResult Index(int? SayfaNo, string sortOrder, string currentFilter, string searchString)
        {
            /// filtreleme-> fiyat aralığı -> pahalıdan ucuza -> ucuzdan pahalıya -> a dan z ye -> z den a ya
            ViewBag.TitleSortParm = sortOrder == "Title" ? "Title desc" : "Title";
            ViewBag.AuthorSortParm = sortOrder == "Author" ? "Author desc" : "Author";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";

            if (searchString != null)
            {
                SayfaNo = 1;
            }
            else
            {
                searchString = currentFilter;

            }

            ViewBag.currentFilter = searchString;
            int _sayfaNo = SayfaNo ?? 1;
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
                        IsitApproved = pp.IsitApproved,
                             categoryId = pp.categoryId

                    }
                ).Where(x => x.IsitApproved == true).OrderByDescending(m => m.productPoint).ToPagedList<ViewModelUrun>(_sayfaNo, 10);

            if (!String.IsNullOrEmpty(searchString))
            {
                //page = 1;
                model = (from pp in db.Products
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
                            IsitApproved = pp.IsitApproved,
                            categoryId = pp.categoryId

                        }
                    ).Where(x => x.IsitApproved == true && x.productName.ToUpper().Contains(searchString.ToUpper())).OrderByDescending(m => m.productPoint).ToPagedList<ViewModelUrun>(_sayfaNo, 10);


                //&& s.productName.ToUpper().Contains(searchString.ToUpper())
            }


            if (!String.IsNullOrEmpty(currentFilter))
            {
                //page = 1;
                int categori = int.Parse(currentFilter);
                model = (from pp in db.Products
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
                            IsitApproved = pp.IsitApproved,
                            categoryId = pp.categoryId

                        }
                    ).Where(x => x.IsitApproved == true && x.categoryId == categori).OrderByDescending(m => m.productPoint).ToPagedList<ViewModelUrun>(_sayfaNo, 10);


           
            }

            //switch (sortOrder)
            //{
            //    case "Author":
            //        Insights = Articles.OrderBy(s => s.Author);
            //        break;
            //    case "Author desc":
            //        Insights = Articles.OrderByDescending(s => s.Author);
            //        break;
            //    case "Title":
            //        Insights = Articles.OrderBy(s => s.Title);
            //        break;
            //    case "Title desc":
            //        Insights = Articles.OrderByDescending(s => s.Title);
            //        break;
            //    case "Date":
            //        Insights = Articles.OrderBy(s => s.DatePublished);
            //        break;
            //    default:
            //        Insights = Articles.OrderByDescending(s => s.DatePublished);
            //        break;
            //}
            //int pageSize = 3;
            //int pageNumber = (page ?? 1);


            //int _sayfaNo = SayfaNo ?? 1;
            // var MusteriListesi = Veri.Musterilers.OrderByDescending(m => m.MusteriID).ToPagedList<Musteriler>(_sayfaNo, 10);

            //var model = (from pp in db.Products
            //             join pr in db.productPicture on pp.Id equals pr.productId
            //             select new ViewModelUrun
            //             {
            //                 productPicture = pr.productPicture1,
            //                 Id = pp.Id,
            //                 price = pp.price,
            //                 stockNumber = pp.stockNumber,
            //                 productName = pp.productName,
            //                 discountRate = pp.discountRate,
            //                 productPoint = pp.productPoint,
            //                 IsitApproved = pp.IsitApproved

            //             }
            //    ).Where(x => x.IsitApproved == true).OrderByDescending(m => m.productPoint).ToPagedList<ViewModelUrun>(_sayfaNo, 10);
            return View(model);
        }

        public ActionResult SolCategoriesistele()
        {
            var model = db.Categories.ToList(); //.Where(x => x.topCategoryId == 0)
            return View(model);
        }

        public ActionResult AltKategoriListele(int id)
        {
            var model = db.Categories.Where(x => x.topCategoryId == id).ToList();
            return Json(new {result = "ok", data = model}, JsonRequestBehavior.AllowGet);
            return View(model);

        }


        public ActionResult UrunAyrinti(int id)
        {
            var urun = (from pp in db.Products
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
                        IsitApproved = pp.IsitApproved,
                        categoryId = pp.categoryId

                    }
                ).Where(x => x.Id == id).FirstOrDefault();

            return View(urun);

        }
    }
}