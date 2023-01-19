using BookStore.Domain.Models;

namespace BookStore.Domain.Services;

public class LocalDiskImageStorage : IImageStorage
{
    readonly DomainOptions.BookOptions _bookOptions;
    public LocalDiskImageStorage(DomainOptions options)
    {
        _bookOptions = options.Book;
    }

    public async Task SaveBookImage(BookImage bookImage, byte[] fileContent)
    {
        var folderName = Path.Combine(_bookOptions.LocalBookImageBasePath, DateTime.Now.ToString("yyyyMM"));
        if (!Directory.Exists(folderName))
            Directory.CreateDirectory(folderName);

        var randomId = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("==", "");
        var newFileName = $"{randomId}.{Path.GetExtension(bookImage.FilePath)}";
        var newFilePath = Path.Combine(folderName, newFileName);

        try
        {
            using var fs = new FileStream(newFilePath!, FileMode.Create);
            await fs.WriteAsync(fileContent);
        }
        catch (Exception ex)
        {
            throw new BookStoreDomainException($"An error occured while saving file '{newFilePath}'", ex);
        }
    }
}
