using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAbstractDBProvider.DBProvider
{
    public class SQLServer : AbstractDBProvider
    {
        public override void ConnectedDatabase(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString(), x => x.MigrationsAssembly("SqlServerMigrations"));
        }
        public override string ConnectionString()
        {
            IConfiguration config = GetStringAppsetting.ConnectString();
            return config.GetConnectionString("SQLServer");
        }
    }
}