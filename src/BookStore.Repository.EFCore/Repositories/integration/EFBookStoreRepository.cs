using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookStore.Repository.EFCore.Repositories;

public class EFBookStoreRepository : EFIntegrationBase, IBookStoreRepository
{
    public EFBookStoreRepository(BookStoreDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<GetBookWithImage.Rs>> GetBookWithImage(GetBookWithImage.Qy qy)
    {
        var builder = new SqlBuilder();
        var selector = builder.AddTemplate(@"
SELECT 
  b.Id AS BookId
 ,b.BookName
 ,b.Price
 ,bi.FilePath AS ImageFilePath
FROM Book b JOIN BookImage bi ON bi.BookId = b.Id
/**where**/
/**orderby**/
/**fetch**/
"
        );

        if (qy.PageNum >= 1 && qy.PageSize >= 1)
        {
            builder.Fetch((qy.PageNum.Value - 1) * qy.PageSize.Value, qy.PageSize.Value);
        }

        if (qy.SortBy == "Id")
        {
            builder.OrderBy(qy.SortBy, isAsc: (qy.SortDirection.ToUpper() == "ASC"));
        }
        else
        {
            var propertyNames = typeof(GetBookWithImage.Rs).GetProperties().Select(t => t.Name.ToLower()).ToList();
            if (propertyNames.Contains(qy.SortBy.ToLower().Trim()))
                builder.OrderBy(qy.SortBy, isAsc: (qy.SortDirection.ToUpper() == "ASC"));
            else
                builder.OrderBy("BookId", isAsc: (qy.SortDirection.ToUpper() == "ASC"));
        }

        if (qy.BookName != null)
            builder.Where("b.BookName LIKE '%@BookName%'", new { qy.BookName });

        if (qy.Author != null)
            builder.Where("b.Author = @Author", new { qy.Author });

        var q = await _conn.QueryAsync<GetBookWithImage.Rs>(selector.RawSql, selector.Parameters);
        return q;
    }
}

