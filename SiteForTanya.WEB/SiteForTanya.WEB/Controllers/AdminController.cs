using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiteForTanya.WEB.Models;

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

        [HttpGet]
        [Authorize]
        public ActionResult DeleteSet()
        {
            ViewBag.ViewName = "AdminDeleteSet";
            Repository<SetEntity> repository = new Repository<SetEntity>();
            IEnumerable<SetEntity> sets =  repository.GetList();
            List<string> names = sets.Select(set=>set.Name).ToList();
            SelectList setNames = new SelectList(names);
            return View(setNames);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSet(string setName)
        {
            Repository<SetEntity> repository = new Repository<SetEntity>();
            SetEntity set = repository.GetList().Where(s => s.Name == setName).FirstOrDefault();
            repository.Delete(set.Id);

            string path = Server.MapPath("~/Content/Images/Sets/" + setName);
            DirectoryInfo setDirectory = new DirectoryInfo(path);
            setDirectory.Delete(true);

            ViewBag.Text = "Set is Deleted!";
            return View("ShowInfo");
        }

        [HttpPost]       
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSet(string setName, string setDescription, string resultHtml, string resultHtmlWithoutNotResultElements, IEnumerable<HttpPostedFileBase> uploads)
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
            string html = resultHtmlWithoutNotResultElements.Replace("&lt;" , "<").Replace("&gt;" , ">");
            SetEntity set = new SetEntity { Name = setName, Html = resultHtml, HtmlWithoutNotResultElements = resultHtmlWithoutNotResultElements};
            Repository<SetEntity> repository = new Repository<SetEntity>();
            repository.Create(set);
            ViewBag.Alex = html;
            ViewBag.ViewName = "AdminSuccessfulSetSaving";
            ViewBag.Text = "Set is saved!";
            return View("ShowInfo");
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

        [HttpPost]
        public JsonResult Upload()
        {
            foreach (string file in Request.Files)
            {
                //var upload = Request.Files[file];
                //if (upload != null)
                //{
                //    // получаем имя файла
                //    string fileName = System.IO.Path.GetFileName(upload.FileName);
                //    // сохраняем файл в папку Files в проекте
                //    upload.SaveAs(Server.MapPath("~/Files/" + fileName));
                //}
            }
            return Json("файл загружен");
        }


    }
}