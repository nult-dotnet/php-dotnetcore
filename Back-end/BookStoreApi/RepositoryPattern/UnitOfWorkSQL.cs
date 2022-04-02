using BookStoreApi.DBContext;
using BookStoreApi.Models;
using LibraryAbstractDBProvider.DBContext;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.RepositoryPattern
{
    public class UnitOfWorkSQL<TEntity> : AbstractUnitOfWork<TEntity>, IDisposable where TEntity : class
    {
        private SQLContext _context = new SQLContext();
        private GenericRepositorySQL<TEntity> _repository;
        public override GenericRepositorySQL<TEntity> Repository
        {
            get
            {
                if(this._repository is null)
                {
                    this._repository = new GenericRepositorySQL<TEntity>(_context);
                }
                return this._repository;
            }
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}