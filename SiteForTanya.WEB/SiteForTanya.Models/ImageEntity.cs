using System;

namespace SiteForTanya.Models
{
    public class ImageEntity : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public DateTime AddingTime { get; set; }
    }
}