using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDesktop.BookStoreDatabase;
using LibraryAbstractDBProvider;
namespace BookStoreDesktop.DatabaseFactory
{
    public enum DBProvider
    {
        SQLServer,
        PostgreSQL
    }
    public class CreateDatabaseProvider
    {
        
        public IDatabaseProvider CreateDatabase()
        {  
            string databaseDefault = GetStringAppsetting.DatabaseDefault();
            Type T = Type.GetType($"BookStoreDesktop.DatabaseProvider.{databaseDefault}");
            if(T is null)
            {
                throw new NotImplementedException("Default Database not a valid database type.");
            }
            var DBProvider = Activator.CreateInstance(T) as IDatabaseProvider;
            return DBProvider;
        }
    }
}