using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteForTanya.Models
{
    public class BlogEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string ShortDescription { get; set; }
        public DateTime AddingTime { get; set; }
        public string ImageName { get; set; }
    }
}