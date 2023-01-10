using BookStore.Domain.Models;

namespace BookStore.Domain.Services;
public interface IImageStorage
{
    /// <summary>
    /// 將來源圖片檔存到圖片儲存庫，並將圖片位址資料回寫 bookImage
    /// </summary>
    /// <param name="sourceImageFilePath"></param>
    /// <param name="bookImage"></param>
    /// <returns></returns>
    void SaveBookImage(string sourceImageFilePath, BookImage bookImage);
}

public class FakeImageStorage : IImageStorage
{
    public void SaveBookImage(string sourceImageFilePath, BookImage bookImage)
    {
    }
}
