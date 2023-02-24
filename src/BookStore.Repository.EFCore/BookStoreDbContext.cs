using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BookStore.Repository.EFCore
{
    public partial class BookStoreDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
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


            #region Asp.Net Core Identity tables 手動設定
            // required for using Identity
            base.OnModelCreating(modelBuilder);

            // change the names of all identity tables
            modelBuilder.Entity<IdentityUser>(entity => { entity.ToTable(name: "IdentityUsers"); });
            modelBuilder.Entity<IdentityRole>(entity => { entity.ToTable(name: "IdentityRoles"); });
            modelBuilder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("IdentityUserRoles"); });
            modelBuilder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("IdentityUserClaims"); });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("IdentityUserLogins"); });
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("IdentityRoleClaims"); });
            modelBuilder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("IdentityUserTokens"); });
            #endregion
        }
    }
}
