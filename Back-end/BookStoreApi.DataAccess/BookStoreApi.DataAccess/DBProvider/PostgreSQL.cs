using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAbstractDBProvider.DBProvider
{
    public class PostgreSQL : AbstractDBProvider
    {
        public override void ConnectedDatabase(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString(), x => x.MigrationsAssembly("PostgreSQLMigrations"));
        }

        public override string ConnectionString()
        {
            IConfiguration config = GetStringAppsetting.ConnectString();
            return config.GetConnectionString("PostgreSQL");
        }
    }
}