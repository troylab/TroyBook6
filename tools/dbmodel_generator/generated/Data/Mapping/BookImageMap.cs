using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository.EFCore.Mapping
{
    public partial class BookImageMap
        : IEntityTypeConfiguration<BookStore.Domain.Models.BookImage>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<BookStore.Domain.Models.BookImage> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("BookImage", "dbo");

            // key
            builder.HasKey(t => t.Id);

            // properties
            builder.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd();

            builder.Property(t => t.BookId)
                .IsRequired()
                .HasColumnName("BookId")
                .HasColumnType("int");

            builder.Property(t => t.FilePath)
                .IsRequired()
                .HasColumnName("FilePath")
                .HasColumnType("nvarchar(450)")
                .HasMaxLength(450);

            builder.Property(t => t.CreateOn)
                .HasColumnName("CreateOn")
                .HasColumnType("datetime2(3)");

            builder.Property(t => t.UpdateOn)
                .HasColumnName("UpdateOn")
                .HasColumnType("datetime2(3)");

            // relationships
            builder.HasOne(t => t.Book)
                .WithMany(t => t.BookImages)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("FK_BookImage_Book");

            #endregion
        }

        #region Generated Constants
        public struct Table
        {
            public const string Schema = "dbo";
            public const string Name = "BookImage";
        }

        public struct Columns
        {
            public const string Id = "Id";
            public const string BookId = "BookId";
            public const string FilePath = "FilePath";
            public const string CreateOn = "CreateOn";
            public const string UpdateOn = "UpdateOn";
        }
        #endregion
    }
}
