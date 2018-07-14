using System;
using System.Collections.Generic;
using System.Linq;
using SiteForTanya.Models;
using SiteForTanya.DAL.EntityFramework;

namespace SiteForTanya.WEB.Models
{
    public class ImageProcessor
    {
        public ImageViewModel GetImagesNames(string keyWords, int imagesCountOnPage, int pageNumber)
        {
            Repository<ImageEntity> imageEntityRepository = new Repository<ImageEntity>();

            if (keyWords == String.Empty)
            {
                var allImages = imageEntityRepository.GetList();
                List<ImageBriefInfo> imageNames = allImages.OrderByDescending(img => img.AddingTime).Skip((pageNumber - 1) * imagesCountOnPage).Take(imagesCountOnPage).
                    Select(img => new ImageBriefInfo { name = img.Name, hasTags = (img.Tags != null && img.Tags != string.Empty) ? true : false }).ToList();
                return new ImageViewModel { imageInfos = imageNames, imageCount = allImages.Count() };
            }
            else
            {
                List<string> words = keyWords.Split(' ').ToList();
                var allImages = imageEntityRepository.GetList().Where(img => CommonMethods.TagContainsWord(img, words));
                List<ImageBriefInfo> imageNames = allImages.OrderByDescending(img => img.AddingTime).Skip((pageNumber - 1) * imagesCountOnPage).Take(imagesCountOnPage).
                    Select(img => new ImageBriefInfo { name = img.Name, hasTags = (img.Tags != null && img.Tags != string.Empty) ? true : false }).ToList();
                return new ImageViewModel { imageInfos = imageNames, imageCount = allImages.Count() };
            }
        }
    }
}