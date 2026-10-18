using DiscountDemo;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<PriceCalculator>();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
app.MapGet("/api/version", () =>
{
    var file = Path.Combine(app.Environment.ContentRootPath, "build-info.json");
    return Results.Content(File.ReadAllText(file), "application/json");
});
app.MapPost("/api/discount", (DiscountRequest request, PriceCalculator calculator) =>
{
    try
    {
        var total = calculator.ApplyDiscount(request.Price, request.Percent);
        return Results.Ok(new { originalPrice = request.Price, percent = request.Percent, total });
    }
    catch (ArgumentOutOfRangeException)
    {
        return Results.BadRequest(new { error = "Giá phải không âm và phần trăm giảm phải từ 0 đến 100." });
    }
});

app.Run();

public sealed record DiscountRequest(decimal Price, int Percent);
public partial class Program { }
