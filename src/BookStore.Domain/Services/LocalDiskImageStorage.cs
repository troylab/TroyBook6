using BookStore.Domain.Models;

namespace BookStore.Domain.Services;

public class LocalDiskImageStorage : IImageStorage
{
    readonly DomainOptions.BookOptions _bookOptions;
    public LocalDiskImageStorage(DomainOptions options)
    {
        _bookOptions = options.Book;
    }

    public void SaveBookImage(string sourceImageFilePath, BookImage bookImage)
    {
        if (!File.Exists(sourceImageFilePath))
            throw new BookStoreDomainException($"The source image file '{sourceImageFilePath}' doesn't exists");

        var folderName = Path.Combine(_bookOptions.LocalBookImageBasePath, DateTime.Now.ToString("yyyyMM"));
        if (!Directory.Exists(folderName))
            Directory.CreateDirectory(folderName);

        var randomId = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("==", "");
        var newFileName = $"{randomId}.{Path.GetExtension(sourceImageFilePath)}";
        var newFilePath = Path.Combine(folderName, newFileName);

        try
        {
            File.Move(sourceImageFilePath, newFilePath);
        }
        catch (Exception ex)
        {
            throw new BookStoreDomainException($"An error occured while moving file from '{sourceImageFilePath}' to '{newFilePath}'", ex);
        }

        bookImage.FilePath = sourceImageFilePath;
    }
}
