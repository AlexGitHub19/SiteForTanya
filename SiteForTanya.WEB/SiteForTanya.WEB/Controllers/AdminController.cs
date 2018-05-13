using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteForTanya.WEB.Controllers
{
    public class AdminController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.ViewName = "AdminIndex";
            return View();
        }

        [Authorize]
        public ActionResult Sets()
        {
            ViewBag.ViewName = "AdminSets";
            return View();
        }

        [Authorize]
        public ActionResult Images()
        {
            ViewBag.ViewName = "AdminImages";
            return View();
        }
        [Authorize]
        public ActionResult Blog()
        {
            ViewBag.ViewName = "AdminBlog";
            return View();
        }

        [Authorize]
        public ActionResult AddSet()
        {
            ViewBag.ViewName = "AdminAddSet";
            return View();
        }

        [Authorize]
        public ActionResult SaveSet(string setName, string setDescription, string resultHtml, IEnumerable<HttpPostedFileBase> uploads)
        {
            string path = Server.MapPath("~/Content/Images/Sets/"+ setName);
            DirectoryInfo setDirectory = new DirectoryInfo(path);
            setDirectory.Create();
            if (uploads != null)
            {
                foreach (var file in uploads)
                {
                    if (file != null)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        file.SaveAs(Server.MapPath("~/Content/Images/Sets/" + setName + "/" + fileName));
                    }
                }
            }            
            ViewBag.ViewName = "AdminSuccessfulSetSaving";
            string html = resultHtml.Replace("&lt;" , "<").Replace("&gt;" , ">"); ;
            ViewBag.Alex = html;
            return View("SuccessfulSetSaving");
        }

        public JsonResult CheckSetName(string setName)
        {
            string path = Server.MapPath("~/Content/Images/Sets/" + setName);
            DirectoryInfo setDirectory = new DirectoryInfo(path);
            if (setDirectory.Exists)
            {
                return Json(new { result = "False" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = "True" }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}