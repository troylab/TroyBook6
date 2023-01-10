namespace BookStore.Domain;

public class DomainOptions
{
    public BookOptions Book { get; set; } = new BookOptions();

    public class BookOptions
    {
        /// <summary>
        /// 本機的書籍圖片儲存目錄，預設為 ./images/book
        /// </summary>
        public string LocalBookImageBasePath { get; set; } = @"./images/book";
        /// <summary>
        /// 設定本機存取圖片url的基本目錄，預設為 /bookimage
        /// </summary>
        public string LocalBookImageBaseUrl { get; set; } = @"/bookimages";
    }
}