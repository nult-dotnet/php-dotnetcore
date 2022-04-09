using BookStoreApi.DataAccess.GenericRepository;
using BookStoreApi.DataAccess.UnitOfWork;
using BookStoreApi.DBContext;
using BookStoreApi.Models;
using LibraryAbstractDBProvider.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookStoreApi.RepositoryPattern
{
    public class UnitOfWorkSQL : AbsUnitOfWork,IDisposable
    {
        private SQLContext _context;
        private IDbContextTransaction _transaction;
        private bool disposed = false;
        public UnitOfWorkSQL()
        {
            _context = new SQLContext();
        }
        public override object Context
        {
            get { return this._context; }
        }

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

        public override void Commit()
        {
            _transaction.Commit();
        }

        public override void CreateTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public override void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }

        public override void Save()
        {
            _context.SaveChanges();
        }
        public class Repository<TEntity> : GenericRepositorySQL<TEntity>,IRepository<TEntity> where TEntity : class
        {
            public Repository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        }
    }
}