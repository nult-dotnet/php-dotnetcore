
using LibraryAbstractDBProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreDesktop.DatabaseFactory
{
    public class DBFactory : IDBAbstractFactory
    {
        public IDatabaseProvider GetDatabase()
        {
            CreateDatabaseProvider databaseProvider = new CreateDatabaseProvider();
            return databaseProvider.CreateDatabase();
        }
    }
}