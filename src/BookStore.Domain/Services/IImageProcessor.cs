
namespace BookStore.Domain.Services;
public interface IImageProcessor
{
    byte[] Resize(byte[] fileContent, int width, int height);
    /// <summary>
    /// 將圖片壓縮及刪除metadata
    /// </summary>
    /// <param name="filePath"></param>
    byte[] Compress(byte[] fileContent);
}

public class FakeImageProcessor : IImageProcessor
{
    public byte[] Compress(byte[] fileContent)
    {
        return fileContent;
    }

    public byte[] Resize(byte[] fileContent, int width, int height)
    {
        return fileContent;
    }
}
