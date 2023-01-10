namespace BookStore.API.Test;

public class BookControllerTests : IClassFixture<TestServerFixture>
{
    private readonly TestServerFixture _fixture;
    public BookControllerTests(TestServerFixture fixture) => _fixture = fixture;


    [Fact]
    public async Task GetBookWithImage_Success()
    {
        var rq = new GetBookWithImage.Qy
        {
        };

        var (r, b) = await _fixture.SendAsync<IEnumerable<GetBookWithImage.Rs>>(HttpMethod.Post, "/api/Book/GetBookWithImage", rq);

        Assert.Equal(200, (int)r.StatusCode);
        Assert.NotEmpty(b);
    }
}
