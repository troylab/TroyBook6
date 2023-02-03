using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository.EFCore.Mapping
{
    public partial class BookMap
        : IEntityTypeConfiguration<BookStore.Domain.Models.Book>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<BookStore.Domain.Models.Book> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("Book", "dbo");

            // key
            builder.HasKey(t => t.Id);

            // properties
            builder.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();

            builder.Property(t => t.BookName)
                .IsRequired()
                .HasColumnName("BookName")
                .HasColumnType("nvarchar(450)")
                .HasMaxLength(450);

            builder.Property(t => t.Author)
                .HasColumnName("Author")
                .HasColumnType("nvarchar(100)")
                .HasMaxLength(100);

            builder.Property(t => t.Price)
                .HasColumnName("Price")
                .HasColumnType("int");

            builder.Property(t => t.CreateOn)
                .HasColumnName("CreateOn")
                .HasColumnType("datetime2(3)");

            builder.Property(t => t.UpdateOn)
                .HasColumnName("UpdateOn")
                .HasColumnType("datetime2(3)");

            // relationships
            #endregion
        }

        #region Generated Constants
        public struct Table
        {
            public const string Schema = "dbo";
            public const string Name = "Book";
        }

        public struct Columns
        {
            public const string Id = "Id";
            public const string BookName = "BookName";
            public const string Author = "Author";
            public const string Price = "Price";
            public const string CreateOn = "CreateOn";
            public const string UpdateOn = "UpdateOn";
        }
        #endregion
    }
}
