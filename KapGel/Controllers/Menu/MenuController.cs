using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;
using System.IO;

namespace KapGel.Controllers.Menu
{
    public class MenuController : Controller
    {
        KapGelEntities db = new KapGelEntities();
        // GET: Menu
        public ActionResult Index()
        {
            var model = db.Menus.ToList();
            return View(model);
        }

 
        [HttpGet]
        public ActionResult MenuEkleGuncelle(int id)
        {
            if (id == 0)
            {
                return View(new Menus());
            }
            else
            {
                var model = db.Menus.Find(id);
                return View(model);
            }
        }

        [HttpPost]
        public JsonResult MenuEkleGuncelle(Menus mn)
        {
            if (mn.Id == 0)
            {
                Menus newMenu = new Menus()
                {
                    Id = mn.Id,
                    menuName=mn.menuName,
                    link=mn.link,
                    sequence=mn.sequence,
                };

                db.Menus.Add(newMenu);
              

               
              
                    db.SaveChanges();
                
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var menuVarmi = db.Menus.Find(mn.Id);
                if (menuVarmi != null)
                {
                    menuVarmi.Id = mn.Id;
                    menuVarmi.menuName = mn.menuName;
                    menuVarmi.sequence = mn.sequence;
                    menuVarmi.link = mn.link;
                   
                    db.Entry(menuVarmi);
                    db.SaveChanges();
                }
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult MenuSil(int id)
        {

            var menuVarmi = this.db.Menus.Find(id);
            if (menuVarmi != null)
            {
                this.db.Menus.Remove(menuVarmi);
                this.db.SaveChanges();
                return RedirectToAction("Index", "Menu");
                // return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "error", message = "Menu Bulunamadı" }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}