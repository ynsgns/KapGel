using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;
using KapGel.Models;


namespace KapGel.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

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
                ).Where(x => x.IsitApproved == true).ToList();
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
    }
}