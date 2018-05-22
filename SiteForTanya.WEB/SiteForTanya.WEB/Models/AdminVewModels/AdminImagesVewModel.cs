using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteForTanya.WEB.Models.AdminVewModels
{
    public class AdminImagesVewModel
    {
        public int TotalCount { get; set; }
        public int CountImagesOnPage { get; set; }
        public List<string> ImageNames { get; set; }
    }
}