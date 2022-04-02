using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreDesktop.Models;
using LibraryAbstractDBProvider;

namespace BookStoreDesktop.BookStoreDatabase
{
    public class BookStoreContext : DbContext
    {
        private readonly string ConnectionString;
        private readonly CreateDatabase DBProvider = new CreateDatabase();
        public BookStoreContext()
        {
            ConnectionString = DBProvider.GetDBProvider().ConnectionString();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DBProvider.GetDBProvider().ConnectedDatabase(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().Property(b => b.Id).ValueGeneratedOnAdd();
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}