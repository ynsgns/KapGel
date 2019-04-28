using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;


namespace KapGel.Controllers
{
    public class AdressController : Controller
    {
        KapGelEntities db = new KapGelEntities();
        TokenController tkn = new TokenController();
        // GET: Adress
        public ActionResult Index()
        {
            int usrId = tkn.UserIdGetir();
            var model = db.Adress.Where(x=>x.UserId == usrId).ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult AdresEkleGuncelle(int id)
        {
            if (id == 0)
            {
                Adress yeni = new Adress();
               
             
                yeni.UserId = tkn.UserIdGetir();
                return View(yeni);
            }
            else
            {
                var model = db.Adress.Find(id);
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult AdresEkleGuncelle(Adress adr)
        {
            if (adr.Id == 0)
            {
                Adress newadress= new Adress();
            
                newadress.cityName = adr.cityName;
                newadress.districtName = adr.districtName;
                newadress.neighborhoodName = adr.neighborhoodName;
                newadress.UserId = adr.UserId;
                newadress.adress1 = adr.adress1;
                db.Adress.Add(newadress);
                db.SaveChanges();
                return RedirectToAction("Index"); 
            }
            else
            {
                var adresVarmi = db.Adress.Find(adr.Id);
                if (adresVarmi != null)
                {
                
                    adresVarmi.cityName = adr.cityName;
                    adresVarmi.districtName = adr.districtName;
                    adresVarmi.neighborhoodName = adr.neighborhoodName;
                    adresVarmi.UserId = adr.UserId;
                    adresVarmi.adress1 = adr.adress1;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
             
            }

        }
        public JsonResult AdresSil(int id) 
        {
            var adressVarmi = db.Adress.Find(id);
            if (adressVarmi != null)
            {

                this.db.Adress.Remove(adressVarmi);
                this.db.SaveChanges();
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "error", message = "Adres Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}