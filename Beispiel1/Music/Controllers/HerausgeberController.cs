using Music.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Music.Controllers
{
    public class HerausgeberController : Controller
    {
        private MusicContext db = new MusicContext();

        // GET: Herausgeber
        public ActionResult Index()
        {
            var herausgeberList = db.Herausgebers.ToList();

            //herausgeberList.Add(new Herausgeber { HerausgeberId = 1, Name = "Sony"});
            //herausgeberList.Add(new Herausgeber { HerausgeberId = 2, Name = "nöd Sony" });

            return View(herausgeberList);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Create2(Herausgeber herausgeber)
        {
            db.Herausgebers.Add(herausgeber);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}