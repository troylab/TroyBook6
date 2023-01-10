namespace BookStore.Domain;

public class BookStoreDomainException : Exception
{
    public BookStoreDomainException() { }
    public BookStoreDomainException(string message) : base(message) { }
    public BookStoreDomainException(string message, Exception inner) : base(message, inner) { }
}