using BookStoreApi.Models;
using LibraryAbstractDBProvider;
namespace BookStoreApi.RepositoryPattern
{
    public interface IUnitOfWork
    {
        object Context { get; }
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Save();
    }
}