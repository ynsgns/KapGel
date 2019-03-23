using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KapGel.Controllers
{
    using KapGel.Models.EntityFramework;

    public class TokenController : Controller
    {
        // GET: Token 
        public  string IsimGetir()
        {
            string isim = "";
            string token = System.Web.HttpContext.Current.Request.Cookies["Token"].Value;
            if (token != "" && token != null)
            {
                KapGelEntities db = new KapGelEntities();
                var model = db.Token.FirstOrDefault(x => x.tokenCode == token);
                if (model != null) isim = db.Users.FirstOrDefault(x => x.id == model.Id).NameSurname;
                else
                {
                    TokenSil();
                }
            }

            return isim;
        }
        public  int YetkiGetir()
        {
            int yetki = 0;
            string token = System.Web.HttpContext.Current.Request.Cookies["Token"].Value;
            if (token != "" && token != null)
            {
                KapGelEntities db = new KapGelEntities();
                var model = db.Token.FirstOrDefault(x => x.tokenCode == token);
                if (model != null)
                {
                    if (model.authority != null) yetki = (int)model.authority;
                }
                else
                {
                    TokenSil();
                }
            }
            return yetki;
        }
        public  int UserIdGetir()
        {
            int id = 0;
            string token = System.Web.HttpContext.Current.Request.Cookies["Token"].Value;
            if (token != "" && token != null)
            {
                KapGelEntities db = new KapGelEntities();
                var model = db.Token.FirstOrDefault(x => x.tokenCode == token);
                if (model != null) id = (int)model.userId;
                else
                {
                    TokenSil();
                }
            }

            return id;
        }
        public  void TokenOlustur(string id, string yetki)
        {
            KapGelEntities db = new KapGelEntities();
            int usersId = int.Parse(id);
            var kullaniciVarmi = db.Token.Where(x => x.userId == usersId).FirstOrDefault();
            if (kullaniciVarmi != null)
            {
                db.Token.Remove(kullaniciVarmi);
                db.SaveChanges();
            }
            string token = "";
            do
            {
                token = Guid.NewGuid().ToString();
            }
            while (db.Token.Where(x => x.tokenCode == token).FirstOrDefault()?.tokenCode != null);
            var users = db.Users.Find(id);
            Token tkn = new Token()
            {
                tokenCode = token,
                authority = users.authority,
                userId = users.id,
            };
            db.Token.Add(tkn);
            db.SaveChanges();
            HttpCookie userToken = new HttpCookie("Token", token);
            userToken.Expires = DateTime.Now.AddHours(12);
            System.Web.HttpContext.Current.Response.Cookies.Add(userToken);
        }
        public  void TokenSil()
        {
            string token = System.Web.HttpContext.Current.Request.Cookies["Token"].Value;
            KapGelEntities db = new KapGelEntities();
            var model = db.Token.FirstOrDefault(x => x.tokenCode == token);
            if (model != null)
            {
                db.Token.Remove(model);
                var a = new HttpCookie("Token");
                a.Expires = DateTime.Now.AddHours(-12);
                System.Web.HttpContext.Current.Response.Cookies.Add(a);
            }

        }
    }
}