using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAbstractDBProvider.DBContext
{
    public class GenericRepositorySQL<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal DbContext context;
        internal DbSet<TEntity> dbSet;
        public GenericRepositorySQL(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }
        public async virtual Task<IEnumerable<TEntity>> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<TEntity> query = this.dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (orderBy is null)
            {
                return query.ToList();
            }
            return orderBy(query).ToList();
        }
        public async virtual Task<TEntity> GetByID(object id)
        {
            return this.dbSet.Find(id);
        }
        public async virtual Task Insert(TEntity entity)
        {
            this.dbSet.Add(entity);
            this.context.SaveChanges();
        }
        public async virtual Task Delete(object id)
        {
            TEntity entityDelete = this.dbSet.Find(id);
            Delete(entityDelete);
            this.context.SaveChanges();
        }
        public async virtual Task Delete(TEntity entityDelete)
        {
            if (this.context.Entry(entityDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityDelete);
            }
            dbSet.Remove(entityDelete);
            this.context.SaveChanges();
        }
        public async virtual Task Update(TEntity entityUpdate)
        {
            this.dbSet.Attach(entityUpdate);
            this.context.Entry(entityUpdate).State = EntityState.Modified;
            this.context.SaveChanges();
        }
    }
}
