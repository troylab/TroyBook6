using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Repository.Test;

[Collection("Database collection")]
public class BookStoreRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly IBookStoreRepository _repo;

    public BookStoreRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _repo = _fixture.Sp.GetRequiredService<IBookStoreRepository>()!;
    }

    [Fact]
    public async Task GetBookWithImage_Success()
    {
        var qy = new GetBookWithImage.Qy
        {
        };

        var rs = await _repo.GetBookWithImage(qy);

        Assert.NotEmpty(rs);
    }
}
