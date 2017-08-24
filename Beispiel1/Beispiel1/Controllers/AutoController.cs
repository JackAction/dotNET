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
            return Content("yuhuuu");
        }

        public ActionResult Message()
        {
            return Content("<script>alert('Welcome To All');</script>");
        }
    }
}