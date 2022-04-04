using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAbstractDBProvider.DBContext
{
    public class GenericRepositoryMongoDB<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly IMongoDBContext _dbContext;
        private IMongoCollection<TEntity> _mongoCollection;
        public GenericRepositoryMongoDB(IMongoDBContext mongoDBContext)
        {
            this._dbContext = mongoDBContext;
            this._mongoCollection = _dbContext.GetCollection<TEntity>(typeof(TEntity).Name);
        }
        public async virtual Task Delete(object id) => await this._mongoCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id));

        public virtual async Task Delete(TEntity entityDelete)
        {
            var IdProperty = entityDelete.GetType().GetProperty("Id").GetValue(entityDelete, null);
            await this._mongoCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", IdProperty));
        }

        public async virtual Task<IEnumerable<TEntity>> Get(
            Expression<Func<TEntity,
            bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            if (filter is null)
            {
                var all = await this._mongoCollection.FindAsync(Builders<TEntity>.Filter.Empty);
                return all.ToList();
            }
            var allFiler = await this._mongoCollection.FindAsync(filter);
            return allFiler.ToList();
        }

        public virtual async Task<TEntity> GetByID(object id)
        {
            var data = await this._mongoCollection.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.FirstOrDefault();
        }
        public virtual async Task Insert(TEntity entity) => await this._mongoCollection.InsertOneAsync(entity);

        public virtual async Task Update(TEntity entityUpdate)
        {
            var IdProperty = entityUpdate.GetType().GetProperty("Id").GetValue(entityUpdate, null);
            await this._mongoCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", IdProperty), entityUpdate);
        }
    }
}
