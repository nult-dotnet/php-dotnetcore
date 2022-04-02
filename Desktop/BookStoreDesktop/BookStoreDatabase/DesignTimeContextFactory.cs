using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDesktop.DatabaseFactory;
using LibraryAbstractDBProvider;

namespace BookStoreDesktop.BookStoreDatabase
{
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<BookStoreContext>
    {
        public BookStoreContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            string databaseDefault = GetStringAppsetting.DatabaseDefault();
            var connectionString = "";
            return new BookStoreContext();
        }
    }
}