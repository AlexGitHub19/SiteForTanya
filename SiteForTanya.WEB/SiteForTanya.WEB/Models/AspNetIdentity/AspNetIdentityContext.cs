using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteForTanya.WEB.Models.AspNetIdentity
{
    public class AspNetIdentityContext : IdentityDbContext<IdentityUser>
    {
        public AspNetIdentityContext() : base("DbConnection") { }

        public static AspNetIdentityContext Create()
        {
            return new AspNetIdentityContext();
        }
    }
}