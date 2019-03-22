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
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    using KapGel.Models;

    using Newtonsoft.Json;

    public class LoginController : Controller
    {

        // GET: Login
        public ActionResult Index()
        {

            return View();
        }
        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
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
                    TokenController tk = new TokenController();
                    tk.TokenOlustur(model.id.ToString(), model.authority.ToString());
                    return Json(new { result = "Giris Basarılı" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    GirisSay();
                    if (IpEngelle()) return Json(new { result = "10 dan fazla hatalı giriş yaptığınız için 30 dakika giriş yapamazsınız." }, JsonRequestBehavior.AllowGet);
                    return Json(new { result = "Şifre hatalı" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                GirisSay();
                if (IpEngelle()) return Json(new { result = "10 dan fazla hatalı giriş yaptığınız için 30 dakika giriş yapamazsınız." }, JsonRequestBehavior.AllowGet);
                return Json(new { result = "Kayıtlı böyle bir email adresi bulanamadı" }, JsonRequestBehavior.AllowGet);
            }
        }

        public void GirisSay()
        {
            KapGelEntities db = new KapGelEntities();
            string ipAdrssAddress = GetIPAddress();
            var ipDb = db.Log.Where(x => x.ıpAdress.Equals(ipAdrssAddress)).FirstOrDefault();
            if (ipDb == null)
            {
                Log lg = new Log();
                lg.ıpAdress = ipAdrssAddress;
                lg.date = DateTime.Today;
                lg.falseInputCount = lg.falseInputCount++;
                db.SaveChanges();
            }
            else
            {

                ipDb.falseInputCount = ipDb.falseInputCount++;
                db.Entry(ipDb);
                db.SaveChanges();
            }
        }
        public bool IpEngelle()
        {
            KapGelEntities db = new KapGelEntities();
            string ipAdrssAddress = GetIPAddress();
            var ipDb = db.Log.Where(x => x.ıpAdress.Equals(ipAdrssAddress)).FirstOrDefault();
            if (ipDb != null)
            {
                if (ipDb.falseInputCount == 10)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void ResimOlustur()
        {
            string kod = "";
            kod = RastgeleVeriUret();
            //Üretilen kodu Session nesnesine aktarır.
            Session.Add("kod", kod);
            //Rastgele üretilen metini alıp resme dönüştürelim.
            //boş bir resim dosyası oluştur.
            Bitmap bmp = new Bitmap(100, 21);
            //Graphics sınıfı ile resmin kontrolunu alır.
            Graphics g = Graphics.FromImage(bmp);
            //DrawString 20‘ye 0 kordinatına kodu‘u yazdırır.
            g.DrawString(kod, new Font("Comic Sanns MS", 15), new SolidBrush(Color.Black), 20, 0);
            //Resmi binary olarak alıp sayfaya yazdırmak ıcın MemoryStream kullandık.
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            var base64Data = Convert.ToBase64String(ms.ToArray());
            var imageCode = "data:image/png;base64," + base64Data;
            g.Dispose();
            bmp.Dispose();
            ms.Close();
            ms.Dispose();
        }

        public string RastgeleVeriUret()
        {
            string deger = "";
            //Türkçe karakterleri kullanmaktan vazgeçtim.
            string dizi = "ABCDEFGHIJKLMNOPRSTUVYZ0123456789";
            Random r = new Random();
            //Toplam 6 karakterden oluşan rastgele bir metin oluşturalım.
            for (int i = 0; i < 5; i++)
            {
                deger = deger + dizi[r.Next(0, 33)];
            }
            return deger;
        }

        [HttpGet]
        public ActionResult SifremiUnuttum()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SifremiUnuttum(FormCollection fc)
        {
            KapGelEntities db = new KapGelEntities();
            string email = fc["email"];
            var response = Request["g-recaptcha-response"];
            const string secret = "6LeKKSMUAAAAAC4s-mflMky8XggtaatxKcx-cQ1y";
            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            if (!captchaResponse.Success)
            {
                TempData["Message"] = "Lütfen güvenliği doğrulayınız.";
                return this.View();
            }
            else
                TempData["Message"] = "Güvenlik başarıyla doğrulanmıştır.";


            var model = db.Users.Where(x => x.eMail.Equals(email)).FirstOrDefault();
            if (model != null)
            {
                Random rnd = new Random();
                int kod = rnd.Next(100000, 999999);


                SmtpClient sc = new SmtpClient();
                sc.Port = 587;
                sc.Host = "smtp-mail.outlook.com";
                sc.EnableSsl = true;
                sc.Credentials = new NetworkCredential("begum-2018@hotmail.com", "Firat1995.");
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("begum-2018@hotmail.com", "Begüm KARATAŞ");
                mail.To.Add(email);
                mail.CC.Add(email);
                mail.Subject = "E-Posta Konusu";
                mail.IsBodyHtml = true;
                mail.Subject = "Şifre Degiştirme İsteği Bildirimi";
                mail.Body += "<h2>  Merhaba " + "Begüm KARATAŞ"
                                              + " Şifre Degiştirme İsteğiniz Alınmıştır.  Kodunuz <span style='color:red;'> "
                                              + kod.ToString()
                                              + "</span>  Sitemize girerek şifrenizi Güncelleyiniz </h2>  </br>  "; //randomkeyimizi 6 karatere düşdük
                //mail.Attachments.Add(new Attachment(@"C:\Rapor.xlsx"));
                //mail.Attachments.Add(new Attachment(@"C:\Sonuc.pptx"));
                sc.Send(mail);
            }
            return RedirectToAction("SifremiUnuttum", "Login");
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
                var yenileme = db.ForgotPassword.FirstOrDefault(x => x.userId == model.id && x.fotgotKey == kod);
                if (yenileme != null)
                {

                    db.ForgotPassword.Where(x => x.userId == model.id && x.fotgotKey == kod);
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
