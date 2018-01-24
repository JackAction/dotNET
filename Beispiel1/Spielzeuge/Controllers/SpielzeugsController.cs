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
using NinjaNye.SearchExtensions;

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
            var test = db.Spielzeugs.Where(s => s.Aktiv == true).Include(r => r.Reservierungen).Include(r => r.Bilder).ToList();
            return View(db.Spielzeugs.Where(s => s.Aktiv == true).Include(r => r.Reservierungen).Include(r => r.Bilder).ToList());
        }

        // POST: Spielzeugs/Index/5
        // Filtern
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Index(string datumVon, string datumBis, string search)
        {
            if (datumVon == "" || datumBis == "")
            {
                // NuGet Package für Suche:
                // http://ninjanye.github.io/SearchExtensions/stringsearch.html
                return View(db.Spielzeugs.Where(s => s.Aktiv == true).Include(r => r.Reservierungen).Include(r => r.Bilder).Search(x => x.Name, x => x.Details).Containing(search).ToList());
            }
            DateTime DatumVon = DateTime.ParseExact(datumVon, "MM/dd/yyyy", CultureInfo.CurrentCulture);
            DateTime DatumBis = DateTime.ParseExact(datumBis, "MM/dd/yyyy", CultureInfo.CurrentCulture);
            if (DatumVon > DatumBis)
            {
                ViewBag.ErrorMsg = "1. Datum muss vor 2. Datum sein.";
                return View(db.Spielzeugs.Where(s => s.Aktiv == true).Include(r => r.Reservierungen).Include(r => r.Bilder).ToList());
            }
            return View(db.Spielzeugs.Where(s => s.Aktiv == true).Include(r => r.Reservierungen).Include(r => r.Bilder).Where(z => !z.Reservierungen.Any(b => b.DatumVon <= DatumBis && b.DatumBis >= DatumVon)).Search(x => x.Name, x => x.Details).Containing(search).ToList());
        }

        // GET: Spielzeugs
        [Authorize(Roles = "Admin")]
        public ActionResult IndexAdmin()
        {
            if (TempData["error"] != null)
            {
                ViewBag.ErrorMsg = TempData["error"].ToString();
            }
            return View(db.Spielzeugs.Include(r => r.Reservierungen).Include(r => r.Bilder).ToList());
        }

        // GET: Spielzeugs/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id, string datumVon, string datumBis)
        {
            if (TempData["error"] != null)
            {
                ViewBag.ErrorMsg = TempData["error"].ToString();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Spielzeug spielzeug = db.Spielzeugs.Include(r => r.Reservierungen).Include(r => r.Bilder).SingleOrDefault(c => c.SpielzeugId == id);
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
            if (datumVon == "" || datumBis == "")
            {
                TempData["error"] = "Bitte Von und Bis Datum angeben um zu Reservieren.";
                return RedirectToAction("Index");
            }
            DateTime DatumVon = DateTime.ParseExact(datumVon, "MM/dd/yyyy", CultureInfo.CurrentCulture);
            DateTime DatumBis = DateTime.ParseExact(datumBis, "MM/dd/yyyy", CultureInfo.CurrentCulture);

            string errorMsg = Validation(spielzeug, DatumVon, DatumBis);
            if ( errorMsg != "")
            {
                TempData["error"] = errorMsg;
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
            if (datumVon == "" || datumBis == "")
            {
                TempData["error"] = "Bitte Von und Bis Datum angeben um zu Reservieren.";
                return RedirectToAction("Details", new { id = spielzeug.SpielzeugId });
            }
            DateTime DatumVon = DateTime.ParseExact(datumVon, "MM/dd/yyyy", CultureInfo.CurrentCulture);
            DateTime DatumBis = DateTime.ParseExact(datumBis, "MM/dd/yyyy", CultureInfo.CurrentCulture);

            string errorMsg = Validation(spielzeug, DatumVon, DatumBis);
            if (errorMsg != "")
            {
                TempData["error"] = errorMsg;
                return RedirectToAction("Details", new { id = spielzeug.SpielzeugId, datumVon = datumVon, datumBis = datumBis });
            }
            if (ModelState.IsValid)
            {
                ReserveOnDB(spielzeug, DatumVon, DatumBis);
                return RedirectToAction("Details", new { id = spielzeug.SpielzeugId, datumVon = datumVon, datumBis = datumBis });
            }
            return RedirectToAction("Details", new { id = spielzeug.SpielzeugId, datumVon = datumVon, datumBis = datumBis });
        }

        private string Validation(Spielzeug spielzeug, DateTime datumVon, DateTime datumBis)
        {
            if (datumVon > datumBis)
            {
                return "1. Datum muss vor 2. Datum sein.";
            }

            if (db.Spielzeugs.Include(r => r.Reservierungen).Any(z => z.SpielzeugId == spielzeug.SpielzeugId & z.Reservierungen.Any(b => b.DatumVon <= datumBis && b.DatumBis >= datumVon)))
            {
                return "Spielzeug bereits ausgeliehen in diesem Zeitraum.";
            }
            return "";
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
            Spielzeug spielzeug = db.Spielzeugs.Include(r => r.Reservierungen).Include(r => r.Bilder).SingleOrDefault(c => c.SpielzeugId == id);
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
            Spielzeug spielzeug = db.Spielzeugs.Include(r => r.Reservierungen).Include(r => r.Bilder).SingleOrDefault(c => c.SpielzeugId == id);
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
            Spielzeug spielzeug = db.Spielzeugs.Include(r => r.Reservierungen).Include(r => r.Bilder).SingleOrDefault(c => c.SpielzeugId == id);
            if (spielzeug == null)
            {
                return HttpNotFound();
            }
            if (spielzeug.Reservierungen.Count > 0)
            {
                TempData["error"] = "Spielzeug nicht löschbar, da Reservierungen existieren.";
                return RedirectToAction("IndexAdmin");
            }
            return View(spielzeug);
        }

        // POST: Spielzeugs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Spielzeug spielzeug = db.Spielzeugs.Include(r => r.Reservierungen).Include(r => r.Bilder).SingleOrDefault(c => c.SpielzeugId == id);
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
