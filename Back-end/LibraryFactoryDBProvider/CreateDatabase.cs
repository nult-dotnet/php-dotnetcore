using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryFactoryDBProvider
{
    public static class CreateDatabase
    {
        public static IDatabaseProvider GetDBProvider()
        {
            Type T = Type.GetType($"LibraryFactoryDBProvider.DBProvider.{GetStringAppsetting.DatabaseDefault()}");
            if(T is null)
            {
               throw new NotImplementedException("Default database not a vaild database type");
            }
            var DBProvider = Activator.CreateInstance(T) as IDatabaseProvider;
            return DBProvider;
        }
    }
}