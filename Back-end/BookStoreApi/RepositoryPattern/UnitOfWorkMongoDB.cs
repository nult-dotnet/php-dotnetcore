using BookStoreApi.DBContext;
using BookStoreApi.Models;
using LibraryAbstractDBProvider.DBContext;

namespace BookStoreApi.RepositoryPattern
{
    public class UnitOfWorkMongoDB<TEntity> : AbstractUnitOfWork<TEntity> where TEntity : class
    {
        private MongoDBContext mongoContext = new MongoDBContext();
        private GenericRepositoryMongoDB<TEntity> _repository;
        public override GenericRepositoryMongoDB<TEntity> Repository
        {
            get
            {
                if(this._repository is null)
                {
                    this._repository = new GenericRepositoryMongoDB<TEntity>(mongoContext);
                }
                return this._repository;
            }
        }
    }
}