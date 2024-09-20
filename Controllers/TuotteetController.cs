using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TilausDBApp.Models;
using PagedList;

namespace TilausDBApp.Controllers
{
    public class TuotteetController : Controller
    {
        private TilausDBEntities1 db = new TilausDBEntities1();

        // GET: Tuotteet
        public ActionResult Index(string sortOrder, string currentFilter1, string searchString1, int? page, int? pagesize)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.ProductNameSortParm = String.IsNullOrEmpty(sortOrder) ? "productname_desc" : "";
            ViewBag.UnitPriceSortParm = sortOrder == "UnitPrice" ? "UnitPrice_desc" : "UnitPrice";

            //Hakufiltterin laitto muistiin
            if (searchString1 != null)
            {
                page = 1;
            } else
            {
                searchString1 = currentFilter1;
            }

            ViewBag.currentFilter1 = searchString1;

            var tuotteet = from p in db.Tuotteet
                           select p;
            if (!String.IsNullOrEmpty(searchString1)) //Jos hakufiltteri on käytössä, niin käytetään sitä ja sen lisäksi lajitellaan tulokset
            {
                switch (sortOrder)
                {
                    case "productname_desc":
                        tuotteet = tuotteet.Where(p => p.Nimi.Contains(searchString1)).OrderByDescending(p => p.Nimi);
                        break;
                    case "UnitPrice":
                        tuotteet = tuotteet.Where(p => p.Nimi.Contains(searchString1)).OrderBy(p => p.Ahinta);
                        break;
                    case "UnitPrice_desc":
                        tuotteet = tuotteet.Where(p => p.Nimi.Contains(searchString1)).OrderByDescending(p => p.Ahinta);
                        break;
                    default:
                        tuotteet = tuotteet.Where(p => p.Nimi.Contains(searchString1)).OrderBy(p => p.Nimi);
                        break;
                }
                
            } else // Tässä hakufiltteri EI OLE käytössä, joten lajitellaan koko tulosjoukko ilman suodatuksia
            {
                switch (sortOrder)
                {
                    case "productname_desc":
                        tuotteet = tuotteet.OrderByDescending(p => p.Nimi);
                        break;
                    case "UnitPrice":
                        tuotteet = tuotteet.OrderBy(p => p.Ahinta);
                        break;
                    case "UnitPrice_desc":
                        tuotteet = tuotteet.OrderByDescending(p => p.Ahinta);
                        break;
                    default:
                        tuotteet = tuotteet.OrderBy(p => p.Nimi);
                        break;
                }
            };


            List<Categories> lstCategories = new List<Categories>();

            var categoryList = from cat in db.Categories
                               select cat;

            Categories tyhjaCategory = new Categories();
            tyhjaCategory.CategoryID = 0;
            tyhjaCategory.CategoryName = "";
            tyhjaCategory.CategoryIDCategoryName = "";
            lstCategories.Add(tyhjaCategory);

            foreach (Categories category in categoryList)
            {
                Categories yksiCategory = new Categories();
                yksiCategory.CategoryID = category.CategoryID;
                yksiCategory.CategoryName = category.CategoryName;
                yksiCategory.CategoryIDCategoryName = category.CategoryID.ToString() + " - " + category.CategoryName;
                //Taulun luokkamääritykseen Models-kansiossa piti lisätä tämä "uusi" kenttä = CategoryIDCategoryName
                lstCategories.Add(yksiCategory);
            }
            ViewBag.CategoryID = new SelectList(lstCategories, "CategoryID", "CategoryIDCategoryName", ProductCategory);






            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "OUT";
            }
            else ViewBag.LoggedStatus = "IN";
            //return View(db.Tuotteet.ToList());
            //return View(tuotteet);
            int pageSize = (pagesize ?? 10); //Tämä palauttaa sivukoon taikka jos pagesize on null, niin palauttaa koon 10 riviä per sivu
            int pageNumber = (page ?? 1); //Tämä palauttaa sivunumeron taikka jos page on null, niin palauttaa numeron yksi
            return View(tuotteet.ToPagedList(pageNumber, pageSize));
        }

        // GET: Tuotteet/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "OUT";
                return RedirectToAction("login", "home");
            }
            else ViewBag.LoggedStatus = "IN";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            if (tuotteet == null)
            {
                return HttpNotFound();
            }
            return View(tuotteet);
        }

        // GET: Tuotteet/Create
        public ActionResult Create()
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "OUT";
                return RedirectToAction("login", "home");
            }
            else ViewBag.LoggedStatus = "IN";
            return View();
        }

        // POST: Tuotteet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TuoteID,Nimi,Ahinta,Kuva,ImageLink")] Tuotteet tuotteet)
        {
            if (ModelState.IsValid)
            {
                db.Tuotteet.Add(tuotteet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tuotteet);
        }

        // GET: Tuotteet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "OUT";
                return RedirectToAction("login", "home");
            }
            else ViewBag.LoggedStatus = "IN";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            if (tuotteet == null)
            {
                return HttpNotFound();
            }
            return View(tuotteet);
        }

        // POST: Tuotteet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TuoteID,Nimi,Ahinta,Kuva,ImageLink")] Tuotteet tuotteet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tuotteet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tuotteet);
        }

        // GET: Tuotteet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserName"] == null)
            {
                ViewBag.LoggedStatus = "OUT";
                return RedirectToAction("login", "home");
            }
            else ViewBag.LoggedStatus = "IN";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            if (tuotteet == null)
            {
                return HttpNotFound();
            }
            return View(tuotteet);
        }

        // POST: Tuotteet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tuotteet tuotteet = db.Tuotteet.Find(id);
            db.Tuotteet.Remove(tuotteet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
