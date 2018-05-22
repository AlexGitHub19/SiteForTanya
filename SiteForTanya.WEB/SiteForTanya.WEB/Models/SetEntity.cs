using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteForTanya.WEB.Models
{
    public class SetEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Html { get; set; }
        public string HtmlWithoutNotResultElements { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public DateTime AddingTime { get; set; }
    }
}