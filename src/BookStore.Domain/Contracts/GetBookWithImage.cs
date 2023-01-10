using System;
namespace BookStore.Domain.Contracts;

public class GetBookWithImage
{
    public class Qy
    {
        public int? PageNum { get; set; }
        public int? PageSize { get; set; }
        public string SortBy { get; set; } = "BookId";
        public string SortDirection { get; set; } = "ASC";

        public string? BookName { get; set; }
        public string? Author { get; set; }
    }

    public class Rs
    {
        public int BookId { get; set; }
        public string BookName { get; set; } = default!;
        public int Price { get; set; }
        public string? ImageFilePath { get; set; }
    }
}

