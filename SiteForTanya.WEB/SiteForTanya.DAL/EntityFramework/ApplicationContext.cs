using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SiteForTanya.DAL.Entities;


namespace SiteForTanya.DAL
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext() : base("DbConnection") { }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }
    }
}
