using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAbstractDBProvider
{
    public class CreateDatabase
    {
        public IDatabaseProvider GetDBProvider()
        {
            Type T = Type.GetType($"LibraryAbstractDBProvider.DBProvider.{GetStringAppsetting.DatabaseDefault()}");
            if(T is null)
            {
                throw new NotImplementedException("Default database not a valid database type.");
            }
            var DBProvider = Activator.CreateInstance(T) as IDatabaseProvider;
            return DBProvider;
        }
    }
}