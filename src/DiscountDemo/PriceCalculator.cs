namespace DiscountDemo;

public sealed class PriceCalculator
{
    public decimal ApplyDiscount(decimal price, int percent)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(price);
        if (percent > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(percent), "Phần trăm giảm phải từ 0 đến 100.");
        }

        return decimal.Round(price * (100 - percent) / 100m, 2, MidpointRounding.AwayFromZero);
    }
}
