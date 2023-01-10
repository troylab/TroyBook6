using BookStore.Domain.Models;
using BookStore.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BookStore.Repository.EFCore.Repositories;

public class EFBookImageRepository : EFBaseRepository<BookImage, int>, IBookImageRepository
{
    public EFBookImageRepository(BookStoreDbContext dbContext)
        : base(dbContext)
    {
    }
}
