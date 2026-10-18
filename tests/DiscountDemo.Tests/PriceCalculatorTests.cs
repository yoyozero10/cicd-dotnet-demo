using DiscountDemo;

namespace DiscountDemo.Tests;

public sealed class PriceCalculatorTests
{
    private readonly PriceCalculator calculator = new();

    [Theory]
    [InlineData(100, 0, 100)]
    [InlineData(100, 10, 90)]
    [InlineData(250, 20, 200)]
    [InlineData(100, 100, 0)]
    [InlineData(0, 25, 0)]
    [InlineData(19.99, 15, 16.99)]
    public void ApplyDiscount_ReturnsExpected(decimal price, int percent, decimal expected)
        => Assert.Equal(expected, calculator.ApplyDiscount(price, percent));

    [Fact]
    public void ApplyDiscount_NegativePercent_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(() => calculator.ApplyDiscount(100, -5));

    [Fact]
    public void ApplyDiscount_PercentAbove100_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(() => calculator.ApplyDiscount(100, 101));

    [Fact]
    public void ApplyDiscount_NegativePrice_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(() => calculator.ApplyDiscount(-1, 10));
}
