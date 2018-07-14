using System.Web.Mvc;

namespace SiteForTanya.WEB.Controllers
{
    public class ContactsController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Contacts";
            return View();
        }
    }
}