using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteForTanya.WEB.Controllers
{
    public class WorkController : Controller
    {
        // GET: Work
        public ActionResult Index()
        {
            ViewBag.ViewName = "WorkIndex";
            return View();
        }

        public ActionResult Sets()
        {
            ViewBag.ViewName = "WorkSets";
            return View();
        }
        public ActionResult SingleImages()
        {
            ViewBag.ViewName = "SingleImages";
            return View();
        }
    }
}