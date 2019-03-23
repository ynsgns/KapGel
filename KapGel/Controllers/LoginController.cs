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
    public class LoginController : Controller
    {
        private object db;

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Giris(Users usr)
        {
            KapGelEntities db = new KapGelEntities();
            var model = db.Users.Where(x => x.eMail.Equals(usr.eMail)).FirstOrDefault();
            if (model != null)
            {
                if (model.password == usr.password)
                {
                    return Json(new { result = "Giris Ba sarılı" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "Şifre hatalı" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = "Kayıtlı böyle bir email adresi bulanamadı" }, JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpGet]
        // public ActionResult SifremiUnuttum()
        //{
        //    return View();
        //}
        //[HttpPost]
        public ActionResult SifremiUnuttum(/*string email*/)
        {
            KapGelEntities db = new KapGelEntities();

            //  var model = db.Users.Where(x => x.eMail.Equals(email)).FirstOrDefault();
            //if (model != null)
            //{
            Random rnd = new Random();
            int kod = rnd.Next(100000, 999999);

            //    db.SaveChanges(); //kaydettik

            //    SmtpClient client = new SmtpClient("smtp.live.com", 587);
            //    MailAddress from = new MailAddress("begum-2018@hotmail.com");
            //    MailAddress to = new MailAddress("yunusgunes3556@gmail.com");// userin mailini yazdık
            //    MailMessage msg = new MailMessage(from, to);
            //    msg.IsBodyHtml = true;
            //    msg.Subject = "Şifre Degiştirme İsteği Bildirimi";
            //    msg.Body += "<h2>  Merhaba " + "adasdf" + " Şifre Degiştirme İsteğiniz Alınmıştır.  Kodunuz" + rnd.ToString().Substring(0, 6) + "  Sitemize girerek şifrenizi Güncelleyiniz </h2>  </br>  "; //randomkeyimizi 6 karatere düşdük
            //    NetworkCredential info = new NetworkCredential("begum-2018@hotmail.com", "Firat1995.");
            //// kod çalışıyor mail taraflı izinlerden hata var
            //    client.Credentials = info;
            //    client.Send(msg); ///gönderdik
            ////}


            SmtpClient sc = new SmtpClient();
            sc.Port = 587;
            sc.Host = "smtp-mail.outlook.com";
            sc.EnableSsl = true;
            sc.Credentials = new NetworkCredential("begum-2018@hotmail.com", "Firat1995.");
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("begum-2018@hotmail.com", "Begüm KARATAŞ");

            mail.To.Add("begumkaratas18@gmail.com");
            mail.To.Add("yunusgunes3556@gmail.com");

            mail.CC.Add("yunusgunes3556@gmail.com");
            mail.CC.Add("begumkaratas18@gmail.com");

            mail.Subject = "E-Posta Konusu";
            mail.IsBodyHtml = true;
            mail.Subject = "Şifre Degiştirme İsteği Bildirimi";
            mail.Body += "<h2>  Merhaba " + "Begüm KARATAŞ" + " Şifre Degiştirme İsteğiniz Alınmıştır.  Kodunuz" + kod.ToString() + "  Sitemize girerek şifrenizi Güncelleyiniz </h2>  </br>  "; //randomkeyimizi 6 karatere düşdük


            //mail.Attachments.Add(new Attachment(@"C:\Rapor.xlsx"));
            //mail.Attachments.Add(new Attachment(@"C:\Sonuc.pptx"));

            sc.Send(mail);

            return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public ActionResult SifremiYenile()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult SifremiYenile(string email, string sifre, int kod)
        {
            KapGelEntities db = new KapGelEntities();
            var model = db.Users.Where(x => x.eMail.Equals(email)).FirstOrDefault();
            if (model != null)
            {
                var yenileme = db.ForgotPassword.FirstOrDefault(x => x.userId == model.id && x.forgetKeys == kod);
                if (yenileme != null)
                {

                    db.ForgotPassword.Where(x => x.userId == model.id && x.forgetKeys == kod);
                    Users usr = new Users();
                    usr = db.Users.Find(model.id);
                    usr.password = sifre;
                    db.Entry(usr);
                    return RedirectToAction("Index", "Login");
                }
                return RedirectToAction("SifremiYenile", "Login");
            }
            return RedirectToAction("SifremiYenile", "Login");

        }
    }
}
