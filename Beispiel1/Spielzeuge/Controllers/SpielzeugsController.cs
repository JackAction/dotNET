using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Spielzeuge.Models;
using System.Globalization;

namespace Spielzeuge.Controllers
{
    public class SpielzeugsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Spielzeugs
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (TempData["error"] != null)
            {
                ViewBag.ErrorMsg = TempData["error"].ToString();
            }
            return View(db.Spielzeugs.Where(s => s.Aktiv == true).Include(r => r.Reservierungen).ToList());
        }

        // POST: Spielzeugs/Index/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string datumVon, string datumBis)
        {
            if (datumVon == "" || datumBis == "")
            {
                return RedirectToAction("Index");
            }
            DateTime DatumVon = DateTime.ParseExact(datumVon, "MM/dd/yyyy", CultureInfo.CurrentCulture);
            DateTime DatumBis = DateTime.ParseExact(datumBis, "MM/dd/yyyy", CultureInfo.CurrentCulture);

            return View(db.Spielzeugs.Where(s => s.Aktiv == true).Include(r => r.Reservierungen).Where(z => !z.Reservierungen.Any(b => b.DatumVon <= DatumBis && b.DatumBis >= DatumVon)).ToList());
        }

        // GET: Spielzeugs
        [Authorize(Roles = "Admin")]
        public ActionResult IndexAdmin()
        {
            return View(db.Spielzeugs.Include(r => r.Reservierungen).ToList());
        }

        // GET: Spielzeugs/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id, string datumVon, string datumBis)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spielzeug spielzeug = db.Spielzeugs.Include(r => r.Reservierungen).SingleOrDefault(c => c.SpielzeugId == id);
            if (spielzeug == null)
            {
                return HttpNotFound();
            }
            return View(spielzeug);
        }

        // POST: Spielzeugs/Details/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReserveIndex([Bind(Include = "SpielzeugId,Name,Preis,Details,Aktiv,Ausgeliehen")] Spielzeug spielzeug, string datumVon, string datumBis)
        {
            DateTime DatumVon = DateTime.ParseExact(datumVon, "MM/dd/yyyy", CultureInfo.CurrentCulture);
            DateTime DatumBis = DateTime.ParseExact(datumBis, "MM/dd/yyyy", CultureInfo.CurrentCulture);

            if (DatumVon > DatumBis)
            {
                TempData["error"] = "1. Datum muss vor 2. Datum sein.";
                return RedirectToAction("Index", new { datumVon = datumVon, datumBis = datumBis });
            }
            if (ModelState.IsValid)
            {
                ReserveOnDB(spielzeug, DatumVon, DatumBis);
                return RedirectToAction("Index", new { datumVon = datumVon, datumBis = datumBis });
            }
            return RedirectToAction("Index", new { datumVon = datumVon, datumBis = datumBis });
        }

        // POST: Spielzeugs/Details/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReserveDetail([Bind(Include = "SpielzeugId,Name,Preis,Details,Aktiv,Ausgeliehen")] Spielzeug spielzeug, string datumVon, string datumBis)
        {
            DateTime DatumVon = DateTime.ParseExact(datumVon, "MM/dd/yyyy", CultureInfo.CurrentCulture);
            DateTime DatumBis = DateTime.ParseExact(datumBis, "MM/dd/yyyy", CultureInfo.CurrentCulture);

            if (ModelState.IsValid)
            {
                ReserveOnDB(spielzeug, DatumVon, DatumBis);
                return RedirectToAction("Details", new { id = spielzeug.SpielzeugId, datumVon = datumVon, datumBis = datumBis });
            }
            return RedirectToAction("Details", new { id = spielzeug.SpielzeugId, datumVon = datumVon, datumBis = datumBis });
        }

        private void ReserveOnDB(Spielzeug spielzeug, DateTime datumVon, DateTime datumBis)
        {
            Reservierung reservierung = new Reservierung()
            {
                SpielzeugId = spielzeug.SpielzeugId,
                Spielzeug = spielzeug,
                DatumVon = datumVon,
                DatumBis = datumBis
            };
            db.Reservierungs.Add(reservierung);
            if (spielzeug.Reservierungen == null)
            {
                spielzeug.Reservierungen = new List<Reservierung>();
            }
            spielzeug.Reservierungen.Add(reservierung);
            db.Entry(spielzeug).State = EntityState.Modified;
            db.SaveChanges();
        }


        // GET: Spielzeugs/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult DetailsAdmin(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spielzeug spielzeug = db.Spielzeugs.Find(id);
            if (spielzeug == null)
            {
                return HttpNotFound();
            }
            return View(spielzeug);
        }

        // GET: Spielzeugs/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Spielzeugs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "SpielzeugId,Name,Preis,Details,Aktiv,Ausgeliehen")] Spielzeug spielzeug)
        {
            if (ModelState.IsValid)
            {
                db.Spielzeugs.Add(spielzeug);
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }

            return View(spielzeug);
        }

        // GET: Spielzeugs/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spielzeug spielzeug = db.Spielzeugs.Find(id);
            if (spielzeug == null)
            {
                return HttpNotFound();
            }
            return View(spielzeug);
        }

        // POST: Spielzeugs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "SpielzeugId,Name,Preis,Details,Aktiv,Ausgeliehen")] Spielzeug spielzeug)
        {
            if (ModelState.IsValid)
            {
                db.Entry(spielzeug).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }
            return View(spielzeug);
        }

        // GET: Spielzeugs/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spielzeug spielzeug = db.Spielzeugs.Find(id);
            if (spielzeug == null)
            {
                return HttpNotFound();
            }
            return View(spielzeug);
        }

        // POST: Spielzeugs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Spielzeug spielzeug = db.Spielzeugs.Find(id);
            db.Spielzeugs.Remove(spielzeug);
            db.SaveChanges();
            return RedirectToAction("IndexAdmin");
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
