using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteForTanya.Models
{
    public class SetEntity: IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Html { get; set; }
        public string HtmlWithoutNotResultElements { get; set; }
        public string Tags { get; set; }
        public DateTime AddingTime { get; set; }
        public string MainImageName { get; set; }
    }
}