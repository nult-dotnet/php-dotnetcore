using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDesktop.BookStoreDatabase;
using BookStoreDesktop.Models;
namespace BookStoreDesktop.RepositoryPattern
{
    public class UnitOfWork : IDisposable
    {
        private BookStoreContext _context  = new BookStoreContext();
        private GenericRepository<Category> _categoryRepository;
        private GenericRepository<Book> _bookRepository;
        private GenericRepository<Role> _roleRepository;
        public GenericRepository<Category> CategoryRepository
        {
            get
            {
                if(this._categoryRepository == null)
                {
                    this._categoryRepository = new GenericRepository<Category>(_context);
                }
                return this._categoryRepository;
            }
        }
        public GenericRepository<Book> BookRepository
        {
            get
            {
               if(this._bookRepository == null)
               {
                    this._bookRepository = new GenericRepository<Book>(_context);
               }
                return this._bookRepository;
            }
        }
        public GenericRepository<Role> RoleRepository
        {
            get
            {
                if(this._roleRepository == null)
                {
                    this._roleRepository = new GenericRepository<Role>(_context);
                }
                return this._roleRepository;
            }
        }
        public void Save()
        {
            this._context.SaveChanges();
        }
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this._context.Dispose();
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
