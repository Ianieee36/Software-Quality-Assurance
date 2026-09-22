
namespace Week09QualityLab.Core;

public class DiscountCalculator
{
    public decimal CalculateFinalPrice(decimal originalPrice, string customerType)
    {
        decimal discountRate;

        if(originalPrice < 0) 
            throw new ArgumentOutOfRangeException(nameof(originalPrice));

        switch (customerType)
        {
            case "Premium":
                discountRate = 0.10m;
                break;
            case "VIP":
                discountRate = 0.20m;
                break;
            default:
                discountRate = 0.00m;
                break;    
        }

        return originalPrice - (originalPrice * discountRate); 

    }
}
