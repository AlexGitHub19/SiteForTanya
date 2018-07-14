using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using SiteForTanya.WEB.Models.AspNetIdentity;

namespace SiteForTanya.WEB.Models
{
    public class ApplicationUserManager : UserManager<IdentityUser>
    {
        public ApplicationUserManager(IUserStore<IdentityUser> store)
                : base(store)
        {
        }
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
                                                IOwinContext context)
        {
            AspNetIdentityContext db = context.Get<AspNetIdentityContext>();
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<IdentityUser>(db));
            return manager;
        }
    }
}