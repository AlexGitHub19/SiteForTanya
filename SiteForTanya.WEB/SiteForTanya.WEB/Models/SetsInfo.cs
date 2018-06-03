using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteForTanya.WEB.Models
{
    public class SetsInfo: IEntityInfo
    {
        public int Id { get; set; }
        public string AllTags { get; set; }
        public int TempSetNumber { get; set; }
    }
}