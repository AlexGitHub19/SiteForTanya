using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiteForTanya.WEB.Models;
using SiteForTanya.WEB.Models.AdminVewModels;
using System.Drawing.Imaging;
using System.Text;
using System.Drawing;

namespace SiteForTanya.WEB.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.ViewName = "AdminIndex";
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Sets()
        {
            ViewBag.ViewName = "AdminSets";
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Images()
        {
            ViewBag.ViewName = "AdminImages";
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Blog()
        {
            ViewBag.ViewName = "AdminBlog";
            return View();
        }

        [HttpGet]
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
        public ActionResult SaveSet(string setName, string setDescription, string setTags, string resultHtml, string resultHtmlWithoutNotResultElements, IEnumerable<HttpPostedFileBase> uploads)
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

            Repository<SetsInfo> setsInfoRepository = new Repository<SetsInfo>();
            SetsInfo setsIfo = setsInfoRepository.GetList().First();
            string resultTags = String.Empty;
            if (!String.IsNullOrEmpty(setTags))
            {
                string[] tagsList = setTags.Split(';');
                for (int i = 0; i < tagsList.Length; i++)
                {
                    string tag = tagsList[i].Trim().ToLower();
                    resultTags += tag;
                    if (i != tagsList.Length - 1)
                    {
                        resultTags += ",";
                    }

                    if (!setsIfo.AllTags.Contains(tag))
                    {
                        if (setsIfo.AllTags != String.Empty)
                        {
                            setsIfo.AllTags += ",";
                        }
                        setsIfo.AllTags += tagsList[i].Trim().ToLower();
                    }
                }
            }

            SetEntity set = new SetEntity { Name = setName, Html = resultHtml, HtmlWithoutNotResultElements = resultHtmlWithoutNotResultElements, AddingTime = DateTime.Now, Description = setDescription };
            Repository<SetEntity> repository = new Repository<SetEntity>();
            repository.Create(set);
            ViewBag.Alex = html;
            ViewBag.ViewName = "AdminSuccessfulSetSaving";
            ViewBag.Text = "Set is saved!";
            return View("ShowInfo");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CheckSetName(string setName)
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult GetSetsNames(string keyWords, int setsCountOnPage, int pageNumber)
        {
            Repository<SetEntity> setEntityRepository = new Repository<SetEntity>();

            if (keyWords == String.Empty)
            {
                var allSets = setEntityRepository.GetList();
                var setNames = allSets.OrderByDescending(set => set.AddingTime).Skip((pageNumber - 1) * setsCountOnPage).Take(setsCountOnPage).Select(set => new { value = set.Name });
                return Json(new { setNames = setNames, setCount = allSets.Count() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<string> words = keyWords.Split(' ').ToList();
                var allSets = setEntityRepository.GetList().Where(set => TagContainsWord(set, words));
                var imageNames = allSets.OrderByDescending(set => set.AddingTime).Skip((pageNumber - 1) * setsCountOnPage).Take(setsCountOnPage).Select(set => new { value = set.Name });
                return Json(new { imageNames = imageNames, imageCount = allSets.Count() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult UploadImages()
        {
            string resultString = "Files are seccessfully uploaded";
            string resultType = "success";

            bool isImgInfoModified = false;
            Repository<ImagesInfo> imageInfoRepository = new Repository<ImagesInfo>();

            ImagesInfo imgInfo = imageInfoRepository.GetList().First();

            Repository<ImageEntity> imageRepository = new Repository<ImageEntity>();

            foreach (string file in Request.Files)
            {
                imgInfo.TotalCount++;
                var upload = Request.Files[file];
                string fileName = "Image" + imgInfo.TotalCount + upload.FileName.Substring(upload.FileName.LastIndexOf('.'));

                if (upload != null)
                {
                    string foundTags = String.Empty;
                    string resultTags = String.Empty;
                    using (Image inputImage = Image.FromStream(upload.InputStream))
                    {
                        try
                        {
                            PropertyItem basicTag = inputImage.GetPropertyItem(40094); // Hex 9C9E
                            if (basicTag != null)
                            {                               
                                foundTags = Encoding.Unicode.GetString(basicTag.Value).Replace("\0", string.Empty);
                                string[] tagsList = foundTags.Split(';');
                                for(int i = 0; i< tagsList.Length; i++)
                                {
                                    string tag = tagsList[i].Trim().ToLower();
                                    resultTags += tag;
                                    if (i != tagsList.Length - 1)
                                    {
                                        resultTags += ",";
                                    }

                                    if (!imgInfo.AllTags.Contains(tag))
                                    {
                                        if (imgInfo.AllTags!= String.Empty)
                                        {
                                            imgInfo.AllTags += ",";
                                        }
                                        imgInfo.AllTags += tagsList[i].Trim().ToLower();                                     
                                    }
                                }
                            }
                        }
                        // ArgumentException is thrown when GetPropertyItem(int) is not found
                        catch (ArgumentException)
                        {
                            resultString = "No data in property \"Tag\" in some image or Property \"Tags\" isn't found in this type of file";
                            resultType = "danger";
                        }
                    }

                    upload.SaveAs(Server.MapPath("~/Content/Images/Images/" + fileName));
                    ImageEntity image = new ImageEntity { Name = fileName, Tags = resultTags != string.Empty? resultTags : null, AddingTime = DateTime.Now};
                    imageRepository.Create(image);
                    isImgInfoModified = true;
                }
            }

            if (isImgInfoModified)
            {
                imageInfoRepository.Update(imgInfo);
            }

            return Json(new {type = resultType, resultString = resultString });
        }

        [HttpGet]
        public ActionResult ImagesAutocompleteSearch(string term)
        {
            Repository<ImagesInfo> imageInfoRepository = new Repository<ImagesInfo>();
            ImagesInfo imgInfo = imageInfoRepository.GetList().First();
            if (imgInfo.AllTags == String.Empty)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            var words = imgInfo.AllTags.Split(',').Where(x=>x.Contains(term.ToLower())).OrderBy(s => s).Select(a => new { value = a });

            return Json(words, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SetsAutocompleteSearch(string term)
        {
            Repository<SetsInfo> setInfoRepository = new Repository<SetsInfo>();
            SetsInfo setInfo = setInfoRepository.GetList().First();
            if (setInfo.AllTags == String.Empty)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            var words = setInfo.AllTags.Split(',').Where(x => x.Contains(term.ToLower())).OrderBy(s => s).Select(a => new { value = a });

            return Json(words, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult GetImagesNames(string keyWords, int imagesCountOnPage, int pageNumber)
        {
            Repository<ImageEntity> imageEntityRepository = new Repository<ImageEntity>();

            if (keyWords == String.Empty)
            {
                var allImages = imageEntityRepository.GetList();
                var imageNames = allImages.OrderByDescending(img => img.AddingTime).Skip((pageNumber-1)* imagesCountOnPage).Take(imagesCountOnPage).Select(img => new { value = img.Name });
                return Json(new { imageNames = imageNames, imageCount = allImages.Count() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<string> words = keyWords.Split(' ').ToList();
                var allImages = imageEntityRepository.GetList().Where(img => TagContainsWord(img, words));
                var imageNames = allImages.OrderByDescending(img => img.AddingTime).Skip((pageNumber - 1) * imagesCountOnPage).Take(imagesCountOnPage).Select(img => new { value = img.Name });
                return Json(new { imageNames = imageNames, imageCount = allImages.Count() }, JsonRequestBehavior.AllowGet);
            }
        }

        private bool TagContainsWord<T>(T item, List<string> words) where T: IEntity
        {
            if (item.Tags == null)
            {
                return false;
            }
            foreach (string tag in item.Tags.Split(','))
            {
                foreach (string word in words)
                {
                    if (word == tag)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImage(string imageName)
        {
            Repository<ImageEntity> imageInfoRepository = new Repository<ImageEntity>();

            ImageEntity image = imageInfoRepository.GetList().Where(img => img.Name.Substring(0, img.Name.IndexOf('.')) == imageName).FirstOrDefault();
            if (image != null)
            {
                imageInfoRepository.Delete(image.Id);
                string strFileFullPath = Server.MapPath("~/Content/Images/Images/" + image.Name);
                if (System.IO.File.Exists(strFileFullPath))
                {
                    System.IO.File.Delete(strFileFullPath);
                }

                return Json(new { result = "Success"}, JsonRequestBehavior.AllowGet);
            }

            return Json(new {result = "Fail"}, JsonRequestBehavior.AllowGet);
        }

    }
}