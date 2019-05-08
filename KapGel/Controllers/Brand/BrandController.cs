using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using KapGel.Models.EntityFramework;

namespace KapGel.Controllers.Brand
{
    public class BrandController : Controller
    {
        // GET: Brand
        KapGelEntities db = new KapGelEntities();
        public ActionResult Index()
        {
            var model = db.Brands.ToList();
            return View(model);

        }
        [HttpGet]

        public ActionResult MarkaEkleGuncelle(int id)
        {
            if (id == 0)
            {
                return View(new Brands());
            }
            else
            {
                var model = db.Brands.Find(id);
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult MarkaEkleGuncelle(Brands brd, HttpPostedFileBase picture)
        {
            if (brd.Id == 0)
            {
                Brands newBrands = new Brands()
                {
                    Id = brd.Id,
                    brandName = brd.brandName,
                    brandPicture = brd.brandPicture,

                };

                db.Brands.Add(newBrands);
                var fileName = Path.GetFileName(picture.FileName);


                //store file in the Books folder
                var path = Path.Combine(Server.MapPath("~/Uploads/brandPicture"), fileName);
                try
                {
                    picture.SaveAs(path);
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
                var markaVarmi = db.Brands.Find(brd.Id);
                if (markaVarmi != null)
                {
                    markaVarmi.Id = brd.Id;
                    markaVarmi.brandName = brd.brandName;
                    markaVarmi.brandPicture = brd.brandPicture;


                    db.Entry(markaVarmi);
                    db.SaveChanges();
                }
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MarkaSil(int id)
        {

            var markaVarmi = this.db.Brands.Find(id);
            if (markaVarmi != null)
            {
                this.db.Brands.Remove(markaVarmi);
                this.db.SaveChanges();
                return RedirectToAction("Index", "Marka");
                // return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "error", message = "Marka Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }


        }
    }
}