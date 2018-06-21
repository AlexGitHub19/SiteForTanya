using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteForTanya.WEB.Models
{
    public class ImageViewModel
    {
        public List<ImageBriefInfo> imageInfos { get; set; }
        public int imageCount { get; set; }
    }

    public class ImageBriefInfo
    {
        public string name { get; set; }
        public bool hasTags { get; set; }
    }
}