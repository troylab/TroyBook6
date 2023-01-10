using System;
namespace BookStore.Domain.Repositories;

public interface IBookStoreRepository
{
    Task<IEnumerable<GetBookWithImage.Rs>> GetBookWithImage(GetBookWithImage.Qy qy);
}

