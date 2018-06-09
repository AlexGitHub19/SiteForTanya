using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteForTanya.WEB.Models.AdminVewModels
{
    public class AdminChangeSetViewModel
    {
        public string SetName { get; set; }       
        public string Tags { get; set; }
        public string Html { get; set; }
        public int TempSetNumber { get; set; }
        public string MainImageName { get; set; }
    }
}