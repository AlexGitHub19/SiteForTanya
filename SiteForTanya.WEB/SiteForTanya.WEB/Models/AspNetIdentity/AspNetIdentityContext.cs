using Microsoft.AspNet.Identity.EntityFramework;

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