using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Spielzeuge.Models;

namespace Spielzeuge.Controllers
{
    public class BildsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Show(int id)
        {
            var imageData = db.Bilds.SingleOrDefault(c => c.BildId == id).ImageByte;

        return File(imageData, "image/jpg");
        }


        // GET: Bilds
        public ActionResult Index()
        {
            var bilds = db.Bilds.Include(b => b.Spielzeug);
            return View(bilds.ToList());
        }

        // GET: Bilds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bild bild = db.Bilds.Find(id);
            if (bild == null)
            {
                return HttpNotFound();
            }
            return View(bild);
        }

        // GET: Bilds/Create
        public ActionResult Create()
        {
            ViewBag.SpielzeugId = new SelectList(db.Spielzeugs, "SpielzeugId", "Name");
            return View();
        }

        // POST: Bilds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BildId,SpielzeugId,ImageByte")] Bild bild, HttpPostedFileBase file)
        {
            if (file == null)
            {
                return RedirectToAction("Edit", "Spielzeugs", new { id = bild.SpielzeugId });
            }
            if (ModelState.IsValid)
            {
                bild.ImageByte = new byte[file.ContentLength];
                file.InputStream.Read(bild.ImageByte, 0, file.ContentLength);
 
                db.Bilds.Add(bild);
                db.SaveChanges();
                return RedirectToAction("Edit", "Spielzeugs", new { id = bild.SpielzeugId });
            }

            ViewBag.SpielzeugId = new SelectList(db.Spielzeugs, "SpielzeugId", "Name", bild.SpielzeugId);
            return RedirectToAction("Edit", "Spielzeugs", new { id = bild.SpielzeugId });
        }

        // GET: Bilds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bild bild = db.Bilds.Find(id);
            if (bild == null)
            {
                return HttpNotFound();
            }
            ViewBag.SpielzeugId = new SelectList(db.Spielzeugs, "SpielzeugId", "Name", bild.SpielzeugId);
            return View(bild);
        }

        // POST: Bilds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BildId,SpielzeugId,ImageByte")] Bild bild)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bild).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SpielzeugId = new SelectList(db.Spielzeugs, "SpielzeugId", "Name", bild.SpielzeugId);
            return View(bild);
        }

        // GET: Bilds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bild bild = db.Bilds.Find(id);
            if (bild == null)
            {
                return HttpNotFound();
            }
            return View(bild);
        }

        // POST: Bilds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bild bild = db.Bilds.Include(s => s.Spielzeug).SingleOrDefault(c => c.BildId == id);
            db.Bilds.Remove(bild);
            db.SaveChanges();
            return RedirectToAction("Edit", "Spielzeugs", new { id = bild.SpielzeugId });
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
