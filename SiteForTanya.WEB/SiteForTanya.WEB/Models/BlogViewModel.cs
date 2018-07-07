using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteForTanya.WEB.Models
{
    public class BlogViewModel
    {
        public List<BlogInfo> BlogInfos { get; set; }
        public int BlogItemsCount { get; set; }
    }

    public class BlogInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
    }
}