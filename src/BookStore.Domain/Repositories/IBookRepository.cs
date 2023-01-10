namespace BookStore.Domain.Repositories;

/// <summary>
/// 存取 Book 的 repository 介面，, 繼承 IRepository，指定 Key 欄位的資料型態為 string
/// </summary>
public interface IBookRepository : IRepository<Book, int>
{
}
