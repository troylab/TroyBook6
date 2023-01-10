using System;
using System.Collections.Generic;

namespace BookStore.Domain.Models
{
    public partial class Book
    {
        public Book()
        {
            #region Generated Constructor
            BookImages = new HashSet<BookImage>();
            #endregion
        }

        #region Generated Properties
        public int Id { get; set; }

        public string BookName { get; set; } = null!;

        public string? Author { get; set; }

        public int? Price { get; set; }

        public DateTime? CreateOn { get; set; }

        public DateTime? UpdateOn { get; set; }

        #endregion

        #region Generated Relationships
        public virtual ICollection<BookImage> BookImages { get; set; }

        #endregion

    }
}
