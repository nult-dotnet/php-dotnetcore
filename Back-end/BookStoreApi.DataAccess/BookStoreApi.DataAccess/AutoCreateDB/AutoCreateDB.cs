using BookStoreApi.DBContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreApi.DataAccess.AutoCreateDB
{
    public class AutoCreateDB
    {
        public void CreateDB(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<SQLContext>();
                context.Database.Migrate();
            }
        }
    }
    public enum Database
    {
        MongoDB, SQLServer,PostgreSQL
    }
}