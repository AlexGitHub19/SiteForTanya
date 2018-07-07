using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SiteForTanya.WEB.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext() : base("DbConnection") { }

        public DbSet<SetEntity> SetEntities { get; set; }
        public DbSet<ImageEntity> ImageEntities { get; set; }
        public DbSet<BlogEntity> BlogEntities { get; set; }
        public DbSet<ImagesInfo> ImagesInfos { get; set; }
        public DbSet<SetsInfo> SetsInfos { get; set; }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }
    }
}