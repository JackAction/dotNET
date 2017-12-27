using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beispiel1.Controllers
{
    public class LastwagenController : Controller
    {
        // GET: Lastwagen
        public ActionResult Index()
        {
            return View();
        }

        //[Route("Lastwagen/Stop/{Sekunden}")]
        //public ActionResult Stop(int Sekunden)
        //{

        //    ViewBag.Tiiiitel = "Stop";
        //    ViewBag.Sekunden = Sekunden;
        //    ViewBag.Teeext = $"in {Sekunden} Sekunden.";

        //    return View();
        //}


        // ohne Route
        // QueryString geht nur mit GET
        public ActionResult Stop()
        {
            ViewBag.Tiiiitel = "Stop";

            if (Request.QueryString.AllKeys.Contains("Sekunden"))
            {
                int Sekunden = int.Parse(Request.QueryString["Sekunden"]);

                ViewBag.Sekunden = Sekunden;
                ViewBag.Teeext = $"in {Sekunden} Sekunden.";

                return View();
            }
            else
            {
                return HttpNotFound("Sekunden parameter gits nid");
            }


   

        }


    }
}