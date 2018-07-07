using SiteForTanya.WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteForTanya.WEB.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        public ActionResult Index()
        {
            ViewBag.ViewName = "BlogIndex";
            return View();
        }

        [HttpGet]
        public ActionResult BlogItem(string blogName)
        {
            try
            {
                Repository<BlogEntity> blogRepository = new Repository<BlogEntity>();
                BlogEntity blogEntity = blogRepository.GetList().First(b => b.Name == blogName);
                ViewBag.ViewName = "BlogBlogItem";
                return View(blogEntity);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Exception");
            }
        }

        [HttpGet]
        public ActionResult GetBlogInfos(int pageNumber)
        {
            BlogProcessor blogProcessor = new BlogProcessor();
            return Json(blogProcessor.GetBlogInfos(pageNumber), JsonRequestBehavior.AllowGet);
        }
    }
}