using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TilausDBApp.Models;
using TilausDBApp.ViewModels;

namespace TilausDBApp.Controllers
{
    public class OrdersController : Controller
    {
        private TilausDBEntities1 db = new TilausDBEntities1();

        // GET: Orders
        public ActionResult Index()
        {
            var tilaukset = db.Tilaukset.Include(t => t.Asiakkaat).Include(t => t.Postitoimipaikat);
            return View(tilaukset.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            return View(tilaukset);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi");
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Postinumero,Tilauspvm,Toimituspvm")] Tilaukset tilaukset)
        {
            if (ModelState.IsValid)
            {
                db.Tilaukset.Add(tilaukset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
            return View(tilaukset);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
            return View(tilaukset);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TilausID,AsiakasID,Toimitusosoite,Postinumero,Tilauspvm,Toimituspvm")] Tilaukset tilaukset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tilaukset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AsiakasID = new SelectList(db.Asiakkaat, "AsiakasID", "Nimi", tilaukset.AsiakasID);
            ViewBag.Postinumero = new SelectList(db.Postitoimipaikat, "Postinumero", "Postitoimipaikka", tilaukset.Postinumero);
            return View(tilaukset);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            if (tilaukset == null)
            {
                return HttpNotFound();
            }
            return View(tilaukset);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tilaukset tilaukset = db.Tilaukset.Find(id);
            db.Tilaukset.Remove(tilaukset);
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

        public ActionResult Ordersummary()
        {
            var orderSummary = from t in db.Tilaukset
                               join tr in db.Tilausrivit on t.TilausID equals tr.TilausID
                               join tu in db.Tuotteet on tr.TuoteID equals tu.TuoteID
                               join aa in db.Asiakkaat on t.AsiakasID equals aa.AsiakasID

                               select new OrderSummaryData
                               {
                                   TilausID = t.TilausID,
                                   AsiakasID = t.AsiakasID,
                                   Tilauspvm = (DateTime)t.Tilauspvm,
                                   Toimituspvm = (DateTime)t.Toimituspvm,
                                   TuoteID = tu.TuoteID,
                                   Ahinta = tr.Ahinta,
                                   Nimi = aa.Nimi,
                                   Postinumero = t.Postinumero,
                                   //WeekdayName = t.Tilauspvm.HasValue ? GetFinnishWeekday(t.Tilauspvm.Value) : string.Empty

                               };


            return View(orderSummary);
        }
        public ActionResult TilausOtsikot()
        {
            var tilaukset = db.Tilaukset.Include(t => t.Asiakkaat).Include(t => t.Postitoimipaikat);
            return View(tilaukset.ToList());
        }

        private string GetFinnishWeekday(DateTime date)
        {
            var culture = new System.Globalization.CultureInfo("fi-FI");
            return culture.DateTimeFormat.GetDayName(date.DayOfWeek);
        }

        public ActionResult _TilausRivit(int? tilausid)
        {
            var orderRowsList = from tr in db.Tilausrivit
                               join tu in db.Tuotteet on tr.TuoteID equals tu.TuoteID
                               join aa in db.Asiakkaat on tu.Ahinta equals aa.AsiakasID
                               where tr.TilausID == tilausid
                               select new OrderRows
                               {
                                   TilausID = tr.TilausID,
                                   TuoteID = tu.TuoteID,
                                   Ahinta = tu.Ahinta,
                                   Nimi = aa.Nimi,


                               };


            return PartialView(orderRowsList);
        }
    }
}
