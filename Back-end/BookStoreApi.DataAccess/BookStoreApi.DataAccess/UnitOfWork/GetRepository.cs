using BookStoreApi.DataAccess.GenericRepository;
using BookStoreApi.RepositoryPattern;
using LibraryAbstractDBProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApi.DataAccess.UnitOfWork
{
    public static class GetRepository<T> where T : class
    {
        public static IRepository<T> Repository(IUnitOfWork unitOfWork)
        {
            string databaseDefault = GetStringAppsetting.DatabaseDefault();
            try
            {
                if (databaseDefault.Equals("MongoDB"))
                {
                    return new UnitOfWorkMongoDB.Repository<T>(unitOfWork);
                }
                return new UnitOfWorkSQL.Repository<T>(unitOfWork);
            }
            catch
            {
                throw new NotImplementedException("Database deafault not a valid database type");
            }
        }
    }
}