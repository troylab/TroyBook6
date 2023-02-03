using System;
using System.Collections.Generic;

namespace BookStore.Domain.Models
{
    public partial class BookImage
    {
        public BookImage()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public int Id { get; set; }

        public int BookId { get; set; }

        public string FilePath { get; set; } = null!;

        public DateTime? CreateOn { get; set; }

        public DateTime? UpdateOn { get; set; }

        #endregion

        #region Generated Relationships
        public virtual Book Book { get; set; } = null!;

        #endregion

    }
}
