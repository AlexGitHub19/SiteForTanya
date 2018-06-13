using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteForTanya.WEB.Models.CommonViewModels
{
    public class SetViewModel
    {
        public List<SetBriefInfo> sets { get; set; }
        public int setsCount { get; set; }
    }

    public class SetBriefInfo
    {
        public string setName { get; set; }
        public string mainImageName { get; set; }
    }
}