using BookStoreApi.Models;
using LibraryAbstractDBProvider;

namespace BookStoreApi.RepositoryPattern
{
    public abstract class AbstractUnitOfWork<TEntity> : IUnitOfWork<TEntity> where TEntity : class
    {
        public abstract IGenericRepository<TEntity> Repository { get; }
    }
}
