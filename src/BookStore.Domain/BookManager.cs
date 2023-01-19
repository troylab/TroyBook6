using BookStore.Domain.Models;
using BookStore.Domain.Repositories;
using BookStore.Domain.Services;
using System.Linq.Expressions;

namespace BookStore.Domain;

public class BookManager
{
    readonly IBookRepository _bookRepository;
    readonly IBookStoreRepository _bookStoreRepository;
    readonly IImageStorage _imageStorage;
    readonly IImageProcessor _imageProcessor;

    /*透過 constructor injection 將相關依賴項目*/
    public BookManager(
        IBookRepository bookRepository,
        IBookStoreRepository bookStoreRepository,
        IImageStorage imageStorage,
        IImageProcessor imageProcessor
        )
    {
        _bookRepository = bookRepository;
        _bookStoreRepository = bookStoreRepository;
        _imageStorage = imageStorage;
        _imageProcessor = imageProcessor;
    }

    public void CreateBook(Book book)
    {
        Ensure.ArgumentNotEmpty(book, nameof(book));
        //TODO:做一些欄位的檢查

        _bookRepository.Insert(book);
        _bookRepository.SaveChanges();
    }

    /// <summary>
    /// 建立 BookImage 並將來源圖片存到圖片儲存庫
    /// </summary>
    /// <param name="sourceImageFilePath"></param>
    /// <returns></returns>
    public BookImage CreateBookImage(string filePath, byte[] fileContent)
    {
        _imageProcessor.Compress(fileContent);

        var img = new BookImage
        {
            FilePath = filePath
        };
        _imageStorage.SaveBookImage(img, fileContent);

        return img;
    }

    public async Task<IEnumerable<GetBookWithImage.Rs>> GetBooks(GetBookWithImage.Qy qy)
    {
        var r = await _bookStoreRepository.GetBookWithImage(qy);
        return r;
    }

    public Book? GetBookById(int id)
    {
        return _bookRepository.GetByID(id);
    }

    public void UpdateBook(Book book)
    {
        _bookRepository.Update(book);
        _bookRepository.SaveChanges();
    }
}
