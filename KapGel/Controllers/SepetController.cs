using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KapGel.Models;
using KapGel.Models.EntityFramework;

namespace KapGel.Controllers
{
    public class SepetController : Controller
    {
        // GET: Sepet
        KapGelEntities db = new KapGelEntities();

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SepetListe(string id)
        {

            string[] sepet = id.Split(',');
            string[] Liste;
            //for (int i = 0; i < sepet.Length; i++)
            //{
            //    a = sepet[i];
            //}

            List<SepetModel> sptListe = new List<SepetModel>();


            int tekrar = 0;
            for (int i = 0; i < sepet.Length; i++)
            {
                tekrar = 0;
                for (int j = i; j < sepet.Length; j++)
                {
                    if (sepet[i] == sepet[j]) tekrar++;
                }

                if (tekrar >= 1)
                {

                    bool varmi = false;
                    foreach (var s in sptListe)
                    {
                        int iid = int.Parse(sepet[i].ToString());
                        if (s.UrunId == iid)
                        {
                            varmi = true;
                            break;
                        }
                    }

                    if (varmi == false)
                    {
                        SepetModel sptYdk = new SepetModel();
                        sptYdk.UrunId = int.Parse(sepet[i].ToString());
                        sptYdk.Adet = tekrar;
                        sptListe.Add(sptYdk);
                    }
                    else
                    {
                        varmi = false;

                    }



                }
            }

            //return Json(new { result = "ok", data = sptListe }, JsonRequestBehavior.AllowGet);

            List<ViewModelUrun> urunlerList = new List<ViewModelUrun>();
            foreach (var i in sptListe)
            {
                ViewModelUrun urunn = (from pp in db.Products
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
                                           categoryId = pp.categoryId,
                                           adet = i.Adet

                                       }
                    ).Where(x => x.Id == i.UrunId).FirstOrDefault();
                urunlerList.Add(urunn);
            }

            return View(urunlerList);

            //   return View(sptListe);
        }
    }
}