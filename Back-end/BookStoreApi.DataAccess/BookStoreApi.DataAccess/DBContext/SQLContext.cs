using BookStoreApi.Models;
using LibraryAbstractDBProvider;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApi.DBContext
{
    public class SQLContext : DbContext 
    {
        private readonly string ConnectionString;
        private readonly CreateDatabase DBProvider = new CreateDatabase();
        public SQLContext()
        {
            ConnectionString = DBProvider.GetDBProvider().ConnectionString();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DBProvider.GetDBProvider().ConnectedDatabase(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BillDetail>().HasKey(table => new { table.BookId,table.BillId });
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetail { get; set; }
    }
}