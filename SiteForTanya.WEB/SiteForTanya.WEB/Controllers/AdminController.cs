using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiteForTanya.WEB.Models;
using System.Drawing.Imaging;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

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
            Repository<SetsInfo> repositorySetInfo = new Repository<SetsInfo>();
            SetsInfo setInfo = repositorySetInfo.GetList().First();
            if (setInfo.TempSetNumber == int.MaxValue)
            {
                setInfo.TempSetNumber = 0;
            }
            setInfo.TempSetNumber++;
            int tempSetNumber = setInfo.TempSetNumber;
            repositorySetInfo.Update(setInfo);
            return View(tempSetNumber);
        }

        //[HttpGet]
        //[Authorize]
        //public ActionResult DeleteSet()
        //{
        //    ViewBag.ViewName = "AdminDeleteSet";
        //    Repository<SetEntity> repository = new Repository<SetEntity>();
        //    IEnumerable<SetEntity> sets =  repository.GetList();
        //    List<string> names = sets.Select(set=>set.Name).ToList();
        //    SelectList setNames = new SelectList(names);
        //    return View(setNames);
        //}

        //[HttpPost]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteSet(string setName)
        //{
        //    Repository<SetEntity> repository = new Repository<SetEntity>();
        //    SetEntity set = repository.GetList().Where(s => s.Name == setName).FirstOrDefault();
        //    repository.Delete(set.Id);

        //    string path = Server.MapPath("~/Content/Images/Sets/" + setName);
        //    DirectoryInfo setDirectory = new DirectoryInfo(path);
        //    setDirectory.Delete(true);

        //    ViewBag.Text = "Set is Deleted!";
        //    return View("ShowInfo");
        //}

        [HttpPost]       
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSet(string setName, string setDescription, string setTags, string resultHtml, string resultHtmlWithoutNotResultElements, string tempSetNumber)
        {
            string mainImageName = null;    
            string setPath = Server.MapPath("~/Content/Images/Sets/"+ setName.Trim());
            DirectoryInfo setDirectory = new DirectoryInfo(setPath);
            setDirectory.Create(); 
            string tempDirectoryPath = Server.MapPath("~/Content/Images/Temp/" + "Set" + tempSetNumber);
            DirectoryInfo tempSetDirectory = new DirectoryInfo(tempDirectoryPath);
            FileInfo[] images = tempSetDirectory.GetFiles();
            foreach (FileInfo image in images)
            {
                string fileName = image.Name.Substring(0, image.Name.LastIndexOf('.'));
                if (resultHtmlWithoutNotResultElements.Contains(fileName))
                {
                    image.MoveTo(setPath + "/" + image.Name);
                }
                else if (fileName == "ImageMainImage")
                {
                    mainImageName = image.Name;
                    changeMainImageSizeAndSaveToSetFolder(setPath + "/" + image.Name, image.FullName, 300, 450);
                }
            }
            
            tempSetDirectory.Delete(true);

            string html = resultHtmlWithoutNotResultElements.Replace("&lt;" , "<").Replace("&gt;" , ">");

            Repository<SetsInfo> setsInfoRepository = new Repository<SetsInfo>();
            SetsInfo setsIfo = setsInfoRepository.GetList().First();
            string resultTags = String.Empty;
            bool isSetInfoModified = false;
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
                        isSetInfoModified = true;
                    }
                }
            }

            if (isSetInfoModified)
            {
                setsInfoRepository.Update(setsIfo);
            }

            SetEntity set = new SetEntity { Name = setName.Trim(), Html = resultHtml, HtmlWithoutNotResultElements = resultHtmlWithoutNotResultElements,
                Tags = resultTags, Description = setDescription, AddingTime = DateTime.Now, MainImageName = mainImageName};
            Repository<SetEntity> repository = new Repository<SetEntity>();
            repository.Create(set);
            ViewBag.Alex = html;
            ViewBag.ViewName = "AdminSuccessfulSetSaving";
            ViewBag.Text = "Set is saved!";
            return View("ShowInfo");
        }

        private void changeMainImageSizeAndSaveToSetFolder(string newPath, string tempPath, int newHeight, int newWidth)
        {
            Image image = Image.FromFile(tempPath);
            using (image)
            {
                Bitmap newImage = new Bitmap(newWidth, newHeight);
                Graphics g = Graphics.FromImage(newImage);
                g.InterpolationMode = InterpolationMode.High;
                g.DrawImage(image, 0, 0, newWidth, newHeight);
                newImage.Save(newPath);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CheckSetName(string setName)
        {
            string path = Server.MapPath("~/Content/Images/Sets/" + setName.Trim());
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
                var sets = allSets.OrderByDescending(set => set.AddingTime).Skip((pageNumber - 1) * setsCountOnPage).Take(setsCountOnPage).Select(set => new { setName = set.Name, mainImageName = set.MainImageName });
                return Json(new { sets = sets, setsCount = allSets.Count() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<string> words = keyWords.Split(' ').ToList();
                var allSets = setEntityRepository.GetList().Where(set => TagContainsWord(set, words));
                var sets = allSets.OrderByDescending(set => set.AddingTime).Skip((pageNumber - 1) * setsCountOnPage).Take(setsCountOnPage).Select(set => new { setName = set.Name, mainImageName = set.MainImageName });
                return Json(new { sets = sets, setsCount = allSets.Count() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult UploadSetImage(int tempSetNumber, string imageNumber)
        {
            string path = Server.MapPath("~/Content/Images/Temp/" + "Set" + tempSetNumber);
            DirectoryInfo setDirectory = new DirectoryInfo(path);
            if (!setDirectory.Exists)
            {
                setDirectory.Create();
            }            

            var uploadImage = Request.Files[0];
            if (uploadImage != null)
            {
                // получаем имя файла
                //string fileName = System.IO.Path.GetFileName(uploadImage.FileName);

                string fileName = "Image" + imageNumber + uploadImage.FileName.Substring(uploadImage.FileName.LastIndexOf('.'));
                // сохраняем файл в папку Files в проекте
                uploadImage.SaveAs(Server.MapPath("~/Content/Images/Temp/" + "Set" + tempSetNumber + "/" + fileName));
            }
            return Json("Ok");
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
            Repository<ImageEntity> imageRepository = new Repository<ImageEntity>();

            ImageEntity image = imageRepository.GetList().Where(img => img.Name.Substring(0, img.Name.IndexOf('.')) == imageName).FirstOrDefault();
            if (image != null)
            {
                imageRepository.Delete(image.Id);
                string strFileFullPath = Server.MapPath("~/Content/Images/Images/" + image.Name);
                if (System.IO.File.Exists(strFileFullPath))
                {
                    System.IO.File.Delete(strFileFullPath);
                }

                if (!String.IsNullOrEmpty(image.Tags))
                {
                    recalculateAllTags<ImagesInfo>(imageRepository.GetList());
                }

                return Json(new { result = "Success"}, JsonRequestBehavior.AllowGet);
            }

            return Json(new {result = "Fail"}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSet(string setName)
        {
            Repository<SetEntity> setsRepository = new Repository<SetEntity>();
            
            SetEntity set = setsRepository.GetList().Where(s => s.Name == setName).FirstOrDefault();
            if (set != null)
            {
                setsRepository.Delete(set.Id);
                string setDirectoryPath = Server.MapPath("~/Content/Images/Sets/" + setName);
                if (Directory.Exists(setDirectoryPath))
                {
                    Directory.Delete(setDirectoryPath, true);
                }

                if (!String.IsNullOrEmpty(set.Tags))
                {
                    recalculateAllTags<SetsInfo>(setsRepository.GetList());
                }
                return Json(new { result = "Success" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = "Fail" }, JsonRequestBehavior.AllowGet);
        }

        private void recalculateAllTags<T>(IEnumerable<IEntity> entities) where T:class, IEntityInfo
        {
            Repository<T> entityInfoRepository = new Repository<T>();
            T entityInfo = entityInfoRepository.GetList().First();
            string resultAllTags = String.Empty;
            foreach (IEntity entity in entities)
            {
                if (!String.IsNullOrEmpty(entity.Tags))
                {
                    string[] tagsList = entity.Tags.Split(',');
                    for (int i = 0; i < tagsList.Length; i++)
                    {
                        if (!resultAllTags.Contains(tagsList[i]))
                        {
                            if (resultAllTags != String.Empty)
                            {
                                resultAllTags += ",";
                            }
                            resultAllTags += tagsList[i];
                        }
                    }
                }
            }

            entityInfo.AllTags = resultAllTags;
            entityInfoRepository.Update(entityInfo);
        }

    }
}