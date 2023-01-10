
namespace BookStore.Repository.EFCore;

public class BookStoreEFRepositoryExcpetion : Exception
{
    public BookStoreEFRepositoryExcpetion() { }
    public BookStoreEFRepositoryExcpetion(string message) : base(message) { }
    public BookStoreEFRepositoryExcpetion(string message, Exception inner) : base(message, inner) { }
}
