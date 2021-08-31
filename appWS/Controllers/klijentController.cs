using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using apiWS.Models;
using System.Text;


namespace appWS.Controllers
{
    public class klijentController : Controller
    {
        // GET: klijent
        public ActionResult Index()
        {
            ViewBag.ddlKategorija = UzmiKategoriju();
            ViewBag.kat = prikaziKategorije();
            return View();
        }

        public IEnumerable<SelectListItem> UzmiKategoriju()
        {
            using (var db = new WSbazaEntities())
            {
                List<SelectListItem> kategorije = db.Kategorijas
                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.ID.ToString(),
                            Text = n.NAZIV
                        }).ToList();
                //var katPrva = new SelectListItem()
                //{
                //    Value = null,
                //    Text = "--- Odaberi kategoriju ---"
                //};
                //kategorije.Insert(0, katPrva);
                return new SelectList(kategorije, "Value", "Text");
            }
        }
        public string prikaziKategorije()
        {
            using (var db = new WSbazaEntities())
            {
                var ukupnoProizvoda = db.Proizvods.Select(x => x.Id).ToList().Count();

                var brojiKategorije = db.Database.SqlQuery<joinTable>("select count(PROIZVOD.ID_KAT)as broj, Kategorija.NAZIV as kn, Kategorija.ID as ik from proizvod join kategorija on PROIZVOD.ID_KAT = Kategorija.ID group by kategorija.naziv,kategorija.ID").ToList();

                StringBuilder sb = new StringBuilder();
                int brojac = 1;

                sb.Append("<input type ='radio' id = 'odabir' name='odabir' value='0' checked> Sve ("+ ukupnoProizvoda + ") <br/>");
               
                foreach (var pro in brojiKategorije)
                {                
                    sb.Append("<input type ='radio' id = 'odabir' name='odabir' value=" + brojac + "> " + pro.kn + " (" + pro.broj + ")  <br/>");

                    brojac++;
                                
                }
                return sb.ToString();
            }
        }

    }
}