using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beispiel1.Controllers
{
    public class AutoController : Controller
    {
        public string fahren()
        {
            return "faaahre";
        }

        public string schalteInGang(int gang)
        {
            return $"aktueller Gang: {gang}";
        }

        public ActionResult Index()
        {
            //return Content("yuhuuu");

            ViewBag.Tiiiitel = "hey title string";
            ViewBag.he = "asdf";
            
            return View();
        }

        public ActionResult Message()
        {
            return Content("<script>alert('Welcome To All');</script>");
        }


        public ActionResult MachSpoiler()
        {
            return View("CreateHeckspoiler");
        }

    }
}