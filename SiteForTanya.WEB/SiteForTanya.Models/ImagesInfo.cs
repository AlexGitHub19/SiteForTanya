using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteForTanya.Models
{
    public class ImagesInfo: IEntityInfo
    {
        public int Id { get; set; }
        public string AllTags { get; set; }
        public int TotalCount { get; set; }
    }
}