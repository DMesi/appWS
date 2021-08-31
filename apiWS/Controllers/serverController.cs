using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;   // Microsoft.AspNet.WebApi.Cors - NUGET
using System.IO;
using System.Web;

using apiWS.Models;



namespace apiWS.Controllers
{
    public class serverController : ApiController
    {
        [Route("api/server/UzmiSveIzBaze")]
        [HttpGet()]
        public IEnumerable<Proizvod> UzmiSveIzBaze()
        {
            using (var db = new WSbazaEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                return db.Proizvods.ToList();

            }

        }


        [Route("api/server/UzmiSveIzBazeId/{id}")]
        [HttpGet()]
        public HttpResponseMessage UzmiSveIzBazeId(int id)
        {
            using (var db = new WSbazaEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                var proizvod = db.Proizvods.Where(z => z.Id_kat == id).ToList();

                if (proizvod != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, proizvod);
                }
                else
                {

                    return Request.CreateResponse(HttpStatusCode.NotFound, "Proizvod sa ID = " + id + " nije pronadjen! ");
                }
            }

        }


        [HttpPost]
        public HttpResponseMessage UploadFiles()
        {
            using (var db = new WSbazaEntities())
            {
                //Create the Directory.
                string path = HttpContext.Current.Server.MapPath("~/Slike/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Fetch the File.
                HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

                //Fetch the File Name.
                string fileName = HttpContext.Current.Request.Form["fileName"] + Path.GetExtension(postedFile.FileName);

                Proizvod noviP = new Proizvod
                {
                    Id_kat = Convert.ToInt32(HttpContext.Current.Request.Form["ID_kat"]),
                    Naziv = HttpContext.Current.Request.Form["naziv"],
                    Opis = HttpContext.Current.Request.Form["opis"],
                    Cena = Convert.ToDouble(HttpContext.Current.Request.Form["cena"]),
                    Lager = Convert.ToInt32(HttpContext.Current.Request.Form["lager"]),
                    Slika = fileName
                };

                db.Proizvods.Add(noviP);
                db.SaveChanges();


                //Save the File.
                postedFile.SaveAs(path + fileName);

                //Send OK Response to Client.
                return Request.CreateResponse(HttpStatusCode.OK, fileName);
            }
        }


        [HttpPut()]
        public HttpResponseMessage IzmeniProizvod()
        {
            try
            {
                using (var db = new WSbazaEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;

                    var ID_proizvoda = Convert.ToInt32(HttpContext.Current.Request.Form["ID"]);
                    //var uploadSlika = HttpContext.Current.Request.Form["ID"];

                    var proizvod = db.Proizvods.FirstOrDefault(z => z.Id == ID_proizvoda);

                    if (proizvod == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Proizvod sa ID " + ID_proizvoda + " nije pronadjen za izmenu!");
                    }
                    else
                    {
                        string path = HttpContext.Current.Server.MapPath("~/Slike/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        //Fetch the File.


                        if (HttpContext.Current.Request.Files.Count > 0)
                        {
                            HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];
                            //Fetch the File Name.
                            string fileName = HttpContext.Current.Request.Form["fileName"] + Path.GetExtension(postedFile.FileName);

                            proizvod.Id_kat = Convert.ToInt32(HttpContext.Current.Request.Form["ID_kat"]);
                            proizvod.Naziv = HttpContext.Current.Request.Form["naziv"];
                            proizvod.Opis = HttpContext.Current.Request.Form["opis"];
                            proizvod.Cena = Convert.ToDouble(HttpContext.Current.Request.Form["cena"]);
                            proizvod.Lager = Convert.ToInt32(HttpContext.Current.Request.Form["lager"]);
                            proizvod.Slika = fileName;

                            db.SaveChanges();
                            postedFile.SaveAs(path + fileName);
                            return Request.CreateResponse(HttpStatusCode.OK, proizvod);

                        }
                        else
                        {
                            proizvod.Id_kat = Convert.ToInt32(HttpContext.Current.Request.Form["ID_kat"]);
                            proizvod.Naziv = HttpContext.Current.Request.Form["naziv"];
                            proizvod.Opis = HttpContext.Current.Request.Form["opis"];
                            proizvod.Cena = Convert.ToDouble(HttpContext.Current.Request.Form["cena"]);
                            proizvod.Lager = Convert.ToInt32(HttpContext.Current.Request.Form["lager"]);


                            db.SaveChanges();

                            return Request.CreateResponse(HttpStatusCode.OK, proizvod);




                        }


                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //[HttpDelete()]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (var db = new WSbazaEntities())
                {
                    var proizvod = db.Proizvods.FirstOrDefault(z => z.Id == id);
                    if (proizvod == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                            "Nije pronadjen proizvod sa Id = " + id.ToString() + "");
                    }
                    else
                    {
                        db.Proizvods.Remove(proizvod);
                        db.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }



    }
}
