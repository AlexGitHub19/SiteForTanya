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
            ViewBag.ViewName = "HomeIndex";
            return View();
        }

        public ActionResult Product(string id)
        {
            ViewBag.ViewName = "HomeProduct";
            ViewBag.Message = "Pruduct page";

            return View(id);
        }

    }
}