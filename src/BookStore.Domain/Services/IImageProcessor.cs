
namespace BookStore.Domain.Services;
public interface IImageProcessor
{
    void Resize(string filePath, int width, int height);
    /// <summary>
    /// 將圖片壓縮及刪除metadata
    /// </summary>
    /// <param name="filePath"></param>
    void Compress(string filePath);
}

public class FakeImageProcessor : IImageProcessor
{
    public void Compress(string filePath)
    {
    }

    public void Resize(string filePath, int width, int height)
    {
    }
}
