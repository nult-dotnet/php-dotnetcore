using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookStoreDesktop.BookStoreDatabase;
using Microsoft.EntityFrameworkCore;

namespace BookStoreDesktop.RepositoryPattern
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal BookStoreContext context;
        internal DbSet<TEntity> dbSet;
        public GenericRepository(BookStoreContext context)  
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public virtual IEnumerable<TEntity> Get(
           Expression<Func<TEntity,bool>> filter = null,
           Func<IQueryable<TEntity>,IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties="")
        {
            IQueryable<TEntity> query = this.dbSet; 
            if(filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if(orderBy is null)
            {
                return query.ToList();
            }
            return orderBy(query).ToList();
        }
        public virtual TEntity GetByID(object id)
        {
            return this.dbSet.Find(id);
        }
        public virtual void Insert(TEntity entity)
        {
            this.dbSet.Add(entity);
        }
        public virtual void Delete(object id)
        {
            TEntity entityDelete = this.dbSet.Find(id);
            Delete(entityDelete);
        }
        public virtual void Delete(TEntity entityDelete)
        {
            if(this.context.Entry(entityDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityDelete);
            }
            dbSet.Remove(entityDelete);
        }
        public virtual void Update(TEntity entityUpdate)
        {
            this.dbSet.Attach(entityUpdate);
            this.context.Entry(entityUpdate).State = EntityState.Modified;
        }
    }
}
