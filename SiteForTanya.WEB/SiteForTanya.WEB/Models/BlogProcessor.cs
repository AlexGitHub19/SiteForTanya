﻿using System.Collections.Generic;
using System.Linq;
using SiteForTanya.Models;
using SiteForTanya.DAL.EntityFramework;

namespace SiteForTanya.WEB.Models
{
    public class BlogProcessor
    {
        public BlogViewModel GetBlogInfos(int pageNumber)
        {
            Repository<BlogEntity> blogEntityRepository = new Repository<BlogEntity>();

            List<BlogEntity> AllBlogEntities = blogEntityRepository.GetList().ToList();
            List<BlogInfo> resultBlogEntities = AllBlogEntities.OrderByDescending(b => b.AddingTime).Skip((pageNumber - 1) * 10).Take(10).
                Select(b => new BlogInfo { Name = b.Name, ShortDescription = b.ShortDescription, Id = b.Id, ImageName = b.ImageName, Date = b.AddingTime.ToShortDateString()}).ToList();
            return new BlogViewModel { BlogInfos = resultBlogEntities, BlogItemsCount = AllBlogEntities.Count() };
        }
    }
}