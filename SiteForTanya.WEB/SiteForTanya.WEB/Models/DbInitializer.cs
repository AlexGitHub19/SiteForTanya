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
    public class DbInitializer : CreateDatabaseIfNotExists<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            // создаем пользователей
            var admin = new ApplicationUser {UserName = "admin" };
            string password = "123456";
            var result = userManager.Create(admin, password);
            context.SaveChanges();

            Repository<ImagesInfo> imageInfoRepository = new Repository<ImagesInfo>(context);
            imageInfoRepository.Create(new ImagesInfo { AllTags = String.Empty});

            Repository<SetsInfo> setsInfoRepository = new Repository<SetsInfo>(context);
            setsInfoRepository.Create(new SetsInfo { AllTags = String.Empty });

            // если создание пользователя прошло успешно
            base.Seed(context);
        }
    }
}