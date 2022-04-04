using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryFactoryDBProvider.DBProvider
{
    public class SQLServer : IDatabaseProvider
    {
        public void ConnectedDatabase(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString(), x => x.MigrationsAssembly("SqlServerMigrations"));
        }
        public string ConnectionString()
        {
            IConfiguration config = GetStringAppsetting.ConnectString();
            return config.GetConnectionString("SQLServer");
        }
    }
}