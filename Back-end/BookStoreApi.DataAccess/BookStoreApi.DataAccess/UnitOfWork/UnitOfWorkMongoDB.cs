using BookStoreApi.DataAccess.GenericRepository;
using BookStoreApi.DataAccess.UnitOfWork;
using BookStoreApi.DBContext;
using BookStoreApi.Models;
using LibraryAbstractDBProvider.DBContext;

namespace BookStoreApi.RepositoryPattern
{
    public class UnitOfWorkMongoDB : AbsUnitOfWork,IDisposable
    {
        private MongoDBContext _context;
       
        public UnitOfWorkMongoDB()
        {
            _context = new MongoDBContext();
        }
        public override object Context
        {
            get { return _context; }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public override void Save()
        {
            _context.SaveChange();
        }
        public class Repository<T> : GenericRepositoryMongoDB<T>,IRepository<T> where T : class
        {
            public Repository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        }
    }
}