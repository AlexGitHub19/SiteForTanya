using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SiteForTanya.WEB.Models
{
    public class Repository<TEntity> where TEntity:class
    {
        ApplicationContext context;
        DbSet<TEntity> dbSet;
        private bool isDisposed = false;

        public Repository(ApplicationContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public Repository()
        {
            this.context = new ApplicationContext();
            dbSet = context.Set<TEntity>();
        }

        public TEntity GetItem(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<TEntity> GetList()
        {
            return dbSet.ToList();
        }

        public void Create(TEntity item)
        {
            dbSet.Add(item);
            context.SaveChanges();
        }

        public void Update(TEntity item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            TEntity item = dbSet.Find(id);
            dbSet.Remove(item);
            context.SaveChanges();
        }

        public void Save()
        {
            context.SaveChanges();
        }

    }
}