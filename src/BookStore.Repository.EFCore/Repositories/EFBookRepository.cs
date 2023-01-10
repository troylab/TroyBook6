using BookStore.Domain.Models;
using BookStore.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BookStore.Repository.EFCore.Repositories;

/// <summary>
/// 存取 Book 的 EntityFramework repository, 繼承  EFBaseRepository，指定 Key 欄位的資料型態為 string
/// </summary>
public class EFBookRepository : EFBaseRepository<Book, int>, IBookRepository
{
    public EFBookRepository(BookStoreDbContext dbContext)
        : base(dbContext)
    {
    }
}
