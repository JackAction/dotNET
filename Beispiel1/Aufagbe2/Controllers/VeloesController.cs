using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Aufagbe2.Models;

namespace Aufagbe2.Controllers
{
    public class VeloesController : Controller
    {
        private StudentContext db = new StudentContext();

        // GET: Veloes
        public ActionResult Index()
        {
            return View(db.Veloes.ToList());
        }

        // GET: Veloes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Velo velo = db.Veloes.Find(id);
            if (velo == null)
            {
                return HttpNotFound();
            }
            return View(velo);
        }

        // GET: Veloes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Veloes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VeloId,Description")] Velo velo)
        {
            if (ModelState.IsValid)
            {
                db.Veloes.Add(velo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(velo);
        }

        // GET: Veloes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Velo velo = db.Veloes.Find(id);
            if (velo == null)
            {
                return HttpNotFound();
            }
            return View(velo);
        }

        // POST: Veloes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VeloId,Description")] Velo velo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(velo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(velo);
        }

        // GET: Veloes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Velo velo = db.Veloes.Find(id);
            if (velo == null)
            {
                return HttpNotFound();
            }
            return View(velo);
        }

        // POST: Veloes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Velo velo = db.Veloes.Find(id);
            db.Veloes.Remove(velo);
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
