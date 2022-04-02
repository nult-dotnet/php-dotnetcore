using BookStoreApi.Models;
using LibraryAbstractDBProvider;
namespace BookStoreApi.RepositoryPattern
{
    public interface IUnitOfWork<TEntity> where TEntity : class
    {
        public IGenericRepository<TEntity> Repository { get; }
    }
}