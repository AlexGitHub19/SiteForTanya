using System.Data.Entity;
using SiteForTanya.Models;

namespace SiteForTanya.DAL.EntityFramework
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DbConnection") { }

        public DbSet<SetEntity> SetEntities { get; set; }
        public DbSet<ImageEntity> ImageEntities { get; set; }
        public DbSet<BlogEntity> BlogEntities { get; set; }
        public DbSet<ImagesInfo> ImagesInfos { get; set; }
        public DbSet<SetsInfo> SetsInfos { get; set; }
        public DbSet<ExceptionEntity> ExceptionEntities { get; set; }
    }
}