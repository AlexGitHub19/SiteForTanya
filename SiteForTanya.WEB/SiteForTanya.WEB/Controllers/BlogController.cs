﻿using SiteForTanya.WEB.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using SiteForTanya.Models;
using SiteForTanya.DAL.EntityFramework;

namespace SiteForTanya.WEB.Controllers
{
    public class BlogController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.Title = "Blog";
                ViewBag.Controller = "Blog";
                return View();
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }            
        }

        [HttpGet]
        public ActionResult BlogItem(string blogName)
        {
            try
            {
                Repository<BlogEntity> blogRepository = new Repository<BlogEntity>();
                BlogEntity blogEntity = blogRepository.GetList().First(b => b.Name == blogName);
                ViewBag.Title = "Blog " + blogName;
                ViewBag.Controller = "Blog";
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