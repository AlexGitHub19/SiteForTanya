using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SiteForTanya.WEB.Models
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