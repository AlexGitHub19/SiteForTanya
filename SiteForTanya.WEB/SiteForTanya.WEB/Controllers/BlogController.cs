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
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ViewName = "BlogIndex";
            ViewBag.Title = "Blog";
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
                ViewBag.Title = "Blog " + blogName;
                return View(blogEntity);
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        public ActionResult GetBlogInfos(int pageNumber)
        {           
            try
            {
                BlogProcessor blogProcessor = new BlogProcessor();
                return Json(blogProcessor.GetBlogInfos(pageNumber), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        private ActionResult ProcessException(Exception exception)
        {
            ExceptionProcessor exceptionProcessor = new ExceptionProcessor();
            exceptionProcessor.process(exception);
            return View("Exception");
        }
    }
}