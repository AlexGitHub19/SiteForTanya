using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SiteForTanya.WEB.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace SiteForTanya.WEB.Models
{
    public class DbInitializer : DropCreateDatabaseAlways<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            // создаем пользователей
            var admin = new ApplicationUser {UserName = "admin" };
            string password = "123456";
            var result = userManager.Create(admin, password);
            context.SaveChanges();

            // если создание пользователя прошло успешно
            base.Seed(context);
        }
    }
}