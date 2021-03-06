﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using SiteForTanya.WEB.Models;
using SiteForTanya.WEB.Models.AdminVewModels;
using System.Drawing.Imaging;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using SiteForTanya.WEB.Models.WorkViewModels;
using SiteForTanya.Models;
using SiteForTanya.DAL.EntityFramework;
using System.Web;
using SiteForTanya.WEB.Models.AspNetIdentity;
using System.Text.RegularExpressions;

namespace SiteForTanya.WEB.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                ViewBag.Title = "Admin Home";
                return View();
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Exceptions()
        {
            try
            {
                ViewBag.Title = "Admin Exception";
                return View();
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Sets()
        {
            try
            {
                ViewBag.Title = "Admin Sets";
                return View();
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Set(string name)
        {
            try
            {
                Repository<SetEntity> setRepostory = new Repository<SetEntity>();
                SetEntity set = setRepostory.GetList().FirstOrDefault(s => s.Name == name);
                if (set == null)
                {
                    return RedirectToAction("Home");
                }

                ViewBag.Title = "Set " + name;
                int imageCount = new Regex("imgLoaded").Matches(set.HtmlWithoutNotResultElements).Count;
                SetViewModel vm = new SetViewModel { Name = name, Html = set.HtmlWithoutNotResultElements, ImagesCount = imageCount };
                return View(vm);
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Images()
        {
            try
            {
                ViewBag.Title = "Admin Images";
                return View();
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Blog()
        {
            try
            {
                ViewBag.Title = "Admin Blog";
                return View();
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult BlogItem(string blogName)
        {
            try
            {
                Repository<BlogEntity> blogRepository = new Repository<BlogEntity>();
                BlogEntity blogEntity = blogRepository.GetList().First(b => b.Name == blogName);
                ViewBag.Title = "Admin blog " + blogName;
                return View(blogEntity);
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddBlog()
        {
            try
            {
                ViewBag.Title = "Admin Add Blog";
                return View();
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddBlog(string blogName, string blogShortDescription, string resultHtml, HttpPostedFileBase inputImage)
        {
            try
            {
                Repository<BlogEntity> blogRepository = new Repository<BlogEntity>();
                BlogEntity blogEntity = new BlogEntity { Name = blogName.Trim(), Text = resultHtml.Replace("&lt;", "<").Replace("&gt;", ">"), ShortDescription = blogShortDescription, AddingTime = DateTime.Now };
                blogRepository.Create(blogEntity);

                string fileName = null;
                if (blogEntity.Id != 0)
                {
                    fileName = blogEntity.Id + inputImage.FileName.Substring(inputImage.FileName.LastIndexOf('.'));
                    inputImage.SaveAs(Server.MapPath("~/Content/Images/BlogImages/" + fileName));
                }

                BlogEntity savedBlogItem = blogRepository.GetList().First(b => b.Id == blogEntity.Id);
                savedBlogItem.ImageName = fileName;
                blogRepository.Update(savedBlogItem);


                ViewBag.Text = "Blog item is saved!";
                ViewBag.Title = "Info";
                return View("ShowInfo");
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangeBlog(int id)
        {
            try
            {
                Repository<BlogEntity> blogRepository = new Repository<BlogEntity>();
                BlogEntity blogEntity = blogRepository.GetList().First(b => b.Id == id);
                ViewBag.Title = "Admin Change Blog " + blogEntity.Name;
                return View(blogEntity);
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult SaveChangingBlog(int id, string blogName, string shortDescription, string resultHtml, HttpPostedFileBase inputImage)
        {
            try
            {
                Repository<BlogEntity> blogRepository = new Repository<BlogEntity>();
                BlogEntity blogEntity = blogRepository.GetList().First(b => b.Id == id);
                blogEntity.Name = blogName;
                blogEntity.Text = resultHtml.Replace("&lt;", "<").Replace("&gt;", ">");
                blogEntity.ShortDescription = shortDescription;

                if (inputImage != null)
                {
                    string fileName = blogEntity.Id + inputImage.FileName.Substring(inputImage.FileName.LastIndexOf('.'));
                    inputImage.SaveAs(Server.MapPath("~/Content/Images/BlogImages/" + fileName));
                    blogEntity.ImageName = fileName;
                }

                blogRepository.Update(blogEntity);

                ViewBag.Text = "Blog item is changed!";
                ViewBag.Title = "Info";
                return View("ShowInfo");
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBlogItem(int blogItemId)
        {
            try
            {
                Repository<BlogEntity> blogRepository = new Repository<BlogEntity>();
                BlogEntity blogEntity = blogRepository.GetList().First(b => b.Id == blogItemId);

                if (blogEntity != null)
                {
                    blogRepository.Delete(blogEntity.Id);                 
                    return Json(new { result = "Success" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { result = "Fail" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ExceptionProcessor exceptionProcessor = new ExceptionProcessor();
                exceptionProcessor.process(ex);
                return Json(new { result = "Fail" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult CheckBlogName(string blogName)
        {
            try
            {
                Repository<BlogEntity> blogRepository = new Repository<BlogEntity>();
                BlogEntity blogEntity = blogRepository.GetList().FirstOrDefault(b => b.Name == blogName);

                if (blogEntity == null)
                {
                    return Json(new { result = "True" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = "False" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }          
        }

        [HttpGet]
        [Authorize]
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

        [HttpGet]
        [Authorize]
        public ActionResult AddSet()
        {
            try
            {
                ViewBag.Title = "Admin Add Set";
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
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangeSet(string setName)
        {
            try
            {
                ViewBag.Title = "Admin Change Set " + setName;

                AdminChangeSetViewModel viewModel = new AdminChangeSetViewModel();
                Repository<SetsInfo> repositorySetInfo = new Repository<SetsInfo>();
                SetsInfo setInfo = repositorySetInfo.GetList().First();
                if (setInfo.TempSetNumber == int.MaxValue)
                {
                    setInfo.TempSetNumber = 0;
                }
                setInfo.TempSetNumber++;
                repositorySetInfo.Update(setInfo);

                Repository<SetEntity> setEntityRepository = new Repository<SetEntity>();
                SetEntity set = setEntityRepository.GetList().Where(s => s.Name == setName).First();

                string setPath = Server.MapPath("~/Content/Images/Sets/" + set.Name);
                DirectoryInfo setDirectory = new DirectoryInfo(setPath);
                FileInfo[] images = setDirectory.GetFiles();
                foreach (FileInfo image in images)
                {
                    if (image.Name.Contains("ImageMainImage"))
                    {
                        viewModel.MainImageName = image.Name;
                        break;
                    }
                }

                viewModel.TempSetNumber = setInfo.TempSetNumber;
                viewModel.SetName = set.Name;
                viewModel.Html = set.Html;
                viewModel.Tags = set.Tags;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }           
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeSet(string setName, string setTags, string resultHtml, string resultHtmlWithoutNotResultElements, string tempSetNumber)
        {

            try
            {
                Repository<SetEntity> setsRepository = new Repository<SetEntity>();

                string mainImageName = null;
                string setPath = Server.MapPath("~/Content/Images/Sets/" + setName.Trim());
                DirectoryInfo setDirectory = new DirectoryInfo(setPath);
                string tempDirectoryPath = Server.MapPath("~/Content/Images/Temp/" + "Set" + tempSetNumber);
                DirectoryInfo tempSetDirectory = new DirectoryInfo(tempDirectoryPath);

                FileInfo[] oldImages = setDirectory.GetFiles();
                foreach (FileInfo image in oldImages)
                {
                    string fileName = image.Name.Substring(0, image.Name.LastIndexOf('.'));
                    //fileName.Contains("Image") is added to not delete Thumb.db
                    if (fileName.Contains("Image") && !(fileName == "ImageMainImage") && !resultHtmlWithoutNotResultElements.Contains(fileName))
                    {
                        image.Delete();
                    }
                }

                bool mainImageChanged = false;
                if (tempSetDirectory.Exists)
                {
                    FileInfo[] newImages = tempSetDirectory.GetFiles();
                    foreach (FileInfo image in newImages)
                    {
                        string fileName = image.Name.Substring(0, image.Name.LastIndexOf('.'));
                        if (resultHtmlWithoutNotResultElements.Contains(fileName))
                        {
                            FileInfo imageWithSameName = new FileInfo(setPath + "/" + image.Name);
                            if (imageWithSameName.Exists)
                            {
                                imageWithSameName.Delete();
                            }
                            using (Image imageFromPath = Image.FromFile(image.FullName))
                            {
                                double height = imageFromPath.Height;
                                double width = imageFromPath.Width;
                                double proportion = height / width;
                                double newHeight = 1000;
                                int newWidth = (int)(newHeight / proportion);
                                if (newWidth > 5120)
                                {
                                    newWidth = 3000;
                                    newHeight = (int)(newWidth * proportion);
                                }
                                Bitmap resizedImage = changeMainImageSize(image.FullName, (int)newHeight, newWidth);

                                if (resizedImage != null)
                                {
                                    resizedImage.Save(setPath + "/" + image.Name);
                                }
                            }
                        }
                        else if (fileName == "ImageMainImage")
                        {
                            mainImageChanged = true;
                            mainImageName = image.Name;
                            Bitmap resizedImage =  changeMainImageSize(image.FullName, 300, 450);
                            if (resizedImage != null)
                            {
                                resizedImage.Save(setPath + "/" + image.Name);
                            }
                        }
                    }

                    tempSetDirectory.Delete(true);
                }

                string resultTags = String.Empty;
                if (!String.IsNullOrEmpty(setTags))
                {
                    Char[] separators = { ',', ';', ' ' };
                    string[] tagsList = DeleteLastSemicolon(setTags.Trim()).Split(separators);
                    for (int i = 0; i < tagsList.Length; i++)
                    {
                        if (tagsList[i] == "")
                        {
                            continue;
                        }
                        string tag = tagsList[i].Trim().ToLower();
                        resultTags += tag;
                        resultTags += ";";
                    }
                }

                string html = resultHtmlWithoutNotResultElements.Replace("&lt;", "<").Replace("&gt;", ">");

                SetEntity set = setsRepository.GetList().First(s => s.Name == setName);
                set.HtmlWithoutNotResultElements = html;
                set.Html = resultHtml.Replace("&lt;", "<").Replace("&gt;", ">");
                set.Tags = resultTags != string.Empty ? DeleteLastSemicolon(resultTags) : null;
                if (mainImageChanged)
                {
                    set.MainImageName = mainImageName;
                }

                List<SetEntity> sets = setsRepository.GetList().ToList();
                sets.Add(set);
                setsRepository.Update(set);
                ViewBag.Text = "Set is changed!";
                ViewBag.Title = "Info";
                DeleteTempFolders();
                return View("ShowInfo");
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }           
        }


        [HttpPost]       
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSet(string setName, string setTags, string resultHtml, string resultHtmlWithoutNotResultElements, string tempSetNumber)
        {

            try
            {
                string mainImageName = null;
                string setPath = Server.MapPath("~/Content/Images/Sets/" + setName.Trim());
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
                        using (Image imageFromPath = Image.FromFile(image.FullName))
                        {
                            double height = imageFromPath.Height;
                            double width = imageFromPath.Width;
                            double proportion = height / width;
                            double newHeight = 1000;
                            int newWidth = (int)(newHeight / proportion);
                            if (newWidth > 5120)
                            {
                                newWidth = 3000;
                                newHeight = (int)(newWidth * proportion);
                            }
                            Bitmap resizedImage = changeMainImageSize(image.FullName, (int)newHeight, newWidth);

                            if (resizedImage != null)
                            {
                                resizedImage.Save(setPath + "/" + image.Name);
                            }
                        }
                    }
                    else if (fileName == "ImageMainImage")
                    {
                        mainImageName = image.Name;
                        Bitmap resizedImage =  changeMainImageSize(image.FullName, 300, 450);
                        if (resizedImage != null)
                        {
                            resizedImage.Save(setPath + "/" + image.Name);
                        }

                    }
                }

                tempSetDirectory.Delete(true);

                string html = resultHtmlWithoutNotResultElements.Replace("&lt;", "<").Replace("&gt;", ">");

                string resultTags = String.Empty;
                if (!String.IsNullOrEmpty(setTags))
                {
                    Char[] separators = { ',', ';', ' ' };
                    string[] tagsList = DeleteLastSemicolon(setTags.Trim()).Split(separators);
                    for (int i = 0; i < tagsList.Length; i++)
                    {
                        if (tagsList[i] == "")
                        {
                            continue;
                        }
                        string tag = tagsList[i].Trim().ToLower();
                        resultTags += tag;
                        resultTags += ";";
                    }
                }

                SetEntity set = new SetEntity
                {
                    Name = setName.Trim(),
                    Html = resultHtml.Replace("&lt;", "<").Replace("&gt;", ">"),
                    HtmlWithoutNotResultElements = html,
                    Tags = resultTags != string.Empty ? DeleteLastSemicolon(resultTags) : null,
                    AddingTime = DateTime.Now,
                    MainImageName = mainImageName
                };
                Repository<SetEntity> repository = new Repository<SetEntity>();
                repository.Create(set);
                ViewBag.Text = "Set is saved!";
                ViewBag.Title = "Info";
                DeleteTempFolders();
                return View("ShowInfo");
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }           
        }

        private void DeleteTempFolders()
        {
            string tempDirectoryPath = Server.MapPath("~/Content/Images/Temp");
            DirectoryInfo tempSetDirectory = new DirectoryInfo(tempDirectoryPath);
            DirectoryInfo[] folders = tempSetDirectory.GetDirectories();
            foreach (DirectoryInfo folder in folders)
            {
                if ((DateTime.Now - Directory.GetCreationTime(folder.FullName)).Hours > 10)
                {
                    folder.Delete(true);
                }
            }
        }

        private Bitmap changeMainImageSize(string tempPath, int newHeight, int newWidth)
        {
            Image image = Image.FromFile(tempPath);
            Bitmap newImage = null;
            using (image)
            {
                newImage = new Bitmap(newWidth, newHeight);
                Graphics g = Graphics.FromImage(newImage);
                g.InterpolationMode = InterpolationMode.High;
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CheckSetName(string setName)
        {
            try
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
            catch (Exception ex)
            {
                return ProcessException(ex);
            }          
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult UploadSetImage(int tempSetNumber, string imageNumber)
        {          
            try
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
                    string fileName = "Image" + imageNumber + uploadImage.FileName.Substring(uploadImage.FileName.LastIndexOf('.'));
                    uploadImage.SaveAs(Server.MapPath("~/Content/Images/Temp/" + "Set" + tempSetNumber + "/" + fileName));
                }
                return Json("Ok");
            }
            catch (Exception ex)
            {
                ExceptionProcessor exceptionProcessor = new ExceptionProcessor();
                exceptionProcessor.process(ex);
                return Json("Exception");
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public JsonResult UploadImages()
        {
            try
            {
                string resultString = "Files are seccessfully uploaded";
                string resultType = "success";

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
                                    Char[] separators = { ',', ';', ' '};
                                    string[] tagsList = DeleteLastSemicolon(foundTags.Trim()).Split(separators);
                                    for (int i = 0; i < tagsList.Length; i++)
                                    {
                                        if(tagsList[i] == "")
                                        {
                                            continue;
                                        }
                                        string tag = tagsList[i].Trim().ToLower();
                                        resultTags += tag;
                                        resultTags += ";";
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

                        string imagePath = Server.MapPath("~/Content/Images/Images/LargeImages/Temp" + fileName);
                        upload.SaveAs(imagePath);
                        imageInfoRepository.Update(imgInfo);

                        //FileInfo savedImage = new FileInfo(Server.MapPath("~/Content/Images/Images/LargeImages/" + fileName));
                        double height;
                        double width;
                        double proportion;
                        double newHeight;
                        int newWidth;


                        using (Image imageFromPath = Image.FromFile(imagePath))
                        {
                            height = imageFromPath.Height;
                            width = imageFromPath.Width;
                            proportion = height / width;
                            newHeight = 2000;
                            newWidth = (int)(newHeight / proportion);
                            if (newWidth > 5120)
                            {
                                newWidth = 5120;
                                newHeight = (int)(newWidth * proportion);
                            }
                            Bitmap resizedImage = changeMainImageSize(imagePath, (int)newHeight, newWidth);

                            if (resizedImage != null)
                            {
                                resizedImage.Save(Server.MapPath("~/Content/Images/Images/LargeImages/") + fileName);
                            }
                        }

                        using (Image imageFromPath = Image.FromFile(imagePath))
                        {
                            newHeight = 600;
                            newWidth = (int)(newHeight / proportion);
                            if (newWidth > 5120)
                            {
                                newWidth = 5120;
                                newHeight = (int)(newWidth * proportion);
                            }
                            Bitmap resizedImage = changeMainImageSize(imagePath, (int)newHeight, newWidth);
                            if (resizedImage != null)
                            {
                                resizedImage.Save(Server.MapPath("~/Content/Images/Images/MiddleImages/") + fileName);
                            }
                        }

                        using (Image imageFromPath = Image.FromFile(imagePath))
                        {
                            newHeight = 200;
                            newWidth = (int)(newHeight / proportion);
                            if (newWidth > 5120)
                            {
                                newWidth = 5120;
                                newHeight = (int)(newWidth * proportion);
                            }
                            Bitmap resizedImage = changeMainImageSize(imagePath, (int)newHeight, newWidth);
                            if (resizedImage != null)
                            {
                                resizedImage.Save(Server.MapPath("~/Content/Images/Images/SmallImages/") + fileName);
                            }
                        }

                        System.IO.File.Delete(imagePath);

                        ImageEntity image = new ImageEntity { Name = fileName, Tags = resultTags != string.Empty ? DeleteLastSemicolon(resultTags) : null, AddingTime = DateTime.Now };
                        imageRepository.Create(image);
                    }
                }

                return Json(new { type = resultType, resultString = resultString });
            }
            catch (Exception ex)
            {
                ExceptionProcessor exceptionProcessor = new ExceptionProcessor();
                exceptionProcessor.process(ex);
                return Json("Exception");
            }
        }

        [HttpGet]
        public ActionResult ImagesAutocompleteSearch(string term)
        {
            return Json(CommonMethods.AutocompleteSearch<ImageEntity>(term), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SetsAutocompleteSearch(string term)
        {
            return Json(CommonMethods.AutocompleteSearch<SetEntity>(term), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteImage(string imageName)
        {          
            try
            {
                Repository<ImageEntity> imageRepository = new Repository<ImageEntity>();

                ImageEntity image = imageRepository.GetList().Where(img => img.Name.Substring(0, img.Name.IndexOf('.')) == imageName).FirstOrDefault();
                if (image != null)
                {
                    imageRepository.Delete(image.Id);
                    if (System.IO.File.Exists(Server.MapPath("~/Content/Images/Images/LargeImages/" + image.Name)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/Content/Images/Images/LargeImages/" + image.Name));
                    }
                    if (System.IO.File.Exists(Server.MapPath("~/Content/Images/Images/MiddleImages/" + image.Name)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/Content/Images/Images/MiddleImages/" + image.Name));
                    }
                    if (System.IO.File.Exists(Server.MapPath("~/Content/Images/Images/SmallImages/" + image.Name)))
                    {
                        System.IO.File.Delete(Server.MapPath("~/Content/Images/Images/SmallImages/" + image.Name));
                    }

                    return Json(new { result = "Success" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { result = "Fail" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSet(string setName)
        {            
            try
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

                    return Json(new { result = "Success" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { result = "Fail" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetExceptionInfos(DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            try
            {
                Repository<ExceptionEntity> exceptionRepository = new Repository<ExceptionEntity>();

                Func<ExceptionEntity, bool> condition;
                if (dateFrom != null && dateTo != null)
                {
                    condition = e => e.Date > dateFrom && e.Date < dateTo;
                }
                else
                {
                    if (dateFrom == null && dateTo == null)
                    {
                        condition = e => true;
                    }
                    else if (dateFrom != null)
                    {
                        condition = e => e.Date > dateFrom;
                    }
                    else
                    {
                        condition = e => e.Date < dateTo;
                    }
                }
                return Json(exceptionRepository.GetList().Where(condition).Select(e=>new {Date = e.Date.ToString(), Message = e.Mesage, StackTrace = e.StackTrace}).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string currentPassword, string newPassword)
        {
            try
            {
                if (ApplicationUserManager.ChangePassword(currentPassword, newPassword))
                {
                    return Json("OK");
                }
                else
                {
                    return Json("Fail");
                }
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

        private string DeleteLastSemicolon(string line)
        {
            string result = line;
            if (line.ToCharArray().Last() == ';')
            {
                result = line.Substring(0, line.Length - 1);
            }

            return result;
        }

    }
}