using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models.EntityFramework;
using System.Net.Mail;
using System.Net;

namespace KapGel.Controllers
{
    public class AdminController : Controller
    {
        KapGelEntities db = new KapGelEntities();
       
        // GET: Admin
        public ActionResult Index()
        {
            var model = db.Users.ToList();
            return View(model);
        }
        public ActionResult ekle()
        {
            return View();
        }

    }
}
