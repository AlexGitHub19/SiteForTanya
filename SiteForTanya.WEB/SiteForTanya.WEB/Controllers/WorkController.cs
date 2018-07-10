using SiteForTanya.WEB.Models;
using SiteForTanya.WEB.Models.WorkViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteForTanya.WEB.Controllers
{
    public class WorkController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                ViewBag.Title = "Work";
                return View();
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        public ActionResult Sets()
        {
            try
            {
                ViewBag.Title = "Sets";
                return View();
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        public ActionResult Images()
        {
            try
            {
                ViewBag.Title = "Images";
                return View();
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        public ActionResult Set(string name)
        {         
            try
            {
                Repository<SetEntity> setRepostory = new Repository<SetEntity>();
                SetEntity set = setRepostory.GetList().FirstOrDefault(s => s.Name == name);

                ViewBag.Title = "Set " + name;
                SetVewModel vm = new SetVewModel { Name = name, Html = set.HtmlWithoutNotResultElements };
                return View(vm);
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        public ActionResult GetSetsNames(string keyWords, int setsCountOnPage, int pageNumber)
        {
            try
            {
                SetProcessor setProcessor = new SetProcessor();
                return Json(setProcessor.GetSetsNames(keyWords, setsCountOnPage, pageNumber), JsonRequestBehavior.AllowGet);
            }          
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        public ActionResult GetImagesNames(string keyWords, int imagesCountOnPage, int pageNumber)
        {            
            try
            {
                ImageProcessor imageProcessor = new ImageProcessor();
                return Json(imageProcessor.GetImagesNames(keyWords, imagesCountOnPage, pageNumber), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        public ActionResult SetsAutocompleteSearch(string term)
        {
            return Json(CommonMethods.AutocompleteSearch<SetsInfo>(term), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ImagesAutocompleteSearch(string term)
        {
            return Json(CommonMethods.AutocompleteSearch<ImagesInfo>(term), JsonRequestBehavior.AllowGet);
        }

        private ActionResult ProcessException(Exception exception)
        {
            ExceptionProcessor exceptionProcessor = new ExceptionProcessor();
            exceptionProcessor.process(exception);
            return View("Exception");
        }
    }
}