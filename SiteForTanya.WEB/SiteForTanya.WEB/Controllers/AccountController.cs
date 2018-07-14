using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SiteForTanya.WEB.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SiteForTanya.WEB.Models.LoginViewModels;
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;
using SiteForTanya.WEB.Models.AspNetIdentity;
using SiteForTanya.Models;

namespace SiteForTanya.WEB.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                createAdmin();

                IdentityUser user = await UserManager.FindAsync(model.Login, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Incorrect login or password");
                }
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    if (String.IsNullOrEmpty(returnUrl))
                        return RedirectToAction("Index", "Admin");
                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        private void createAdmin()
        {
            AspNetIdentityContext context = new AspNetIdentityContext();

            if (context.Users.ToList().Count == 0)
            {
                var userManager = new ApplicationUserManager(new UserStore<IdentityUser>(context));
                var admin = new IdentityUser { UserName = "admin" };
                string password = "123456";
                var result = userManager.Create(admin, password);
                context.SaveChanges();

                Repository<ImagesInfo> imageInfoRepository = new Repository<ImagesInfo>();
                imageInfoRepository.Create(new ImagesInfo { AllTags = String.Empty });

                Repository<SetsInfo> setsInfoRepository = new Repository<SetsInfo>();
                setsInfoRepository.Create(new SetsInfo { AllTags = String.Empty });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}