using System.Reflection.Metadata;
using BookLibrary.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Repositories
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>()
                .HasKey(i => i.Id);
        }
    }
}
