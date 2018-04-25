using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteForTanya.WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Work()
        {
            ViewBag.Message = "My work";

            return View();
        }

        public ActionResult Contacts()
        {
            ViewBag.Message = "My    contact page.";

            return View();
        }

        public ActionResult Blog()
        {
            ViewBag.Message = "My Blog";

            return View();
        }

        public ActionResult Product(string productName)
        {
            ViewBag.Message = "Pruduct page";

            return View(productName);
        }

    }
}