using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteForTanya.WEB.Controllers
{
    public class ContactsController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.ViewName = "ContactsIndex";
            ViewBag.Title = "Contacts";
            return View();
        }
    }
}