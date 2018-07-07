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
            ViewBag.ViewName = "WorkIndex";
            return View();
        }

        [HttpGet]
        public ActionResult Sets()
        {
            ViewBag.ViewName = "WorkSets";
            return View();
        }

        [HttpGet]
        public ActionResult Images()
        {
            ViewBag.ViewName = "WorkImages";
            return View();
        }

        [HttpGet]
        public ActionResult Set(string name)
        {
            Repository<SetEntity> setRepostory = new Repository<SetEntity>();
            SetEntity set = setRepostory.GetList().FirstOrDefault(s=>s.Name == name);
            if (set == null)
            {
                return RedirectToAction("Home");
            }


            ViewBag.ViewName = "WorkSet";
            SetVewModel vm = new SetVewModel { Name = name, Html = set.HtmlWithoutNotResultElements};
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetSetsNames(string keyWords, int setsCountOnPage, int pageNumber)
        {
            SetProcessor setProcessor = new SetProcessor();
            return Json(setProcessor.GetSetsNames(keyWords, setsCountOnPage, pageNumber), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetImagesNames(string keyWords, int imagesCountOnPage, int pageNumber)
        {
            ImageProcessor imageProcessor = new ImageProcessor();
            return Json(imageProcessor.GetImagesNames(keyWords, imagesCountOnPage, pageNumber), JsonRequestBehavior.AllowGet);
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
    }
}