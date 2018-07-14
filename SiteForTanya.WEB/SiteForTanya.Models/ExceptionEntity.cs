using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteForTanya.WEB.Models
{
    public class ExceptionEntity
    {
        public int Id { get; set; }
        public string Mesage { get; set; }
        public string StackTrace { get; set; }
        public DateTime Date { get; set; }
    }
}