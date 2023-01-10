using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BookStore.Repository.EFCore
{
    public partial class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options)
            : base(options)
        {
        }

        #region Generated Properties
        public virtual DbSet<BookStore.Domain.Models.BookImage> BookImages { get; set; } = null!;

        public virtual DbSet<BookStore.Domain.Models.Book> Books { get; set; } = null!;

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Generated Configuration
            modelBuilder.ApplyConfiguration(new BookStore.Repository.EFCore.Mapping.BookImageMap());
            modelBuilder.ApplyConfiguration(new BookStore.Repository.EFCore.Mapping.BookMap());
            #endregion
        }
    }
}
