using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DiscountDemo.Tests;

public sealed class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;
    public ApiTests(WebApplicationFactory<Program> factory) => client = factory.CreateClient();

    [Fact]
    public async Task Discount_ValidInput_ReturnsCalculatedTotal()
    {
        var response = await client.PostAsJsonAsync("/api/discount", new { price = 100, percent = 10 });
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(90m, body.GetProperty("total").GetDecimal());
    }

    [Theory]
    [InlineData(100, -5)]
    [InlineData(100, 101)]
    [InlineData(-1, 10)]
    public async Task Discount_InvalidInput_ReturnsBadRequest(decimal price, int percent)
    {
        var response = await client.PostAsJsonAsync("/api/discount", new { price, percent });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Health_ReturnsHealthy()
    {
        var body = await client.GetFromJsonAsync<JsonElement>("/health");
        Assert.Equal("healthy", body.GetProperty("status").GetString());
    }

    [Fact]
    public async Task Version_ReturnsBuildInformation()
    {
        var body = await client.GetFromJsonAsync<JsonElement>("/api/version");
        Assert.False(string.IsNullOrWhiteSpace(body.GetProperty("version").GetString()));
        Assert.False(string.IsNullOrWhiteSpace(body.GetProperty("commit").GetString()));
    }
}
