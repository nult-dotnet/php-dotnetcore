using BookStoreApi.RepositoryPattern;
using LibraryAbstractDBProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApi.DataAccess.UnitOfWork
{
    public static class GetUnitOfWork
    {
        public static IUnitOfWork UnitOfWork()
        {
            string databaseDefault = GetStringAppsetting.DatabaseDefault();
            try
            {
                if (databaseDefault.Equals("MongoDB"))
                {
                    return new UnitOfWorkMongoDB();
                }
                return new UnitOfWorkSQL();
            }
            catch
            {
                throw new NotImplementedException("Database deafault not a valid database type");
            }
        }
    }
}