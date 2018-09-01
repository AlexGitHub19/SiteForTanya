using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Web;
using System.Web.Mvc;

namespace SiteForTanya.WEB.Models.AspNetIdentity
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

        public static bool ChangePassword(string currentPassword, string newPassword) {
            var manager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new AspNetIdentityContext()));
            var result = manager.ChangePassword(HttpContext.Current.User.Identity.GetUserId(), currentPassword, newPassword);
            return result.Succeeded;
        }
    }
}