using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteForTanya.WEB.Models
{
    public class ImageEntity : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public DateTime AddingTime { get; set; }
    }
}