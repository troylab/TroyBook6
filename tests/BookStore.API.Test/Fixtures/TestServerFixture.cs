using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Test;

public class TestServerFixture
{
    public PlaygroundApplication Application { get; private set; }

    private readonly HttpClient _appClient;

    public TestServerFixture()
    {
        Application = new PlaygroundApplication();
        _appClient = Application.CreateClient();
        _appClient.Timeout = TimeSpan.FromDays(1);
    }

    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) => _appClient.SendAsync(request);

    public async Task<(HttpResponseMessage, T)> SendAsync<T>(HttpMethod method, string path, object? body = null)
    {
        var httpRq = new HttpRequestMessage(method, path);

        if (body != null)
            httpRq.Content = new StringContent(JPac.Serialize(body), Encoding.UTF8, "application/json");

        var response = await _appClient.SendAsync(httpRq);
        var rspText = await response.Content.ReadAsStringAsync();

        T resBody = default!;

        resBody = JPac.Deserialize<T>(rspText)!;

        return (response, resBody!);
    }
}

